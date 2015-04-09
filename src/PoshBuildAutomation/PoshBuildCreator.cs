using System.IO;

namespace PoshBuildAutomation
{
    public class PoshBuildCreator
    {
        public PoshBuildCreator(string solutionLocation, string projectName)
        {
            var resourceDirectory = Directory.GetCurrentDirectory() + @"\Resources\PoshBuild";

            DirectoryCopy.Copy(resourceDirectory, solutionLocation);

            // todo replace tokens in settings file

        }
    }
}
