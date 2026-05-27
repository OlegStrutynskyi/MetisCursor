using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class ScheduleOverviewPage : BasePage
    {
        public ScheduleOverviewPage(IPage page) : base(page) { }

        public Task ExpectOpenedAsync() => Expect(GetPageTitleLocator()).ToHaveTextAsync("Schedule Overview");
    }
}
