namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class Edge
    {
        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }
            
        public Vertex Start { get; }
        public Vertex End { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 397 + End.GetHashCode();
            }
        }
    }
}