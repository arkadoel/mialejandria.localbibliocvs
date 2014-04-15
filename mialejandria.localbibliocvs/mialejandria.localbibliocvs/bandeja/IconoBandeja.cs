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
                App.mainWindow.Show();
            }
            void mnuBlog_Click(object sender, EventArgs e)
            {
                App.mainWindow.Show();
                secciones.blog b = new secciones.blog();

                System.Windows.Window win = new System.Windows.Window();
                win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                win.Height = 500;
                win.Width = 700;
                win.Title = "Blog Descubriendo Mi Alejandria";
                win.Content = new System.Windows.Controls.Grid();
                (win.Content as System.Windows.Controls.Grid).Children.Add(b);
                var u = new UriBuilder("www.mialejandria.blogspot.com?m=1");
                b.navegador.Navigate(u.Uri, UriKind.RelativeOrAbsolute);
                win.Show();
                

            }
        #endregion

    }
}
