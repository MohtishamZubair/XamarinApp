namespace SalesAgentDistribution.Model
{
    internal class AppUser
    {
        private   static string _password = "003332625277";
        private   static string _name= "mohsin@saas.com";

        internal static string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        internal static string Password
        {
            get { return _password; }
            set { _password = value; }
        }        
    }
}