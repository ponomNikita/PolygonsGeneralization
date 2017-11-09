using System.Drawing;
using PolygonGeneralization.WinForms.Interfaces;

namespace PolygonGeneralization.WinForms
{
    public class DrawerFactory : IDrawerFactory
    {
        private Graphics _graphics;

        public DrawerFactory(Graphics graphics)
        {
            _graphics = graphics;
        }

        public IDrawer CreateDrawer()
        {
            return new Drawer(_graphics);
        }
    }
}
