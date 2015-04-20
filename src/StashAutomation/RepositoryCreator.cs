using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using StashAutomation.Models;

namespace StashAutomation
{
    public class RepositoryCreator
    {       
        private const string ScmId = "git";
        private readonly WebClient _client;
        
        public RepositoryCreator()
        {
            _client = new WebClient();
        
        }

        // TODO make this async again
        // TODO Use oAuth instead of Basic Auth
        public void CreateAsync(string repoName, string stashProjectKey, string stashUrl, string stashBase64Credentials)
        {
            _client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
            _client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + stashBase64Credentials);

            var stashApiBaseUrl = string.Format("http://{0}/rest/api/1.0/projects/", stashUrl);
            var createRepoUrl = string.Format("{0}{1}/repos/", stashApiBaseUrl, stashProjectKey);
            var repository = new Repository{ Name = repoName, ScmId = ScmId };

            try
            {
                _client.UploadData(createRepoUrl, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(repository)));
            }
            catch (Exception ex)
            {
                // TODO return encapsulated error response object so can display nice message
                throw;
            }
        }
       
        public void Publish(string repoLocation, string repoName, string stashProjectKey, string stashUrl)
        {
            AddGitIgnoresToRepo(repoLocation);
            
            var repoUrl = string.Format("ssh://git@{0}/{1}/{2}.git", stashUrl, stashProjectKey, repoName);

            var batchCommand = new StringBuilder();
            batchCommand.AppendLine("cd " + repoLocation);
            batchCommand.AppendLine("git init");
            batchCommand.AppendLine("git add .");
            batchCommand.AppendLine("git commit -m \"Initial Automated Commit\"");
            batchCommand.AppendLine("git remote add origin " + repoUrl);
            batchCommand.AppendLine("git remote -v"); // verfiy nre remote url
            batchCommand.AppendLine("git push origin master");

            var tempBatchFile = repoLocation + "\\" + "publishNewRepo.bat";
            File.AppendAllText(tempBatchFile, batchCommand.ToString());

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(tempBatchFile)
            };
            process.Start();
            process.WaitForExit();
        }

        private void AddGitIgnoresToRepo(string repoLocation)
        {
            var resourceDirectory = Directory.GetCurrentDirectory() + @"\Resources\Stash\";
            File.Copy(resourceDirectory + ".gitattributes", repoLocation + "\\" + ".gitattributes");
            File.Copy(resourceDirectory + ".gitignore", repoLocation + "\\" + ".gitignore");
        }
    }
}
