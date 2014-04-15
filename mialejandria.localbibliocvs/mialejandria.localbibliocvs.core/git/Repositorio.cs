using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace mialejandria.localbibliocvs.core.git
{
    public class Repositorio
    {
        public Repository Repo { get; set; }

        public Repositorio() { }

        public Repositorio(string ruta)
        {
            Repo = new Repository(ruta);

        }

        public TreeChanges VerCambios(string commitID1, string commitID2)
        {
           // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(commitID1).Tree;
            Tree t2 = Repo.Lookup<Commit>(commitID2).Tree;

            var changes = Repo.Diff.Compare<TreeChanges>(t1, t2);
            var stats = Repo.Diff.Compare<PatchStats>(t1, t2);
            return changes;
        }

        public PatchStats VerEstadisticaCambios(string commitID1, string commitID2)
        {
            // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(commitID1).Tree;
            Tree t2 = Repo.Lookup<Commit>(commitID2).Tree;

            var stats = Repo.Diff.Compare<PatchStats>(t1, t2);
            
            return stats;
        }
        
    }
}
