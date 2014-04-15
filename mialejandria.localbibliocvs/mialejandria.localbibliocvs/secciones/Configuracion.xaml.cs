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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mialejandria.localbibliocvs.secciones
{
    /// <summary>
    /// Lógica de interacción para Configuracion.xaml
    /// </summary>
    public partial class Configuracion : UserControl
    {
        public Configuracion()
        {
            InitializeComponent();
            this.Loaded += Configuracion_Loaded;
        }

        void Configuracion_Loaded(object sender, RoutedEventArgs e)
        {
            txtNombre.Text = core.GestionConf.GIT_USER;
            txtEmail.Text = core.GestionConf.GIT_EMAIL;
        }
    }
}
