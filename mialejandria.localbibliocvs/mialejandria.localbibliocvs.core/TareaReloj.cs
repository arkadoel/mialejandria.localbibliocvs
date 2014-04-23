using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mialejandria.localbibliocvs.core
{
    public class TareaReloj
    {
        public string Nombre { get; set; }
        public string Hora { get; set; }
        public bool lunes { get; set; }
        public bool martes { get; set; }
        public bool miercoles { get; set; }
        public bool jueves { get; set; }
        public bool viernes { get; set; }
        public bool sabado { get; set; }
        public bool domingo { get; set; }

        public TareaReloj()
        {
        }

        public static bool stringToBool(string valor)
        {
            if (valor == "true") return true;
            else return false;
        }
    }
}
