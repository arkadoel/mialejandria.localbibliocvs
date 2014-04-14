using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace mialejandria.localbibliocvs.core
{
    public class GestionarXML
    {
        public static string ArchivoConfiguracion = @".\DiproloConf.xml";

        /// <summary>
        /// Carga el documento de archivo de configuracion
        /// </summary>
        /// <returns></returns>
        public static XDocument CargaXMLConfiguracion()
        {
            return CargarDocumento(ArchivoConfiguracion);
        }

        /// <summary>
        /// Carga un documento xml para usarlo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XDocument CargarDocumento(string path)
        {
            XDocument doc = XDocument.Load(path);
            return doc;
        }

        /// <summary>
        /// Obtener los repositorios guardados en el 
        /// archivo de configuracion
        /// </summary>
        /// <returns></returns>
        public static List<git.Repositorio> ReposEnConfig()
        {
            XDocument doc = CargaXMLConfiguracion();
            List<git.Repositorio> lista = new List<git.Repositorio>();
            var repos = from u in doc.Elements("Repos").Elements()
                        select u;

            if (repos!=null)
            {
                git.Repositorio repotmp = null;

                foreach (var xml in repos)
                {
                    repotmp = new git.Repositorio(xml.Attribute("path").Value.ToString());
                    lista.Add(repotmp);
                }
            }

            return lista;
        }
    
    }
}
