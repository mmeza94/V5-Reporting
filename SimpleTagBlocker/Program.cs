using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SimpleScadaCenexion
{
    class Program
    {
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private extern static int ShowWindow(System.IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            //ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
            Console.WriteLine("inicias");

            ScadaAdquisidor scadaAdq = new ScadaAdquisidor();
            scadaAdq.inicializar();
            //scadaAdq.activar();
            scadaAdq.activarOnliBlock();
            string val = string.Empty;
            bool flag = true;
            while(flag)
            {
                Console.WriteLine("Desea Salir del Sistema S/N");
                val = Console.ReadLine();
                if (val.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                {
                    flag = false;
                    break;
                }
                Console.WriteLine("Desea Bloquear S/N");
                val = Console.ReadLine();
                if (val.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                    scadaAdq.bloquear();
                Console.WriteLine("Desea dESBloquear S/N");
                val = Console.ReadLine();
                if (val.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                    scadaAdq.desbloquear();

            }
            //scadaAdq.desActivar();
            scadaAdq.desActivarOnliBlock();
        }    
    }
}
