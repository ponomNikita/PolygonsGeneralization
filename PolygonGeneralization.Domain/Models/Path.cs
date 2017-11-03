namespace PolygonGeneralization.Domain.Models
{
    public class Path
    {
        public Path(params Point[] points)
        {
            Points = points;
        }

        public Point[] Points { get; }
    }
}
