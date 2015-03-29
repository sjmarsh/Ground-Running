using System;
using System.IO;

namespace VisualStudioAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
                       
            if (args == null || args.Length == 0) // todo: also test for help, -h, /?, etc..
            {
                Console.WriteLine("No arguments specified. Usage: VisualStudioAutomation.exe [solutionName], [solutionLocation]");
                Console.ReadKey();    
            }
            else
            {
                // todo: better named params
                var solutionName = args.Length > 0 && !string.IsNullOrEmpty(args[0]) ? args[0] : "test-one";
                var projectName = solutionName;  // todo: later allow for specifying project name different to solution?
                var solutionLocation = args.Length > 1 && !string.IsNullOrEmpty(args[1]) ? args[1] : @"c:\Temp2\TestAutoCreateProject";
                
                var hasTestProject = true;
                var hasNuspec = true;

                //var templatePath = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\ProjectTemplates\CSharp\Web\Version2012\1033\WebApplicationProject45\WebApplicationProject45.vstemplate";

                Console.WriteLine("Creating solution named: {0}", solutionName);
                
                var solutionCreator = new SolutionCreator();

                solutionCreator.Create(solutionLocation, projectName, hasTestProject, hasNuspec);
                
                Console.WriteLine("Done creating. Press any key to exit.");
                Console.ReadKey();    
            }
        }
    }
}
