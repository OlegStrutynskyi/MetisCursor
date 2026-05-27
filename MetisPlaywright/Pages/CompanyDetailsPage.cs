using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class CompanyDetailsPage : BasePage
    {
        public CompanyDetailsPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;
        private ILocator CompanyNameInput => Page.Locator("//label[normalize-space()='Customer Name']/following-sibling::span/input");
        private ILocator CompanyNameError => Page.Locator("//label[normalize-space()='Customer Name']/following-sibling::div");
        private ILocator IndustryDropdown => Page.Locator("//label[normalize-space()='Industry']/following-sibling::span/span");
        private ILocator IndustryError => Page.Locator("//label[normalize-space()='Industry']/following-sibling::div");
        private ILocator BusinessTypeDropdown => Page.Locator("//label[normalize-space()='Business Type']/following-sibling::span/span");
        private ILocator CountryDropdown => Page.Locator("//label[normalize-space()='Country']/following-sibling::span/span");
        private ILocator OpenDropdownOptions => Page.Locator("//div[contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]");
        private ILocator SaveBtn => Page.GetByRole(AriaRole.Button, new() { Name = "Save" });

        public async Task OpenCreateNewCustomerPageForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickCustomerAccountsIconAsync();
            var customerAccountsPage = new CustomerAccountsPage(Page);
            await customerAccountsPage.ClickCreateCustomerAccountBtnAsync();
            await Expect(PageTitle).ToHaveTextAsync("Create New Customer Account");
        }

        public async Task<string> GetPageTitleAsync()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }

        public Task FillCustomerNameAsync(string customerName) => CompanyNameInput.FillAsync(customerName);

        public Task ClickSaveAsync() => SaveBtn.ClickAsync();

        public Task ExpectCustomerNameErrorAsync(string expectedText) =>
            Expect(CompanyNameError).ToHaveTextAsync(expectedText);

        public Task ExpectIndustryErrorAsync(string expectedText) =>
            Expect(IndustryError).ToHaveTextAsync(expectedText);

        public async Task<IReadOnlyList<string>> GetIndustryDropdownOptionsAsync()
        {
            await IndustryDropdown.ClickAsync();
            return await GetOpenDropdownOptionTextsAsync();
        }

        public async Task SelectRandomIndustryOptionAsync()
        {
            await IndustryDropdown.ClickAsync();
            await SelectRandomOpenDropdownOptionAsync();
        }

        public async Task<IReadOnlyList<string>> GetBusinessTypeDropdownOptionsAsync()
        {
            await BusinessTypeDropdown.ClickAsync();
            return await GetOpenDropdownOptionTextsAsync();
        }

        public async Task SelectRandomBusinessTypeOptionAsync()
        {
            await BusinessTypeDropdown.ClickAsync();
            await SelectRandomOpenDropdownOptionAsync();
        }

        public async Task<IReadOnlyList<string>> GetCountryDropdownOptionsAsync()
        {
            await CountryDropdown.ClickAsync();
            return await GetOpenDropdownOptionTextsAsync();
        }

        public async Task SelectRandomCountryOptionAsync()
        {
            await CountryDropdown.ClickAsync();
            await SelectRandomOpenDropdownOptionAsync();
        }

        private async Task<IReadOnlyList<string>> GetOpenDropdownOptionTextsAsync()
        {
            await OpenDropdownOptions.First.WaitForAsync();
            var options = await OpenDropdownOptions.AllInnerTextsAsync();
            return options.Select(option => option.Trim()).ToList();
        }

        private async Task SelectRandomOpenDropdownOptionAsync()
        {
            await OpenDropdownOptions.First.WaitForAsync();
            var count = await OpenDropdownOptions.CountAsync();
            var randomOptionIndex = Random.Shared.Next(count);
            await OpenDropdownOptions.Nth(randomOptionIndex).ClickAsync();
        }
    }
}
