using System.Drawing;

namespace PolygonGeneralization.WinForms.Interfaces
{
    public interface IDrawerFactory
    {
        IDrawer CreateDrawer();
        void SetGraphics(Graphics graphics);
    }
}