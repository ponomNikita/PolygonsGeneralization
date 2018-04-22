namespace PolygonGeneralization.Domain
{
    public class GeneralizerOptions
    {
        // Plygon generalization options
        public double MinDistance { get; set; }
        public double MinDistanceCoeff { get; set; }
        
        // Linear generalization options
        public double MaxDifferenceInPercent { get; set; }
    }
}