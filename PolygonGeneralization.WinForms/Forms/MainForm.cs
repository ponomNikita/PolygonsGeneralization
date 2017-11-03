using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(IGisDataReader dataReader)
        {
            var polygons = dataReader.ReadFromFile("Building_Footprint_2016.shp");
            InitializeComponent();
        }
    }
}
