using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
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
        
        public event EventHandler MapsUpdatedEvent;

        public void AddHandler(EventHandler handler)
        {
            _logger.AddEventHandler(handler);
        }

        public string Meta => $"{_meta}\n\n{_logger.GetLog()}";


        public bool IsMapLoaded { get; private set; }

        public void Exit()
        {
            Application.Exit();
        }

        public void ImportMap()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resources.FileDialogFilter;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    var filename = dialog.SafeFileName;

                    Task.Run(() => 
                    {

                        _mapFileName = filename;

                        _logger.Log("Reading map from file...");
                        var map = _dataReader.ReadFromFile(_mapFileName);
                        _logger.Log("Done");
                        
                        _logger.Log("Saving into database...");
                        _dbService.SaveMap(map);
                        MapsUpdatedEvent?.Invoke(this, EventArgs.Empty);
                        _logger.Log("Done");

                    });
                    
                }
            }
        }

        public void OpenMap()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resources.FileDialogFilter;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    var filename = dialog.SafeFileName;

                    Task.Run(() =>
                    {

                        _mapFileName = filename;

                        _logger.Log("Reading map from file...");
                        var map = _dataReader.ReadFromFile(_mapFileName);
                        _logger.Log("Done");

                        // TODO убрать в команду
                        _logger.Log("Initialize screen adapter...");
                        _screenAdapter = new ScreenAdapter(_canvas.Width, _canvas.Height, 
                            map.Polygons.SelectMany(p => p.Paths).SelectMany(p => p.Points).ToArray());
                        _logger.Log("Done");

                        _drawablePolygons = map.Polygons.Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();

                        _canvas.Invalidate();
                        IsMapLoaded = true;

                    });

                }
            }
        }

        public void OpenMap(Guid mapId)
        {
            Task.Run(() =>
            {
                var extrimalPoints = _dbService.GetExtrimalPoints(mapId);

                // TODO убрать в команду
                _logger.Log("Initialize screen adapter...");
                _screenAdapter = new ScreenAdapter(_canvas.Width, _canvas.Height, extrimalPoints);
                _logger.Log("Done");

                DrawMap(mapId);
                IsMapLoaded = true;
            });
        }

        private void DrawMap(Guid mapId)
        {
            var polygons = _dbService.GetPolygons(mapId, _screenAdapter.Bbox.LeftDown, _screenAdapter.Bbox.RightTop);

            _drawablePolygons = polygons.Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();

            _canvas.Invalidate();
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

        public IDictionary<Guid, string> GetAvailableMaps()
        {
            var maps = _dbService.GetMaps();

            return maps.ToDictionary(m => m.Id, m => m.Name);
        }
    }
}
