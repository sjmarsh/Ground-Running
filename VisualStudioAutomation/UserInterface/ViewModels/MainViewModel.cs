using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualStudioAutomation;


namespace UserInterface.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private SolutionCreator _solutionCreator;

        public MainViewModel()
        {
           ProjectTemplates = new  ObservableCollection<string>{ "Default Class Library" };
           ProjectTemplate = ProjectTemplates.FirstOrDefault();

           ProjectLocation = @"c:\Temp\TestAutomation\";

           _solutionCreator = new SolutionCreator();
        }

        public ObservableCollection<string> ProjectTemplates { get; set; }

        private string _projectName;
        public string ProjectName 
        {
            get 
            { 
                return _projectName; 
            }
            set
            {
                _projectName = value;
                NotifyOfPropertyChange(() => ProjectName);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _projectLocation;
        public string ProjectLocation
        {
            get
            {
                return _projectLocation;
            }
            set
            {
                _projectLocation = value;
                NotifyOfPropertyChange(() => ProjectLocation);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        public string ProjectTemplate { get; set; }        
        public bool HasTestProject { get; set; }
        public bool HasNuspec { get; set; }
        public bool HasPoshBuild { get; set; }
        public bool HasStashRepository { get; set; }

        public bool CanCreate 
        {
            get
            { 
                return !string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(ProjectLocation);
            } 
        }

        public void Create()
        {
            var solutionPath = ProjectLocation + @"\" + ProjectName + @"\"; // TODO - this should happen in the creator. Not here.
            _solutionCreator.Create(solutionPath, ProjectName, HasTestProject, HasNuspec);
        }
    }
}
