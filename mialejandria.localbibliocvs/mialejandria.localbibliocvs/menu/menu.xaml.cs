using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace mialejandria.localbibliocvs.menu
{
    /// <summary>
    /// Lógica de interacción para menu.xaml
    /// </summary>
    public partial class menu : UserControl
    {
        public const int TAM_MAX = 150;
        public const int TAM_MIN = 60;
        private DropShadowEffect sombra { get; set; }
        
        public menu()
        {            
            InitializeComponent();
            lblMenu.MouseLeftButtonDown += lblMenu_MouseLeftButtonDown;
            this.Loaded += menu_Loaded;

            PonerEfectosLabels();
        }

        private void PonerEfectosLabels()
        {
            this.lblMenu.MouseEnter += Efectos.Label_MouseEnter;
            this.lblMenu.MouseLeave += Efectos.Label_MouseLeave;
            this.lblConfiguracion.MouseEnter += Efectos.Label_MouseEnter;
            this.lblConfiguracion.MouseLeave += Efectos.Label_MouseLeave;
            this.lblTareas.MouseEnter += Efectos.Label_MouseEnter;
            this.lblTareas.MouseLeave += Efectos.Label_MouseLeave;
            this.lblBlog.MouseEnter += Efectos.Label_MouseEnter;
            this.lblBlog.MouseLeave += Efectos.Label_MouseLeave;
        }

        void menu_Loaded(object sender, RoutedEventArgs e)
        {
            //Direction="-90" BlurRadius="10" ShadowDepth="10"
            sombra = Efectos.getSombra1();
            fondo.Effect = null;
            this.Height = TAM_MIN;
            
        }

        void lblMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Height == TAM_MIN)
            {
                //this.Height = TAM_MAX;
                fondo.Effect = sombra;
                Efectos.AbrirPanel(this, TAM_MIN, TAM_MAX);
                
            }
            else
                CerrarMenu();
        }

        /// <summary>
        /// Cierra el menu de la ventana principal
        /// </summary>
        public void CerrarMenu()
        {
            if (this.Height > TAM_MIN)
            {
                Efectos.CerrarPanel(this, TAM_MIN, TAM_MAX);
                fondo.Effect = null;
            }
        }

        private void MenuSeleccionado(object sender, MouseButtonEventArgs e)
        {
            string nombre = (sender as Label).Name.ToString().Replace("lbl", "");
            switch (nombre)
            {
                case "Configuracion":
                    IrA_Cofiguracion();
                    break;
                case "Blog":
                    IrA_navegador();
                    break;
                case "Tareas":
                    IrA_Tareas();
                    break;
            }
            CerrarMenu();
        }

        public void IrA_Tareas()
        {
            secciones.GestorTareas c = new secciones.GestorTareas();
            App.mainWindow.navegador.Children.Clear();
            App.mainWindow.navegador.Children.Add(c);
            App.mainWindow.Show();
        }

        /// <summary>
        /// Permite ver la ventana con las noticias del blog
        /// </summary>
        public void IrA_navegador()
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

        /// <summary>
        /// Permite ir y ver la configuracion en la ventana
        /// </summary>
        public void IrA_Cofiguracion()
        {
            secciones.Configuracion c = new secciones.Configuracion();
            App.mainWindow.navegador.Children.Clear();
            App.mainWindow.navegador.Children.Add(c);
            App.mainWindow.Show();
        }
    }
}
