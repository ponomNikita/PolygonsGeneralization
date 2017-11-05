using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.WinForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(IGisDataReader dataReader)
        {
            //var polygons = dataReader.ReadFromFile("nizhniy-novgorod_russia_osm_polygon.geojson");
            InitializeComponent();
        }
    }
}
