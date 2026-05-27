using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class SettingsPage : BasePage
    {
        public SettingsPage(IPage page) : base(page) { }

        // Global Permissions section
        private ILocator GlobalPermissionsSection => Page.Locator("//h1[normalize-space()='Global Permissions']//ancestor::div[contains(@class,'sticky')]");
        private ILocator GlobalPermissionsTitle => Page.Locator("//h1[normalize-space()='Global Permissions']");
        private ILocator GlobalPermissionsMessage => Page.Locator("(//h1[normalize-space()='Global Permissions']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator EditGlobalPermissionsBtn => Page.Locator("//button[normalize-space()='Edit Global Permissions']");

        // Company Settings section
        private ILocator CompanySettingsSection => Page.Locator("//h1[normalize-space()='Company Settings']//ancestor::div[contains(@class,'sticky')]");
        private ILocator CompanySettingsTitle => Page.Locator("//h1[normalize-space()='Company Settings']");
        private ILocator CompanySettingsMessage => Page.Locator("(//h1[normalize-space()='Company Settings']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator EditCompanySettingsBtn => Page.Locator("//button[normalize-space()='Edit Company Settings']");

        // Personal Settings section
        private ILocator PersonalSettingsSection => Page.Locator("//h1[normalize-space()='Personal Settings']//ancestor::div[contains(@class,'sticky')]");
        private ILocator PersonalSettingsTitle => Page.Locator("//h1[normalize-space()='Personal Settings']");
        private ILocator PersonalSettingsMessage => Page.Locator("(//h1[normalize-space()='Personal Settings']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator EditMyPersonalSettingsBtn => Page.Locator("//button[normalize-space()='Edit My Personal Settings']");

        // Notification Settings section
        private ILocator NotificationSettingsSection => Page.Locator("//h1[normalize-space()='Notification Settings']//ancestor::div[contains(@class,'sticky')]");
        private ILocator NotificationSettingsTitle => Page.Locator("//h1[normalize-space()='Notification Settings']");
        private ILocator NotificationSettingsMessage => Page.Locator("(//h1[normalize-space()='Notification Settings']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator NotificationSettingsBtn => Page.Locator("//button[normalize-space()='Notification Settings']");

        // Optimization Settings section
        private ILocator OptimizationSettingsSection => Page.Locator("//h1[normalize-space()='Optimization Settings']//ancestor::div[contains(@class,'sticky')]");
        private ILocator OptimizationSettingsTitle => Page.Locator("//h1[normalize-space()='Optimization Settings']");
        private ILocator OptimizationSettingsMessage => Page.Locator("(//h1[normalize-space()='Optimization Settings']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator EditOptimizationSettingsBtn => Page.Locator("//button[normalize-space()='Edit Optimization Settings']");

        // Customers section
        private ILocator CustomersSection => Page.Locator("//h1[normalize-space()='Customers']//ancestor::div[contains(@class,'sticky')]");
        private ILocator CustomersTitle => Page.Locator("//h1[normalize-space()='Customers']");
        private ILocator CustomersMessage => Page.Locator("(//h1[normalize-space()='Customers']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator ManageCustomersBtn => Page.Locator("//button[normalize-space()='Manage Customers']");

        // Teams section
        private ILocator TeamsSection => Page.Locator("//h1[normalize-space()='Teams']//ancestor::div[contains(@class,'sticky')]");
        private ILocator TeamsTitle => Page.Locator("//h1[normalize-space()='Teams']");
        private ILocator TeamsMessage => Page.Locator("(//h1[normalize-space()='Teams']//ancestor::div[contains(@class,'flex')]/following-sibling::div)[1]//label");
        private ILocator ManageTeamsBtn => Page.Locator("//button[normalize-space()='Manage Teams']");

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickSettingsIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Settings");
        }

        public async Task OpenForEmptyTenantAsync()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickSettingsIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Settings");
        }

        // Global Permissions section
        public ILocator GetGlobalPermissionsSection() => GlobalPermissionsSection;
        public Task<string> GetGlobalPermissionsTitleAsync() => GetSectionTextAsync(GlobalPermissionsTitle);
        public Task<string> GetGlobalPermissionsMessageAsync() => GetSectionTextAsync(GlobalPermissionsMessage);
        public ILocator GetEditGlobalPermissionsBtn() => EditGlobalPermissionsBtn;
        public async Task<GlobalPermissionsPage> ClickEditGlobalPermissionsBtnAsync()
        {
            await EditGlobalPermissionsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Global Permissions");
            return new GlobalPermissionsPage(Page);
        }

        // Company Settings section
        public ILocator GetCompanySettingsSection() => CompanySettingsSection;
        public Task<string> GetCompanySettingsTitleAsync() => GetSectionTextAsync(CompanySettingsTitle);
        public Task<string> GetCompanySettingsMessageAsync() => GetSectionTextAsync(CompanySettingsMessage);
        public ILocator GetEditCompanySettingsBtn() => EditCompanySettingsBtn;
        public async Task<CompanySettingsPage> ClickEditCompanySettingsBtnAsync()
        {
            await EditCompanySettingsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Company Settings");
            return new CompanySettingsPage(Page);
        }

        // Personal Settings section
        public ILocator GetPersonalSettingsSection() => PersonalSettingsSection;
        public Task<string> GetPersonalSettingsTitleAsync() => GetSectionTextAsync(PersonalSettingsTitle);
        public Task<string> GetPersonalSettingsMessageAsync() => GetSectionTextAsync(PersonalSettingsMessage);
        public ILocator GetEditMyPersonalSettingsBtn() => EditMyPersonalSettingsBtn;
        public async Task<PersonOverviewPage> ClickEditMyPersonalSettingsBtnAsync()
        {
            await EditMyPersonalSettingsBtn.ClickAsync();
            var personOverviewPage = new PersonOverviewPage(Page);
            await personOverviewPage.ExpectPersonSettingsModalVisibleAsync();
            return personOverviewPage;
        }

        // Notification Settings section
        public ILocator GetNotificationSettingsSection() => NotificationSettingsSection;
        public Task<string> GetNotificationSettingsTitleAsync() => GetSectionTextAsync(NotificationSettingsTitle);
        public Task<string> GetNotificationSettingsMessageAsync() => GetSectionTextAsync(NotificationSettingsMessage);
        public ILocator GetNotificationSettingsBtn() => NotificationSettingsBtn;
        public async Task<NotificationSettingsPage> ClickNotificationSettingsBtnAsync()
        {
            await NotificationSettingsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("NOTIFICATION SETTINGS");
            return new NotificationSettingsPage(Page);
        }

        // Optimization Settings section
        public ILocator GetOptimizationSettingsSection() => OptimizationSettingsSection;
        public Task<string> GetOptimizationSettingsTitleAsync() => GetSectionTextAsync(OptimizationSettingsTitle);
        public Task<string> GetOptimizationSettingsMessageAsync() => GetSectionTextAsync(OptimizationSettingsMessage);
        public ILocator GetEditOptimizationSettingsBtn() => EditOptimizationSettingsBtn;
        public async Task<OptimizationSettingsPage> ClickEditOptimizationSettingsBtnAsync()
        {
            await EditOptimizationSettingsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Optimization Settings");
            return new OptimizationSettingsPage(Page);
        }

        // Customers section
        public ILocator GetCustomersSection() => CustomersSection;
        public Task<string> GetCustomersTitleAsync() => GetSectionTextAsync(CustomersTitle);
        public Task<string> GetCustomersMessageAsync() => GetSectionTextAsync(CustomersMessage);
        public ILocator GetManageCustomersBtn() => ManageCustomersBtn;
        public async Task<CustomerAccountsPage> ClickManageCustomersBtnAsync()
        {
            await ManageCustomersBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Customer Accounts");
            return new CustomerAccountsPage(Page);
        }

        // Teams section
        public ILocator GetTeamsSection() => TeamsSection;
        public Task<string> GetTeamsTitleAsync() => GetSectionTextAsync(TeamsTitle);
        public Task<string> GetTeamsMessageAsync() => GetSectionTextAsync(TeamsMessage);
        public ILocator GetManageTeamsBtn() => ManageTeamsBtn;
        public async Task<TeamsOveviewPage> ClickManageTeamsBtnAsync()
        {
            await ManageTeamsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Teams Overview");
            return new TeamsOveviewPage(Page);
        }

        public Task ExpectDefaultSectionsVisibleAsync() => Task.WhenAll(
            Expect(GlobalPermissionsSection).ToBeVisibleAsync(),
            Expect(CompanySettingsSection).ToBeVisibleAsync(),
            Expect(PersonalSettingsSection).ToBeVisibleAsync(),
            Expect(NotificationSettingsSection).ToBeVisibleAsync(),
            Expect(OptimizationSettingsSection).ToBeVisibleAsync(),
            Expect(CustomersSection).ToBeVisibleAsync(),
            Expect(TeamsSection).ToBeVisibleAsync());

        // Auto-retries visibility before reading text so callers never get a stale empty string
        // while the Settings page is still rendering its sections.
        private static async Task<string> GetSectionTextAsync(ILocator locator)
        {
            await Expect(locator).ToBeVisibleAsync();
            return (await locator.TextContentAsync())?.Trim() ?? string.Empty;
        }
    }
}
