using PoshBuildAutomation;
using VisualStudioAutomation;

namespace GroundRunning
{
    public class Automate
    {
        private string _projectName;
        private string _solutionLocation;
        private string _projectTemplatePath;
        private string _testTemplatePath;
        private bool _hasTestProject;
        private bool _hasNuspec;
        private bool _hasPoshBuild;

        private readonly SolutionCreator _visualStudioSolutionCreator;
        private readonly PoshBuildCreator _poshBuildCreator;

        public Automate()
        {
            _visualStudioSolutionCreator = new SolutionCreator();
           _poshBuildCreator = new PoshBuildCreator();
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

        public void Create()
        {
            _visualStudioSolutionCreator.Create(_solutionLocation, _projectName, _hasTestProject, _hasNuspec, _projectTemplatePath, _testTemplatePath);

            if (_hasPoshBuild)
            {
                _poshBuildCreator.Create(_solutionLocation, _projectName);
            }
        }

        public async void CreateAsync()
        {
            await _visualStudioSolutionCreator.CreateAsync(_solutionLocation, _projectName, _hasTestProject, _hasNuspec, _projectTemplatePath, _testTemplatePath);

            // make async
            if (_hasPoshBuild)
            {
                _poshBuildCreator.Create(_solutionLocation, _projectName);
            }
        }
    }
}
