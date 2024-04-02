using Octokit;
using AboutMe.Data;
using System.Diagnostics;
namespace AboutMe.Services
{
    public class GitHubRepoService : IGitHubRepoService
    {

        public async Task<List<GitHubRepo>> GetPublicRepositories(ApplicationInfo applicationInfo)
        {
            Credentials credentials = new Credentials(applicationInfo.GithubCred);
            GitHubClient gitHubClient = new GitHubClient(new Octokit.ProductHeaderValue("GitHubRepoViewer")) { Credentials = credentials };
            List<GitHubRepo> repositories = new List<GitHubRepo>();

            try
            {
                IReadOnlyList<Repository> repos = await gitHubClient.Repository.GetAllForUser(applicationInfo.GithubName);
                foreach (var repo in repos)
                {
                    repositories.Add(GetGitHubRepo(repo));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return repositories;
        }

        private GitHubRepo GetGitHubRepo(Repository repo)
        {
            return new GitHubRepo
            {
                Title = repo.Name,
                Url = repo.HtmlUrl,
                UploadDate = repo.CreatedAt,
                PrimaryLanguage = repo.Language
            };
        }
    }
}