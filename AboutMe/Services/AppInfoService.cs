using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AboutMe.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AboutMe.Services
{
    public class AppInfoService : IAppInfoService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AppInfoService> _logger;

        public AppInfoService(ApplicationDbContext dbContext, ILogger<AppInfoService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ApplicationInfo> GetAppInfoAsync()
        {
            try
            {
                ApplicationInfo applicationInfo = await _dbContext.ApplicationInfos
                    .Include(info => info.Buttons)
                    .FirstOrDefaultAsync() ?? new ApplicationInfo { Bio = "Error, this shouldn't be missing so you broke something!", };
                return applicationInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application info");
                throw;
            }
        }

        public async Task UpdateAppInfoAsync(ApplicationInfo updatedAppInfo)
        {
            try
            {
                ApplicationInfo existingAppInfo = await GetAppInfoAsync();

                existingAppInfo.Bio = updatedAppInfo.Bio;
                existingAppInfo.GithubCred = updatedAppInfo.GithubCred;
                existingAppInfo.GithubName = updatedAppInfo.GithubName;

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully updated application info");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application info");
                throw;
            }
        }

        public async Task UpdateButtonsAsync(List<ButtonInfo> newButtons)
        {
            try
            {
                ApplicationInfo existingAppInfo = await GetAppInfoAsync();
                Dictionary<int, ButtonInfo> existingButtonsMap = existingAppInfo.Buttons.ToDictionary(b => b.Id);
                HashSet<int> newButtonIds = newButtons.Select(b => b.Id).ToHashSet();

                // Add new buttons
                List<ButtonInfo> buttonsToAdd = newButtons.Where(b => !existingButtonsMap.ContainsKey(b.Id)).ToList();
                if (buttonsToAdd.Any())
                {
                    await _dbContext.AddRangeAsync(buttonsToAdd);
                    _logger.LogInformation("Adding {count} new buttons", buttonsToAdd.Count);
                }

                // Update existing buttons
                foreach (ButtonInfo newButton in newButtons)
                {
                    if (existingButtonsMap.TryGetValue(newButton.Id, out ButtonInfo existingButton))
                    {
                        existingButton.ButtonText = newButton.ButtonText;
                        existingButton.ButtonUrl = newButton.ButtonUrl;
                        existingButton.ButtonColourHex = newButton.ButtonColourHex;
                    }
                }
                _logger.LogInformation("Updated existing buttons");

                List<ButtonInfo> buttonsToDelete = existingButtonsMap.Values
                    .Where(eb => !newButtonIds.Contains(eb.Id))
                    .ToList();

                if (buttonsToDelete.Any())
                {
                    _dbContext.ButtonInfos.RemoveRange(buttonsToDelete);
                    _logger.LogInformation("Deleting {count} buttons", buttonsToDelete.Count);
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully completed button updates");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating buttons");
                throw;
            }
        }
    }
}