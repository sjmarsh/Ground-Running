﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace GroundRunning.GUI.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel()
        {
           InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            ProjectTemplates = new ObservableCollection<string> { "Default Class Library" };
            ProjectTemplate = ProjectTemplates.FirstOrDefault();

            ProjectName = "My-Project";
            ProjectLocation = @"c:\Temp2\";

            HasTestProject = true;
            HasNuspec = true;
            HasPoshBuild = true;
            HasStashRepository = true;
            StashProjectKey = "TOOL";

            IsCreating = false;
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

        private string _stashUrl;
        public string StashUrl
        {
            get { return _stashUrl; }
            set
            {
                _stashUrl = value;
                NotifyOfPropertyChange(() => StashUrl);
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
                    && HasValidStashDetails()
                    && !IsCreating;
            } 
        }

        private bool HasValidStashDetails()
        {
            return (!HasStashRepository ||
                   (HasStashRepository && 
                        !string.IsNullOrEmpty(StashProjectKey) &&
                        !string.IsNullOrEmpty(StashUrl) &&
                        !string.IsNullOrEmpty(StashUserName) &&
                        !string.IsNullOrEmpty(StashPassword)
                   ));
        }

        public async void Create()
        {
            var automate = new Automate();

            automate.VisualStudioSolution()
                   .With().ProjectName(ProjectName)
                   .And().SolutionLocation(ProjectLocation);

            automate.With().TestProject();
            automate.With().Nuspec();
            automate.With().PoshBuild();
            automate.StashRepository()
                .With().StashProjectKey(StashProjectKey); 
            
            Task createProject = automate.CreateAsync();

            IsCreating = true;
            await Task.WhenAll(createProject);  // todo - this is still locking the UI thread.  why?
            IsCreating = false;
        }
    }
}
