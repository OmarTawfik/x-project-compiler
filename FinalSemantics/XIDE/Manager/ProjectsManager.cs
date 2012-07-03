namespace XIDE.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LanguageCompiler;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Semantics;
    using LanguageTranslator.Translators.CPP;

    public struct ProjectData
    {
        private string projectName;
        private string projectLocation;

        public ProjectData(string projectName, string projectLocation)
        {
            this.projectName = projectName;
            this.projectLocation = projectLocation;
        }

        public string ProjectName
        {
            get { return this.projectName; }
        }

        public string ProjectLocation
        {
            get { return this.projectLocation; }
        }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProjectsManager
    {
        private static ProjectSettings currentProject;
        private static List<ProjectData> recentProjects;
        public static event Action<ProjectSettings> CurrentProjectChanged;

        static ProjectsManager()
        {
            ////HERE WE CHECK FOR THE PROJECTS DIRECTORY AND LOAD THE ALL.
            recentProjects = new List<ProjectData>();
        }

        public static ProjectSettings CurrentProject
        {
            get { return currentProject; }
        }

        public static List<ProjectData> RecentProjects
        {
            get { return recentProjects; }
        }

        public static void LoadProjectFromLocation(string location)
        {
            currentProject = ProjectSettings.LoadProject(location);
            CurrentProjectChanged(currentProject);
        }

        public static void CreateNewProject(string title, string location)
        {
            currentProject = ProjectSettings.CreateProject(location, title);
            CurrentProjectChanged(currentProject);
        }

        public static bool BuildAndRunCurrentProject()
        {
            PCCPPTranslator cpp = new PCCPPTranslator();
            //CompilerService.Instance.Clear();

            //for (int i = 0; i < cpp.BackendClasses.Count; i++)
            //{
            //    CompilerService.Instance.ParseFile(cpp.BackendClasses[i].XlangCode, cpp.BackendClasses[i].Classname);

            //}

            //foreach (string codefile in ProjectsManager.CurrentProject.CodeFiles)
            //{
            //    CompilerService.Instance.ParseFile(System.IO.File.ReadAllText(codefile), System.IO.Path.GetFileName(codefile));
            //}

            //if (CompilerService.Instance.Errors.Count > 0)
            //{
            //    return false;
            //}

            //CompilerService.Instance.CheckSemantics();

            //if (CompilerService.Instance.Errors.Count > 0)
            //{
            //    return false;
            //}

            //List<ClassDefinition> classList = new List<ClassDefinition>();

            //foreach (ClassDefinition classdef in CompilerService.Instance.ClassesList.Values)
            //{
            //    classList.Add(classdef);
            //}

            //cpp.Translate(classList);
            //cpp.Build();
            cpp.Run();

            return true;
        }
    }
}
