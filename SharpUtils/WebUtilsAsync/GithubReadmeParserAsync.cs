using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUtils.WebUtilsAsync
{
    /// <summary>
    /// A helper class for parsing a Github README.MD file.
    /// </summary>
    public static class GithubReadmeParserAsync
    {
        /// <summary>
        /// Attempts to download a string after a given prefix from a README.md. Throws WebException if anything goes awry/the request times out.
        /// </summary>
        /// <param name="ReadmeURL">The URL the README.MD is located at.</param>
        /// <param name="LinePrefix">The prefix to search for. This is not included in the result.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the request will time out.</param>
        /// <param name="cancellationToken">The cancellation token to use download string task.</param>
        /// <returns>The downloaded string if successful, otherwise returns null.</returns>
        public static async Task<string> GetLineFromReadme(string ReadmeURL, string LinePrefix, int TimeOut, CancellationToken cancellationToken)
        {
            string readmeText = await WebRequestsAsync.DownloadStringTimeout(ReadmeURL, TimeOut, cancellationToken);

            if (readmeText != null)
            {
                using (StringReader sr = new StringReader(readmeText))
                {
                    string currentLine;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.StartsWith(LinePrefix))
                        {
                            return currentLine.Substring(LinePrefix.Length);
                        }
                    }
                }
            }

            return null;
        }
    }
}
