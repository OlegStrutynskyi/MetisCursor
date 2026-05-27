using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }

        protected BasePage(IPage page)
        {
            Page = page;
        }

        private ILocator PageTitle => Page.Locator("//h1").First;
        private ILocator GridEmptyMessage => Page.Locator("//tr/td//span[contains(., 'No')]");
        private ILocator GridPaginatorNextPageArrow => Page.Locator("//div[contains(@class,'e-next e-icons')]");
        private ILocator GridTotalRecords => Page.Locator("//span[contains(@class,'e-pagecountmsg')]");
        protected ILocator SearchIcon => Page.Locator("//span[contains(@class,'e-search')]");
        protected ILocator SortIcon => Page.Locator("//button[contains(@class,'sort-dropdown')]");
        protected ILocator CancelBtn => Page.Locator("//button[normalize-space()='Cancel']");
        protected ILocator SaveBtn => Page.Locator("//button[normalize-space()='Save']");
        protected ILocator ModalTitle => Page.Locator("//div[@role='dialog']//h3").First;

        // The paginator caption looks like "1 of 1 pages (3 items)" — we only care about the
        // count inside the parentheses. Pre-compiled regex avoids per-call allocations.
        private static readonly Regex GridTotalRecordsRegex = new(@"\((\d+)\s+items?\)", RegexOptions.Compiled);

        public ILocator GetPageTitleLocator() => PageTitle;

        public async Task<string> GetPageTitleTextAsync()
        {
            await Expect(PageTitle).ToBeVisibleAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }

        public Task<bool> IsGridEmptyAsync() => GridEmptyMessage.IsVisibleAsync();

        public async Task<string> GetGridEmptyMessageAsync()
        {
            await Expect(GridEmptyMessage).ToContainTextAsync("No");
            return (await GridEmptyMessage.TextContentAsync()) ?? string.Empty;
        }

        // Exposed to derived pages that paginate through grids (e.g. DashboardPage).
        protected ILocator GetGridPaginatorNextPageArrowLocator() => GridPaginatorNextPageArrow;

        public ILocator GetSearchIcon() => SearchIcon;
        public Task ClickSearchIconAsync() => SearchIcon.ClickAsync();
        public ILocator GetSortIcon() => SortIcon;
        public Task ClickSortIconAsync() => SortIcon.ClickAsync();
        public ILocator GetCancelBtn() => CancelBtn;
        public Task ClickCancelBtnAsync() => CancelBtn.ClickAsync();
        public ILocator GetSaveBtn() => SaveBtn;
        public Task ClickSaveBtnAsync() => SaveBtn.ClickAsync();
        public ILocator GetModalTitle() => ModalTitle;

        public async Task<string> GetModalTitleTextAsync()
        {
            await Expect(ModalTitle).ToBeVisibleAsync();
            return (await ModalTitle.TextContentAsync()) ?? string.Empty;
        }

        public async Task<int> GetGridTotalRecordsAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            await Expect(GridTotalRecords).ToContainTextAsync("item");

            var full = (await GridTotalRecords.TextContentAsync()) ?? string.Empty;
            var match = GridTotalRecordsRegex.Match(full);
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }
    }
}
