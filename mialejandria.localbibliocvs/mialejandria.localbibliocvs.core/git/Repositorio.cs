using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;
using System.IO;

namespace mialejandria.localbibliocvs.core.git
{
    public class Repositorio
    {
        public string Path { get; set; }

        public Repository Repo { get; set; }

        public Repositorio() { }

        public Repositorio(string ruta)
        {
            Repo = new Repository(ruta);
            Path = ruta;
        }

        /// <summary>
        /// Iniciar el repositorio
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="forzar">Crear repositorio si no lo es todavia</param>
        public Repositorio(string ruta, bool forzar)
        {
            if (Directory.Exists(ruta + @"\.git"))
            {
                Repo = new Repository(ruta);
            }
            else if (forzar==true)
            {
                Repo = new Repository(Repository.Init(ruta, false));
            }
            Path = ruta;
        }

        /// <summary>
        /// Permite comparar dos commits para detectar los cambios entre ambos
        /// </summary>
        /// <param name="commitID1"></param>
        /// <param name="commitID2"></param>
        /// <returns></returns>
        public TreeChanges VerCambios(string commitID1, string commitID2)
        {
           // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(commitID1).Tree;
            Tree t2 = Repo.Lookup<Commit>(commitID2).Tree;

            var changes = Repo.Diff.Compare<TreeChanges>(t1, t2);
            var stats = Repo.Diff.Compare<PatchStats>(t1, t2);
            return changes;
        }

        /// <summary>
        /// Se obtienen las estadisticas de la diferencia entre dos commits
        /// </summary>
        /// <param name="commitID1"></param>
        /// <param name="commitID2"></param>
        /// <returns></returns>
        public PatchStats VerEstadisticaCambios(string commitID1, string commitID2)
        {
            // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(commitID1).Tree;
            Tree t2 = Repo.Lookup<Commit>(commitID2).Tree;

            var stats = Repo.Diff.Compare<PatchStats>(t1, t2);
            
            return stats;
        }

        /// <summary>
        /// Comprueba si el fichero esta entre las extensiones aceptadas
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="extra">Lista de extensiones a añadir a la busqueda</param>
        /// <returns></returns>
        public static Boolean extensionesPermitidas(string ruta, List<string> extra)
        {
            Boolean encontrado = false;

            if (string.IsNullOrWhiteSpace(ruta) == false)
            {
                System.IO.FileInfo fich = new FileInfo(ruta);
                string ext = fich.Extension;
                

                List<string> extensiones = new List<string>(){
                    ".cs",".java", ".c", ".h", ".xml"
                };

                if (extra != null)
                {
                    extensiones.AddRange(extra);
                }

                for (int i = 0; i < extensiones.Count() - 1 && !encontrado; i++)
                {
                    if (ext == (extensiones[i]))
                    {
                        encontrado = true;
                    }
                }
            }

            return encontrado;
        }

        public Boolean esRepositorioIniciado()
        {
            if (Directory.Exists(Path + @"\.git"))
            {
                return true;
            }
            else return false;
        }

        public Commit getCommitByID(string id)
        {
            //feed.Logs.WriteText("Get commit by ID", "Se obtiene un commit de " + NombreProyecto);
            return Repo.Lookup<Commit>(id);
        }

        /// <summary>
        /// Muestra el estado del indice del control de versiones
        /// </summary>
        /// <returns></returns>
        public RepositoryStatus getStatus()
        {
            //feed.Logs.WriteText("Get status", "Se obtiene el status de " + NombreProyecto);
            if (esRepositorioIniciado())
            {
                Repo = new Repository(Path);
                return Repo.Index.RetrieveStatus();
            }
            else return null;
        }

        /// <summary>
        /// Añade todos los archivos a diestro y siniestro
        /// </summary>
        public void git_stage_all()
        {
            RepositoryStatus status = getStatus();
            if (status.Modified.Count() > 0 || status.Untracked.Count() > 0)
            {
                foreach (var archivo in status)
                {
                    try
                    {
                        if (extensionesPermitidas(archivo.FilePath, null))
                        {
                            Repo.Index.Stage(archivo.FilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //feed.Logs.WriteError("Error en git_stage_all", ex);
                    } //ignorar errores y seguir

                }
            }
        }

        /// <summary>
        /// Hace un commit con los datos del usuario actual
        /// </summary>
        /// <param name="message"></param>
        public void git_autoCommit()
        {
            //feed.Logs.WriteText("AutoCommit", "Se hace un commit automatico");

            string fecha = "Guardado automatico día: " +
                DateTime.Today.ToShortDateString() + " " +
                DateTime.Now.ToLongTimeString();
            git_commit(GestionConf.GIT_USER, GestionConf.GIT_EMAIL, fecha);
        }

        /// <summary>
        /// Permite hacer un guardado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        public void git_commit(string name, string email, string message)
        {
            //feed.Logs.WriteText("Commit", "Se hace commit normal de " + this.NombreProyecto);
            //repo.Index.Remove(estado.FilePath,false); 
            Commit newC = null;
            RepositoryStatus status = getStatus();
            ExplicitPathsOptions explicitOptions = new ExplicitPathsOptions();
            

            if (status.Modified.Count() > 0)
            {
                foreach (var archivo in status.Modified)
                {
                    if (extensionesPermitidas(archivo.FilePath, null))
                    {
                        Repo.Index.Stage(archivo.FilePath);
                    }
                }
            }

            if (status.Modified.Count() > 0 || status.Staged.Count() > 0 ||
                status.Removed.Count() > 0 || status.Added.Count() > 0 || status.Missing.Count() > 0)
            {
                CommitOptions options = new CommitOptions();

                Signature signature = new Signature(name, email, DateTimeOffset.Now);
                try
                {
                    
                    options.AmendPreviousCommit = false;

                    newC = Repo.Commit(message,signature,signature, options);
                   
                    // /*Deprecated*/ newC = Repo.Commit(message, signature, signature, false);
                    // newC.Parents
                }
                catch (Exception ex)
                {
                    options.AmendPreviousCommit = true;
                    newC = Repo.Commit(message, signature, signature, options);
                    //newC = Repo.Commit(message, signature, signature, true);
                }
            }

        }
        
        /// <summary>
        /// Meter en el indice un archivo
        /// </summary>
        /// <param name="filePath"></param>
        public void git_trackFile(string filePath)
        {
            try
            {
                Repo.Index.Stage(filePath);
            }
            catch { }
        }

    }
}
