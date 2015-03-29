using System.IO;
using System.Xml;
using System.Linq;

namespace VisualStudioAutomation
{
    public class NuspecCreator
    {
        public void Create()
        {
            XmlDocument nuspecTemplate = new XmlDocument();
            nuspecTemplate.Load(Directory.GetCurrentDirectory() + @"\Resources\nuspec-template.xml");

            var docEl = nuspecTemplate.DocumentElement;
            // linq to xml ?
        }
         
        
    }
}