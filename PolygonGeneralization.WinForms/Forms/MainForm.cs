using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.WinForms.ViewModels;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly MainFormViewModel _viewModel;
        private BindingSource _metaBinding;

        public MainForm(IGisDataReader dataReader)
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel(Canvas, dataReader);

            InitializeMetaSection();
        }

        private void InitializeMetaSection()
        {
            var metaTextLabel = new Label();
            metaTextLabel.AutoSize = true;
            metaTextLabel.Height = Meta.Height;
            metaTextLabel.Width = Meta.Width;

            _metaBinding = new BindingSource();
            _metaBinding.DataSource = _viewModel;
            metaTextLabel.DataBindings.Add("Text", _metaBinding, "Meta");

            Meta.Controls.Add(metaTextLabel);

            _viewModel.MetaChangedEvent += MetaChangedHandler;
        }

        private void MetaChangedHandler(object sender, System.EventArgs e)
        {
            _metaBinding.ResetBindings(false);
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
