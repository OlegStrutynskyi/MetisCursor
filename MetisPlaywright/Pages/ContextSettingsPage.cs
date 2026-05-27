using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class ContextSettingsPage : BasePage
    {
        public ContextSettingsPage(IPage page) : base(page) { }

        // modal frame
        private ILocator ContextSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Context Settings')]");

        // tabs
        private ILocator ConfigurationTab => Page.Locator("//div[@role='dialog']//span[normalize-space()='Configuration']").First;
        private ILocator PrivacyTab => Page.Locator("//div[@role='dialog']//span[normalize-space()='Privacy']").First;
        private ILocator LabelsTab => Page.Locator("//div[@role='dialog']//span[normalize-space()='Labels']").First;
        private ILocator NotificationsTab => Page.Locator("//div[@role='dialog']//span[normalize-space()='Notifications']").First;
        private ILocator ChatTab => Page.Locator("//div[@role='dialog']//span[normalize-space()='Chat']").First;

        // footer buttons
        private ILocator CancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator SetupLaterBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Setup later']").First;
        private ILocator CreateBtn => Page.Locator("//div[@role='dialog']//button[contains(normalize-space(),'Create')]").First;
        private ILocator CloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;

        // Configuration tab
        private ILocator ConfigurationTabMessage => Page.Locator("//label[normalize-space()='Context Title']/ancestor::div[contains(@class,'flex items-center gap-2')]/preceding-sibling::div/p");
        private ILocator ContextTitleInput => Page.Locator("//label[normalize-space()='Context Title']/../following-sibling::div//input");
        private ILocator ContextTitleError => Page.Locator("//label[normalize-space()='Context Title']/../../following-sibling::div[contains(@class,'validation-message')]").First;
        private ILocator ContextDescriptionInput => Page.Locator("//label[normalize-space()='Context Description']/../following-sibling::div//textarea");
        private ILocator CustomerAccountDropdown => Page.Locator("//label[normalize-space()='Customer Account']/../following-sibling::div//input");
        private ILocator CustomerAccountArrow => Page.Locator("//label[normalize-space()='Customer Account']/../following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator CustomerAccountDropdownOption => Page.Locator("//li//span[@class='text-base']");
        private ILocator CustomerAccountError => Page.Locator("//label[normalize-space()='Customer Account']/../../following-sibling::div[contains(@class,'validation-message')]").First;
        private ILocator PredecessorsInput => Page.Locator("//label[normalize-space()='Predecessors']/../following-sibling::div//input");
        private ILocator SuccessorsInput => Page.Locator("//label[normalize-space()='Successors']/../following-sibling::div//input");
        private ILocator TopError => Page.Locator("//li[@class='validation-message']");

        // Privacy tab
        private ILocator PrivacyTabMessage => Page.Locator("//p[@class='mb-4 mt-4']");
        private ILocator AutomaticallyAddChildContextUserToggle => Page.Locator("//span[@class='e-switch-off']");
        private ILocator AdministratorsInput => Page.Locator("//label[normalize-space()='Administrators']/../following-sibling::div//input");
        private ILocator ParticipantsInput => Page.Locator("//label[normalize-space()='Participants']/../following-sibling::div//input");
        private ILocator ObserversInput => Page.Locator("//label[normalize-space()='Observers']/../following-sibling::div//input");

        // Labels tab
        private ILocator LabelsTabMessage => Page.Locator("//label[normalize-space()='Labels']/ancestor::div[contains(@class,'items-center')]/preceding-sibling::p");
        private ILocator LabelsInput => Page.Locator("//label[normalize-space()='Labels']/../following-sibling::div//input");
        private ILocator LabelsInputJobOption => Page.Locator("//li[normalize-space()='Job']");
        private ILocator SelectedLabels => Page.Locator("//span[@class='text-xs font-medium']");

        // Notifications tab
        private ILocator NotificationsTabMessage => Page.Locator("//p[@class='mb-4 mt-4']");
        private ILocator StatusUpdatesToggle => Page.Locator("//strong[normalize-space()='Status Updates']/../preceding-sibling::div//span[@class='e-switch-inner']");
        private ILocator AssignmentsNotificationsToggle => Page.Locator("//strong[normalize-space()='Assignment Notifications']/../preceding-sibling::div//span[@class='e-switch-inner']");
        private ILocator FileUploadsToggle => Page.Locator("//strong[normalize-space()='File Uploads']/../preceding-sibling::div//span[@class='e-switch-inner']");

        // Chat tab
        private ILocator ChatTabMessage => Page.Locator("//p[@class='mb-4 mt-4']");
        private ILocator EnableChatToggle => Page.Locator("//strong[normalize-space()='Enable Chat']/../preceding-sibling::div//span[@class='e-switch-inner']");
        
        // modal-level actions
        public Task ClickConfigurationTabAsync() => ConfigurationTab.ClickAsync();
        public Task ClickPrivacyTabAsync() => PrivacyTab.ClickAsync();
        public Task ClickLabelsTabAsync() => LabelsTab.ClickAsync();
        public Task ClickNotificationsTabAsync() => NotificationsTab.ClickAsync();
        public Task ClickChatTabAsync() => ChatTab.ClickAsync();
        public Task ClickCancelAsync() => CancelBtn.ClickAsync();
        public Task ClickSetupLaterAsync() => SetupLaterBtn.ClickAsync();
        public Task ClickCreateAsync() => CreateBtn.ClickAsync();
        public Task ClickCloseAsync() => CloseIcon.ClickAsync();

        public async Task<string> GetCreateBtnTextAsync()
        {
            await Expect(CreateBtn).ToBeVisibleAsync();
            return (await CreateBtn.TextContentAsync()) ?? string.Empty;
        }

        public Task<string> GetContextSettingsTitleAsync() => GetModalTitleTextAsync();

        public async Task<ContextSettingsPage> GetContextSettingsPageAutoTests1Async()
        {
            var builderPage = await new BuilderPage(Page).GetContextBuilderPageAutoTests1Async();
            return await builderPage.ClickCreateNewContextBtnAsync();
        }

        // Configuration tab actions
        public Task FillContextTitleAsync(string title) => ContextTitleInput.FillAsync(title);
        public Task FillContextDescriptionAsync(string description) => ContextDescriptionInput.FillAsync(description);

        public async Task<string> GetContextTitleAsync()
        {
            await Expect(ContextTitleInput).ToBeVisibleAsync();
            return await ContextTitleInput.InputValueAsync();
        }

        public async Task<string> GetConfigurationTabMessageAsync()
        {
            await Expect(ConfigurationTabMessage).ToBeVisibleAsync();
            return (await ConfigurationTabMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetPrivacyTabMessageAsync()
        {
            await Expect(PrivacyTabMessage).ToBeVisibleAsync();
            return (await PrivacyTabMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetContextTitleErrorAsync()
        {
            await Expect(ContextTitleError).ToBeVisibleAsync();
            return (await ContextTitleError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetCustomerAccountErrorAsync()
        {
            await Expect(CustomerAccountError).ToBeVisibleAsync();
            return (await CustomerAccountError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<IReadOnlyList<string>> GetTopError()
        {
            var errors = await TopError.AllInnerTextsAsync();
            return errors.Select(e => e.Trim()).ToList();
        }

        public async Task<IReadOnlyList<string>> GetCustomerAccountOptionsAsync()
        {
            await CustomerAccountArrow.ClickAsync();
            await Expect(CustomerAccountDropdownOption.First).ToBeVisibleAsync();
            var options = await CustomerAccountDropdownOption.AllInnerTextsAsync();
            return options.Select(o => o.Trim()).ToList();
        }

        public async Task SelectCustomerAccountRandomlyAsync()
        {
            await CustomerAccountArrow.ClickAsync();
            await Expect(CustomerAccountDropdownOption.First).ToBeVisibleAsync();
            var optionsCount = await CustomerAccountDropdownOption.CountAsync();
            if (optionsCount == 0)
            {
                throw new InvalidOperationException("No customer account options available");
            }
            var randomIndex = Random.Shared.Next(0, optionsCount);
            await CustomerAccountDropdownOption.Nth(randomIndex).ClickAsync();
        }

        // Labels tab actions
        public async Task<string> GetLabelsTabMessageAsync()
        {
            await Expect(LabelsTabMessage).ToBeVisibleAsync();
            return (await LabelsTabMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetNotificationsTabMessageAsync()
        {
            await Expect(NotificationsTabMessage).ToBeVisibleAsync();
            return (await NotificationsTabMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetChatTabMessageAsync()
        {
            await Expect(ChatTabMessage).ToBeVisibleAsync();
            return (await ChatTabMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task SelectJobLabelAsync()
        {
            await LabelsInput.ClickAsync();
            await LabelsInputJobOption.ClickAsync();
            await Expect(SelectedLabels).ToContainTextAsync("Job");
        }

        public async Task<IReadOnlyList<string>> GetSelectedLabelsAsync()
        {
            var labels = await SelectedLabels.AllInnerTextsAsync();
            return labels.Select(l => l.Trim()).ToList();
        }

        // assertions
        public Task ExpectContextSettingsModalVisibleAsync() =>
            Expect(ContextSettingsModal).ToBeVisibleAsync(new() { Timeout = 15_000 });

        public Task ExpectDefaultTabsVisibleAsync() => Task.WhenAll(
            Expect(ConfigurationTab).ToBeVisibleAsync(),
            Expect(PrivacyTab).ToBeVisibleAsync(),
            Expect(LabelsTab).ToBeVisibleAsync(),
            Expect(NotificationsTab).ToBeVisibleAsync(),
            Expect(ChatTab).ToBeVisibleAsync());

        public Task ExpectConfigurationControlsVisibleAsync() => Task.WhenAll(
            Expect(ContextTitleInput).ToBeVisibleAsync(),
            Expect(ContextDescriptionInput).ToBeVisibleAsync(),
            Expect(CustomerAccountDropdown).ToBeVisibleAsync(),
            Expect(PredecessorsInput).ToBeVisibleAsync(),
            Expect(SuccessorsInput).ToBeVisibleAsync(),
            Expect(CancelBtn).ToBeVisibleAsync(),
            Expect(SetupLaterBtn).ToBeVisibleAsync(),
            Expect(CreateBtn).ToBeVisibleAsync());

        public Task ExpectPrivacyControlsVisibleAsync() => Task.WhenAll(
            Expect(AutomaticallyAddChildContextUserToggle).ToBeVisibleAsync(),
            Expect(AdministratorsInput).ToBeVisibleAsync(),
            Expect(ParticipantsInput).ToBeVisibleAsync(),
            Expect(ObserversInput).ToBeVisibleAsync(),
            Expect(CancelBtn).ToBeVisibleAsync(),
            Expect(SetupLaterBtn).ToBeVisibleAsync(),
            Expect(CreateBtn).ToBeVisibleAsync());

        public Task ExpectLabelsControlsVisibleAsync() => Task.WhenAll(
            Expect(LabelsInput).ToBeVisibleAsync(),
            Expect(CancelBtn).ToBeVisibleAsync(),
            Expect(SetupLaterBtn).ToBeVisibleAsync(),
            Expect(CreateBtn).ToBeVisibleAsync());

        public Task ExpectNotificationsControlsVisibleAsync() => Task.WhenAll(
            Expect(StatusUpdatesToggle).ToBeVisibleAsync(),
            Expect(AssignmentsNotificationsToggle).ToBeVisibleAsync(),
            Expect(FileUploadsToggle).ToBeVisibleAsync(),
            Expect(CancelBtn).ToBeVisibleAsync(),
            Expect(SetupLaterBtn).ToBeVisibleAsync(),
            Expect(CreateBtn).ToBeVisibleAsync());

        public Task ExpectChatControlsVisibleAsync() => Task.WhenAll(
            Expect(EnableChatToggle).ToBeVisibleAsync(),
            Expect(CancelBtn).ToBeVisibleAsync(),
            Expect(SetupLaterBtn).ToBeVisibleAsync(),
            Expect(CreateBtn).ToBeVisibleAsync());
    }
}
