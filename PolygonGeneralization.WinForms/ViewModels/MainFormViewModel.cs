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

                _meta.VisiblePolygonsCount = 0;

                foreach (var polygon in _drawablePolygons)
                {
                    if (polygon.IsVisible())
                    {
                        _meta.VisiblePolygonsCount++;
                        polygon.Draw();
                    }
                }

                _meta.TotalPolygonsCount = _drawablePolygons.Count;
                _meta.InMemoryPolygonsCount = _drawablePolygons.Count;
                _meta.Zoom = _screenAdapter.Zoom;

                MetaChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MoveUp()
        {
            _screenAdapter.Bbox.MoveUp();
            _canvas.Invalidate();
        }

        public void ModeDown()
        {
            _screenAdapter.Bbox.ModeDown();
            _canvas.Invalidate();
        }

        public void MoveLeft()
        {
            _screenAdapter.Bbox.MoveLeft();
            _canvas.Invalidate();
        }

        public void MoveRight()
        {
            _screenAdapter.Bbox.MoveRight();
            _canvas.Invalidate();
        }
    }
}
