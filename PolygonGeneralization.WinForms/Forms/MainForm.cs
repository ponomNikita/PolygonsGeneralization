using System.Drawing;
using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Logger;
using PolygonGeneralization.WinForms.ViewModels;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly MainFormViewModel _viewModel;
        private BindingSource _metaBinding;
        private TextBox _metaControl;

        public MainForm(IGisDataReader dataReader, IDbService dbService, ILogger logger)
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel(Canvas, dataReader, dbService, logger);

            InitializeMetaSection();
        }

        public sealed override Rectangle DisplayRectangle => base.DisplayRectangle;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

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
            _viewModel.OpenFile();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            _viewModel.Paint(e);
        }
        
    }
}
