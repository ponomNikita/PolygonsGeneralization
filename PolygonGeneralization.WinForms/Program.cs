using System;
using System.Windows.Forms;
using Ninject;
using PolygonGeneralization.WinForms.Forms;
using PolygonGeneralization.WinForms.Ninject;

namespace PolygonGeneralization.WinForms
{
    static class Program
    {
        static KernelBase _kernel;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _kernel = NinjectConfiguration.GetKernel();

            var mainForm = _kernel.Get<MainForm>();

            Application.Run(mainForm);
        }
    }
}
