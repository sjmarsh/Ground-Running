using System;
using System.Configuration;

namespace GroundRunning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: Lots more work to make the console app more usable.

            // defaults 
            string defaultProjectName = ConfigurationManager.AppSettings["DefaultProjectName"];
            string defaultProjectLocation = ConfigurationManager.AppSettings["DefaultProjectLocation"];
            string stashProjectKey = ConfigurationManager.AppSettings["DefaultStashProjectKey"];
            string stashRepoUrl = ConfigurationManager.AppSettings["StashRepoUrl"];
            string stashPublishUrl = ConfigurationManager.AppSettings["StashPublishUrl"];


            if (args == null || args.Length == 0) // todo: also test for help, -h, /?, etc..
            {
                Console.WriteLine("No arguments specified. Usage: GroundRunning.exe [solutionName], [solutionLocation]");
                Console.ReadKey();
            }
            else
            {
                // todo: better named params
                var solutionName = args.Length > 0 && !string.IsNullOrEmpty(args[0]) ? args[0] : defaultProjectName;
                var solutionLocation = args.Length > 1 && !string.IsNullOrEmpty(args[1]) ? args[1] : defaultProjectLocation;

                // example web project template
                //var templatePath = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\ProjectTemplates\CSharp\Web\Version2012\1033\WebApplicationProject45\WebApplicationProject45.vstemplate";

                Console.WriteLine("Creating solution named: {0}", solutionName);

                var automate = new Automate();

                automate.VisualStudioSolution()
                  .With().ProjectName(solutionName)
                  .And().SolutionLocation(solutionLocation)
                .Include().TestProject(true)
                .Include().Nuspec(true);
                //.Include().PoshBuild(true);
                //.Include().StashRepository(false)
                //    .With().StashProjectKey(stashProjectKey)
                //    .With().StashUrl(stashRepoUrl)
                //    .With().StashUrl(stashPublishUrl)
                //    .With().StashBase64Credentials(GetBase64Credentials()); 

                var result = automate.Create();

                if(result.WasSuccessful)
                {
                    Console.WriteLine("Done creating. Press any key to exit.");
                }
                else
                {
                    Console.WriteLine("Errors occurred creating. Check log file for details");
                }
                
                Console.ReadKey();    
            }
        }

        private static string GetBase64Credentials()
        {
            throw new NotImplementedException();
        }
    }
}
