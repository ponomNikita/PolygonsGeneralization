using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Infrastructure.Logger;
using PolygonGeneralization.WinForms.Interfaces;
using PolygonGeneralization.WinForms.Models;
using PolygonGeneralization.WinForms.Properties;

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
        private readonly IDbService _dbService;
        private readonly ILogger _logger;

        public MainFormViewModel(Panel canvas, 
            IGisDataReader dataReader,
            IDbService dbService,
            ILogger logger)
        {
            _dbService = dbService;
            _canvas = canvas;
            _dataReader = dataReader;
            _drawerFactory = new DrawerFactory(canvas.CreateGraphics());
            _meta = new MetaInfo();
            _logger = logger;
        }

        public void AddHandler(EventHandler handler)
        {
            _logger.AddEventHandler(handler);
        }

        public string Meta => $"{_meta}\n\n{_logger.GetLog()}";

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
                    var filename = dialog.SafeFileName;

                    Task.Run(() => {

                        _mapFileName = filename;

                        _logger.Log("Reading map from file...");
                        var map = _dataReader.ReadFromFile(_mapFileName);
                        _logger.Log("Done");

                        _logger.Log("Initialize screen adapter...");
                        _logger.Log("Done");
                        
                        _logger.Log("Saving into database...");
                        _dbService.SaveMap(map);
                        _logger.Log("Done");

                        //_drawablePolygons = _polygons.Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();
                        //_canvas.Invalidate();
                    });
                    
                }
            }
        }

        public void Paint(PaintEventArgs paintEventArgs)
        {
            if (_drawablePolygons != null)
            {
                _logger.Log("Drawing....");
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
                _logger.Log("Done");
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
