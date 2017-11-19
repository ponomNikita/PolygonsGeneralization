using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms
{
    public struct BBox
    {
        private const double MOVING_STEP = 10;

        public BBox(Point leftDown, Point rightTop)
        {
            LeftDown = leftDown;
            RightTop = rightTop;
        }

        public bool HasPoint(Point point)
        {
            return point.X >= LeftDown.X &&
                   point.X <= RightTop.X &&
                   point.Y >= LeftDown.Y &&
                   point.Y <= RightTop.Y;
        }

        public Point LeftDown { get; }
        public Point RightTop { get; }

        public void MoveUp()
        {
            LeftDown.Y += MOVING_STEP;
            RightTop.Y += MOVING_STEP;
        }

        public void ModeDown()
        {
            LeftDown.Y -= MOVING_STEP;
            RightTop.Y -= MOVING_STEP;
        }

        public void MoveLeft()
        {
            LeftDown.X -= MOVING_STEP;
            RightTop.X -= MOVING_STEP;
        }

        public void MoveRight()
        {
            LeftDown.X += MOVING_STEP;
            RightTop.X += MOVING_STEP;
        }
    }
    public class ScreenAdapter
    {
        private readonly double _scale;

        public ScreenAdapter(int mapWidth, int mapHeight, Point[] points, double scale = 25)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            _scale = scale;

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
            return new Point2D((int)((point.X - Bbox.LeftDown.X) / Zoom), (int)((point.Y - Bbox.LeftDown.Y) / Zoom));
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

            var height = (maxX - minX) / factor / _scale;
            var width = (maxY - minY) / factor / _scale;
            var widthDelta = (maxX - minX - width) / 2;
            var heightDelta = (maxY - minY - height) / 2;

            if (maxX - minX > maxY - minY)
            {
                Bbox = new BBox(new Point(minX + widthDelta, minY + heightDelta),
                    new Point(minX + widthDelta, minY + heightDelta + height));
                Zoom = height/MapHeight;
            }
            else
            {
                Bbox = new BBox(new Point(minX + widthDelta, minY + heightDelta),
                    new Point(minX + widthDelta + width, maxY + heightDelta));
                Zoom = width/MapWidth;
            }
        }
    }
}