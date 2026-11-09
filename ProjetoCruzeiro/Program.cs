
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            SaveManager.Inicializar();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Cruzeiro());
        }
    }
}
