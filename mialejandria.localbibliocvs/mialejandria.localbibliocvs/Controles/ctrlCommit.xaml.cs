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

namespace mialejandria.localbibliocvs.Controles
{
    /// <summary>
    /// Lógica de interacción para ctrlCommit.xaml
    /// </summary>
    public partial class ctrlCommit : UserControl
    {
        public core.git.Repositorio.CommitShortInfo CommitInfo { get; set; }
        public ctrlCommit(core.git.Repositorio.CommitShortInfo _c)
        {
            InitializeComponent();
            CommitInfo = _c;
            this.Loaded += ctrlCommit_Loaded;
            this.BotonCommit.MouseLeftButtonDown += BotonCommit_MouseLeftButtonDown;
            this.lblFecha.MouseLeftButtonDown += BotonCommit_MouseLeftButtonDown;
        }

        void ctrlCommit_Loaded(object sender, RoutedEventArgs e)
        {
            fondoDescrip.Visibility = System.Windows.Visibility.Hidden;
            txtDescripcion.Visibility = System.Windows.Visibility.Hidden;
            PonerDatos();
        }

        void BotonCommit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VerOcultarDescripcion();
        }

        private void VerOcultarDescripcion()
        {
            if (fondoDescrip.Visibility == System.Windows.Visibility.Visible)
            {
                fondoDescrip.Visibility = System.Windows.Visibility.Hidden;
                txtDescripcion.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                fondoDescrip.Visibility = System.Windows.Visibility.Visible;
                txtDescripcion.Visibility = System.Windows.Visibility.Visible;
                PonerDatos();
            }
        }

        private void PonerDatos()
        {
            if (CommitInfo != null)
            {
                txtDescripcion.Text = CommitInfo.Mensaje;
                lblFecha.Content = CommitInfo.Fecha.ToShortDateString() + " " + CommitInfo.Fecha.ToShortTimeString();
            }
        }
    }
}
