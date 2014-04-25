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
    /// Lógica de interacción para VisorRamas.xaml
    /// </summary>
    public partial class VisorRamas : UserControl
    {
        public VisorRamas()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlCommit c = null;
            for (int i = 0; i < 10; i++)
            {
                c = new ctrlCommit();
                Componentes.Children.Add(c);
            }
        }
    }
}
