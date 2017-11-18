
using System.Text;

namespace PolygonGeneralization.WinForms.ViewModels
{
    public class MetaInfo
    {
        public double Zoom { get; set; }
        public int TotalPolygonsCount { get; set; }
        public int VisiblePolygonsCount { get; set; }
        public int InMemoryPolygonsCount { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"Zoom: {Zoom}");
            result.AppendLine($"Total polygons count: {TotalPolygonsCount}");
            result.AppendLine($"In memory polygons count: {InMemoryPolygonsCount}");
            result.AppendLine($"Visible polygons count: {VisiblePolygonsCount}");

            return result.ToString();
        }
    }
}
