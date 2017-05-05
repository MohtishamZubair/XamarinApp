namespace SalesAgentDistribution.View
{
   public class Metrics
    {

        private static readonly Metrics _instance = new Metrics();

        private Metrics()
        {

        }

        protected void SessionData()
        {
        }

        public double Width { get; set; } //Width

        public double Height { get; set; } //Height
        public static Metrics Instance { get { return _instance; } }
    }
}