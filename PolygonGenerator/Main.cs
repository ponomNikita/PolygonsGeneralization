using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;
using Path = PolygonGeneralization.Domain.Models.Path;
using Point = System.Drawing.Point;

namespace PolygonGenerator
{
    public partial class Main : Form
    {
        private PictureBox _canvas;
        private const int CANVAS_SIZE = 10000;

        private SuperClipper _clipper = new SuperClipper();
        private VectorGeometry _vectorGeometry = new VectorGeometry();
        private readonly List<Point> _points = new List<Point>();
        private List<Polygon> _polygons = new List<Polygon>();
        private List<Polygon> _union = new List<Polygon>();
        private string _fileName;

        public Main()
        {
            InitializeComponent();
        }
        private void Initialize()
        {
            WindowState = FormWindowState.Maximized;

            Container.AutoScroll = true;

            _canvas = new PictureBox();
            _canvas.Width = CANVAS_SIZE;
            _canvas.Height = CANVAS_SIZE;
            _canvas.MouseClick += OnCanvasMouseClick;
            _canvas.Paint += OnCanvasPaint;

            Container.Controls.Add(_canvas);
        }

        private void OnCanvasPaint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.AliceBlue);
            for (var i = 0; i < CANVAS_SIZE; i += 15)
            {
                e.Graphics.DrawLine(pen, new Point(0, i), new Point(CANVAS_SIZE, i));
                e.Graphics.DrawLine(pen, new Point(i, 0), new Point(i, CANVAS_SIZE));
            }

            pen.Color = Color.Black;

            if (_points.Count > 1)
            {
                e.Graphics.DrawLines(pen, _points.ToArray());
            }

            DrawPolygons(_polygons, e.Graphics, Color.Black);
            if (_union.Any())
            {
                DrawPolygons(_union, e.Graphics, Color.Blue);
            }

            pen.Color = Color.Red;
            _points.ForEach(p => e.Graphics.FillEllipse(pen.Brush, p.X, p.Y, 3, 3));
        }

        private void OnCanvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OnRightMouseClick(sender, e);
                return;
            }

            var location = e.Location;
            Coords.Text = $"X:{location.X} Y:{location.Y}";
            
            _points.Add(location);

            if (_points.Count > 1)
            {
                if (DistanceSqr(location, _points.First()) < 36)
                {
                    _points.Remove(location);

                    _polygons.Add(new Polygon(_points.ToArray()));

                    PolygonsCount.Text = $"Polygons: {_polygons.Count}";
                    
                    _points.Clear();

                    _canvas.Invalidate();
                }
            }

            _canvas.Invalidate();
        }

        private void OnRightMouseClick(object sender, MouseEventArgs e)
        {
            Undo();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private int DistanceSqr(Point a, Point b)
        {
            return (a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y);
        }

        private void SavePolygons(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                var json = JsonConvert.SerializeObject(_polygons, Formatting.Indented);

                File.WriteAllText(fileName, json);

                var bitmap = new Bitmap(CANVAS_SIZE, CANVAS_SIZE);
                _canvas.DrawToBitmap(bitmap, new Rectangle(0, 0, CANVAS_SIZE, CANVAS_SIZE));

                bitmap.Save($"{_fileName}_image.png", ImageFormat.Png);
            }
            else
            {
                SavePolygonsAs();
            }
        }

        private void SavePolygonsAs()
        {
            var dialog = new SaveFileDialog();
            dialog.CheckPathExists = false;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = dialog.FileName;
                SavePolygons(_fileName);
            }
        }

        private void Undo()
        {
            if (_points.Count > 0)
            {
                _points.RemoveAt(_points.Count - 1);
            }
            else if (_polygons.Count > 0)
            {
                _polygons.RemoveAt(_polygons.Count - 1);
            }

            PolygonsCount.Text = $"Polygons: {_polygons.Count}";

            _canvas.Invalidate();
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SavePolygonsAs();
        }

        private void SaveBtn_Click_1(object sender, EventArgs e)
        {
            SavePolygons(_fileName);
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = dialog.FileName;
                var json = File.ReadAllText(_fileName);
                _polygons = JsonConvert.DeserializeObject<List<Polygon>>(json);
                
                _canvas.Invalidate();
            }
        }

        private void WithDeltaArea_CheckStateChanged(object sender, EventArgs e)
        {
            _canvas.Invalidate();
        }

        private void UnionBtn_Click(object sender, EventArgs e)
        {
            double minDistance;
            if (_polygons.Count < 2 || !double.TryParse(MinDistanceTextBox.Text, out minDistance))
                return;

            var union = _clipper.Union(new PolygonGeneralization.Domain.Models.Polygon(
                new Path(_polygons[0].Points
                .Select(it => new PolygonGeneralization.Domain.Models.Point(it.X, it.Y)).ToArray())),
                new PolygonGeneralization.Domain.Models.Polygon(
                new Path(_polygons[1].Points
                .Select(it => new PolygonGeneralization.Domain.Models.Point(it.X, it.Y)).ToArray())), minDistance);


            _union = union.Select(it =>
                    new Polygon(it.Paths[0].Points.Select(p => new Point((int) p.X, (int) p.Y)).ToArray())).ToList();

            _canvas.Invalidate();
        }

        private void DrawPolygons(IEnumerable<Polygon> polygons, Graphics graphics, Color color)
        {
            var pen = new Pen(color);
            foreach (var polygon in polygons)
            {
                pen.Width = 3;
                if (!WithDeltaArea.Checked)
                {
                    graphics.DrawPolygon(pen, polygon.Points.ToArray());
                }
                else
                {
                    double minDistance;
                    if (double.TryParse(MinDistanceTextBox.Text, out minDistance))
                    {
                        var massCenter = polygon.GetMassCenter();
                        var modifiedPoint = _vectorGeometry
                            .IncreaseContour(polygon.Points
                                    .Select(it => new PolygonGeneralization.Domain.Models.Point(it.X, it.Y)).ToArray(),
                                new PolygonGeneralization.Domain.Models.Point(massCenter.X, massCenter.Y), minDistance / 2);

                        graphics.DrawPolygon(pen, modifiedPoint.Select(it => new Point((int)it.X, (int)it.Y))
                            .ToArray());
                    }
                }
            }
        }

        private void DrawSourceBtn_Click(object sender, EventArgs e)
        {
            _union.Clear();
            _canvas.Invalidate();
        }
    }
}
