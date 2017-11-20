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
        private Label _metaTextLabel;

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
            _metaTextLabel = new Label();
            _metaTextLabel.AutoSize = true;
            _metaTextLabel.Height = Meta.Height;
            _metaTextLabel.Width = Meta.Width;

            _metaBinding = new BindingSource();
            _metaBinding.DataSource = _viewModel;
            _metaTextLabel.DataBindings.Add("Text", _metaBinding, "Meta");

            Meta.Controls.Add(_metaTextLabel);

            _viewModel.AddHandler(MetaChangedHandler);
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
