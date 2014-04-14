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

        
    }
}
