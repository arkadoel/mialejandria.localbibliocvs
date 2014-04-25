using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mialejandria.localbibliocvs.pkgManager
{

    public class Paquete
    {
        #region "Definicion objetos paquete"
        public class PackageInfo
        {
            public string Nombre { get; set; }
            public string Version { get; set; }
            public string UpdatesURL { get; set; }
        }


        #endregion
        
        //Propiedades
        public PackageInfo InfoPaquete { get; set; }
        public string InstallFirst { get; set; }
        public string Installer { get; set; }
        public string InstallLast { get; set; }
    }
}
