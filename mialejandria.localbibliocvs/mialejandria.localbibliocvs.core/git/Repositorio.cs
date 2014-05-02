using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;
using System.IO;
using System.Collections;

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
        /// Elemento resumido de como es un commit
        /// </summary>
        public class CommitShortInfo
        {
            public string ID { get; set; }
            public string Autor { get; set; }
            public DateTime Fecha { get; set; }
            public string RepoPath { get; set; }
            public string Mensaje { get; set; }
        }

        /// <summary>
        /// Transforma un commit normal en commitShortInfo
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static CommitShortInfo toCommitShortInfo(Commit c)
        {
            if (c != null)
            {
                CommitShortInfo csi = new CommitShortInfo();
                csi.ID = c.Id.Sha;
                csi.Autor = c.Author.Name;
                csi.Fecha = c.Committer.When.DateTime;
                csi.Mensaje = c.MessageShort;
                return csi;
            }
            else return null;
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
        public TreeChanges VerCambios(string cPadre, string cHijo)
        {
           // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(cPadre).Tree;
            Tree t2 = Repo.Lookup<Commit>(cHijo).Tree;

            var changes = Repo.Diff.Compare<TreeChanges>(t1, t2);           
            
            return changes;
        }

        /// <summary>
        /// Se obtienen las estadisticas de la diferencia entre dos commits
        /// </summary>
        /// <param name="commitID1"></param>
        /// <param name="commitID2"></param>
        /// <returns></returns>
        public PatchStats VerEstadisticaCambios(string cPadre, string cHijo)
        {
            // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
            Tree t1 = Repo.Lookup<Commit>(cPadre).Tree;
            Tree t2 = Repo.Lookup<Commit>(cHijo).Tree;

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
                    ".cs",".java",".csproj",".htm",".html", 
                    ".c", ".h", ".xml",".sln", ".suo",".config", ".xaml"
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

        public Blob getBlobByID(string id)
        {
            Commit c = getCommitByID(id);

            return null;
        }

        /// <summary>
        /// Obtiene los cambios entre un commit hijo y un commit padre 
        /// solo pasando el commit actual
        /// </summary>
        /// <param name="commitId"></param>
        /// <returns></returns>
        public TreeChanges ObtenerCambiosConPadre(string commitId)
        {
            Commit chijo = getCommitByID(commitId);
            Commit cpadre = null;

            if (chijo != null)
            {
                if (chijo.Parents.Count() > 0)
                {
                    cpadre = chijo.Parents.First();
                }
            }

            if (cpadre != null)
            {
                return VerCambios(cpadre.Id.Sha, chijo.Id.Sha);
            }
            else return null;
        }

        /// <summary>
        /// Obtiene los cambios entre un commit hijo y un commit padre 
        /// solo pasando el commit actual
        /// </summary>
        /// <param name="commitId"></param>
        /// <returns></returns>
        public Patch ObtenerDifCodigoConPadre(string commitId)
        {
            Commit chijo = getCommitByID(commitId);
            Commit cpadre = null;

            if (chijo != null)
            {
                if (chijo.Parents != null)
                {
                    if (chijo.Parents.Count() >= 0)
                    {
                        cpadre = chijo.Parents.First();
                    }
                }
            }

            if (cpadre != null)
            {
                // feed.Logs.WriteText("Ver cambios", "Se desea ver los cambios del proyecto " + NombreProyecto);
                Tree t1 = cpadre.Tree;
                Tree t2 = chijo.Tree;

                var stats = Repo.Diff.Compare<Patch>(t1, t2);
                return stats;
            }
            else return null;
        }


        public List<string> ListaArchivosCambios()
        {
            return null;
        }

        /// <summary>
        /// Muestra el estado del indice del control de versiones
        /// </summary>
        /// <returns></returns>
        public RepositoryStatus getStatus()
        {
            //feed.Logs.WriteText("Get status", "Se obtiene el status de " + NombreProyecto);
            try
            {
                if (esRepositorioIniciado())
                {
                    Repo = new Repository(Path);
                    StatusOptions op = new StatusOptions();
                   

                    return Repo.Index.RetrieveStatus();
                }
                else return null;
            }
            catch(Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// Permite saber si existen cambios a guardar
        /// </summary>
        /// <returns></returns>
        public bool HayCambiosQueGuardar()
        {
            bool hay = false;

            RepositoryStatus status = getStatus();

            if (status.Modified.Count() > 0 || status.Staged.Count() > 0
                || status.Added.Count() > 0 || status.Removed.Count() > 0)
            {
                hay = true;
            }

            return hay;
        }

        /// <summary>
        /// Añade todos los archivos a diestro y siniestro
        /// </summary>
        public void git_stage_all()
        {
             
                RepositoryStatus status = getStatus();
                Console.WriteLine("stage..");
                if (status != null)
                {
                    Console.WriteLine("status NOT null..");
                    if (status.Modified.Count() > 0 || status.Untracked.Count() > 0)
                    {
                        StatusEntry archivo = null;

                        for (int i = 0; i < status.Count() - 1; i++)
                        {
                            try
                            {
                                archivo = status.ElementAt(i);
                                Repo.Index.Stage(archivo.FilePath);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Error en git_stage_all: " + ex.Message);
                                Console.ForegroundColor = ConsoleColor.White;

                            } //ignorar errores y seguir

                        }
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

            if (status != null)
            {
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

            }
            CommitOptions options = new CommitOptions();

            Signature signature = new Signature(name, email, DateTimeOffset.Now);
            try
            {

                options.AmendPreviousCommit = false;

                newC = Repo.Commit(message, signature, signature, options);

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
    
        
        
        /// <summary>
        /// Meter en el indice un archivo
        /// </summary>
        /// <param name="filePath"></param>
        public void git_trackFile(string filePath)
        {
            try
            {
                try
                {
                    ExplicitPathsOptions op = new ExplicitPathsOptions();
                    
                    FileStatus sta = Repo.Index.RetrieveStatus(filePath);
                    if (sta == FileStatus.Nonexistent)
                    {
                        Repo.Index.Unstage(filePath);
                    }
                    core.logs.Logs.WriteText("Control Git", "estado: " + sta.ToString() + " archivo: " + filePath);
                }
                catch(Exception ex)
                {
                    core.logs.Logs.WriteError("Error track file", ex);
                }

                Repo.Index.Stage(filePath);
            }
            catch (Exception ex){
                var l = filePath;
            }
        }

        public void git_ForceStage(string filePath)
        {
            FileStatus sta = Repo.Index.RetrieveStatus(filePath);
            if (sta == FileStatus.Modified)
            {
                Repo.Index.Stage(filePath);
            }
            
        }

        public Branch getRama(string name)
        {
            Branch rama = null;
            var ramas = from r in Repo.Branches
                   where r.Name.Contains(name) == true
                   select r;

            if (ramas.Count() > 0)
            {
                rama = ramas.First();
            }

            return rama;
        }

        /// <summary>
        /// Lista TODOS los commits de una determinada rama
        /// </summary>
        /// <param name="branchName"></param>
        /// <returns></returns>
        public List<CommitShortInfo> listarCommitsRama(string branchName)
        {
            return listarCommitsRama(branchName, -1);
        }

        /// <summary>
        /// Lista un determinado numero de commits de una rama
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="TOP"></param>
        /// <returns></returns>
        public List<CommitShortInfo> listarCommitsRama(string branchName, int TOP)
        {
            List<CommitShortInfo> lista = new List<CommitShortInfo>();
            Branch rama = getRama(branchName);

            if (rama != null)
            {
                if (TOP == -1)
                {
                    var grupo = (from c in rama.Commits
                                 select new
                                 {
                                     autor = c.Author.Name,
                                     id = c.Id.Sha,
                                     fecha = c.Committer.When,
                                     mensaje = c.Message
                                 });

                    CommitShortInfo cinfo = null;

                    foreach (var commit in grupo)
                    {
                        cinfo = new CommitShortInfo();
                        cinfo.Autor = commit.autor;
                        cinfo.Fecha = commit.fecha.DateTime;
                        cinfo.ID = commit.id;
                        cinfo.RepoPath = this.Path;
                        cinfo.Mensaje = commit.mensaje;
                        lista.Add(cinfo);
                    }
                }
                else
                {
                    var grupo = (from c in rama.Commits
                                 select new
                                 {
                                     autor = c.Author.Name,
                                     id = c.Id.Sha,
                                     fecha = c.Committer.When,
                                     mensaje = c.Message
                                 }).Take(TOP);

                    CommitShortInfo cinfo = null;

                    foreach (var commit in grupo)
                    {
                        cinfo = new CommitShortInfo();
                        cinfo.Autor = commit.autor;
                        cinfo.Fecha = commit.fecha.DateTime;
                        cinfo.ID = commit.id;
                        cinfo.RepoPath = this.Path;
                        cinfo.Mensaje = commit.mensaje;
                        lista.Add(cinfo);
                    }

                }
            }
            

            return lista;
        }
    }
}
