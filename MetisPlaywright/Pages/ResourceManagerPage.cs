using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class ResourceManagerPage : BasePage
    {
        public ResourceManagerPage(IPage page) : base(page) { }

        private ILocator ManageAssetTypesBtn => Page.Locator("//button[normalize-space()='Manage Asset Types']");
        private ILocator CreateResourceBtn => Page.Locator("//button[normalize-space()='Create Resource']");

        private ILocator AllTab => Page.Locator("//div[text()='All']");
        private ILocator AssetTab => Page.Locator("//div[text()='Asset']");
        private ILocator ConsumableTab => Page.Locator("//div[text()='Consumable']");
        private ILocator CredentialTab => Page.Locator("//div[text()='Credential']");
        private ILocator KnowledgeTab => Page.Locator("//div[text()='Knowledge']");
        private ILocator SkillTab => Page.Locator("//div[text()='Skill']");
        private ILocator ArchivedTab => Page.Locator("//div[text()='Archived']");

        private ILocator GridResourceName => Page.Locator("//tr/td[1]//span[@class='truncate text-base']");
        private ILocator GridResourceType => Page.Locator("//td[3]//span[@class='e-chip-text']");
        private ILocator GridResourceStatus => Page.Locator("//td[4]//span[@class='e-chip-text']");
        private ILocator Skill2ThreeDots => Page.Locator("//span[normalize-space()='DONT DELETE Skill 2 ARCHIVED']/ancestor::td/following-sibling::td[4]//span[contains(@class,'e-icons')]");
        private ILocator Skill2ThreeDotsEdit => Page.Locator("//div[@class='e-tip-content']/p[normalize-space()='Edit']");
        private ILocator Skill2ThreeDotsArchive => Page.Locator("//div[@class='e-tip-content']/p[normalize-space()='Archive']");
        private ILocator Skill2ThreeDotsUnarchive => Page.Locator("//div[@class='e-tip-content']/p[normalize-space()='Unarchive']");
        private ILocator Skill2Status => Page.Locator("//span[normalize-space()='DONT DELETE Skill 2 ARCHIVED']/ancestor::td/following-sibling::td[3]//span");

        private ILocator ResourceSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Resource Settings')]").First;
        private ILocator ResourceSettingsNameInput => Page.Locator("//label[normalize-space()='Name']/following-sibling::div//input");
        private ILocator ResourceSettingsNameError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Name']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ResourceSettingsTypeDropdown => Page.Locator("//label[normalize-space()='Type']/following-sibling::div//input");
        private ILocator ResourceSettingsTypeDropdownArrow => Page.Locator("//label[normalize-space()='Type']/following-sibling::div");
        private ILocator ResourceSettingsTypeDropdownOption => Page.Locator("//li[contains(@class, 'e-list-item')]");
        private ILocator ResourceSettingsTypeError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Type']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ResourceSettingsVolumeInStockInput => Page.Locator("//label[normalize-space()='Volume in stock']/following-sibling::div//input");
        private ILocator ResourceSettingsUnitOfMeasureArrow => Page.Locator("//label[normalize-space()='Unit of measure']/following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator ResourceSettingsAssetTypeArrow => Page.Locator("//label[normalize-space()='Asset Type']/following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator ResourceSettingsCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator ResourceSettingsSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save']").First;
        private ILocator ResourceSettingsCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;
        private ILocator DropdownOptionByText(string option) => Page.Locator($"//li[normalize-space()='{option}']");

        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickResourceManagerIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Resource Manager");
        }

        public async Task OpenForEmptyTenantAsync()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickResourceManagerIconAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Resource Manager");
        }

        public async Task<AssetTypesPage> ClickManageAssetTypesBtnAsync()
        {
            await ManageAssetTypesBtn.ClickAsync();
            var assetTypesPage = new AssetTypesPage(Page);
            await Expect(assetTypesPage.GetPageTitleLocator()).ToHaveTextAsync("Asset Types");
            return assetTypesPage;
        }

        public async Task ClickCreateResourceBtnAsync()
        {
            await CreateResourceBtn.ClickAsync();
            await Expect(ResourceSettingsModal).ToBeVisibleAsync();
        }

        public ILocator GetAllTab() => AllTab;
        public ILocator GetAssetTab() => AssetTab;
        public ILocator GetConsumableTab() => ConsumableTab;
        public ILocator GetCredentialTab() => CredentialTab;
        public ILocator GetKnowledgeTab() => KnowledgeTab;
        public ILocator GetSkillTab() => SkillTab;
        public ILocator GetArchivedTab() => ArchivedTab;

        public Task ClickAllTabAsync() => AllTab.ClickAsync();
        public Task ClickAssetTabAsync() => AssetTab.ClickAsync();
        public Task ClickConsumableTabAsync() => ConsumableTab.ClickAsync();
        public Task ClickCredentialTabAsync() => CredentialTab.ClickAsync();
        public Task ClickKnowledgeTabAsync() => KnowledgeTab.ClickAsync();
        public Task ClickSkillTabAsync() => SkillTab.ClickAsync();
        public Task ClickArchivedTabAsync() => ArchivedTab.ClickAsync();
        public Task ClickSkill2ThreeDotsAsync() => Skill2ThreeDots.ClickAsync();

        public async Task ClickSkill2ThreeDotsEditAsync()
        {
            await ClickSkill2ThreeDotsAsync();
            await Skill2ThreeDotsEdit.ClickAsync();
        }

        public async Task ClickSkill2ThreeDotsArchiveAsync()
        {
            await ClickSkill2ThreeDotsAsync();
            await Skill2ThreeDotsArchive.ClickAsync();
        }

        public async Task ClickSkill2ThreeDotsUnarchiveAsync()
        {
            await ClickSkill2ThreeDotsAsync();
            await Skill2ThreeDotsUnarchive.ClickAsync();
        }

        public async Task<string> GetSkill2StatusTextAsync()
        {
            await Expect(Skill2Status).ToBeVisibleAsync();
            return ((await Skill2Status.InnerTextAsync()) ?? string.Empty).Trim();
        }

        private async Task<IReadOnlyList<string>> GetAllGridCellTextsAcrossPagesAsync(ILocator columnLocator)
        {
            // Syncfusion grid may flicker after load; paginator repaints asynchronously between pages.
            await Page.WaitForTimeoutAsync(1000);

            var values = new List<string>();
            var nextPageArrow = GetGridPaginatorNextPageArrowLocator();

            while (true)
            {
                values.AddRange(await columnLocator.AllInnerTextsAsync());

                var classAttribute = await nextPageArrow.GetAttributeAsync("class");
                if (classAttribute != null && classAttribute.Contains("e-disable"))
                {
                    break;
                }

                await nextPageArrow.ClickAsync();
                await Page.WaitForTimeoutAsync(500);
            }

            return values;
        }

        public async Task<IReadOnlyList<string>> GetResourceNamesAsync() =>
            await GetAllGridCellTextsAcrossPagesAsync(GridResourceName);

        public async Task<IReadOnlyList<string>> GetResourceTypeValuesAsync() =>
            await GetAllGridCellTextsAcrossPagesAsync(GridResourceType);

        public async Task<IReadOnlyList<string>> GetResourceStatusValuesAsync() =>
            await GetAllGridCellTextsAcrossPagesAsync(GridResourceStatus);

        public async Task<int> GetResourcesCountAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            return await GridResourceName.CountAsync();
        }

        public Task<string> GetResourceSettingsTitleAsync() => GetModalTitleTextAsync();

        public async Task<string> GetResourceSettingsNameErrorAsync()
        {
            await ResourceSettingsNameError.WaitForAsync();
            return (await ResourceSettingsNameError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetResourceSettingsTypeErrorAsync()
        {
            await ResourceSettingsTypeError.WaitForAsync();
            return (await ResourceSettingsTypeError.TextContentAsync()) ?? string.Empty;
        }

        public async Task FillResourceSettingsNameAsync(string value)
        {
            // Syncfusion + Blazor textbox binding does not reliably pick up a single FillAsync —
            // value stays empty in the model and Save raises "'Name' is required". Wait until the
            // input is actually editable (modal animations / Blazor first render can leave it
            // visible-but-not-yet-ready), then type the value character-by-character so Blazor
            // flushes each input event into its state.
            await Expect(ResourceSettingsNameInput).ToBeEditableAsync();
            await ResourceSettingsNameInput.ClickAsync();
            await ResourceSettingsNameInput.PressSequentiallyAsync(value, new() { Delay = 30 });
            await Expect(ResourceSettingsNameInput).ToHaveValueAsync(value);
        }

        // Syncfusion numeric spinbutton: set DOM value + input/change, then Tab to commit (typing alone is flaky)
        public async Task FillResourceSettingsVolumeAsync(string value)
        {
            await Expect(ResourceSettingsVolumeInStockInput).ToBeVisibleAsync();
            await Expect(ResourceSettingsVolumeInStockInput).ToBeEditableAsync();
            const string script = "(el, v) => { el.focus(); el.value = v; el.dispatchEvent(new Event('input', { bubbles: true })); el.dispatchEvent(new Event('change', { bubbles: true })); }";
            await ResourceSettingsVolumeInStockInput.EvaluateAsync(script, value);
            await ResourceSettingsVolumeInStockInput.PressAsync("Tab");
        }

        public Task ClickResourceSettingsCancelAsync() => ResourceSettingsCancelBtn.ClickAsync();
        public Task ClickResourceSettingsSaveAsync() => ResourceSettingsSaveBtn.ClickAsync();
        public Task ClickResourceSettingsCloseAsync() => ResourceSettingsCloseIcon.ClickAsync();

        public async Task<IReadOnlyList<string>> GetResourceTypeOptionsAsync()
        {
            await ResourceSettingsTypeDropdownArrow.ClickAsync();
            await ResourceSettingsTypeDropdownOption.First.WaitForAsync();
            return await ResourceSettingsTypeDropdownOption.AllInnerTextsAsync();
        }

        public async Task SelectResourceTypeAsync(string option)
        {
            await ResourceSettingsTypeDropdownArrow.ClickAsync();
            await DropdownOptionByText(option).ClickAsync();
        }

        public async Task SelectUnitOfMeasureAsync(string option)
        {
            await ResourceSettingsUnitOfMeasureArrow.ClickAsync();
            await DropdownOptionByText(option).ClickAsync();
        }

        public async Task SelectAssetTypeAsync(string option)
        {
            await ResourceSettingsAssetTypeArrow.ClickAsync();
            await DropdownOptionByText(option).ClickAsync();
        }

        public async Task ExpectResourceSettingsModalVisibleAsync()
        {
            await Expect(ResourceSettingsModal).ToBeVisibleAsync();
        }

        public async Task ExpectResourceSettingsControlsVisibleAsync()
        {
            await Expect(ResourceSettingsNameInput).ToBeVisibleAsync();
            await Expect(ResourceSettingsTypeDropdown).ToBeVisibleAsync();
            await Expect(ResourceSettingsSaveBtn).ToBeVisibleAsync();
            await Expect(ResourceSettingsCancelBtn).ToBeVisibleAsync();
            await Expect(ResourceSettingsCloseIcon).ToBeVisibleAsync();
        }

        public async Task ExpectDefaultTabsAndToolbarVisibleAsync()
        {
            await ExpectVisibleWithDescriptionAsync(AllTab, "tab \"All\"");
            await ExpectVisibleWithDescriptionAsync(AssetTab, "tab \"Asset\"");
            await ExpectVisibleWithDescriptionAsync(ConsumableTab, "tab \"Consumable\"");
            await ExpectVisibleWithDescriptionAsync(CredentialTab, "tab \"Credential\"");
            await ExpectVisibleWithDescriptionAsync(KnowledgeTab, "tab \"Knowledge\"");
            await ExpectVisibleWithDescriptionAsync(SkillTab, "tab \"Skill\"");
            await ExpectVisibleWithDescriptionAsync(ArchivedTab, "tab \"Archived\"");
            await ExpectVisibleWithDescriptionAsync(ManageAssetTypesBtn, "button \"Manage Asset Types\"");
            await ExpectVisibleWithDescriptionAsync(CreateResourceBtn, "button \"Create Resource\"");
        }

        private async Task ExpectVisibleWithDescriptionAsync(ILocator locator, string description)
        {
            try
            {
                await Expect(locator).ToBeVisibleAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Resource Manager default view — not displayed: {description}.", ex);
            }
        }
    }
}
