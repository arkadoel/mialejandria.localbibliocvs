using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mialejandria.localbibliocvs.core
{
    class TareaProgramador
    {
        public string Estado { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Asignada { get; set; }

        public TareaProgramador()
        {
        }

        public static bool stringToBool(string valor)
        {
            if (valor == "true") return true;
            else return false;
        }

    }
}
