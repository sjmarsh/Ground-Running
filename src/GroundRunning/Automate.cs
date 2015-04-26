using System;
using System.IO;
using System.Threading.Tasks;
using PoshBuildAutomation;
using StashAutomation;
using VisualStudioAutomation;

namespace GroundRunning
{
    public class Automate
    {
        private string _projectName;
        private string _solutionLocation;
        private string _projectTemplatePath;
        private string _testTemplatePath;
        private string _stashProjectKey;
        private string _stashRepoUrl;
        private string _stashPublishUrl;
        private string _stashBase64Credentials;
        private bool _hasTestProject;
        private bool _hasNuspec;
        private bool _hasPoshBuild;
        private bool _hasStashRepository;

        private readonly SolutionCreator _visualStudioSolutionCreator;
        private readonly PoshBuildCreator _poshBuildCreator;
        private readonly RepositoryCreator _stashRepositoryCreator;
        

        public Automate()
        {
            _visualStudioSolutionCreator = new SolutionCreator();
            _poshBuildCreator = new PoshBuildCreator();
            _stashRepositoryCreator = new RepositoryCreator();
        }

        public Automate VisualStudioSolution()
        {
            return this;
        }

        public Automate Include()
        {
            return this;
        }

        public Automate With()
        {
            return this;
        }

        public Automate Using()
        {
            return this;
        }
        
        public Automate And()
        {
            return this;
        }

        public Automate ProjectName(string projectName)
        {
            _projectName = projectName;
            return this;
        }

        public Automate ProjectTemplatePath(string projectTemplatePath)
        {
            _projectTemplatePath = projectTemplatePath;
            return this;
        }

        public Automate SolutionLocation(string solutionLocation)
        {
            _solutionLocation = solutionLocation;
            return this;
        }

        public Automate TestProject(bool hasTestProject = false)
        {
            _hasTestProject = hasTestProject;
            return this;
        }

        public Automate TestTemplatePath(string testTemplatePath)
        {
            _testTemplatePath = testTemplatePath;
            return this;
        }

        public Automate Nuspec(bool hasNuspec = false)
        {
            _hasNuspec = hasNuspec;
            return this;
        }

        public Automate PoshBuild(bool hasPoshBuild = false)
        {
            _hasPoshBuild = hasPoshBuild;
            return this;
        }

        public Automate StashRepository(bool hasStashRepository = false)
        {
            _hasStashRepository = hasStashRepository;
            return this;
        }

        public Automate StashProjectKey(string stashProjectKey)
        {
            _stashProjectKey = stashProjectKey;
            return this;
        }

        public Automate StashRepoUrl(string stahRepoUrl)
        {
            _stashRepoUrl = stahRepoUrl;
            return this;
        }

        public Automate StashPublishUrl(string stahPublishUrl)
        {
            _stashPublishUrl = stahPublishUrl;
            return this;
        }

        public Automate StashBase64Credentials(string base64Credentials)
        {
            _stashBase64Credentials = base64Credentials;
            return this;
        }

        public void Create()
        {
            var repoFolderPath = CreateRepoFolder(_solutionLocation, _projectName);
            var repoName = GetRepoName(_projectName);

            _visualStudioSolutionCreator.Create(repoFolderPath, _projectName, _hasTestProject, _hasNuspec, _projectTemplatePath, _testTemplatePath);
            
            if (_hasPoshBuild)
            {
                _poshBuildCreator.Create(repoFolderPath, _projectName);
            }

            if (_hasStashRepository)
            {
                _stashRepositoryCreator.Create(repoName, _stashProjectKey, _stashRepoUrl, _stashBase64Credentials);
                _stashRepositoryCreator.Publish(repoFolderPath, repoName, _stashProjectKey, _stashPublishUrl);
            }
        }

        public async Task CreateAsync()
        {
            await Task.Run(() => Create());
        }

        private string CreateRepoFolder(string solutionLocation, string projectName)
        {
            var repoFolderPath = solutionLocation + "\\" + GetRepoName(projectName);
            if (Directory.Exists(repoFolderPath))
            {
                throw new ArgumentException(string.Format("Repo already exists at: {0}", repoFolderPath));
            }
            else
            {
                Directory.CreateDirectory(repoFolderPath);
            }
            return repoFolderPath;
        }

        private string GetRepoName(string projectName)
        {
            return projectName.Replace(".", "-").ToLower();
        }
    }
}
