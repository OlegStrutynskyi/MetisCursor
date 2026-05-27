using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IPage page) : base(page) { }

        private ILocator DashboardTitle => Page.Locator("//h1[contains(text(),'Dashboard')]").First;
        private ILocator TotalCounter => Page.Locator("//div[contains(text(),'Total')]/following-sibling::div/h1");
        private ILocator InProgressCounter => Page.Locator("//div[contains(text(),'In Progress')]/following-sibling::div/h1");
        private ILocator DelayedCounter => Page.Locator("//div[contains(text(),'Delayed')]/following-sibling::div/h1");
        private ILocator CompletedCounter => Page.Locator("//div[contains(text(),'Completed')]/following-sibling::div/h1");
        private ILocator AllTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='All']");
        private ILocator InProgressTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='In Progress']");
        private ILocator PendingTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='Pending']");
        private ILocator DelayedTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='Delayed']");
        private ILocator CompletedTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='Completed']");
        private ILocator ArchivedTab => Page.Locator("//div[contains(@class,'tab-item')]/div[normalize-space()='Archived']");
        private ILocator ShowChildrenBtn => Page.Locator("//button[normalize-space()='Show Children']");

        private ILocator GridContextNames => Page.Locator("//tr/td[3]//div[@class='text-base']");
        private ILocator GridContextRow(string contextName) =>
            Page.Locator($"//div[@class='text-base' and normalize-space()='{contextName}']");

        public async Task OpenForEmptyTenantAsync()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.CorrectEmailEmptyAutoTests1);
            await loginPage.FillPasswordAsync(Config.CorrectPassword);
            await loginPage.ClickLoginAsync();
            await Expect(DashboardTitle).ToContainTextAsync("Dashboard");
        }

        public async Task OpenForAutoTests1Async()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.CorrectEmailAutoTests1);
            await loginPage.FillPasswordAsync(Config.CorrectPassword);
            await loginPage.ClickLoginAsync();
            await Expect(DashboardTitle).ToContainTextAsync("Dashboard");
        }

        public async Task<string> GetDashboardTitleAsync()
        {
            await DashboardTitle.WaitForAsync();
            return (await DashboardTitle.TextContentAsync()) ?? string.Empty;
        }

        public Task<int> GetTotalCounterValueAsync() => GetCounterValueAsync(TotalCounter);
        public Task<int> GetInProgressCounterValueAsync() => GetCounterValueAsync(InProgressCounter);
        public Task<int> GetDelayedCounterValueAsync() => GetCounterValueAsync(DelayedCounter);
        public Task<int> GetCompletedCounterValueAsync() => GetCounterValueAsync(CompletedCounter);

        public Task ClickAllTabAsync() => AllTab.ClickAsync();
        public Task ClickInProgressTabAsync() => InProgressTab.ClickAsync();
        public Task ClickPendingTabAsync() => PendingTab.ClickAsync();
        public Task ClickDelayedTabAsync() => DelayedTab.ClickAsync();
        public Task ClickCompletedTabAsync() => CompletedTab.ClickAsync();
        public Task ClickArchivedTabAsync() => ArchivedTab.ClickAsync();
        public Task ClickShowChildrenBtnAsync() => ShowChildrenBtn.ClickAsync();

        public async Task<IReadOnlyList<string>> GetContextNamesAsync()
        {
            await GridContextNames.First.WaitForAsync();
            return await GridContextNames.AllInnerTextsAsync();
        }

        public async Task<int> GetContextNamesCountAsync()
        {
            await GridContextNames.First.WaitForAsync();
            return await GridContextNames.CountAsync();
        }

        public async Task<IReadOnlyList<string>> GetAllContextNamesAsync()
        {
            await GridContextNames.First.WaitForAsync();
            var names = new List<string>();
            var nextPageArrow = GetGridPaginatorNextPageArrowLocator();

            while (true)
            {
                names.AddRange(await GridContextNames.AllInnerTextsAsync());

                var classAttribute = await nextPageArrow.GetAttributeAsync("class");
                if (classAttribute != null && classAttribute.Contains("e-disable"))
                {
                    break;
                }

                await nextPageArrow.ClickAsync();
                //paginator updates the grid asynchronously; short non-blocking wait avoids reading stale rows
                await Page.WaitForTimeoutAsync(500);
            }

            return names;
        }

        public async Task<int> GetAllContextNamesCountAsync()
        {
            //grid renders asynchronously after login; give it a moment before deciding empty/non-empty
            await Page.WaitForTimeoutAsync(3000);

            if (await IsGridEmptyAsync())
            {
                return 0;
            }

            var totalCount = 0;
            var nextPageArrow = GetGridPaginatorNextPageArrowLocator();

            while (true)
            {
                totalCount += await GridContextNames.CountAsync();

                var classAttribute = await nextPageArrow.GetAttributeAsync("class");
                if (classAttribute != null && classAttribute.Contains("e-disable"))
                {
                    break;
                }

                await nextPageArrow.ClickAsync();
                await Page.WaitForTimeoutAsync(500);
            }

            return totalCount;
        }

        public async Task<ContextOverviewPage> ClickGridContextAsync(string contextName)
        {
            await Expect(GridContextRow(contextName)).ToBeVisibleAsync();
            await GridContextRow(contextName).ClickAsync();
            var contextOverviewPage = new ContextOverviewPage(Page);
            await contextOverviewPage.ExpectOpenedAsync();
            return contextOverviewPage;
        }

        public async Task ExpectDefaultControlsVisibleAsync()
        {
            await Expect(TotalCounter).ToBeVisibleAsync();
            await Expect(InProgressCounter).ToBeVisibleAsync();
            await Expect(DelayedCounter).ToBeVisibleAsync();
            await Expect(CompletedCounter).ToBeVisibleAsync();
            await Expect(AllTab).ToBeVisibleAsync();
            await Expect(InProgressTab).ToBeVisibleAsync();
            await Expect(PendingTab).ToBeVisibleAsync();
            await Expect(DelayedTab).ToBeVisibleAsync();
            await Expect(CompletedTab).ToBeVisibleAsync();
            await Expect(ArchivedTab).ToBeVisibleAsync();
            await Expect(ShowChildrenBtn).ToBeVisibleAsync();
        }

        private static async Task<int> GetCounterValueAsync(ILocator counter)
        {
            await counter.WaitForAsync();
            var text = await counter.TextContentAsync();
            return int.Parse(text ?? "0");
        }
    }
}
