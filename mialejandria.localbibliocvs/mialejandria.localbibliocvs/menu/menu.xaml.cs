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
        public const int TAM_MAX = 300;
        public const int TAM_MIN = 60;
        private DropShadowEffect sombra { get; set; }
        
        public menu()
        {            
            InitializeComponent();
            lblMenu.MouseLeftButtonDown += lblMenu_MouseLeftButtonDown;
            this.Loaded += menu_Loaded;
        }

        void menu_Loaded(object sender, RoutedEventArgs e)
        {
            //Direction="-90" BlurRadius="10" ShadowDepth="10"
            sombra = new DropShadowEffect();
            sombra.RenderingBias = RenderingBias.Performance;
            sombra.Direction =-90;
            sombra.BlurRadius = 10;
            sombra.ShadowDepth = 10;
            fondo.Effect = null;
            this.Height = TAM_MIN;
            
        }

        void lblMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Height == TAM_MIN)
            {
                this.Height = TAM_MAX;
                fondo.Effect = sombra;
            }
            else
            {
                fondo.Effect = null;
                this.Height = TAM_MIN;
                
            }
        }
    }
}
