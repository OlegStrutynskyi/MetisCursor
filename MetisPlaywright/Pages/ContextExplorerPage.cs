using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class ContextExplorerPage : BasePage
    {
        public ContextExplorerPage(IPage page) : base(page) { }

        private ILocator CreateNewContextBtn => Page.Locator("//button[normalize-space()='Create New Context']");
        private ILocator CreateNewCustomerAccountBtn => Page.Locator("//button[normalize-space()='Create New Customer Account']");
        private ILocator LabelFilter => Page.Locator("//input[@placeholder='Select label']");
        private ILocator LabelFilterArrow => Page.Locator("//input[@placeholder='Select label']/following-sibling::span[contains(@class,'e-icons')]");
        private ILocator OpenDropdownOptions => Page.Locator("//div[contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]");
        private ILocator AccountFilter => Page.Locator("//input[@placeholder='Account']");
        private ILocator DateRangeFilter => Page.Locator("//input[@placeholder='Date range']");
        private ILocator ColumnsBtn => Page.Locator("//button[normalize-space()='Columns']");

        //grid
        private ILocator GridEmptyMessage => Page.Locator("//div[contains(@class,'justify-center text-sm')]");
        private ILocator GridContextName => Page.Locator("//tr/td[3]//span[contains(@class,'text-sm')]");
        private string GridAutoTestContext1NameXPath =>
            $"//span[contains(@class,'text-sm') and normalize-space()='{Config.AutoTestsContext1}']";

        private ILocator GridAutoTestContext1Name => Page.Locator(GridAutoTestContext1NameXPath);
        private ILocator GridAutoTestContext1Status => Page.Locator($"{GridAutoTestContext1NameXPath}/ancestor::td/following-sibling::td[3]//span[@class='e-chip-text']");
        private ILocator GridAutoTestContext1PathfinderBtn => Page.Locator($"{GridAutoTestContext1NameXPath}/ancestor::td/following-sibling::td[6]//button[@title='Open Pathfinder']");
        private ILocator GridAutoTestContext1OpenNeighborhoodBtn => Page.Locator($"{GridAutoTestContext1NameXPath}/ancestor::td/following-sibling::td[6]//button[@title='Open Neighborhood']");
        private ILocator GridAutoTestContext1ThreeDots => Page.Locator($"{GridAutoTestContext1NameXPath}/ancestor::td/following-sibling::td[6]//span[contains(@class,'e-more-vertical')]");
        private ILocator GridAutoTestContext1Arrow => Page.Locator($"{GridAutoTestContext1NameXPath}/ancestor::span/preceding-sibling::span[contains(@class,'e-treegrid')]");
        private ILocator GridContextNameByText(string contextName) =>
            Page.Locator($"//tr/td[3]//span[contains(@class,'text-sm') and normalize-space()='{contextName}']");

        // Details section
        private ILocator DetailsEmptyMessage => Page.Locator("//div[contains(@class,'justify-center') and contains(@class,'border-gray-200')]");
        private ILocator DetailsContextName => Page.Locator("//h2");
        private ILocator DetailsOpenBtn => Page.Locator("//span[normalize-space()='Open']");
        private ILocator DetailsAddChildBtn => Page.Locator("//span[normalize-space()='Add Child']");
        private ILocator DetailsAddDependencyBtn => Page.Locator("//span[normalize-space()='Add Dependency']");
        private ILocator DetailsContextStatus => Page.Locator("//span[normalize-space()='Status:']/following-sibling::div//span[@class='e-chip-text']");
        private ILocator DetailsContextLabel => Page.Locator("//span[normalize-space()='Label:']/following-sibling::div//span[@class='e-chip-text']");
        private ILocator DetailsAddDependencyModal => Page.Locator("//div[contains(@class,'modal-content relative')]");
        private ILocator DetailsAddDependencyModalTitle => Page.Locator("//div[contains(@class,'modal-content relative')]//h3");
        private ILocator DetailsChildrenGrid => Page.Locator("//div[contains(@class,'rounded-md')]//div[contains(@class,'sf-grid')]");
        private ILocator DetailsChildrenGridContextName => Page.Locator("//div[contains(@class,'rounded-md')]//div[contains(@class,'sf-grid')]//span[contains(@class,'font-medium')]");

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickContextExplorerIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Contexts");
        }

        public async Task OpenForEmptyTenantAsync()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickContextExplorerIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Contexts");
        }

        public async Task<string> GetContextExplorerPageTitleAsync()
        {
            var full = await GetPageTitleTextAsync();
            if (full.Contains('\n'))
            {
                return full.Split('\n')[1].Trim();
            }
            return full;
        }

        public async Task<BuilderPage> ClickCreateNewContextBtnAsync()
        {
            await CreateNewContextBtn.ClickAsync();
            var builderPage = new BuilderPage(Page);
            await builderPage.ExpectDefaultControlsVisibleAsync();
            return builderPage;
        }

        public async Task<CreateCompanyPage> ClickCreateNewCustomerAccountBtnAsync()
        {
            await CreateNewCustomerAccountBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToContainTextAsync("Create New Customer Account");
            return new CreateCompanyPage(Page);
        }

        public Task ExpectDefaultControlsVisibleAsync() => Task.WhenAll(
            Expect(CreateNewContextBtn).ToBeVisibleAsync(),
            Expect(CreateNewCustomerAccountBtn).ToBeVisibleAsync(),
            Expect(LabelFilter).ToBeVisibleAsync(),
            Expect(AccountFilter).ToBeVisibleAsync(),
            Expect(DateRangeFilter).ToBeVisibleAsync());

        public Task ExpectColumnsButtonVisibleAsync() => Expect(ColumnsBtn).ToBeVisibleAsync();

        public async Task<IReadOnlyList<string>> GetLabelFilterOptionsAsync()
        {
            await LabelFilterArrow.ClickAsync();
            await OpenDropdownOptions.First.WaitForAsync();
            var options = await OpenDropdownOptions.AllInnerTextsAsync();
            return options.Select(option => option.Trim()).ToList();
        }

        //grid
        public async Task<IReadOnlyList<string>> GetContextNamesAsync()
        {
            await GridContextName.First.WaitForAsync();
            var names = await GridContextName.AllInnerTextsAsync();
            return names
                .Select(name => name.Trim())
                .Where(name => !name.Contains("No label records"))
                .ToList();
        }

        public Task<int> GetContextNamesCountAsync() => GridContextName.CountAsync();

        public async Task<int> GetGridRecordsCountAsync()
        {
            await Page.WaitForTimeoutAsync(2000);
            if (await GridEmptyMessage.IsVisibleAsync())
            {
                return 0;
            }
            return await GetContextNamesCountAsync();
        }

        public ILocator GetGridEmptyMessage() => GridEmptyMessage;

        public async Task<string> GetGridEmptyMessageTextAsync()
        {
            await Expect(GridEmptyMessage).ToBeVisibleAsync();
            return (await GridEmptyMessage.TextContentAsync()) ?? string.Empty;
        }

        public ILocator GetGridAutoTestContext1Name() => GridAutoTestContext1Name;
        public ILocator GetGridAutoTestContext1Status() => GridAutoTestContext1Status;
        public ILocator GetGridAutoTestContext1PathfinderBtn() => GridAutoTestContext1PathfinderBtn;
        public ILocator GetGridAutoTestContext1OpenNeighborhoodBtn() => GridAutoTestContext1OpenNeighborhoodBtn;
        public ILocator GetGridAutoTestContext1ThreeDots() => GridAutoTestContext1ThreeDots;

        public async Task<string> GetGridAutoTestContext1NameTextAsync()
        {
            await Expect(GridAutoTestContext1Name).ToBeVisibleAsync();
            return (await GridAutoTestContext1Name.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetGridAutoTestContext1StatusTextAsync()
        {
            await Expect(GridAutoTestContext1Status).ToBeVisibleAsync();
            return (await GridAutoTestContext1Status.TextContentAsync()) ?? string.Empty;
        }

        public Task ClickGridAutoTestContext1NameAsync() => GridAutoTestContext1Name.ClickAsync();
        public Task ClickGridAutoTestContext1ArrowAsync() => GridAutoTestContext1Arrow.ClickAsync();

        public Task ExpectContextVisibleInGridAsync(string contextName) =>
            Expect(GridContextNameByText(contextName)).ToBeVisibleAsync(new() { Timeout = 15_000 });

        public Task ExpectContextHiddenInGridAsync(string contextName) =>
            Expect(GridContextNameByText(contextName)).ToBeHiddenAsync(new() { Timeout = 15_000 });

        public async Task ClickGridContextByNameAsync(string contextName)
        {
            var contextLocator = GridContextNameByText(contextName);
            await Expect(contextLocator).ToBeVisibleAsync(new() { Timeout = 15_000 });
            await contextLocator.ClickAsync();
        }

        public async Task<ContextOverviewPage> ClickGridContextByNameAndOpenContextOverviewAsync(string contextName)
        {
            await ClickGridContextByNameAsync(contextName);
            return await ClickDetailsOpenBtnAndOpenContextOverviewAsync();
        }

        public async Task<ContextOverviewPage> ClickDetailsOpenBtnAndOpenContextOverviewAsync()
        {
            await ClickDetailsOpenBtnAsync();
            var contextOverviewPage = new ContextOverviewPage(Page);
            await contextOverviewPage.ExpectOpenedAsync();
            return contextOverviewPage;
        }

        public async Task<BuilderPage> ClickDetailsAddChildBtnAndOpenBuilderAsync()
        {
            await ClickDetailsAddChildBtnAsync();
            var builderPage = new BuilderPage(Page);
            await builderPage.ExpectOpenedAsync();
            return builderPage;
        }

        public ILocator GetDetailsEmptyMessage() => DetailsEmptyMessage;

        public Task ExpectDetailsEmptyMessageVisibleAsync() => Expect(DetailsEmptyMessage).ToBeVisibleAsync();

        public async Task<string> GetDetailsEmptyMessageTextAsync()
        {
            await Expect(DetailsEmptyMessage).ToBeVisibleAsync();
            return (await DetailsEmptyMessage.TextContentAsync()) ?? string.Empty;
        }

        public ILocator GetDetailsContextName() => DetailsContextName;
        public ILocator GetDetailsOpenBtn() => DetailsOpenBtn;
        public ILocator GetDetailsAddChildBtn() => DetailsAddChildBtn;
        public ILocator GetDetailsAddDependencyBtn() => DetailsAddDependencyBtn;
        public ILocator GetDetailsContextStatus() => DetailsContextStatus;
        public ILocator GetDetailsContextLabel() => DetailsContextLabel;

        public async Task<string> GetDetailsContextNameTextAsync()
        {
            await Expect(DetailsContextName).ToBeVisibleAsync();
            return (await DetailsContextName.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetDetailsContextStatusTextAsync()
        {
            await Expect(DetailsContextStatus).ToBeVisibleAsync();
            return (await DetailsContextStatus.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetDetailsContextLabelTextAsync()
        {
            await Expect(DetailsContextLabel).ToBeVisibleAsync();
            return (await DetailsContextLabel.TextContentAsync()) ?? string.Empty;
        }

        public Task ClickDetailsOpenBtnAsync() => DetailsOpenBtn.ClickAsync();
        public Task ClickDetailsAddChildBtnAsync() => DetailsAddChildBtn.ClickAsync();
        public Task ClickDetailsAddDependencyBtnAsync() => DetailsAddDependencyBtn.ClickAsync();

        public ILocator GetDetailsAddDependencyModal() => DetailsAddDependencyModal;
        public ILocator GetModal() => DetailsAddDependencyModal;

        public async Task<string> GetDetailsAddDependencyModalTitleTextAsync()
        {
            await Expect(DetailsAddDependencyModal).ToBeVisibleAsync();
            await Expect(DetailsAddDependencyModalTitle).ToBeVisibleAsync();
            return (await DetailsAddDependencyModalTitle.TextContentAsync()) ?? string.Empty;
        }

        public Task ExpectDetailsActionButtonsVisibleAsync() => Task.WhenAll(
            Expect(DetailsOpenBtn).ToBeVisibleAsync(),
            Expect(DetailsAddChildBtn).ToBeVisibleAsync(),
            Expect(DetailsAddDependencyBtn).ToBeVisibleAsync());

        public Task ExpectDetailsChildrenGridVisibleAsync() =>
            Expect(DetailsChildrenGrid).ToBeVisibleAsync();

        public async Task<IReadOnlyList<string>> GetDetailsChildrenGridContextNamesAsync()
        {
            await Expect(DetailsChildrenGrid).ToBeVisibleAsync();
            await DetailsChildrenGridContextName.First.WaitForAsync();
            var names = await DetailsChildrenGridContextName.AllInnerTextsAsync();
            return names.Select(name => name.Trim()).ToList();
        }

        public Task ExpectGridAutoTestContext1RowVisibleAsync(string expectedStatus) => Task.WhenAll(
            Expect(GridAutoTestContext1Name).ToBeVisibleAsync(),
            Expect(GridAutoTestContext1Name).ToHaveTextAsync(Config.AutoTestsContext1),
            Expect(GridAutoTestContext1Status).ToHaveTextAsync(expectedStatus),
            Expect(GridAutoTestContext1PathfinderBtn).ToBeVisibleAsync(),
            Expect(GridAutoTestContext1OpenNeighborhoodBtn).ToBeVisibleAsync(),
            Expect(GridAutoTestContext1ThreeDots).ToBeVisibleAsync());
    }
}
