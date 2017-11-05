using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.WinForms
{
    public class ScreenInfo
    {
        public ScreenInfo()
        {
            Scale = 1; // 1 пиксель на у.е
        }
        /// <summary>
        /// Масштаб
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// Координаты левого нижнего угла экрана в мировом пространстве
        /// </summary>
        public Point WorldPosition { get; set; }

        /// <summary>
        /// Ширина в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота в пикселях
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Ширина в у.е
        /// </summary>
        public double ScaledWidth => Width / Scale;

        /// <summary>
        /// Высота в у.е
        /// </summary>
        public double ScaledHeight => Height / Scale;
    }
}