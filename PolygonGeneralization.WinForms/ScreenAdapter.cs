using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms
{
    public struct BBox
    {
        public BBox(Point leftDown, Point rightTop)
        {
            LeftDown = leftDown;
            RightTop = rightTop;
        }

        public Point LeftDown { get; }
        public Point RightTop { get; }
    }
    public class ScreenAdapter
    {
        public ScreenAdapter(int mapWidth, int mapHeight, Point[] points)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;

            Initialize(points);
        }

        public Point ZeroPoint { get; set; }
        public double Zoom { get; set; }

        public BBox Bbox { get; private set; }
        public int MapWidth { get; }
        public int MapHeight{ get; }

        /// <summary>
        /// Преобразование точки из декартовых координат в экранные
        /// </summary>
        /// <param name="point">Точка в декартовых координатах</param>
        /// <returns></returns>
        public Point2D ToPixel(Point point)
        {
            return new Point2D((int)((point.X - ZeroPoint.X) / Zoom), (int)((point.Y - ZeroPoint.Y) / Zoom));
        }

        /// <summary>
        /// Инициализация //TODO Добавить описание
        /// </summary>
        /// <param name="locations">Точки с географическими координатами(широта долгота в градусах)</param>
        private void Initialize(Point[] points)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;
            var maxX = double.MinValue;
            var maxY = double.MinValue;

            foreach (var point in points)
            {
                minX = point.X < minX ? point.X : minX;
                maxX = point.X > maxX ? point.X : maxX;

                minY = point.Y < minY ? point.Y : minY;
                maxY = point.Y > maxY ? point.Y : maxY;
            }

            ZeroPoint = new Point(minX, minY);

            var factor = maxX - minX > maxY - minY ? (double)MapWidth / MapHeight : (double)MapHeight / MapWidth;

            if (maxX - minX > maxY - minY)
            {
                var height = (maxX - minX)/factor;
                Bbox = new BBox(new Point(minX, minY), new Point(maxX, minY + height));
                Zoom = height/MapHeight;
            }
            else
            {
                var width = (maxY - minY)/factor;
                Bbox = new BBox(new Point(minX, minY), new Point(minX + width, maxY));
                Zoom = width/MapWidth;
            }
        }
    }
}