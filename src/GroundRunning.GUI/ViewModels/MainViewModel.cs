using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using VisualStudioAutomation;

namespace GroundRunning.GUI.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private SolutionCreator _solutionCreator;

        public MainViewModel()
        {
           ProjectTemplates = new  ObservableCollection<string>{ "Default Class Library" };
           ProjectTemplate = ProjectTemplates.FirstOrDefault();

           ProjectLocation = @"c:\Temp2\TestAutomation\";

           IsCreating = false;
           
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

        private bool _isCreating;
        public bool IsCreating 
        {
            get
            {
                return _isCreating;
            }
            set
            { 
                _isCreating = value;
                NotifyOfPropertyChange(() => IsCreating);
            } 
        }

        public bool CanCreate 
        {
            get
            { 
                return !string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(ProjectLocation);
            } 
        }

        public async void Create()
        {
            Task createProject = _solutionCreator.CreateAsync(ProjectLocation, ProjectName, HasTestProject, HasNuspec);
            IsCreating = true;
            await Task.WhenAll(createProject);  // todo - this is still locking the UI thread.  why?
            IsCreating = false;
        }
    }
}
