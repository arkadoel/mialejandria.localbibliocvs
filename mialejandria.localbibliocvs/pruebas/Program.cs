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
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent;
            string directorioGIT = d.FullName ;
            mialejandria.localbibliocvs.core.git.Repositorio repo = new mialejandria.localbibliocvs.core.git.Repositorio(directorioGIT);
            Console.WriteLine("Abrierto repositorio");

            var rama = (from un in repo.Repo.Branches
                       where un.Name.Contains("master") == true
                       select un).First();
            
            //padre
            Commit c1 =rama.Commits.Last();
            Console.WriteLine("Last commit: " + c1.Committer.When.ToString());
            
            //hijo
            Commit c2 = rama.Commits.First();
            Console.WriteLine("First commit: " + c2.Committer.When.ToString());
            var cambios = repo.VerEstadisticaCambios(c1.Id.ToString(), c2.Id.ToString());

            
            Console.WriteLine("Añadidas: " + cambios.TotalLinesAdded);
            Console.WriteLine("Quitadas: " + cambios.TotalLinesDeleted);
            Console.Read();
        }
    }
}
