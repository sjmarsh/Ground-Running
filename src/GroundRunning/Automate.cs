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

        public Automate TestProject()
        {
            _hasTestProject = true;
            return this;
        }

        public Automate TestTemplatePath(string testTemplatePath)
        {
            _testTemplatePath = testTemplatePath;
            return this;
        }

        public Automate Nuspec()
        {
            _hasNuspec = true;
            return this;
        }

        public Automate PoshBuild()
        {
            _hasPoshBuild = true;
            return this;
        }

        public Automate StashRepository()
        {
            _hasStashRepository = true;
            return this;
        }

        public Automate StashProjectKey(string stashProjectKey)
        {
            _stashProjectKey = stashProjectKey;
            return this;
        }

        public void Create()
        {
            var repoFolderPath = CreateRepoFolder(_solutionLocation, _projectName);
            var repoName = GetRepoName(_projectName);

            if (_hasStashRepository)
            {
                _stashRepositoryCreator.CreateAsync(repoName, _stashProjectKey);
            }

            _visualStudioSolutionCreator.Create(repoFolderPath, _projectName, _hasTestProject, _hasNuspec, _projectTemplatePath, _testTemplatePath);
            
            if (_hasPoshBuild)
            {
                _poshBuildCreator.Create(repoFolderPath, _projectName);
            }

            if (_hasStashRepository)
            {
                _stashRepositoryCreator.Publish(repoFolderPath, repoName, _stashProjectKey);
            }
        }

        public async Task CreateAsync()
        {

            var repoFolderPath = CreateRepoFolder(_solutionLocation, _projectName);
            var repoName = GetRepoName(_projectName);

            if (_hasStashRepository)
            {
                _stashRepositoryCreator.CreateAsync(repoName, _stashProjectKey);
            }

            await _visualStudioSolutionCreator.CreateAsync(repoFolderPath, _projectName, _hasTestProject, _hasNuspec, _projectTemplatePath, _testTemplatePath);

            
            // TODO make async
            if (_hasPoshBuild)
            {
                _poshBuildCreator.Create(repoFolderPath, _projectName);
            }

            if (_hasStashRepository)
            {
                _stashRepositoryCreator.Publish(repoFolderPath, repoName, _stashProjectKey);
            }

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
