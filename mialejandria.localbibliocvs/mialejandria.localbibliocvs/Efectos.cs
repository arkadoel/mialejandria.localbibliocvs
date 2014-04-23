using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace mialejandria.localbibliocvs
{
    public class Efectos
    {
        public static DropShadowEffect getSombra1()
        {
            DropShadowEffect sombra = new DropShadowEffect();
            sombra.RenderingBias = RenderingBias.Performance;
            sombra.Direction = -90;
            sombra.BlurRadius = 10;
            sombra.ShadowDepth = 10;
            return sombra;
        }

        public static void AbrirPanel(UserControl control, int min, int max)
        {
            for (int i = min; i <= max; i +=3)
            {
                control.Height = i;                
                //core.GestionConf.DoEvents(control.Dispatcher);
                core.GestionConf.DoEvents(App.mainWindow.Dispatcher);
                //System.Threading.Thread.Sleep(1);
                
            }
            control.Height = max;
        }

        public static void CerrarPanel(UserControl control, int min, int max)
        {
            double tActual = control.Height;
            for (int i = max; i >= min; i -= 3)
            {
                control.Height = i;
                //core.GestionConf.DoEvents(control.Dispatcher);
                core.GestionConf.DoEvents(App.mainWindow.Dispatcher);
                //System.Threading.Thread.Sleep(1);

            }
            control.Height = min;
        }

        public static void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.Foreground = Brushes.Blue;
        }

        public static void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.Foreground = Brushes.Black;
        }
    }
}
