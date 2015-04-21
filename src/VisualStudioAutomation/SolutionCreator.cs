using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE100;
using VSLangProj;
using VslangProj100;

namespace VisualStudioAutomation
{
    public class SolutionCreator
    {
        public async Task CreateAsync(string solutionLocation, string projectName, bool hasTestProject, bool hasNuspec, string templatePath = null, string testTemplatePath = null)
        {
            await Task.Run(() => Create(solutionLocation, projectName, hasTestProject, hasNuspec, templatePath, testTemplatePath)); 
        }

        public int Create(string solutionLocation, string projectName, bool hasTestProject, bool hasNuspec, string templatePath = null, string testTemplatePath = null)
        {
            // todo: error handling
            // reference automation dlls from c:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\PublicAssemblies\

            var solutionPath = string.Format(@"{0}\{1}\{2}\", solutionLocation, "src", projectName);
            
            var currentDirectory = Directory.GetCurrentDirectory();
            if (templatePath == null)
            {
                const string defaultTemplatePath = @"\DefaultTemplates\ClassLibrary\Empty Bin Class Library\MyTemplate.vstemplate";
                templatePath = currentDirectory + defaultTemplatePath;
            }

            Type type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0");  // vs 2013
            var visualStudio = (DTE2)Activator.CreateInstance(type, true);

            var solution = (Solution4)visualStudio.Solution;
            
            solution.AddFromTemplate(templatePath, solutionPath, projectName);
                                   
            if (hasTestProject)
            {
                if (testTemplatePath == null)
                {
                    const string defaultTestTemplatePath = @"\DefaultTemplates\Test\NUnitTestProject\MyTemplate.vstemplate";
                    testTemplatePath = currentDirectory + defaultTestTemplatePath;
                }
                
                var testProjectName = projectName + ".Test";
                var testProjectPath = solutionLocation + @"\src\" + testProjectName;

                
                // https://msdn.microsoft.com/en-us/library/envdte._solution.addfromtemplate.aspx
                const bool createNewSolution = false;
                solution.AddFromTemplate(testTemplatePath, testProjectPath, testProjectName, createNewSolution);


                // Add project reference to the main project
                // http://stackoverflow.com/questions/11530281/adding-programmatically-in-c-sharp-a-project-reference-as-opposed-to-an-assembl
                // http://blogs.msdn.com/b/vbteam/archive/2004/07/14/183403.aspx
                // NOTE: solution.Projects are not zero based!
                var proj = solution.Projects.Item(1);
                var testProj = solution.Projects.Item(2);
                var vsTestProj = testProj.Object as VSProject;
                vsTestProj.References.AddProject(proj);
            }

            if (hasNuspec)
            {
                var nuspeccreator = new NuspecCreator();
                nuspeccreator.Create(solutionPath, projectName);
            }
 
            solution.Close(true);
            visualStudio.Quit();

            CleanUpAdditionalFolders(solutionLocation);
            CorrectSolutionNameWhereContainsPeriod(solutionLocation, projectName);

            return 0;
            // todo: error handling to return 1 or other codes
        }

        private void CleanUpAdditionalFolders(string solutionLocation)
        {
            // Calling this library from an external program creates an additional folder for some reason. 
            // This is a Work-around to clean up the folders if they exist
            var rootDir = solutionLocation + @"\..\..\";
            var repoName = solutionLocation.Substring(solutionLocation.LastIndexOf("\\"));
            var folderToRemove = rootDir + "\\" + repoName;
            var testFolderToRemove = folderToRemove + ".Test";
            
            if(Directory.Exists(folderToRemove))
            {
                Directory.Delete(folderToRemove, true);
            }

            if(Directory.Exists(testFolderToRemove))
            {
                Directory.Delete(testFolderToRemove, true);
            }
        }

        private void CorrectSolutionNameWhereContainsPeriod(string solutionLocation, string projectName)
        {
            // Also doesn't handle project names with dots in them. eg. "My.New.Project.sln" becomes "My.sln"
            // This is a work-around to correct that issue after the solution has been generated.

            if(projectName.Contains("."))
            {
                var directoryInfo = new DirectoryInfo(solutionLocation + @"\src\");
                var directoryFiles = directoryInfo.GetFiles().ToList(); 
                var solutionFile = directoryFiles.FirstOrDefault(f => f.Extension == ".sln");
                var properSolutionName = directoryInfo.FullName + projectName + ".sln";
                if(solutionFile != null)
                {
                    File.Move(solutionFile.FullName, properSolutionName);
                }
                
                // also drop the .suo file (it will be re-generated by vs next time the solution is opened)
                var suoFile = directoryInfo.GetFiles().ToList().FirstOrDefault(f => f.Extension == ".suo");
                if(suoFile != null)
                {
                    File.Delete(suoFile.FullName);
                }
            }
        }
    }
}