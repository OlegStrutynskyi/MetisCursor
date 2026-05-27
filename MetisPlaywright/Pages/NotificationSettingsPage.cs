using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class NotificationSettingsPage : BasePage
    {
        public NotificationSettingsPage(IPage page) : base(page) { }

        public Task ExpectOpenedAsync() => Expect(GetPageTitleLocator()).ToHaveTextAsync("NOTIFICATION SETTINGS");
    }
}
