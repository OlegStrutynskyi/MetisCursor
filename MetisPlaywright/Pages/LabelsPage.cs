using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class LabelsPage : BasePage
    {
        public LabelsPage(IPage page) : base(page) { }

        private ILocator CreateLabelBtn => Page.Locator("//button[normalize-space()='Create Label']");

        private ILocator GridLabelName => Page.Locator("//tr/td[2]//span");

        private ILocator CreateLabelModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Label')]").First;
        private ILocator CreateLabelNameInput => Page.Locator("//label[normalize-space()='Name']/../following-sibling::div//input");
        private ILocator CreateLabelNameError => Page.Locator("//label[normalize-space()='Name']/../../following-sibling::div[contains(@class,'validation-message')]");
        private ILocator CreateLabelColorSelected => Page.Locator("//span[@class='e-split-preview']");
        private ILocator CreateLabelColorArrow => Page.Locator("//span[contains(@class, 'e-btn-icon e-icons')]");
        private ILocator CreateLabelColorOption => Page.Locator("//span[contains(@class, 'e-tile')]");
        private ILocator CreateLabelColorApplyBtn => Page.Locator("//button[normalize-space()='Apply']");
        private ILocator CreateLabelColorCancelBtn => Page.Locator("//button[@title='Cancel']");
        private ILocator CreateLabelCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator CreateLabelSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Create']").First;
        private ILocator CreateLabelCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickLabelsIconAsync();
            await GetPageTitleLocator().WaitForAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Labels");
        }

        public async Task<string> GetLabelsPageTitleAsync()
        {
            //page title is rendered as multi-line: optional preamble then "Labels"; return only the meaningful trailing line
            var full = await GetPageTitleTextAsync();
            return full.Contains('\n') ? full.Split('\n')[1].Trim() : full;
        }

        public async Task ClickCreateLabelBtnAsync()
        {
            await CreateLabelBtn.ClickAsync();
            await Expect(CreateLabelModal).ToBeVisibleAsync();
        }

        public async Task<IReadOnlyList<string>> GetLabelNamesAsync()
        {
            await GridLabelName.First.WaitForAsync();
            var texts = await GridLabelName.AllInnerTextsAsync();
            return texts
                .Select(t => t.Trim())
                .Where(t => t.Length > 0 && !t.Contains("No label records"))
                .ToList();
        }

        public async Task<int> GetLabelNamesCountAsync()
        {
            await GridLabelName.First.WaitForAsync();
            return await GridLabelName.CountAsync();
        }

        public Task<string> GetCreateLabelTitleAsync() => GetModalTitleTextAsync();

        public async Task<string> GetCreateLabelNameErrorAsync()
        {
            await CreateLabelNameError.WaitForAsync();
            return (await CreateLabelNameError.TextContentAsync()) ?? string.Empty;
        }

        public Task FillCreateLabelNameAsync(string value) => CreateLabelNameInput.FillAsync(value);
        public Task ClickCreateLabelColorArrowAsync() => CreateLabelColorArrow.ClickAsync();
        public Task ClickCreateLabelColorApplyAsync() => CreateLabelColorApplyBtn.ClickAsync();
        public Task ClickCreateLabelColorCancelAsync() => CreateLabelColorCancelBtn.ClickAsync();
        public Task ClickCreateLabelCancelAsync() => CreateLabelCancelBtn.ClickAsync();
        public Task ClickCreateLabelSaveAsync() => CreateLabelSaveBtn.ClickAsync();
        public Task ClickCreateLabelCloseAsync() => CreateLabelCloseIcon.ClickAsync();

        public async Task ClickRandomCreateLabelColorAsync()
        {
            var optionsCount = await CreateLabelColorOption.CountAsync();
            if (optionsCount == 0)
            {
                throw new InvalidOperationException("No color options available");
            }
            var randomIndex = Random.Shared.Next(0, optionsCount);
            await CreateLabelColorOption.Nth(randomIndex).ClickAsync();
        }

        public async Task ExpectCreateLabelModalVisibleAsync()
        {
            await Expect(CreateLabelModal).ToBeVisibleAsync();
        }

        public async Task ExpectCreateLabelControlsVisibleAsync()
        {
            await Expect(CreateLabelNameInput).ToBeVisibleAsync();
            await Expect(CreateLabelColorSelected).ToBeVisibleAsync();
            await Expect(CreateLabelColorArrow).ToBeVisibleAsync();
            await Expect(CreateLabelCancelBtn).ToBeVisibleAsync();
            await Expect(CreateLabelSaveBtn).ToBeVisibleAsync();
            await Expect(CreateLabelCloseIcon).ToBeVisibleAsync();
        }
    }
}
