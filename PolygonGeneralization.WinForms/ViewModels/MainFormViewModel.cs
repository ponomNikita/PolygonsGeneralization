using System.Windows.Forms;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Properties;

namespace PolygonGeneralization.WinForms.ViewModels
{
    public class MainFormViewModel
    {
        private Panel _canvas;
        private string _mapFileName;
        private readonly IGisDataReader _dataReader;
        private Polygon[] _polygons;

        public MainFormViewModel(Panel canvas, IGisDataReader dataReader)
        {
            _canvas = canvas;
            _dataReader = dataReader;
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void OpenFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resources.FileDialogFilter;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _mapFileName = dialog.FileName;
                    _polygons = _dataReader.ReadFromFile(_mapFileName);
                }
            }
        }
    }
}
