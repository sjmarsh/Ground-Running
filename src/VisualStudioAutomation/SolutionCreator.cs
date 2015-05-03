using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE100;
using VSLangProj;
using NLog;
using GroundRunning.Common;

namespace VisualStudioAutomation
{
    public class SolutionCreator
    {
        private Logger _logger;
        private AutomationResult _result;

        public SolutionCreator()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _result = new AutomationResult();
        }

        public AutomationResult Create(string solutionLocation, string projectName, bool hasTestProject, bool hasNuspec, string templatePath = null, string testTemplatePath = null)
        {
            _logger.Info("Creating Visual Studio Solution for {0}", projectName);     
   
            // TODO Visual Studio Version Check
            
            var currentDirectory = Directory.GetCurrentDirectory();
            var solutionPath = string.Format(@"{0}\{1}\{2}\", solutionLocation, "src", projectName);
                        
            if (templatePath == null)
            {
                _logger.Info("No Project Template specified. Using Default Empty Bin Class Library");
                const string defaultTemplatePath = @"\DefaultTemplates\ClassLibrary\Empty Bin Class Library\MyTemplate.vstemplate";
                templatePath = currentDirectory + defaultTemplatePath;
            }

            try
            {
                Type type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0");  // vs 2013
                var visualStudio = (DTE2)Activator.CreateInstance(type, true);
                var solution = (Solution4)visualStudio.Solution;

                // Create the solution
                _logger.Info("Creating the solution file");
                solution.AddFromTemplate(templatePath, solutionPath, projectName);
                
                if (hasTestProject)
                {
                    CreateTestProject(solutionLocation, projectName, testTemplatePath, currentDirectory, solution);
                }

                if (hasNuspec)
                {
                    _logger.Info("Creating nuspec");
                    new NuspecCreator().Create(solutionPath, projectName);
                }

                solution.Close(true);
                visualStudio.Quit();
                _logger.Info("Solution Creation Complete");

                CleanUpAdditionalFolders(solutionLocation);
                CorrectSolutionNameWhereContainsPeriod(solutionLocation, projectName);
            }
            catch(Exception ex)
            {
                _logger.Error("Exception Occurred Generating Visual Studio Solution.", ex);
                _result.AddException(ex);
            }

            return _result;
        }

        private void CreateTestProject(string solutionLocation, string projectName, string testTemplatePath, string currentDirectory, Solution4 solution)
        {
            _logger.Info("Creating Test Project for Solution");
            if (testTemplatePath == null)
            {
                _logger.Info("TestProject Template not specified. Using Default NUnit Test Project.");
                const string defaultTestTemplatePath = @"\DefaultTemplates\Test\NUnitTestProject\MyTemplate.vstemplate";
                testTemplatePath = currentDirectory + defaultTestTemplatePath;
            }

            var testProjectName = projectName + ".Test";
            var testProjectPath = solutionLocation + @"\src\" + testProjectName;

            _logger.Info("Adding the Test Project Named: {0}", testProjectName);
            // https://msdn.microsoft.com/en-us/library/envdte._solution.addfromtemplate.aspx
            const bool createNewSolution = false;
            solution.AddFromTemplate(testTemplatePath, testProjectPath, testProjectName, createNewSolution);
            
            // Add project reference to the main project
            // http://stackoverflow.com/questions/11530281/adding-programmatically-in-c-sharp-a-project-reference-as-opposed-to-an-assembl
            // http://blogs.msdn.com/b/vbteam/archive/2004/07/14/183403.aspx
            // NOTE: solution.Projects are not zero based!
            _logger.Info("Adding project reference to Test Project");
            var proj = solution.Projects.Item(1);
            var testProj = solution.Projects.Item(2);
            var vsTestProj = testProj.Object as VSProject;  // This does not like project names with 4 or more dots and ending with .Web (eg. My.New.Project.Web)
            vsTestProj.References.AddProject(proj);
        }

        private void CleanUpAdditionalFolders(string solutionLocation)
        {
            // Calling this library from an external program creates an additional folder for some reason. 
            // This is a Work-around to clean up the folders if they exist
            _logger.Info("Cleaning up additional folders created during the process");
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
                _logger.Info("Correcting Solution File Name because it contains one or more dots.");
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