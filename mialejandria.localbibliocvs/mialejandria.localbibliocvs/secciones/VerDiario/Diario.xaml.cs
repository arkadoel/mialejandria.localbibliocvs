using LibGit2Sharp;
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

namespace mialejandria.localbibliocvs.secciones.VerDiario
{
    /// <summary>
    /// Lógica de interacción para Diario.xaml
    /// </summary>
    public partial class Diario : UserControl
    {
        private Controles.ctrlCommit cSeleccionado { get; set; }

        public Diario()
        {
            InitializeComponent();
            this.Loaded += Diario_Loaded;
        }

        void Diario_Loaded(object sender, RoutedEventArgs e)
        {
            CargarRama();
        }

        private void CargarRama()
        {
            this.visorRamas.Componentes.Children.Clear();
            List<core.git.Repositorio.CommitShortInfo> lista = core.GestionConf.Repositorios.First().listarCommitsRama("master");
            Controles.ctrlCommit c = null;

            foreach (var commit in lista)
            {
                c = new Controles.ctrlCommit(commit);
                c.MouseDoubleClick += lblFecha_MouseDoubleClick;

                this.visorRamas.Componentes.Children.Add(c);

            }
        }

        void lblFecha_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controles.ctrlCommit c = sender as Controles.ctrlCommit;

            CargarArchivosEnCombo(c);
            cSeleccionado = c;
        }

        private void CargarArchivosEnCombo(Controles.ctrlCommit c)
        {
            var cambios = core.GestionConf.Repositorios.First().ObtenerCambiosConPadre(c.CommitInfo.ID);

            cmbArchivos.Items.Clear();

            foreach (var cambio in cambios)
            {
                cmbArchivos.Items.Add(cambio.Path);
            }
        }

        private void detectarColoreado(string cambio)
        {
            List<int> lineasVerdes = new List<int>();
            List<int> lineasRojas = new List<int>();

            string strCambios = cambio;
            int numlinea = 0;
            String linea = "";
            bool paraEscritura = false;


            for (int i = 0; i < strCambios.Length - 1; i++)
            {
                char c = strCambios[i];

                if (c.ToString() == "@" && strCambios[i + 1].ToString() == "@")
                {
                    paraEscritura = !paraEscritura;
                }

                if (((int)c) != 10)
                {
                    if (paraEscritura == false)
                    {
                        linea += c.ToString();
                    }
                }
                else
                {
                    if (linea[0].ToString() == "-")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        lineasRojas.Add(numlinea);
                    }
                    if (linea[0].ToString() == "+")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        lineasVerdes.Add(numlinea);
                    }

                    linea = linea.Replace("@@", "");
                    Console.WriteLine(linea);
                    linea = "";
                    numlinea++;
                    // Console.ForegroundColor = normal;
                }
            }
            //txt.SelectAll();
            numlinea++;

            txt.CaretPosition = txt.CaretPosition.DocumentStart;


            for (int nlinea = 0; nlinea < numlinea; nlinea++)
            {
                txt.LineDown();
                if (lineasVerdes.Contains(nlinea))
                {
                    colorearLinea(nlinea, Colors.Green);
                }
                else if (lineasRojas.Contains(nlinea))
                {
                    colorearLinea(nlinea, Colors.Red);
                }
                else colorearLinea(nlinea, Colors.Gray);
            }

            /*foreach (int n in lineasRojas)
            {
                colorearRojaLinea(n);
            }*/
        }


        public void colorearLinea(int LineNumber, Color color)
        {
            try
            {

                TextPointer start = txt.CaretPosition.GetLineStartPosition(LineNumber);
                TextPointer stop = txt.CaretPosition.GetLineStartPosition(LineNumber + 1);
                TextRange textrange = new TextRange(start, stop);
                textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            }
            catch { }
        }

        private void btnVer_Click(object sender, RoutedEventArgs e)
        {
            if (cSeleccionado != null)
            {
                var cambios = core.GestionConf.Repositorios.First().ObtenerDifCodigoConPadre(cSeleccionado.CommitInfo.ID);


                txt.SelectAll();
                txt.Selection.Text = cambios[cmbArchivos.Text].Patch.ToString();

                detectarColoreado(cambios.First().Patch);
            }
        }
    }
}
