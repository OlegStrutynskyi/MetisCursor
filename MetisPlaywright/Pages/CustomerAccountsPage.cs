using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class CustomerAccountsPage : BasePage
    {
        public CustomerAccountsPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;
        private ILocator CreateCustomerAccountBtn => Page.Locator("//button[normalize-space()='Create Customer Account']").First;

        //grid
        private ILocator GridEmptyMessage => Page.Locator("//tr/td//span[contains(., 'No')]");
        private ILocator GridCustomerName => Page.Locator("//tr/td[1]//span[@class='ml-4 truncate text-base']");
        private ILocator GridTotalRecords => Page.Locator("//span[contains(@class,'e-pagecountmsg')]");



        public async Task OpenForEmptyTenantAsync()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickCustomerAccountsIconAsync();
            await PageTitle.WaitForAsync();
            await Expect(PageTitle).ToHaveTextAsync("Customer Accounts");
        }

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickCustomerAccountsIconAsync();
            await PageTitle.WaitForAsync();
            await Expect(PageTitle).ToHaveTextAsync("Customer Accounts");
        }

        public async Task<string> GetPageTitleAsync()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }

        public async Task<CompanyDetailsPage> ClickCreateCustomerAccountBtnAsync()
        {
            await CreateCustomerAccountBtn.ClickAsync();
            await PageTitle.WaitForAsync();
            await Expect(PageTitle).ToHaveTextAsync("Create New Customer Account");
            return new CompanyDetailsPage(Page);
        }


        //grid
        public async Task<string> GetCustomerGridEmptyMessageAsync()
        {
            await GridEmptyMessage.WaitForAsync();
            await Expect(GridEmptyMessage).ToContainTextAsync("No");
            return (await GridEmptyMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<IReadOnlyList<string>> GetCustomerNamesAsync()
        {
            await GridCustomerName.First.WaitForAsync();
            return await GridCustomerName.AllInnerTextsAsync();
        }

        public Task<int> GetCustomerNamesCountAsync()
        {
            return GridCustomerName.CountAsync();
        }

        public async Task<int> GetCustomerGridTotalRecordsAsync()
        {
            var text = await GridTotalRecords.TextContentAsync();
            var countText = text?.Split('(').ElementAtOrDefault(1)?.Split(' ').FirstOrDefault();
            var count = int.Parse(countText ?? "0");
            return count;
        }
    }
}


