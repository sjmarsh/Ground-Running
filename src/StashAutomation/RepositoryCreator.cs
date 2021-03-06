﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using StashAutomation.Models;
using NLog;
using GroundRunning.Common;

namespace StashAutomation
{
    public class RepositoryCreator
    {       
        private const string ScmId = "git";
        private readonly WebClient _client;

        private Logger _logger;
        private AutomationResult _result;
        
        public RepositoryCreator()
        {
            _client = new WebClient();

            _logger = LogManager.GetCurrentClassLogger();
            _result = new AutomationResult();
        }

        // TODO Use oAuth instead of Basic Auth
        public AutomationResult Create(string repoName, string stashProjectKey, string stashUrl, string stashBase64Credentials)
        {
            _logger.Info("Creating Stash Repository Named: {0} on Stash Server: {1} using Project Key: {2}", repoName, stashUrl, stashProjectKey);
           
            try
            {
                _client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                _client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + stashBase64Credentials);

                var stashApiBaseUrl = string.Format("http://{0}/rest/api/1.0/projects/", stashUrl);
                var createRepoUrl = string.Format("{0}{1}/repos/", stashApiBaseUrl, stashProjectKey);
                var repository = new Repository { Name = repoName, ScmId = ScmId };

                _client.UploadData(createRepoUrl, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(repository)));
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured creating Stash Repository", ex);
                _result.AddException(ex);
            }

            return _result;
        }
       
        public AutomationResult Publish(string repoLocation, string repoName, string stashProjectKey, string stashPublishUrl)
        {
            _logger.Info("Publishing Repository from local path {0} to Stash {1}", repoLocation, stashPublishUrl);

            try
            {
                AddGitIgnoresToRepo(repoLocation);

                var repoUrl = string.Format("ssh://git@{0}/{1}/{2}.git", stashPublishUrl, stashProjectKey, repoName);

                var batchCommand = new StringBuilder();
                batchCommand.AppendLine("cd " + repoLocation);
                batchCommand.AppendLine("git init");
                batchCommand.AppendLine("git add .");
                batchCommand.AppendLine("git commit -m \"Initial Automated Commit\"");
                batchCommand.AppendLine("git remote add origin " + repoUrl);
                batchCommand.AppendLine("git remote -v"); // verfiy remote url
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
            catch (Exception ex)
            {
                _logger.Error("Error occured Publishing Repository to Stash.", ex);
                _result.AddException(ex);
            }

            return _result;
        }

        private void AddGitIgnoresToRepo(string repoLocation)
        {
            var resourceDirectory = Directory.GetCurrentDirectory() + @"\Resources\Stash\";
            File.Copy(resourceDirectory + ".gitattributes", repoLocation + "\\" + ".gitattributes");
            File.Copy(resourceDirectory + ".gitignore", repoLocation + "\\" + ".gitignore");
        }
    }
}
