﻿using System;
using System.IO;
using System.Security.Policy;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;

namespace VisualStudioAutomation
{
    public class SolutionCreator
    {
        public void Create(string solutionPath, string projectName, bool hasTestProject, bool hasNuspec, string templatePath = null, string testTemplatePath = null)
        {
            // todo: error handling
            // reference automation dlls from c:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\PublicAssemblies\

            var currentDirectory = Directory.GetCurrentDirectory();
            if (templatePath == null)
            {
                const string defaultTemplatePath = @"\DefaultTemplates\ClassLibrary\Empty Bin Class Library\MyTemplate.vstemplate";
                templatePath = currentDirectory + defaultTemplatePath;    
            }

            Type type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0");  // vs 2013
            var visualStudio = (DTE2)Activator.CreateInstance(type, true);

            var solution = (Solution4)visualStudio.Solution;
            
            var project = solution.AddFromTemplate(templatePath, solutionPath, projectName);

            if (hasTestProject)
            {
                if (testTemplatePath == null)
                {
                    const string defaultTestTemplatePath = @"\DefaultTemplates\Test\NUnitTestProject\MyTemplate.vstemplate";
                    testTemplatePath = currentDirectory + defaultTestTemplatePath;
                }
                
                var testProjectName = projectName + ".Test";
                var testProjectPath = solutionPath + @"\" + testProjectName;

                // https://msdn.microsoft.com/en-us/library/envdte._solution.addfromtemplate.aspx
                const bool createNewSolution = false; 
                var testProject = solution.AddFromTemplate(testTemplatePath, testProjectPath, testProjectName, createNewSolution);
            }

            if (hasNuspec)
            {
                // TODO Nuspec stuff
            }

            solution.Close(true);
            visualStudio.Quit();

        }
    }
}