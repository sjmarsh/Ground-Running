using GroundRunning.Common;
using NLog;
using System;
using System.IO;

namespace PoshBuildAutomation
{
    public class PoshBuildCreator
    {
        private Logger _logger;
        private AutomationResult _result;

        public PoshBuildCreator()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _result = new AutomationResult();
        }

        public AutomationResult Create(string solutionLocation, string projectName)
        {
            _logger.Info("Creating PoshBuild files/folders for {0}", projectName);

            try
            {
                var resourceDirectory = Directory.GetCurrentDirectory() + @"\Resources\PoshBuild";
                DirectoryCopy.Copy(resourceDirectory, solutionLocation);

                UpdateSettingsWithProjectName(solutionLocation, projectName);
            }
            catch(Exception ex)
            {
                _logger.Error("Error occurred creating PoshBuild files.", ex);
                _result.AddException(ex);
            }

            return _result;
        }

        private void UpdateSettingsWithProjectName(string solutionLocation, string projectName)
        {
            const string solutionNameToken = "<<SolutionName>>";
            const string octopusProjectNameToken = "<<OctopusProjectName>>";

            var settingsFilePath = solutionLocation + @"\build\settings.ps1";
            
            var tokenizedFileText = File.ReadAllText(settingsFilePath);
            var parsedFileText = tokenizedFileText
                                    .Replace(solutionNameToken, projectName)
                                    .Replace(octopusProjectNameToken, GetOctopusProjectName(projectName));

            File.WriteAllText(settingsFilePath, parsedFileText); // overwrite existing file.
        }

        private string GetOctopusProjectName(string projectName)
        {
            return projectName.Replace(".", " ");
        }
    }
}
