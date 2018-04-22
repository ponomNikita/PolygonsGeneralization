using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.LinearGeneralizer;
using PolygonGeneralization.Infrastructure.Commands;
using PolygonGeneralization.Infrastructure.Logger;
using PolygonGeneralization.WinForms.Interfaces;
using PolygonGeneralization.WinForms.Models;
using PolygonGeneralization.WinForms.Properties;

namespace PolygonGeneralization.WinForms.ViewModels
{
    public enum ViewType
    {
        FROM_FILE,
        FROM_DATABASE
    }

    public class MainFormViewModel
    {
        private readonly Panel _canvas;
        private string _mapFileName;
        private readonly IGisDataReader _dataReader;
        private ScreenAdapter.ScreenAdapter _screenAdapter;
        private readonly IDrawerFactory _drawerFactory;
        private List<DrawablePolygon> _drawablePolygons;
        private readonly MetaInfo _meta;
        private readonly IDbService _dbService;
        private readonly ILogger _logger;
        private Guid _currentMapId;
        private ViewType _viewType;
        private readonly IGeneralizer _generalizer;
        private readonly GeneralizerOptions _generalizerOptions;
        private ILinearGeneralizer _linearGeneralizer;

        public MainFormViewModel(Panel canvas, 
            IGisDataReader dataReader,
            IDbService dbService,
            ILogger logger, 
            IGeneralizer generalizer,
            GeneralizerOptions generalizerOptions,
            ILinearGeneralizer linearGeneralizer)
        {
            _dbService = dbService;
            _canvas = canvas;
            _dataReader = dataReader;
            _drawerFactory = new DrawerFactory(canvas.CreateGraphics());
            _meta = new MetaInfo();
            _logger = logger;
            _generalizer = generalizer;
            _generalizerOptions = generalizerOptions;
            _linearGeneralizer = linearGeneralizer;
            _drawablePolygons = new List<DrawablePolygon>();
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
                        _screenAdapter = new ScreenAdapter.ScreenAdapter(_canvas.Width, _canvas.Height, 
                            map.Polygons.SelectMany(p => p.Paths).SelectMany(p => p.Points).ToArray());
                        _logger.Log("Done");

                        _drawablePolygons.Clear();
                        _drawablePolygons = map.Polygons.Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();

                        _canvas.Invalidate();
                        IsMapLoaded = true;
                        _viewType = ViewType.FROM_FILE;
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
                _screenAdapter = new ScreenAdapter.ScreenAdapter(_canvas.Width, _canvas.Height, extrimalPoints);
                _logger.Log("Done");

                DrawMap(mapId);
                IsMapLoaded = true;
                _viewType = ViewType.FROM_DATABASE;
                _currentMapId = mapId;
            });
        }

        private void DrawMap(Guid mapId)
        {
            var polygons = _dbService.GetPolygons(mapId, _screenAdapter.Bbox.LeftDown, _screenAdapter.Bbox.RightTop);

            var command = new GeneralizePolygonsCommand(_generalizer, polygons.ToList(), 
                _linearGeneralizer,
                _generalizerOptions.MinDistance);
            
            command.Execute();
            
            var generalizedPolygons = command.Result;
            //var generalizedPolygons = polygons.ToList();

            _meta.PolygonsCountAfterGeneralization = generalizedPolygons.Count;
            _meta.TotalPolygonsCount = polygons.Length;
            _meta.InMemoryPolygonsCount = polygons.Length;
            
            _drawablePolygons.Clear();
            _drawablePolygons = generalizedPolygons
                .Select(p => new DrawablePolygon(p, _screenAdapter, _drawerFactory)).ToList();

            _canvas.Invalidate();
        }

        public void Paint(PaintEventArgs paintEventArgs)
        {
            if (_drawablePolygons.Count != 0)
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
                
                _meta.Zoom = _screenAdapter.Zoom;
                _logger.Log("Done");
            }
        }

        public void MoveUp()
        {
            _logger.Log("Moving up");
            _screenAdapter.Bbox.MoveUp();

            if (_viewType == ViewType.FROM_DATABASE)
            {
                DrawMap(_currentMapId);
            }
        }

        public void ModeDown()
        {
            _logger.Log("Moving down");
            _screenAdapter.Bbox.MoveDown();

            if (_viewType == ViewType.FROM_DATABASE)
            {
                DrawMap(_currentMapId);
            }
        }

        public void MoveLeft()
        {
            _logger.Log("Moving left");
            _screenAdapter.Bbox.MoveLeft();

            if (_viewType == ViewType.FROM_DATABASE)
            {
                DrawMap(_currentMapId);
            }
        }

        public void MoveRight()
        {
            _logger.Log("Moving rigth");
            _screenAdapter.Bbox.MoveRight();

            if (_viewType == ViewType.FROM_DATABASE)
            {
                DrawMap(_currentMapId);
            }
        }

        public void Scroll(int scrollNumber)
        {
            if (IsMapLoaded)
            {
                _logger.Log($"Scroll: {scrollNumber}");
                _screenAdapter.Scroll(scrollNumber);

                // TODO сделать подгрузку полигонов в отдельном потоке
                if (_viewType == ViewType.FROM_DATABASE)
                {
                    DrawMap(_currentMapId);
                }
            }
        }

        public IDictionary<Guid, string> GetAvailableMaps()
        {
            var maps = _dbService.GetMaps();

            return maps.ToDictionary(m => m.Id, m => m.Name);
        }
    }
}
