using SharpUtils.WebUtils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUtils.WebUtilsAsync
{
    /// <summary>
    /// A helper class for getting the latest release from a Github repo, and for
    /// checking if an update is available.
    /// </summary>
    public static class GithubReleaseParserAsync
    {
        /// <summary>
        /// Attempt to get the latest release from the repo. Throws WebException if anything goes awry/request times out, or FormatException
        /// if the JSON from the url constructed using the arguments failed to be parsed.
        /// </summary>
        /// <param name="GithubUserName">The username which the repo can be found under.</param>
        /// <param name="GithubRepoName">The name of the repo to get the latest release from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the check times out.</param>
        /// <param name="cancellationToken">The cancellation token to use download string task.</param>
        /// <returns>The latest release tag if successful, otherwise null.</returns>
        public static async Task<string> GetLatestRelease(string GithubUserName, string GithubRepoName, int TimeOut, CancellationToken cancellationToken)
        {
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);
            string jsonData = await WebRequestsAsync.DownloadStringTimeout(fullURL, TimeOut, cancellationToken);

            if (GithubReleaseParser.RepoFound(jsonData))
            {
                Dictionary<string, object> parsedJson = WebRequests.ParseJson(jsonData);
                if (parsedJson != null)
                {
                    return parsedJson["tag_name"].ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Attempt to check if an update is available. Throws WebException if anything goes awry/requset times out, or FormatException
        /// if the JSON from the request failed to be parsed.
        /// </summary>
        /// <param name="GithubUsername">The username of the user who owns the repo to check.</param>
        /// <param name="RepoName">The name of the repo to check.</param>
        /// <param name="Timeout">After this specified amount of time in seconds, the request will time out.</param>
        /// <param name="cancellationToken">The cancellation token to use download string task.</param>
        /// <returns>True/False depending on if an update is availble or not.</returns>
        public static async Task<bool> GetUpdateAvailable(string GithubUsername, string RepoName, int Timeout, CancellationToken cancellationToken)
        {
            string current_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string latestRelease = await GetLatestRelease(GithubUsername, RepoName, Timeout, cancellationToken);
            if (latestRelease != null)
            {
                if (current_version != latestRelease)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
