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
        public static void CargarReposEnConfig()
        {
            XDocument doc = CargaXMLConfiguracion();
            List<git.Repositorio> lista = new List<git.Repositorio>();
            var repos = from u in doc.Elements("conf").Elements("Repos").Elements()
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

            GestionConf.Repositorios = lista;
            LiberarMemoria(doc);
            
        }

        /// <summary>
        /// Cierra y libera la memoria usada por el gestor de XML
        /// </summary>
        /// <param name="doc"></param>
        private static void LiberarMemoria(XDocument doc)
        {
            doc = null;
            GC.Collect();
        }

        /// <summary>
        /// Carga la lista de tareas a ejecutar por el reloj en gestionConf.Tareas
        /// </summary>
        public static void CargarTareasEnConfig()
        {
            XDocument doc = CargaXMLConfiguracion();
            List<Tarea> lista = new List<Tarea>();
            var tars = from u in doc.Elements("conf").Elements("Tareas").Elements()
                        select u;

            if (tars != null)
            {
                Tarea t = null;
                foreach (var elemento in tars)
                {
                    t = new Tarea();
                    t.Nombre = elemento.Attribute("nombre").Value.ToString();
                    t.Hora = elemento.Attribute("hora").Value.ToString();
                    t.lunes = Tarea.stringToBool(elemento.Attribute("lunes").Value.ToString());
                    t.martes = Tarea.stringToBool(elemento.Attribute("martes").Value.ToString());
                    t.miercoles = Tarea.stringToBool(elemento.Attribute("miercoles").Value.ToString());
                    t.jueves = Tarea.stringToBool(elemento.Attribute("jueves").Value.ToString());
                    t.viernes = Tarea.stringToBool(elemento.Attribute("viernes").Value.ToString());
                    t.sabado = Tarea.stringToBool(elemento.Attribute("sabado").Value.ToString());
                    t.domingo = Tarea.stringToBool(elemento.Attribute("domingo").Value.ToString());

                    lista.Add(t);
                }
            }

            GestionConf.Tareas = lista;
            LiberarMemoria(doc);
        }

        /// <summary>
        /// Carga los datos del usuario para git del archivo de configuracion
        /// </summary>
        public static void CargarUsuarioGit()
        {
            XDocument doc = CargaXMLConfiguracion();
            List<Tarea> lista = new List<Tarea>();
            string nombre = doc.Elements("conf").Elements("Git").First().Attribute("userName").Value.ToString();
            string email = doc.Elements("conf").Elements("Git").First().Attribute("userEmail").Value.ToString();

            GestionConf.GIT_USER = nombre;
            GestionConf.GIT_EMAIL = email;
            LiberarMemoria(doc);
        }
    }
}
