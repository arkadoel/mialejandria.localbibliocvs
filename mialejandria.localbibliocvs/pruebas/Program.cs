using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mialejandria.localbibliocvs.core;
using LibGit2Sharp;

namespace pruebas
{
    class Program
    {
        static mialejandria.localbibliocvs.core.git.Repositorio repo;
            
        static void Main(string[] args)
        {
            Console.Title = "Pruebas de estadisticas";
            
            Console.ForegroundColor = ConsoleColor.White;

            DirectoryInfo d = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent;
            string directorioGIT = d.FullName ;
            Console.WriteLine("Abrierto repositorio");
            repo = new mialejandria.localbibliocvs.core.git.Repositorio(directorioGIT);

            EstadisticasDe("Arkadoel", "master");

            Console.Read();
        }

        private static void EstadisticasDe(string programador, string nombreRama)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n\tEstadisticas de: " + programador);
            Console.ForegroundColor = ConsoleColor.White;

            var rama = (from un in repo.Repo.Branches
                        where un.Name.Contains(nombreRama) == true
                        select un).First();

            var misCommits = from un in rama.Commits
                             where un.Committer.Name.Contains(programador)
                             select un;

            //padre
            Commit c1 = misCommits.Last();
            Console.WriteLine("Primer commit: " + c1.Committer.When.DateTime.ToLongDateString());

            //hijo
            Commit c2 = misCommits.First();
            Console.WriteLine("Ultimo commit: " + c2.Committer.When.DateTime.ToLongDateString());
            var cambios = repo.VerEstadisticaCambios(c1.Id.ToString(), c2.Id.ToString());

            Console.WriteLine("\r\n\tEstadisticas: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tAñadidas: " + cambios.TotalLinesAdded);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\tQuitadas: " + cambios.TotalLinesDeleted);

            //diferencia de dias

            TimeSpan ts = c2.Committer.When.DateTime - c1.Committer.When.DateTime;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tDias transcurridos: " + ts.Days);
            float lineasDia = (float)(cambios.TotalLinesAdded - cambios.TotalLinesDeleted) / ts.Days;
            Console.WriteLine("\tMedia de lineas por dia: " + lineasDia.ToString("F2"));
        }
    }
}
