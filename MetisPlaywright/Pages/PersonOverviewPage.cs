using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class PersonOverviewPage : BasePage
    {
        public PersonOverviewPage(IPage page) : base(page) { }

        // Navigation
        private ILocator PersonRowByEmail(string email) =>
            Page.Locator($"//span[@class='text-pmgrey-600' and normalize-space()='{email}']/ancestor::tr");

        // Page header / default view
        private ILocator ManageScheduleBtn => Page.Locator("//button[normalize-space()='Manage Schedule']");
        private ILocator PersonFirstLastNames => Page.Locator("//div[@class='skeleton-wrapper w-60']/p");
        private ILocator PersonThreeDots => Page.Locator("//div[@class='skeleton-wrapper w-60']/following-sibling::div//span[contains(@class,'e-icons e-more-vertical-1')]");
        private ILocator ContextsTab => Page.Locator("//div[contains(@class,'skeleton-wrapper')]//span[contains(text(),'Contexts')]");
        private ILocator ResourcesTab => Page.Locator("//div[contains(@class,'skeleton-wrapper')]//span[contains(text(),'Resources')]");
        private ILocator PersonalDetailsTab => Page.Locator("//div[contains(@class,'skeleton-wrapper')]//span[contains(text(),'Personal Details')]");

        // 3 Dots
        private ILocator EditPerson => Page.Locator("//p[normalize-space()='Edit Person']");
        private ILocator ChangePassword => Page.Locator("//p[normalize-space()='Change password']");
        private ILocator EditSchedule => Page.Locator("//p[normalize-space()='Edit Schedule']");
        private ILocator NotificationSettings => Page.Locator("//p[normalize-space()='Notification Settings']");
        
        // Contexts tab
        private ILocator AllTab => Page.Locator("//div[contains(text(),'All')]");
        private ILocator InProgressTab => Page.Locator("//div[contains(text(),'In Progress')]");
        private ILocator PendingTab => Page.Locator("//div[contains(text(),'Pending')]");
        private ILocator DelayedTab => Page.Locator("//div[contains(text(),'Delayed')]");
        private ILocator CompletedTab => Page.Locator("//div[contains(text(),'Completed')]");
        private ILocator ArchivedTab => Page.Locator("//div[contains(text(),'Archived')]");
        private ILocator SelectLabelDropdown => Page.Locator("//span[@role='combobox']");
        private ILocator SelectLabelArrow => Page.Locator("//span[@class='e-input-group-icon e-ddl-icon e-icons']");
        private ILocator ContextsGrid => Page.Locator("//div[contains(@class,'sf-grid')]");

        // Resources tab
        private ILocator ResourcesAllTab => Page.Locator("//div[contains(text(),'All')]");
        private ILocator CredentialTab => Page.Locator("//div[contains(text(),'Credential')]");
        private ILocator KnowledgeTab => Page.Locator("//div[contains(text(),'Knowledge')]");
        private ILocator SkillTab => Page.Locator("//div[contains(text(),'Skill')]");
        private ILocator ResourcesArchivedTab => Page.Locator("//div[contains(text(),'Archived')]");
        private ILocator ResourcesGrid => Page.Locator("//div[contains(@class,'sf-grid')]");
        private ILocator AddResourceBtn => Page.Locator("//button[@title='Add Resource']");
        private ILocator ResourcesGridResourceByName(string name) =>
            Page.Locator($"//div[contains(@class,'sf-grid')]//span[normalize-space()='{name}']");
        private ILocator ResourcesGridResourceName => Page.Locator("//div[contains(@class,'sf-grid')]//tr/td[1]//span");
        private ILocator ResourceThreeDots(string resourceName) =>
            Page.Locator($"//span[normalize-space()='{resourceName}']/ancestor::tr//span[contains(@class,'e-more-vertical')]");
        private ILocator ResourceThreeDotsArchive => Page.Locator("//div[@class='e-tip-content']/p[normalize-space()='Archive']");
        private ILocator ResourceThreeDotsUnarchive => Page.Locator("//div[@class='e-tip-content']/p[normalize-space()='Unarchive']");

        // Personal Details tab
        private ILocator Name => Page.Locator("//h1[normalize-space()='Name']/following-sibling::p");
        private ILocator Email => Page.Locator("//h1[normalize-space()='Email']/following-sibling::a");
        private ILocator EmergencyInfo => Page.Locator("//h1[normalize-space()='Emergency Info']/following-sibling::p");
        private ILocator CostPerHour => Page.Locator("//h1[normalize-space()='Cost Per Hour']/following-sibling::div//span[@class='e-editable-value']");
        private ILocator CostPerHourInput => Page.Locator("//h1[normalize-space()='Cost Per Hour']/following-sibling::div//input");
        private ILocator CostPerHourSaveBtn => Page.Locator("//button[@title='Save']");

        // Person Settings modal
        private ILocator PersonSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Person Settings')]").First;

        // Manage Resources modal
        private ILocator ManageResourcesModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Manage Resources')]");
        private ILocator ManageResourcesNameInput => Page.Locator("//label[normalize-space()='Name']/following-sibling::div//input");
        private ILocator ManageResourcesTypeDropdown => Page.Locator("//label[normalize-space()='Type']/following-sibling::div//input");
        private ILocator ManageResourcesTypeDropdownArrow => Page.Locator("//label[normalize-space()='Type']/following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator ManageResourcesTypeDropdownOption => Page.Locator("//li[contains(@class, 'e-list-item')]");
        private ILocator ManageResourcesTypeError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Type']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ManageResourcesResourceDropdown => Page.Locator("//label[normalize-space()='Resource']/following-sibling::div//input");
        private ILocator ManageResourcesResourceDropdownArrow => Page.Locator("//label[normalize-space()='Resource']/following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator ManageResourcesResourceDropdownOption => Page.Locator("//li[contains(@class, 'e-list-item')]");
        private ILocator ManageResourcesResourceError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Resource']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ManageResourcesExpiryInput => Page.Locator("//label[normalize-space()='Expiry']/following-sibling::div//input");
        private ILocator ManageResourcesExpiryError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Expiry']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ManageResourcesLevelDropdown => Page.Locator("//label[normalize-space()='Level']/following-sibling::div//input");
        private ILocator ManageResourcesLevelDropdownArrow => Page.Locator("//label[normalize-space()='Level']/following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator ManageResourcesLevelDropdownOption => Page.Locator("//li[contains(@class, 'e-list-item')]");
        private ILocator ManageResourcesLevelError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Level']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator ManageResourcesCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator ManageResourcesSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save']").First;
        private ILocator ManageResourcesCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;
        private ILocator DropdownOptionInOpenPopupByText(string option) =>
            Page.Locator("//div[contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]")
                .Filter(new() { HasTextString = option });

        // Navigation
        public async Task OpenForAutoTests1Async()
        {
            var expectedFullName = $"{Config.AutoTests1FirstName} {Config.AutoTests1LastName}";

            var peoplePage = new PeoplePage(Page);
            await peoplePage.OpenForAutoTests1Async();

            var personRow = PersonRowByEmail(Config.CorrectEmailAutoTests1);
            await personRow.ScrollIntoViewIfNeededAsync();
            await personRow.ClickAsync();

            await Expect(GetPageTitleLocator()).ToHaveTextAsync(expectedFullName);
        }

        public async Task OpenForAutoTests2Async()
        {
            var expectedFullName = $"{Config.AutoTests2FirstName} {Config.AutoTests2LastName}";

            var peoplePage = new PeoplePage(Page);
            await peoplePage.OpenForAutoTests1Async();

            var personRow = PersonRowByEmail(Config.CorrectEmailAutoTests2);
            await personRow.ScrollIntoViewIfNeededAsync();
            await personRow.ClickAsync();

            await Expect(GetPageTitleLocator()).ToHaveTextAsync(expectedFullName);
        }

        // Page header / default view
        public ILocator GetManageScheduleBtn() => ManageScheduleBtn;
        public Task ClickManageScheduleBtnAsync() => ManageScheduleBtn.ClickAsync();

        public async Task<string> GetPersonFirstLastNamesTextAsync()
        {
            await Expect(PersonFirstLastNames).ToBeVisibleAsync();
            return (await PersonFirstLastNames.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetPersonOverviewTitleAsync() => await GetPageTitleTextAsync();

        public Task ExpectDefaultViewVisibleAsync() => Task.WhenAll(
            Expect(ManageScheduleBtn).ToBeVisibleAsync(),
            Expect(PersonFirstLastNames).ToBeVisibleAsync(),
            Expect(PersonThreeDots).ToBeVisibleAsync(),
            Expect(ContextsTab).ToBeVisibleAsync(),
            Expect(ResourcesTab).ToBeVisibleAsync(),
            Expect(PersonalDetailsTab).ToBeVisibleAsync());

        public ILocator GetPersonThreeDots() => PersonThreeDots;
        public Task ClickPersonThreeDotsAsync() => PersonThreeDots.ClickAsync();

        public ILocator GetContextsTab() => ContextsTab;
        public Task ClickContextsTabAsync() => ContextsTab.ClickAsync();

        public ILocator GetResourcesTab() => ResourcesTab;
        public Task ClickResourcesTabAsync() => ResourcesTab.ClickAsync();

        public ILocator GetPersonalDetailsTab() => PersonalDetailsTab;
        public Task ClickPersonalDetailsTabAsync() => PersonalDetailsTab.ClickAsync();


        // 3 Dots
        public ILocator GetEditPerson() => EditPerson;
        public Task ClickEditPersonAsync() => EditPerson.ClickAsync();
        public ILocator GetChangePassword() => ChangePassword;
        public Task ClickChangePasswordAsync() => ChangePassword.ClickAsync();
        public ILocator GetEditSchedule() => EditSchedule;
        public Task ClickEditScheduleAsync() => EditSchedule.ClickAsync();
        public ILocator GetNotificationSettings() => NotificationSettings;
        public Task ClickNotificationSettingsAsync() => NotificationSettings.ClickAsync();

        public Task ExpectThreeDotsMenuOptionsVisibleAsync() => Task.WhenAll(
            Expect(EditPerson).ToBeVisibleAsync(),
            Expect(ChangePassword).ToBeVisibleAsync(),
            Expect(EditSchedule).ToBeVisibleAsync(),
            Expect(NotificationSettings).ToBeVisibleAsync());

        // Contexts tab
        public ILocator GetAllTab() => AllTab;
        public Task ClickAllTabAsync() => AllTab.ClickAsync();
        public ILocator GetInProgressTab() => InProgressTab;
        public Task ClickInProgressTabAsync() => InProgressTab.ClickAsync();
        public ILocator GetPendingTab() => PendingTab;
        public Task ClickPendingTabAsync() => PendingTab.ClickAsync();
        public ILocator GetDelayedTab() => DelayedTab;
        public Task ClickDelayedTabAsync() => DelayedTab.ClickAsync();
        public ILocator GetCompletedTab() => CompletedTab;
        public Task ClickCompletedTabAsync() => CompletedTab.ClickAsync();
        public ILocator GetArchivedTab() => ArchivedTab;
        public Task ClickArchivedTabAsync() => ArchivedTab.ClickAsync();
        public ILocator GetSelectLabelDropdown() => SelectLabelDropdown;
        public Task ClickSelectLabelDropdownAsync() => SelectLabelDropdown.ClickAsync();
        public ILocator GetSelectLabelArrow() => SelectLabelArrow;
        public Task ClickSelectLabelArrowAsync() => SelectLabelArrow.ClickAsync();
        public ILocator GetContextsGrid() => ContextsGrid;

        public Task ExpectContextsDefaultViewVisibleAsync() => Task.WhenAll(
            Expect(AllTab).ToBeVisibleAsync(),
            Expect(InProgressTab).ToBeVisibleAsync(),
            Expect(PendingTab).ToBeVisibleAsync(),
            Expect(DelayedTab).ToBeVisibleAsync(),
            Expect(CompletedTab).ToBeVisibleAsync(),
            Expect(ArchivedTab).ToBeVisibleAsync(),
            Expect(SelectLabelDropdown).ToBeVisibleAsync(),
            Expect(SearchIcon).ToBeVisibleAsync(),
            Expect(SortIcon).ToBeVisibleAsync());

        // Resources tab
        public ILocator GetResourcesAllTab() => ResourcesAllTab;
        public Task ClickResourcesAllTabAsync() => ResourcesAllTab.ClickAsync();
        public ILocator GetCredentialTab() => CredentialTab;
        public Task ClickCredentialTabAsync() => CredentialTab.ClickAsync();
        public ILocator GetKnowledgeTab() => KnowledgeTab;
        public Task ClickKnowledgeTabAsync() => KnowledgeTab.ClickAsync();
        public ILocator GetSkillTab() => SkillTab;
        public Task ClickSkillTabAsync() => SkillTab.ClickAsync();
        public ILocator GetResourcesArchivedTab() => ResourcesArchivedTab;
        public Task ClickResourcesArchivedTabAsync() => ResourcesArchivedTab.ClickAsync();
        public ILocator GetAddResourceBtn() => AddResourceBtn;
        public Task ClickAddResourceBtnAsync() => AddResourceBtn.ClickAsync();

        public Task ExpectResourceDisplayedInResourcesGridAsync(string resourceName) =>
            Expect(ResourcesGridResourceByName(resourceName)).ToBeVisibleAsync();

        public Task ExpectResourceNotDisplayedInResourcesGridAsync(string resourceName) =>
            Expect(ResourcesGridResourceByName(resourceName)).ToHaveCountAsync(0);

        public async Task<IReadOnlyList<string>> GetResourcesGridNamesAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            var names = await ResourcesGridResourceName.AllInnerTextsAsync();
            return names.Select(name => name.Trim()).Where(name => name.Length > 0).ToList();
        }

        public async Task ClickResourceThreeDotsArchiveAsync(string resourceName)
        {
            await ResourceThreeDots(resourceName).ClickAsync();
            await ResourceThreeDotsArchive.ClickAsync();
        }

        public async Task ClickResourceThreeDotsUnarchiveAsync(string resourceName)
        {
            await ResourceThreeDots(resourceName).ClickAsync();
            await ResourceThreeDotsUnarchive.ClickAsync();
        }

        public Task ExpectResourcesDefaultViewVisibleAsync() => Task.WhenAll(
            Expect(ResourcesAllTab).ToBeVisibleAsync(),
            Expect(CredentialTab).ToBeVisibleAsync(),
            Expect(KnowledgeTab).ToBeVisibleAsync(),
            Expect(SkillTab).ToBeVisibleAsync(),
            Expect(ResourcesArchivedTab).ToBeVisibleAsync(),
            Expect(ResourcesGrid).ToBeVisibleAsync(),
            Expect(AddResourceBtn).ToBeVisibleAsync(),
            Expect(SearchIcon).ToBeVisibleAsync(),
            Expect(SortIcon).ToBeVisibleAsync());

        // Personal Details tab
        public async Task<string> GetNameTextAsync()
        {
            await Expect(Name).ToBeVisibleAsync();
            return (await Name.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetEmailTextAsync()
        {
            await Expect(Email).ToBeVisibleAsync();
            return (await Email.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetEmergencyInfoTextAsync()
        {
            await Expect(EmergencyInfo).ToBeVisibleAsync();
            return (await EmergencyInfo.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetCostPerHourTextAsync()
        {
            await Expect(CostPerHour).ToBeVisibleAsync();
            return (await CostPerHour.TextContentAsync()) ?? string.Empty;
        }

        public Task ClickCostPerHourAsync() => CostPerHour.ClickAsync();

        public async Task FillCostPerHourAsync(string value)
        {
            await ClickCostPerHourAsync();
            await Expect(CostPerHourInput).ToBeEditableAsync();
            const string script = """
                (el, v) => {
                    el.focus();
                    el.value = v;
                    el.dispatchEvent(new Event('input', { bubbles: true }));
                    el.dispatchEvent(new Event('change', { bubbles: true }));
                }
                """;
            await CostPerHourInput.EvaluateAsync(script, value);
            await CostPerHourInput.PressAsync("Tab");
        }

        public async Task ClearCostPerHourInputAsync()
        {
            await ClickCostPerHourAsync();
            await Expect(CostPerHourInput).ToBeEditableAsync();
            const string script = """
                (el) => {
                    el.focus();
                    el.value = '';
                    el.dispatchEvent(new Event('input', { bubbles: true }));
                    el.dispatchEvent(new Event('change', { bubbles: true }));
                }
                """;
            await CostPerHourInput.EvaluateAsync(script);
            await CostPerHourInput.PressAsync("Tab");
        }

        public ILocator GetCostPerHourSaveBtn() => CostPerHourSaveBtn;
        public Task ClickCostPerHourSaveBtnAsync() => CostPerHourSaveBtn.ClickAsync();

        public Task ExpectCostPerHourTextAsync(string expectedText) =>
            Expect(CostPerHour).ToHaveTextAsync(expectedText);

        // Person Settings modal
        public Task<string> GetPersonSettingsTitleAsync() => GetModalTitleTextAsync();

        public async Task ExpectPersonSettingsModalVisibleAsync()
        {
            await Expect(PersonSettingsModal).ToBeVisibleAsync();
        }

        // Manage Resources modal
        public async Task ExpectManageResourcesModalVisibleAsync()
        {
            await Expect(ManageResourcesModal).ToBeVisibleAsync();
        }

        public Task<string> GetManageResourcesTitleAsync() => GetModalTitleTextAsync();

        public async Task<string> GetManageResourcesNameValueAsync()
        {
            await Expect(ManageResourcesNameInput).ToBeVisibleAsync();
            return await ManageResourcesNameInput.InputValueAsync();
        }

        public Task ExpectManageResourcesNameInputDisabledAsync() => Expect(ManageResourcesNameInput).ToBeDisabledAsync();

        public Task ExpectManageResourcesAddModalControlsVisibleAsync() => Task.WhenAll(
            Expect(ManageResourcesTypeDropdown).ToBeVisibleAsync(),
            Expect(ManageResourcesResourceDropdown).ToBeVisibleAsync(),
            Expect(ManageResourcesCancelBtn).ToBeVisibleAsync(),
            Expect(ManageResourcesSaveBtn).ToBeVisibleAsync());

        public async Task FillManageResourcesNameAsync(string value)
        {
            await Expect(ManageResourcesNameInput).ToBeEditableAsync();
            await ManageResourcesNameInput.ClickAsync();
            await ManageResourcesNameInput.PressSequentiallyAsync(value, new() { Delay = 30 });
            await Expect(ManageResourcesNameInput).ToHaveValueAsync(value);
        }

        public async Task<string> GetManageResourcesTypeErrorAsync()
        {
            await ManageResourcesTypeError.WaitForAsync();
            return (await ManageResourcesTypeError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetManageResourcesResourceErrorAsync()
        {
            await ManageResourcesResourceError.WaitForAsync();
            return (await ManageResourcesResourceError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetManageResourcesExpiryErrorAsync()
        {
            await ManageResourcesExpiryError.WaitForAsync();
            return (await ManageResourcesExpiryError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetManageResourcesLevelErrorAsync()
        {
            await ManageResourcesLevelError.WaitForAsync();
            return (await ManageResourcesLevelError.TextContentAsync()) ?? string.Empty;
        }

        public async Task FillManageResourcesExpiryAsync(string value)
        {
            await Expect(ManageResourcesExpiryInput).ToBeEditableAsync();
            await ManageResourcesExpiryInput.ClickAsync();
            await ManageResourcesExpiryInput.PressAsync("Control+A");
            await ManageResourcesExpiryInput.FillAsync(value);
            await ManageResourcesExpiryInput.PressAsync("Tab");
            await Expect(ManageResourcesExpiryInput).ToHaveValueAsync(value, new() { Timeout = 15_000 });
        }

        public Task ClickManageResourcesCancelAsync() => ManageResourcesCancelBtn.ClickAsync();
        public Task ClickManageResourcesSaveAsync() => ManageResourcesSaveBtn.ClickAsync();
        public Task ClickManageResourcesCloseAsync() => ManageResourcesCloseIcon.ClickAsync();

        public async Task<IReadOnlyList<string>> GetManageResourcesTypeOptionsAsync()
        {
            await ManageResourcesTypeDropdownArrow.ClickAsync();
            await ManageResourcesTypeDropdownOption.First.WaitForAsync();
            return await ManageResourcesTypeDropdownOption.AllInnerTextsAsync();
        }

        public async Task SelectManageResourcesTypeAsync(string option)
        {
            await ManageResourcesTypeDropdownArrow.ClickAsync();
            await DropdownOptionInOpenPopupByText(option).First.ClickAsync();
            if (string.Equals(option, "Skill", StringComparison.Ordinal))
                await Expect(ManageResourcesLevelDropdown).ToBeVisibleAsync();
            else if (string.Equals(option, "Credential", StringComparison.Ordinal))
                await Expect(ManageResourcesExpiryInput).ToBeVisibleAsync();
        }

        public async Task SelectManageResourcesResourceAsync(string option)
        {
            await Expect(ManageResourcesResourceDropdown).ToBeEnabledAsync();
            await ManageResourcesResourceDropdownArrow.ClickAsync();
            var optionLocator = DropdownOptionInOpenPopupByText(option).First;
            await Expect(optionLocator).ToBeVisibleAsync(new() { Timeout = 60_000 });
            await optionLocator.ClickAsync();
        }

        public async Task SelectManageResourcesLevelAsync(string option)
        {
            await ManageResourcesLevelDropdownArrow.ClickAsync();
            await DropdownOptionInOpenPopupByText(option).First.ClickAsync();
        }

        public async Task ExpectManageResourcesControlsVisibleAsync()
        {
            await Expect(ManageResourcesNameInput).ToBeVisibleAsync();
            await Expect(ManageResourcesTypeDropdown).ToBeVisibleAsync();
            await Expect(ManageResourcesResourceDropdown).ToBeVisibleAsync();
            await Expect(ManageResourcesExpiryInput).ToBeVisibleAsync();
            await Expect(ManageResourcesLevelDropdown).ToBeVisibleAsync();
            await Expect(ManageResourcesSaveBtn).ToBeVisibleAsync();
            await Expect(ManageResourcesCancelBtn).ToBeVisibleAsync();
            await Expect(ManageResourcesCloseIcon).ToBeVisibleAsync();
        }
    }
}
