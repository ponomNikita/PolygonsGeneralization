using System;
using System.Drawing;
using System.Windows.Forms;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.LinearGeneralizer;
using PolygonGeneralization.Infrastructure.Logger;
using PolygonGeneralization.WinForms.Controls;
using PolygonGeneralization.WinForms.ViewModels;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly MainFormViewModel _viewModel;
        private BindingSource _metaBinding;
        private TextBox _metaControl;
        private ScrollTimer _scrollTimer;

        public MainForm(IGisDataReader dataReader, 
            IDbService dbService, 
            ILogger logger, 
            IGeneralizer generalizer, 
            GeneralizerOptions generalizerOptions,
            ILinearGeneralizer linearGeneralizer)
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel(Canvas, dataReader, dbService, logger, 
                generalizer, generalizerOptions, linearGeneralizer);

            InitializeMetaSection();

            InitializeMenu();

            _viewModel.MapsUpdatedEvent += MapsUpdatedEventHandler;

            _scrollTimer = new ScrollTimer();
            _scrollTimer.ScrollEvent += _scrollTimer_ScrollEvent;
        }

        private void InitializeMenu()
        {
            var importFromFileToolStrip = new ToolStripMenuItem();
            var openFromFileToolStrip = new ToolStripMenuItem();
            var exitToolStrip = new ToolStripMenuItem();

            fileToolStripMenuItem.DropDownItems.Clear();
            fileToolStripMenuItem.DropDownItems.Add(importFromFileToolStrip);
            fileToolStripMenuItem.DropDownItems.Add(openFromFileToolStrip);

            if (_viewModel != null)
            {
                var maps = _viewModel.GetAvailableMaps();
                foreach (var map in maps)
                {
                    var openMapToolStrip = new ToolStripMenuItemWithId() {Id = map.Key};
                    openMapToolStrip.Size = new Size(103, 22);
                    openMapToolStrip.Text = $"Open \"{map.Value}\"";
                    openMapToolStrip.Click += (sender, args) =>
                    {
                        var item = sender as ToolStripMenuItemWithId;
                        if (item != null)
                        {
                            _viewModel.OpenMap(item.Id);
                        }
                    };

                    fileToolStripMenuItem.DropDownItems.Add(openMapToolStrip);
                }
            }

            fileToolStripMenuItem.DropDownItems.Add(exitToolStrip);

            importFromFileToolStrip.Name = "importFromFileToolStripMenuItem";
            importFromFileToolStrip.Size = new Size(103, 22);
            importFromFileToolStrip.Text = "Import from file...";
            importFromFileToolStrip.Click += openToolStripMenuItem_Click;

            openFromFileToolStrip.Name = "openFromFileToolStripMenuItem";
            openFromFileToolStrip.Size = new Size(103, 22);
            openFromFileToolStrip.Text = "Open from file...";
            openFromFileToolStrip.Click += OpenFromFileToolStrip_Click; ;

            exitToolStrip.Name = "exitToolStripMenuItem";
            exitToolStrip.Size = new System.Drawing.Size(103, 22);
            exitToolStrip.Text = "Exit";
            exitToolStrip.Click += exitToolStripMenuItem_Click;
        }

        public sealed override Rectangle DisplayRectangle => base.DisplayRectangle;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!_viewModel.IsMapLoaded)
                return;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    _viewModel.MoveUp();
                    break;
                case Keys.Down:
                    _viewModel.ModeDown();
                    break;
                case Keys.Left:
                    _viewModel.MoveLeft();
                    break;
                case Keys.Right:
                    _viewModel.MoveRight();
                    break;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            _scrollTimer.Reset(e.Delta > 0 ? 1 : -1);
        }

        private void _scrollTimer_ScrollEvent(object sender, System.EventArgs e)
        {
            _viewModel.Scroll(_scrollTimer.Result);
        }

        private void InitializeMetaSection()
        {
            _metaControl = new TextBox();
            _metaControl.AutoSize = true;
            _metaControl.Height = Meta.Height;
            _metaControl.Width = Meta.Width;
            _metaControl.TextChanged += MetaTextChangedEventHandler;
            _metaControl.ReadOnly = true;
            _metaControl.WordWrap = true;
            _metaControl.Multiline = true;
            _metaControl.ScrollBars = ScrollBars.Vertical;
            _metaControl.HideSelection = true;

            _metaBinding = new BindingSource();
            _metaBinding.DataSource = _viewModel;
            _metaControl.DataBindings.Add("Text", _metaBinding, "Meta");

            Meta.Controls.Add(_metaControl);

            _viewModel.AddHandler(MetaChangedHandler);
        }

        private void MetaTextChangedEventHandler(object sender, System.EventArgs e)
        {
            MethodInvoker inv = delegate
            {
                _metaControl.SelectionStart = _metaControl.TextLength;
                _metaControl.ScrollToCaret();
            };

            Invoke(inv);
        }

        private void MetaChangedHandler(object sender, System.EventArgs e)
        {
            MethodInvoker inv = delegate
            {
                _metaBinding.ResetBindings(false);
            };

            Invoke(inv);
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _viewModel.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _viewModel.ImportMap();
        }

        private void OpenFromFileToolStrip_Click(object sender, System.EventArgs e)
        {
            _viewModel.OpenMap();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            _viewModel.Paint(e);
        }

        private void MapsUpdatedEventHandler(object sender, System.EventArgs e)
        {
            MethodInvoker inv = InitializeMenu;

            Invoke(inv);
        }

        private void GeneralizeBtn_Click(object sender, System.EventArgs e)
        {
            double minDistance = 0;
            if (double.TryParse(MinDistanceTextBox.Text, out minDistance) &&
                Math.Abs(minDistance) > double.Epsilon)
            {
                _viewModel.DrawMap(minDistance);
            }
        }

        private void DrawSourceBtn_Click(object sender, EventArgs e)
        {
            _viewModel.DrawSource();
        }
    }
}
