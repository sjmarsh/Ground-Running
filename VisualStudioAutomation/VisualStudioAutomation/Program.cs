using EnvDTE100;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudioAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            // reference automation dlls from c:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\PublicAssemblies\

            Type type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0");  // vs 2013
            var visualStudio = (DTE2)Activator.CreateInstance(type, true);

            var solution = (Solution4)visualStudio.Solution;

            var template = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\ProjectTemplates\CSharp\Web\Version2012\1033\WebApplicationProject45\WebApplicationProject45.vstemplate";
            var project = solution.AddFromTemplate(template, @"c:\temp2\newone\", "testproj");

            solution.Close(true);
            visualStudio.Quit();
        }
    }
}
