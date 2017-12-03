using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.WinForms.ScreenAdapter
{
    public struct BBox
    {
        private const double MOVING_STEP_FACTOR = 0.1; // 10% от видимого
        private const double SCROLL_FACTOR = 0.05;

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
            var movingStep = GetMovingStep();
            LeftDown.Y -= movingStep;
            RightTop.Y -= movingStep;
        }

        public void MoveDown()
        {
            var movingStep = GetMovingStep();
            LeftDown.Y += movingStep;
            RightTop.Y += movingStep;
        }

        public void MoveLeft()
        {
            var movingStep = GetMovingStep();
            LeftDown.X -= movingStep;
            RightTop.X -= movingStep;
        }

        public void MoveRight()
        {
            var movingStep = GetMovingStep();
            LeftDown.X += movingStep;
            RightTop.X += movingStep;
        }

        private double GetMovingStep()
        {
            return (RightTop.X - LeftDown.X) * MOVING_STEP_FACTOR;
        }
    }
}