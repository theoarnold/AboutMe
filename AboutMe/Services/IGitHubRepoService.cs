using AboutMe.Data;

namespace AboutMe.Services
{
    public interface IGitHubRepoService
    {
        Task<List<GitHubRepo>> GetPublicRepositories(ApplicationInfo applicationInfo);
    }
}
