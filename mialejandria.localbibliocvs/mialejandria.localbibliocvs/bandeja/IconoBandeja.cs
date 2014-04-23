using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mialejandria.localbibliocvs.bandeja
{
    public class IconoBandeja
    {
        System.Windows.Forms.NotifyIcon icono { get; set; }
        ContextMenu MenuIcono { get; set; }

        public IconoBandeja()
        {
            icono = new System.Windows.Forms.NotifyIcon();
            icono.Visible = true;

            //creacion del menu
            MenuIcono = new ContextMenu();
            MenuItem mnuSalir = new MenuItem("Salir",mnuSalir_Click);
            MenuItem mnuConfiguracion = new MenuItem("Configuracion", mnuConfiguracion_Click);
            MenuItem mnuBlog = new MenuItem("Blog", mnuBlog_Click);
            MenuItem mnuTareas = new MenuItem("Mis Tareas", mnuTareas_Click);

            MenuIcono.MenuItems.Add(mnuTareas);
            MenuIcono.MenuItems.Add(mnuBlog);
            MenuIcono.MenuItems.Add(new MenuItem("-"));
            MenuIcono.MenuItems.Add(mnuConfiguracion);
            MenuIcono.MenuItems.Add(mnuSalir);
            icono.ContextMenu = MenuIcono;

            
            icono.MouseClick += icono_MouseClick;
        }

        

        void icono_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                App.mainWindow.Show();
            }
        }      

        /// <summary>
        /// Muestra un mensaje en la bandeja del sistema
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensaje"></param>
        public void mostrarMensaje(string titulo, string mensaje)
        {            
            icono.Icon = new System.Drawing.Icon(".\\icono.ico");
            icono.ShowBalloonTip(100, titulo, mensaje, ToolTipIcon.Info);
        }

        #region "Eventos de los menus"
            void mnuSalir_Click(object sender, EventArgs e)
            {
                App.CerrarAplicacion();
            }

            void mnuConfiguracion_Click(object sender, EventArgs e)
            {
               App.mainWindow.mnuApp.IrA_Cofiguracion();
            }
            
            void mnuBlog_Click(object sender, EventArgs e)
            {
                App.mainWindow.mnuApp.IrA_navegador();
            }

            void mnuTareas_Click(object sender, EventArgs e)
            {
                App.mainWindow.mnuApp.IrA_Tareas();
            }
        #endregion

    }
}
