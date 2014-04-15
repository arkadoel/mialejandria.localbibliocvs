using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace mialejandria.localbibliocvs.core
{
    public class GestionConf
    {
        public static List<Tarea> Tareas { get; set; }
        public static List<git.Repositorio> Repositorios { get; set; }
        public static string GIT_USER { get; set; }
        public static string GIT_EMAIL { get; set; }


        /// <summary>
        /// Coloca una ventana en la zona inferior derecha de la pantalla
        /// </summary>
        /// <param name="_win"></param>
        public static void ventanaInferiorDerechaPantalla(Window _win)
        {
            _win.Top = System.Windows.SystemParameters.WorkArea.Height - _win.Height;
            _win.Left = System.Windows.SystemParameters.WorkArea.Width - _win.Width;
        }

        public static void DoEvents(Dispatcher dis)
        {
            dis.Invoke(DispatcherPriority.Background, new Action(delegate
            {
            }));
        }
    }
}
