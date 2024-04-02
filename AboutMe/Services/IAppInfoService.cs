using AboutMe.Data;

namespace AboutMe.Services
{
    public interface IAppInfoService
    {
        Task<ApplicationInfo> GetAppInfoAsync();

        Task UpdateAppInfoAsync(ApplicationInfo updatedAppInfo);

        Task UpdateButtonsAsync(List<ButtonInfo> buttInfo);
    }
}


