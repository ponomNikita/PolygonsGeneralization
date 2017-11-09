using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.WinForms.ViewModels;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly MainFormViewModel _viewModel;
        public MainForm(IGisDataReader dataReader)
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel(Canvas, dataReader);
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _viewModel.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _viewModel.OpenFile();
        }
    }
}
