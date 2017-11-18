using System;
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
        private readonly Panel _canvas;
        private string _mapFileName;
        private readonly IGisDataReader _dataReader;
        private Polygon[] _polygons;
        private ScreenAdapter _screenAdapter;
        private readonly IDrawerFactory _drawerFactory;
        private List<DrawablePolygon> _drawablePolygons;
        private readonly MetaInfo _meta;

        public MainFormViewModel(Panel canvas, IGisDataReader dataReader)
        {
            _canvas = canvas;
            _dataReader = dataReader;
            _drawerFactory = new DrawerFactory(canvas.CreateGraphics());
            _meta = new MetaInfo();
        }

        public string Meta => _meta.ToString();

        public event EventHandler MetaChangedEvent;

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

                // TODO сделать нахождение IsVisible полигонов и их отрисовку в одном цикле
                var visiblePolygons = _drawablePolygons.Where(p => p.IsVisible()).ToList();

                visiblePolygons.ForEach(p => p.Draw());

                _meta.TotalPolygonsCount = _drawablePolygons.Count;
                _meta.InMemoryPolygonsCount = _drawablePolygons.Count;
                _meta.VisiblePolygonsCount = visiblePolygons.Count;
                _meta.Zoom = _screenAdapter.Zoom;

                MetaChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
