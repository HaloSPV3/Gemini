using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebUtils
{
    /// <summary>
    /// A helper class for getting the latest release from a Github repo, and for
    /// checking if an update is available.
    /// </summary>
    public static class LatestReleaseParser
    {
        /// <summary>
        /// Attempts to find the Github repo at the given URL. Throws WebException if anything goes awry/the request times out, or FormatException
        /// if parsing the JSON failed.
        /// </summary>
        /// <param name="JsonURL">The URL to find the repo at.</param>
        /// <param name="TimeOut">After this specified time in seconds, the request will time out.</param>
        /// <returns>True if it found the repo, false otherwise.</returns>
        private static bool RepoFound(string JsonURL, int TimeOut)
        {
            Dictionary<string, object> parsedJson = WebRequests.TryParseJson(JsonURL, TimeOut);
            // If the Github API fails to be parsed, it will always contain a message key.
            // From what I have seen, this key isn't present anywhere else in API calls.
            if (parsedJson != null)
            {
                if (parsedJson.ContainsKey("message"))
                {
                    if (parsedJson["message"].ToString() == "Not Found")
                    {
                        return false;
                    }
                }
                return true;
            }

            throw new FormatException($"Failed to parse JSON at url {JsonURL}");
        }

        /// <summary>
        /// Attempt to get the latest release from the repo. Throws WebException if anything goes awry/request times out, or FormatException
        /// if the JSON from the url constructed using the arguments failed to be parsed.
        /// </summary>
        /// <param name="GithubUserName">The username which the repo can be found under.</param>
        /// <param name="GithubRepoName">The name of the repo to get the latest release from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the check times out.</param>
        /// <returns>The latest release tag if successful, otherwise null.</returns>
        public static string TryGetLatestRelease(string GithubUserName, string GithubRepoName, int TimeOut)
        {
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);

            if (RepoFound(fullURL, TimeOut))
            {
                Dictionary<string, object> parsedJson = WebRequests.TryParseJson(fullURL, TimeOut);
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
        /// <returns>True/False depending on if an update is availble or not.</returns>
        public static bool GetUpdateAvailable(string GithubUsername, string RepoName, int Timeout)
        {
            string current_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string latestRelease = TryGetLatestRelease(GithubUsername, RepoName, Timeout);
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
