using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Interfaces;
using PolygonGeneralization.WinForms.Models;
using PolygonGeneralization.WinForms.Properties;
using Point = System.Drawing.Point;

namespace PolygonGeneralization.WinForms.ViewModels
{
    public class MainFormViewModel
    {
        private Panel _canvas;
        private string _mapFileName;
        private readonly IGisDataReader _dataReader;
        private Polygon[] _polygons;
        private ScreenAdapter _screenAdapter;
        private IDrawerFactory _drawerFactory;
        private List<DrawablePolygon> _drawablePolygons;

        public MainFormViewModel(Panel canvas, IGisDataReader dataReader)
        {
            _canvas = canvas;
            _dataReader = dataReader;
            _drawerFactory = new DrawerFactory(canvas.CreateGraphics());
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void OpenFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resources.FileDialogFilter;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _mapFileName = dialog.FileName;
                    _polygons = _dataReader.ReadFromFile(_mapFileName);

                    var allPoints = _polygons.SelectMany(p => p.Paths).SelectMany(p => p.Points).ToArray();

                    _screenAdapter = new ScreenAdapter(_canvas.Width, _canvas.Height, allPoints);

                    _drawablePolygons = _polygons.Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();
                   _canvas.Invalidate();
                }
            }
        }

        public void Paint(PaintEventArgs paintEventArgs)
        {
            if (_drawablePolygons != null)
            {
                _drawerFactory.SetGraphics(paintEventArgs.Graphics);

                _drawablePolygons.ForEach(p => p.Draw());
            }
        }
    }
}
