using System.Configuration;

namespace PolygonGeneralization.Infrastructure
{
    public static class ConnectionStringsProvider
    {
        static ConnectionStringsProvider()
        {
            PolygonsConnectionString = 
                ConfigurationManager.ConnectionStrings["Polygons"].ConnectionString;
        }
        public static string PolygonsConnectionString { get; }
    }
}