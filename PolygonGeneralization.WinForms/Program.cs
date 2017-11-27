using System;
using System.Windows.Forms;
using Ninject;
using PolygonGeneralization.Domain.Interfaces;
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

            Application.ThreadException += (sender, args) =>
            {
                var result = DialogResult.Cancel;

                MessageBox.Show("Opps", args.Exception.ToString(),
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

                if (result == DialogResult.Abort)
                {
                    Application.Exit();
                }
            };

            _kernel = NinjectConfiguration.GetKernel();

            var mainForm = _kernel.Get<MainForm>();

            Application.Run(mainForm);
        }
    }
}
