using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class AssetTypesPage : BasePage
    {
        public AssetTypesPage(IPage page) : base(page) { }

        private ILocator CreateAssetTypeBtn => Page.GetByRole(AriaRole.Button, new() { Name = "Create Asset Type" });

        //grid
        private ILocator GridAssetTypeName => Page.Locator("//tr/td[1]//span");

        //Asset Type Settings modal
        private ILocator AssetTypeSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Asset Type Settings')]").First;
        private ILocator AssetTypeSettingsNameInput => Page.Locator("//label[normalize-space()='Name']/following-sibling::div//input");
        private ILocator AssetTypeSettingsNameError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Name']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator AssetTypeSettingsCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator AssetTypeSettingsSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save']").First;
        private ILocator AssetTypeSettingsCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;

        public async Task OpenForAutoTests1Async()
        {
            var resourceManagerPage = new ResourceManagerPage(Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickManageAssetTypesBtnAsync();
        }
        public async Task OpenForEmptyTenantAsync()
        {
            var resourceManagerPage = new ResourceManagerPage(Page);
            await resourceManagerPage.OpenForEmptyTenantAsync();
            await resourceManagerPage.ClickManageAssetTypesBtnAsync();
        }
        public async Task OpenCreateAssetTypeModalAsync()
        {
            await CreateAssetTypeBtn.ClickAsync();
            await Expect(AssetTypeSettingsModal).ToBeVisibleAsync();
        }

        //grid
        public async Task<IReadOnlyList<string>> GetAssetTypeNamesAsync()
        {
            await GridAssetTypeName.First.WaitForAsync();
            var names = await GridAssetTypeName.AllInnerTextsAsync();
            return names.Select(name => name.Trim()).ToList();
        }
        public Task<int> GetAssetTypeNamesCountAsync() => GridAssetTypeName.CountAsync();

        //Asset Type Settings modal
        public Task<string> GetAssetTypeSettingsTitleAsync() => GetModalTitleTextAsync();
        public Task FillAssetTypeSettingsNameAsync(string name) => AssetTypeSettingsNameInput.FillAsync(name);
        public Task ClickAssetTypeSettingsCancelAsync() => AssetTypeSettingsCancelBtn.ClickAsync();
        public Task ClickAssetTypeSettingsSaveAsync() => AssetTypeSettingsSaveBtn.ClickAsync();
        public Task ClickAssetTypeSettingsCloseAsync() => AssetTypeSettingsCloseIcon.ClickAsync();
        public Task ExpectAssetTypeSettingsModalVisibleAsync() => Expect(AssetTypeSettingsModal).ToBeVisibleAsync();
        public Task ExpectAssetTypeSettingsControlsVisibleAsync()
        {
            return Task.WhenAll(
                Expect(AssetTypeSettingsNameInput).ToBeVisibleAsync(),
                Expect(AssetTypeSettingsCancelBtn).ToBeVisibleAsync(),
                Expect(AssetTypeSettingsSaveBtn).ToBeVisibleAsync(),
                Expect(AssetTypeSettingsCloseIcon).ToBeVisibleAsync());
        }
        public Task ExpectAssetTypeSettingsNameErrorContainsAsync(string expectedText) =>
            Expect(AssetTypeSettingsNameError).ToContainTextAsync(expectedText);
    }
}
