using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Configuration;
using System.Collections.Generic;
using GroundRunning.Common;
using GroundRunning.GUI.Services;

namespace GroundRunning.GUI.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private const string BrowseOtherTemplates = "Browse Other Templates...";
        private string DefaultProjectTemplate = ConfigurationManager.AppSettings["DefaultProjectTemplate"];
        private string DefaultProjectTemplatePath = ConfigurationManager.AppSettings["DefaultProjectTemplatePath"];
        private const string VisualStudioTemplateFilter = "Project Templates (*.vstemplate)|*.vstemplate";
        private FolderBrowserService _folderBrowserService;
        private OpenFileDialogService _openFileDialogService;

        public MainViewModel()
        {
           _folderBrowserService = new FolderBrowserService(); // todo inject
           _openFileDialogService = new OpenFileDialogService(); // todo inject
           InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            ProjectName = ConfigurationManager.AppSettings["DefaultProjectName"];
            ProjectLocation = ConfigurationManager.AppSettings["DefaultProjectLocation"];

            ProjectTemplates = new ObservableCollection<string> 
            { 
                DefaultProjectTemplate,
                BrowseOtherTemplates
            };
            ProjectTemplate = ProjectTemplates.FirstOrDefault();
            ProjectTemplatePath = DefaultProjectTemplatePath;
            VisualStudioTemplateDirectory = ConfigurationManager.AppSettings["VisualStudioTemplateDirectory"];

            HasTestProject = true;
            HasNuspec = true;
            HasPoshBuild = false;
            HasStashRepository = false;
            StashProjectKey = ConfigurationManager.AppSettings["DefaultStashProjectKey"];
            StashRepoUrl = ConfigurationManager.AppSettings["StashRepoUrl"];
            StashPublishUrl = ConfigurationManager.AppSettings["StashPublishUrl"];

            IsCreating = false;
            WasSuccessful = false;
            Errors = new BindableCollection<string>();
            Warnings = new BindableCollection<string>();
        }

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

        public void BrowseProjectLocation()
        {
            _folderBrowserService.ShowDialog();
            ProjectLocation = _folderBrowserService.FolderPath;
        }

        public ObservableCollection<string> ProjectTemplates { get; set; }

        private string _projectTemplate;
        public string ProjectTemplate 
        {
            get
            { 
                return _projectTemplate; 
            }
            set
            {
                _projectTemplate = value;

                if (value == DefaultProjectTemplate)
                {
                    ProjectTemplatePath = DefaultProjectTemplatePath;
                }

                NotifyOfPropertyChange(() => ProjectTemplate);
                NotifyOfPropertyChange(() => IsBrowsingVisualStudioTemplates);
            }
        }

        public bool IsBrowsingVisualStudioTemplates
        {
            get
            {
                return ProjectTemplate == BrowseOtherTemplates;
            }
        }

        private string _projectTemplatePath;
        public string ProjectTemplatePath 
        {
            get
            {
                return _projectTemplatePath;
            }
            set
            {
                _projectTemplatePath = value;
                NotifyOfPropertyChange(() => ProjectTemplatePath);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        public string VisualStudioTemplateDirectory { get; set; }

        public void BrowseProjectTemplates()
        {
            _openFileDialogService.ShowDialog(VisualStudioTemplateFilter, VisualStudioTemplateDirectory); 
            ProjectTemplatePath = _openFileDialogService.FilePath;
        }

        public bool HasTestProject { get; set; }
        public bool HasNuspec { get; set; }
        public bool HasPoshBuild { get; set; }

        #region Stash Properties
      
        private bool _hasStashRepository;
        public bool HasStashRepository
        {
            get
            {
                return _hasStashRepository;
            }
            set
            {
                _hasStashRepository = value;
                NotifyOfPropertyChange(() => HasStashRepository);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _stashProjectKey;
        public string StashProjectKey
        {
            get { return _stashProjectKey; }
            set
            {
                _stashProjectKey = value;
                NotifyOfPropertyChange(() => StashProjectKey);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _stashRepoUrl;
        public string StashRepoUrl
        {
            get { return _stashRepoUrl; }
            set
            {
                _stashRepoUrl = value;
                NotifyOfPropertyChange(() => StashRepoUrl);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _stashPublishUrl;
        public string StashPublishUrl
        {
            get { return _stashPublishUrl; }
            set
            {
                _stashPublishUrl = value;
                NotifyOfPropertyChange(() => StashPublishUrl);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _stashUserName;
        public string StashUserName
        {
            get { return _stashUserName; }
            set
            {
                _stashUserName = value;
                NotifyOfPropertyChange(() => StashUserName);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private string _stashPassword;
        public string StashPassword
        {
            get { return _stashPassword; }
            set
            {
                _stashPassword = value;
                NotifyOfPropertyChange(() => StashPassword);
                NotifyOfPropertyChange(() => CanCreate);
            }
        }

        private bool HasValidStashDetails()
        {
            return (!HasStashRepository ||
                   (HasStashRepository &&
                        !string.IsNullOrEmpty(StashProjectKey) &&
                        !string.IsNullOrEmpty(StashRepoUrl) &&
                        !string.IsNullOrEmpty(StashPublishUrl) &&
                        !string.IsNullOrEmpty(StashUserName) &&
                        !string.IsNullOrEmpty(StashPassword)
                   ));
        }
        
        #endregion

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
                return !string.IsNullOrEmpty(ProjectName) 
                    && !string.IsNullOrEmpty(ProjectLocation)
                    && !string.IsNullOrEmpty(ProjectTemplatePath)
                    && HasValidStashDetails()
                    && !IsCreating;
            } 
        }

        private BindableCollection<string> _errors;
        public BindableCollection<string> Errors
        {
            get { return _errors; }
            set {_errors = value; }
        }

        public bool HasErrors
        {
            get { return Errors.Any(); }
        }

        private BindableCollection<string> _warnings;
        public BindableCollection<string> Warnings
        {
            get { return _warnings; }
            set { _warnings = value; }
        }

        public bool HasWarnings
        {
            get { return Warnings.Any(); }
        }

        private bool _wasSuccessful;
        public bool WasSuccessful 
        {
            get 
            { 
                return _wasSuccessful; 
            }
            set 
            {
                _wasSuccessful = value;
                NotifyOfPropertyChange(() => WasSuccessful);
            }
        }
        
        public async void Create()
        {
            Errors.Clear();
            Warnings.Clear();
            WasSuccessful = false;
            NotifyOfPropertyChange(() => HasErrors);
            NotifyOfPropertyChange(() => HasWarnings);

            var automate = new Automate();
                        
            automate.VisualStudioSolution()
                   .With().ProjectName(ProjectName)
                   .And().SolutionLocation(ProjectLocation)
                   .Using().ProjectTemplatePath(ProjectTemplatePath)
            .Include().TestProject(HasTestProject)
            .Include().Nuspec(HasNuspec)
            .Include().PoshBuild(HasPoshBuild)
            .Include().StashRepository(HasStashRepository)
                .With().StashProjectKey(StashProjectKey)
                .With().StashRepoUrl(StashRepoUrl)
                .With().StashPublishUrl(StashPublishUrl)
                .With().StashBase64Credentials(GetBase64Credentials()); 
            
            Task<AutomationResult> createProject = automate.CreateAsync();

            IsCreating = true;          
            var result = await createProject;
            IsCreating = false;

            if(!result.WasSuccessful)
            {
                Errors.AddRange(result.ErrorMessages);
                NotifyOfPropertyChange(() => HasErrors);
            }
            else
            {
                WasSuccessful = true;
            }

            if(result.HasWarnings)
            {
                Warnings.AddRange(result.WarningMessages);
                NotifyOfPropertyChange(() => HasWarnings);
            }
        }

        private string GetBase64Credentials()
        {
            const string credentialFormat = "{0}:{1}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(credentialFormat, StashUserName, StashPassword)));
        }        
    }
}
