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
        // TODO - this is not very secure. get credentials from user or figure out how to get windows auth
        private const string Base64Credentials = "Base64EncodedCredentialsGoHere"; 
        // TODO: configurize!
        private const string StashServer = "StashServerUrlGoesHere";
        private const string StashApiBaseUrl = @"StashApiBaseUrlGoesHere";
        private const string ScmId = "git";
        private readonly WebClient _client;
        
        public RepositoryCreator()
        {
            _client = new WebClient();
            _client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
            _client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Base64Credentials);

        }

        // TODO make this async again
        public void CreateAsync(string repoName, string stashProjectKey)
        {
            var createRepoUrl = string.Format("{0}{1}/repos/", StashApiBaseUrl, stashProjectKey);
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
       
        public void Publish(string repoLocation, string repoName, string stashProjectKey)
        {
            AddGitIgnoresToRepo(repoLocation);
            
            var repoUrl = string.Format("ssh://git@{0}/{1}/{2}.git", StashServer, stashProjectKey, repoName);

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
