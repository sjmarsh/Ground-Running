using System.IO;

namespace PoshBuildAutomation
{
    public class PoshBuildCreator
    {
        public void Create(string solutionLocation, string projectName)
        {
            var resourceDirectory = Directory.GetCurrentDirectory() + @"\Resources\PoshBuild";
            DirectoryCopy.Copy(resourceDirectory, solutionLocation);
            
            UpdateSettingsWithProjectName(solutionLocation, projectName);

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
