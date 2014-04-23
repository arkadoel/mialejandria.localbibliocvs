using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace mialejandria.localbibliocvs
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Constantes y variables de la aplicacion

        public const string APP_NAME = "Diario de programacion"; //diario de programacion local
        public const string APP_VERSION = "Alpha 0.2014.4.8";
        public static MainWindow mainWindow { get; set; }
        public static Thread HiloReloj { get; set; }


        public static string NombreConVersion()
        {
            return APP_NAME + " (" + APP_VERSION + ")";
        }

        /// <summary>
        /// Terminar la aplicacion normalmente
        /// </summary>
        public static void CerrarAplicacion()
        {
            HiloReloj.Abort();
            App.Current.Shutdown(0);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            //tareas iniciales
            core.GestionConf.CargarDatosConfiguracion();

            //lanzar icono de la barra de tareas
            bandeja.IconoBandeja icono = new bandeja.IconoBandeja();
            icono.mostrarMensaje(App.NombreConVersion(), "Aplicacion iniciada",10);
            
            //enlace para manejar el hilo del reloj
            Thread hilo = new Thread(new ThreadStart(core.reloj.ejecutarReloj));
            HiloReloj = hilo;
            hilo.Start();

            mainWindow = new MainWindow();
            core.GestionConf.ventanaInferiorDerechaPantalla(mainWindow);
            mainWindow.Show();

        }
    }
}
