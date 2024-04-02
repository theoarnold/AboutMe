using Microsoft.EntityFrameworkCore;
using AboutMe.Data;
using System.Diagnostics;

namespace AboutMe.Services
{
    public class AppInfoService : IAppInfoService
    {
        private readonly ApplicationDbContext _dbContext;

        public AppInfoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationInfo> GetAppInfoAsync()
        {
            ApplicationInfo? result = await _dbContext.ApplicationInfos
                .Include(info => info.Buttons)
                .FirstOrDefaultAsync() ?? new ApplicationInfo { Bio = "Error, this shouldn't be missing so you broke something!", };

            return result;
        }

        public async Task UpdateAppInfoAsync(ApplicationInfo updatedAppInfo)
        {
            try
            {
                ApplicationInfo? existingAppInfo = await GetAppInfoAsync();

                /* Update existing app info to the updated info, matching values are checked by EFCore.
                 * Including the individual fields so I don't forget what is considered to be 'AppInfo'.
                */
                if (existingAppInfo.Bio != updatedAppInfo.Bio) { existingAppInfo.Bio = updatedAppInfo.Bio; }
                if (existingAppInfo.GithubCred != updatedAppInfo.GithubCred) { existingAppInfo.GithubCred = updatedAppInfo.GithubCred; }
                if (existingAppInfo.GithubName != updatedAppInfo.GithubName) { existingAppInfo.GithubName = updatedAppInfo.GithubName; }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        // Since there are never more than like 5 buttons it might have been easier just to swap out the table with another list each time, oh well.
        public async Task UpdateButtonsAsync(List<ButtonInfo> buttInfo)
        {
            try
            {
                ApplicationInfo? existingAppInfo = await GetAppInfoAsync();
                List <ButtonInfo> existingButtons = existingAppInfo.Buttons;
                // Identify buttons to be added, updated, and deleted
                HashSet<int> existingButtonIds = new HashSet<int>(existingButtons.Select(eb => eb.Id));

                List<ButtonInfo> buttonsToAdd = buttInfo.Where(b => !existingButtonIds.Contains(b.Id)).ToList();
                List<ButtonInfo> buttonsToUpdate = buttInfo.Where(b => existingButtonIds.Contains(b.Id)).ToList();
                List<ButtonInfo> buttonsToDelete = existingButtons.Where(eb => !buttInfo.Any(b => b.Id == eb.Id)).ToList();

                // Add new buttons
                if (buttonsToAdd is not null)
                {
                    await _dbContext.AddRangeAsync(buttonsToAdd);
                }

                // Update existing buttons
                if (buttonsToUpdate is not null)
                {
                    foreach (ButtonInfo button in buttonsToUpdate)
                    {
                        ButtonInfo existingButton = existingButtons.FirstOrDefault(eb => eb.Id == button.Id);

                        if (existingButton.ButtonText != button.ButtonText){ existingButton.ButtonText = button.ButtonText; }
                        if (existingButton.ButtonUrl != button.ButtonUrl) { existingButton.ButtonUrl = button.ButtonUrl;}
                        if (existingButton.ButtonColourHex != button.ButtonColourHex) { existingButton.ButtonColourHex = button.ButtonColourHex; }
                    }
                }

                // Delete removed buttons
                if (buttonsToDelete is not null)
                {
                    _dbContext.ButtonInfos.RemoveRange(buttonsToDelete);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}