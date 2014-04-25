using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mialejandria.localbibliocvs.core.git
{
    /// <summary>
    /// Clase encargada de la gestion del repositorio local
    /// y de todas las operaciones, puede que tenga que estar 
    /// en otro hilo para ello
    /// </summary>
    public class BotLocalGit
    {
        public static List<string> ArchivosParaGuardar { get; set; }

        public static void VaciarListado()
        {
            
            git.BotLocalGit.ArchivosParaGuardar = new List<string>();
        }


        /// <summary>
        /// Permite buscar y agregar a la lista de archivos que se guardaran
        /// buscando recursivamente en los subdirectorios
        /// </summary>
        /// <param name="dirOrigen"></param>
        /// <param name="dirDestino"></param>
        /// <param name="recursivo">¿hacer copia recursiva?</param>
        public static void SearchFilesToControl(string dirOrigen, bool recursivo)
        {
            try
            {
                
                DirectoryInfo dir = new DirectoryInfo(dirOrigen);
                DirectoryInfo[] dirs = dir.GetDirectories();

                if (dir.Exists == true)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        //Console.WriteLine("\tAgregado al lista: " + file.Name.ToString());
                        //string temppath = Path.Combine(dirDestino, file.Name);
                        //file.CopyTo(temppath, true);

                        //if (git.Repositorio.extensionesPermitidas(file.Extension, null))
                        {
                            ArchivosParaGuardar.Add(file.FullName);
                            
                        }

                    }

                    //copy subdirs
                    if (recursivo == true)
                    {
                        foreach (DirectoryInfo subdir in dirs)
                        {
                            if (subdir.Name.Contains("AppData") == false && subdir.Name.Contains(".git") == false)
                            {
                                Console.WriteLine("\tRevisando Directorio: " + subdir.FullName.ToString());
                                //string tempPath = Path.Combine(dirDestino, subdir.Name);


                                //copiar recursivamente
                                SearchFilesToControl(subdir.FullName, recursivo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
