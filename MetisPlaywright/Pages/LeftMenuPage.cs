using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class LeftMenuPage : BasePage
    {
        public LeftMenuPage(IPage page) : base(page) { }

        private ILocator DashboardIcon => Page.Locator("//div[@title='Dashboard']/img");
        private ILocator ReportsIcon => Page.Locator("//div[@title='Reports']/img");
        private ILocator ContextExplorerIcon => Page.Locator("//div[@title='Context Explorer']/img");
        private ILocator LabelsIcon => Page.Locator("//div[@title='Labels Overview']/img");
        private ILocator TemplatesIcon => Page.Locator("//div[@title='Templates']/img");
        private ILocator ResourceManagerIcon => Page.Locator("//div[@title='Resource Manager']/img");
        private ILocator OptimizationsIcon => Page.Locator("//div[@title='Optimizations']/img");
        private ILocator PeopleIcon => Page.Locator("//div[@title='People']/img");
        private ILocator CustomerAccountsIcon => Page.Locator("//div[@title='Customer Accounts']/img");
        private ILocator StartOptimizationIcon => Page.Locator("//div[@title='Start Optimization']/img");
        private ILocator SupportIcon => Page.Locator("//div[@title='Support']/img");
        private ILocator SettingsIcon => Page.Locator("//div[@title='Settings']/img");
        private ILocator UserIcon => Page.Locator("//div[contains(@class,'sidebar-avatar')]//span[contains(@id,'avatar')]");
        private ILocator LogOutIcon => Page.Locator("//div[contains(@class,'logout-icon')]/img");

        public async Task OpenForEmptyTenantAsync()
        {
            var dashboardPage = new DashboardPage(Page);
            await dashboardPage.OpenForEmptyTenantAsync();
            //sidebar items render slightly after the dashboard title; short non-blocking wait avoids flake on first click
            await Page.WaitForTimeoutAsync(1000);
        }

        public async Task OpenForAutoTests1Async()
        {
            var dashboardPage = new DashboardPage(Page);
            await dashboardPage.OpenForAutoTests1Async();
        }

        public async Task ClickDashboardIconAsync()
        {
            await DashboardIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("?tab=all"));
        }

        public async Task ClickReportsIconAsync()
        {
            await ReportsIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("reports"));
        }

        public async Task ClickContextExplorerIconAsync()
        {
            await ContextExplorerIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("contexts"));
        }

        public async Task ClickLabelsIconAsync()
        {
            await LabelsIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("labels"));
        }

        public async Task ClickTemplatesIconAsync()
        {
            await TemplatesIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("templates"));
        }

        public async Task ClickResourceManagerIconAsync()
        {
            await ResourceManagerIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("resources"));
        }

        public Task ClickOptimizationsIconAsync() => OptimizationsIcon.ClickAsync();

        public async Task ClickPeopleIconAsync()
        {
            await PeopleIcon.WaitForAsync();
            await Expect(PeopleIcon).ToBeEnabledAsync();
            await PeopleIcon.ClickAsync();
            await Page.WaitForFunctionAsync("() => window.location.href.includes('people')");
        }

        public async Task ClickCustomerAccountsIconAsync()
        {
            await CustomerAccountsIcon.WaitForAsync();
            await Expect(CustomerAccountsIcon).ToBeEnabledAsync();
            await CustomerAccountsIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("customers"));
        }

        public Task ClickStartOptimizationIconAsync() => StartOptimizationIcon.ClickAsync();
        public Task ClickSupportIconAsync() => SupportIcon.ClickAsync();

        public async Task ClickSettingsIconAsync()
        {
            await SettingsIcon.ClickAsync();
            await Page.WaitForFunctionAsync("() => window.location.href.includes('settings')");
        }

        public async Task ClickUserIconAsync()
        {
            await UserIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("people/"));
        }

        public async Task ClickLogOutIconAsync()
        {
            await LogOutIcon.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("login"));
        }

        public async Task ExpectNavigationItemsVisibleAsync()
        {
            //Reports, Start Optimization and Support icons are hidden by default — excluded from this assertion
            await Expect(DashboardIcon).ToBeVisibleAsync();
            await Expect(ContextExplorerIcon).ToBeVisibleAsync();
            await Expect(LabelsIcon).ToBeVisibleAsync();
            await Expect(TemplatesIcon).ToBeVisibleAsync();
            await Expect(ResourceManagerIcon).ToBeVisibleAsync();
            await Expect(OptimizationsIcon).ToBeVisibleAsync();
            await Expect(PeopleIcon).ToBeVisibleAsync();
            await Expect(CustomerAccountsIcon).ToBeVisibleAsync();
            await Expect(SettingsIcon).ToBeVisibleAsync();
            await Expect(LogOutIcon).ToBeVisibleAsync();
        }
    }
}
