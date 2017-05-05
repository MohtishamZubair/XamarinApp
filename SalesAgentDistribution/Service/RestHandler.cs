using System;
using System.Collections.Generic;
using SalesAgentDistribution.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SalesAgentDistribution.Helper;
using SalesAgentDistribution.Factory;

namespace SalesAgentDistribution.Service
{
    internal interface IRestHandler<T> : IDisposable where T : class
    {
        List<T> GetAll();
        Task<bool> UpdateAsync(int entityId, T t);
        Task<bool> TriggerPUT(int entityId, string json, int actvityId = -1);
    }

    internal class RestHandler<T> : IRestHandler<T> where T : class
    {
        private string _serviceURL;
        private IServiceQueue _serviceQueue;

        static private AccessToken _accessToken;

        //internal AccessToken AccessedToken
        //{
        //     get
        //    {
        //             return GetFreshAccessTokenAsync().Result;
        //        }
        //        return _accessToken;
        //    }
        //    private set { _accessToken = value; }
        //}

        private async Task<AccessToken> GetFreshAccessTokenAsync()
        {
            try
            {
                _accessToken = await GetAccessToken();
            }
            catch (Exception)
            {}
            return _accessToken;            
        }

        private async Task<AccessToken> GetAccessToken()
        {
            if (_accessToken == null || DateTime.Parse(_accessToken.ExpiresAt) < DateTime.Now)
            {
                var httpClient = GetClient();
                var headers = httpClient.DefaultRequestHeaders;
                headers.Add("Content-Tpye", "application/form-url-encoded");
                string requestParams = string.Format("grant_type=password&username={0}&password={1}", AppUser.Name, AppUser.Password);
                HttpContent content = new StringContent(requestParams);
                var response = httpClient.PostAsync(XamApplication.AccessTokenUrl, content);
                var responseContent = await response.Result.Content.ReadAsStringAsync();
                _accessToken = JsonConvert.DeserializeObject<AccessToken>(responseContent);

            }
            return _accessToken;
        }

        private HttpClient GetClientWithAccessToken()
        {
            var httpClient = GetClient();
            var token = GetFreshAccessTokenAsync().Result;
            if (token != null)
            {
                var headers = httpClient.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", token.Token));
            }

            return httpClient;
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            try
            {
                client.GetAsync(XamApplication.CheckAPIServer);
            }
            catch (Exception ex)
            {
                throw new Exception("false:servers", ex);
            }
            return client;
        }

        public RestHandler(string _serviceURL)
        {
            this._serviceURL = _serviceURL;
        }

        public async Task<bool> UpdateAsync(int entityId, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            var result = await TriggerPUT(entityId, json);
            return result;
        }

        public async Task<bool> TriggerPUT(int entityId, string json, int activityId = -1)
        {
            try
            {
                App.Current.Properties.Add(AppHelper.ON_GOING_ACTIVITY, new BGActivity { TargetObjectId = entityId, ActivityActionName = nameof(TriggerPUT), TargetObjectJSON = json, CreatedDate = DateTime.Now, ServiceURL = _serviceURL, ActivityId = activityId });
                var result = await TriggerPUTPossibleException(entityId, json, activityId);
                App.Current.Properties.Remove(AppHelper.ON_GOING_ACTIVITY);
                return result;
            }
            catch (Exception ex)
            {
                ex.HandleException();
                return false;
            }
        }


        async Task<bool> TriggerPUTPossibleException(int entityId, string json, int activityId = -1)
        {
            bool result = false;
            HttpClient client = GetClientWithAccessToken();
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(AppHelper.JSON_CONTENT);
            var response = await client.PutAsync(string.Format("{0}/{1}", _serviceURL, entityId), httpContent);
            result = response.IsSuccessStatusCode;
            return result;
        }

        List<T> IRestHandler<T>.GetAll()
        {
            var empty = new List<T>(); 
            try
            {
              empty =  GetAllPossibleException();
            }
            catch (Exception)
            {
                
            }
            return empty;
        }

        private List<T> GetAllPossibleException()
        {
            HttpClient client = GetClientWithAccessToken();
            var response = client.GetStringAsync(_serviceURL).Result;
            var list = JsonConvert.DeserializeObject<List<T>>(response);
            return list;
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                if (_serviceQueue != null)
                {
                    _serviceQueue.Dispose();
                }
            }
            disposed = true;
        }
    }
}