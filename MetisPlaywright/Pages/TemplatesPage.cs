using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class TemplatesPage : BasePage
    {
        public TemplatesPage(IPage page) : base(page) { }

        private ILocator CreateNewTemplateBtn => Page.Locator("//button[normalize-space()='Create New Template']");

        private ILocator GridTemplateName => Page.Locator("//tr/td[1]//span");

        private ILocator TemplateSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Template')]").First;
        private ILocator TemplateSettingsNameInput => Page.Locator("//label[normalize-space()='Template Name']/following-sibling::div//input");
        private ILocator TemplateSettingsNameError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Template Name']/following-sibling::div//div[contains(@class,'text-red-600')]");
        private ILocator TemplateSettingsDescriptionInput => Page.Locator("//label[normalize-space()='Template Description']/following-sibling::div//textarea");
        private ILocator TemplateSettingsCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator TemplateSettingsSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save Template']").First;
        private ILocator TemplateSettingsCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickTemplatesIconAsync();
            await GetPageTitleLocator().WaitForAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Templates Overview");
        }

        public async Task<string> GetTemplatesPageTitleAsync()
        {
            //page title is rendered as multi-line: optional preamble then "Templates Overview"; return only the trailing meaningful line
            var full = await GetPageTitleTextAsync();
            return full.Contains('\n') ? full.Split('\n')[1].Trim() : full;
        }

        public async Task ClickCreateNewTemplateBtnAsync()
        {
            await CreateNewTemplateBtn.ClickAsync();
            await Expect(TemplateSettingsModal).ToBeVisibleAsync();
        }

        public async Task<IReadOnlyList<string>> GetTemplateNamesAsync()
        {
            //Syncfusion grid may briefly keep rows attached but hidden right after save; settle delay
            //is more reliable than WaitForAsync(visible) here
            await Page.WaitForTimeoutAsync(1000);
            var texts = await GridTemplateName.AllInnerTextsAsync();
            return texts
                .Select(t => t.Trim())
                .Where(t => t.Length > 0 && !t.Contains("No templates records"))
                .ToList();
        }

        public async Task<int> GetTemplateNamesCountAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            return await GridTemplateName.CountAsync();
        }

        public Task<string> GetTemplateSettingsTitleAsync() => GetModalTitleTextAsync();

        public async Task<string> GetTemplateSettingsNameErrorAsync()
        {
            await TemplateSettingsNameError.WaitForAsync();
            return (await TemplateSettingsNameError.TextContentAsync()) ?? string.Empty;
        }

        public Task FillTemplateSettingsNameAsync(string value) => TemplateSettingsNameInput.FillAsync(value);
        public Task FillTemplateSettingsDescriptionAsync(string value) => TemplateSettingsDescriptionInput.FillAsync(value);
        public Task ClickTemplateSettingsCancelAsync() => TemplateSettingsCancelBtn.ClickAsync();
        public Task ClickTemplateSettingsSaveAsync() => TemplateSettingsSaveBtn.ClickAsync();
        public Task ClickTemplateSettingsCloseAsync() => TemplateSettingsCloseIcon.ClickAsync();

        public async Task ExpectTemplateSettingsModalVisibleAsync()
        {
            await Expect(TemplateSettingsModal).ToBeVisibleAsync();
        }

        public async Task ExpectTemplateSettingsControlsVisibleAsync()
        {
            await Expect(TemplateSettingsNameInput).ToBeVisibleAsync();
            await Expect(TemplateSettingsDescriptionInput).ToBeVisibleAsync();
            await Expect(TemplateSettingsCancelBtn).ToBeVisibleAsync();
            await Expect(TemplateSettingsSaveBtn).ToBeVisibleAsync();
            await Expect(TemplateSettingsCloseIcon).ToBeVisibleAsync();
        }
    }
}
