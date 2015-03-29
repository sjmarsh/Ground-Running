using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace VisualStudioAutomation
{
    public class NuspecCreator
    {
        public void Create(string solutionPath, string projectName)
        {
            XmlDocument nuspec = new XmlDocument();
            nuspec.Load(Directory.GetCurrentDirectory() + @"\Resources\nuspec-template.xml");
            
            var id = nuspec.GetElementsByTagName("id")[0];
            var title = nuspec.GetElementsByTagName("title")[0];

            id.InnerText = projectName;
            title.InnerText = projectName;

            var nuspecName = projectName + ".nuspec";
            var nuspecPath = solutionPath + @"\" + nuspecName; 
            nuspec.Save(nuspecPath);
        }
    }
}