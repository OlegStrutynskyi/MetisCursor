using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class BuilderPage : BasePage
    {
        public BuilderPage(IPage page) : base(page) { }

        // Page header
        private ILocator Name => Page.Locator("//div[@class='skeleton-wrapper ']/h1");
        private ILocator ArchiveContextBtn => Page.Locator("//button[@title='Archive Context']");
        private ILocator ContextSettingsBtn => Page.Locator("//button[@title='Context Settings']");
        private ILocator BackToContextOverviewBtn => Page.Locator("//button[normalize-space()='Back to Context Overview']");
        private ILocator SaveAsTemplateBtn => Page.Locator("//button[normalize-space()='Save as a Template']");
        private ILocator BuildContextBtn => Page.Locator("//button[normalize-space()='Build Context']");
        private ILocator AddChildContextBtn => Page.Locator("//button[@title='Create a child context']");
        private ILocator CreateNewContextBtn => Page.Locator("//button[normalize-space()='Create a New Context']");

        private ILocator GetStartedMessage => Page.Locator("//div[contains(@class,'skeleton-wrapper')]/h1");
        private ILocator BuilderMessage => Page.Locator("//div[contains(@class,'skeleton-wrapper')]/p");

        // Constructor buttons (exclude left-menu labels with class sidebar-nav-label)
        private static string ConstructorButtonXPath(string label) =>
            $"//span[normalize-space()='{label}' and not(contains(@class,'sidebar-nav-label'))]";

        private ILocator AddFieldsBtn => Page.Locator(ConstructorButtonXPath("Add Fields"));
        private ILocator TemplatesBtn => Page.Locator(ConstructorButtonXPath("Templates"));
        private ILocator HeadingBtn => Page.Locator(ConstructorButtonXPath("Heading"));
        private ILocator SubHeadingBtn => Page.Locator(ConstructorButtonXPath("Sub Heading"));
        private ILocator DividerBtn => Page.Locator(ConstructorButtonXPath("Divider"));
        private ILocator SingleLineBtn => Page.Locator(ConstructorButtonXPath("Single Line"));
        private ILocator NumberBtn => Page.Locator(ConstructorButtonXPath("Number"));
        private ILocator TextAreaBtn => Page.Locator(ConstructorButtonXPath("Text Area"));
        private ILocator CurrencyBtn => Page.Locator(ConstructorButtonXPath("Currency"));
        private ILocator DecimalBtn => Page.Locator(ConstructorButtonXPath("Decimal"));
        private ILocator EmailBtn => Page.Locator(ConstructorButtonXPath("Email"));
        private ILocator DateBtn => Page.Locator(ConstructorButtonXPath("Date"));
        private ILocator TimeBtn => Page.Locator(ConstructorButtonXPath("Time"));
        private ILocator DateTimeBtn => Page.Locator(ConstructorButtonXPath("Date Time"));
        private ILocator SingleImageBtn => Page.Locator(ConstructorButtonXPath("Single Image"));
        private ILocator CheckboxBtn => Page.Locator(ConstructorButtonXPath("Checkbox"));
        private ILocator MultiSelectBtn => Page.Locator(ConstructorButtonXPath("Multi-select"));
        private ILocator FileUploadBtn => Page.Locator(ConstructorButtonXPath("File Upload"));
        private ILocator DropdownBtn => Page.Locator(ConstructorButtonXPath("Dropdown"));

        // Save as template modal
        private ILocator SaveAsTemplateModal => Page.Locator("//h3[contains(text(),'Save as a Template')]/ancestor::div[contains(@class,'modal-content')]");
        private ILocator SaveAsTemplateModalTitle => Page.Locator("//div[contains(@class,'modal-content')]//h3");
        private ILocator SaveAsTemplateModalNameInput => Page.Locator("//label[normalize-space()='Template Name']/following-sibling::div//input");
        private ILocator SaveAsTemplateModalNameError => Page.Locator("//div[@role='dialog']//label[normalize-space()='Template Name']/following-sibling::div//div[contains(@class,'text-red')]");
        private ILocator SaveAsTemplateModalDescriptionInput => Page.Locator("//label[normalize-space()='Template Description']/following-sibling::div//textarea");
        private ILocator SaveAsTemplateModalCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator SaveAsTemplateModalSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save Template']").First;
        private ILocator SaveAsTemplateModalCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;

        // Save as template modal actions
        public async Task ClickSaveAsTemplateBtnAsync()
        {
            await SaveAsTemplateBtn.ClickAsync();
            await ExpectSaveAsTemplateModalVisibleAsync();
        }

        public Task ExpectSaveAsTemplateModalVisibleAsync() =>
            Expect(SaveAsTemplateModal).ToBeVisibleAsync(new() { Timeout = 15_000 });

        public async Task<string> GetSaveAsTemplateModalTitleAsync()
        {
            await Expect(SaveAsTemplateModalTitle).ToBeVisibleAsync();
            return (await SaveAsTemplateModalTitle.TextContentAsync()) ?? string.Empty;
        }

        public Task FillSaveAsTemplateModalNameAsync(string name) =>
            SaveAsTemplateModalNameInput.FillAsync(name);

        public async Task<string> GetSaveAsTemplateModalNameAsync()
        {
            await Expect(SaveAsTemplateModalNameInput).ToBeVisibleAsync();
            return await SaveAsTemplateModalNameInput.InputValueAsync();
        }

        public async Task<string> GetSaveAsTemplateModalNameErrorAsync()
        {
            await Expect(SaveAsTemplateModalNameError).ToBeVisibleAsync();
            return (await SaveAsTemplateModalNameError.TextContentAsync()) ?? string.Empty;
        }

        public Task FillSaveAsTemplateModalDescriptionAsync(string description) =>
            SaveAsTemplateModalDescriptionInput.FillAsync(description);

        public async Task<string> GetSaveAsTemplateModalDescriptionAsync()
        {
            await Expect(SaveAsTemplateModalDescriptionInput).ToBeVisibleAsync();
            return await SaveAsTemplateModalDescriptionInput.InputValueAsync();
        }

        public Task ExpectSaveAsTemplateModalControlsVisibleAsync() => Task.WhenAll(
            Expect(SaveAsTemplateModalCancelBtn).ToBeVisibleAsync(),
            Expect(SaveAsTemplateModalSaveBtn).ToBeVisibleAsync());

        public Task ClickSaveAsTemplateModalCancelAsync() => SaveAsTemplateModalCancelBtn.ClickAsync();
        public Task ClickSaveAsTemplateModalSaveAsync() => SaveAsTemplateModalSaveBtn.ClickAsync();
        public Task ClickSaveAsTemplateModalCloseAsync() => SaveAsTemplateModalCloseIcon.ClickAsync();

        public async Task<BuilderPage> GetContextBuilderPageAutoTests1Async()
        {
            var contextExplorerPage = new ContextExplorerPage(Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();
            return await contextExplorerPage.ClickCreateNewContextBtnAsync();
        }

        public async Task<ContextSettingsPage> ClickCreateNewContextBtnAsync()
        {
            await Expect(CreateNewContextBtn).ToBeEnabledAsync();
            await CreateNewContextBtn.ClickAsync();
            var contextSettingsPage = new ContextSettingsPage(Page);
            await contextSettingsPage.ExpectContextSettingsModalVisibleAsync();
            return contextSettingsPage;
        }

        public async Task<ContextSettingsPage> ClickContextSettingsBtnAsync()
        {
            await ContextSettingsBtn.ClickAsync();
            var contextSettingsPage = new ContextSettingsPage(Page);
            await contextSettingsPage.ExpectContextSettingsModalVisibleAsync();
            return contextSettingsPage;
        }

        public async Task<ContextSettingsPage> ClickAddChildContextBtnAsync()
        {
            await AddChildContextBtn.ClickAsync();
            var contextSettingsPage = new ContextSettingsPage(Page);
            await contextSettingsPage.ExpectContextSettingsModalVisibleAsync();
            return contextSettingsPage;
        }

        public Task ExpectOpenedAsync() => Task.WhenAll(
            Expect(GetPageTitleLocator()).ToHaveTextAsync("Builder"),
            Expect(Name).ToBeVisibleAsync());

        public Task ExpectDefaultControlsVisibleAsync() =>
            Expect(CreateNewContextBtn).ToBeVisibleAsync(new() { Timeout = 15_000 });

        public Task ExpectExistingContextControlsVisibleAsync() => Task.WhenAll(
            Expect(ArchiveContextBtn).ToBeVisibleAsync(),
            Expect(ContextSettingsBtn).ToBeVisibleAsync(),
            Expect(BackToContextOverviewBtn).ToBeVisibleAsync(),
            Expect(SaveAsTemplateBtn).ToBeVisibleAsync(),
            Expect(BuildContextBtn).ToBeVisibleAsync(),
            Expect(AddChildContextBtn).ToBeVisibleAsync());

        public Task ExpectConstructorButtonsVisibleAsync() => Task.WhenAll(
            Expect(AddFieldsBtn).ToBeVisibleAsync(),
            Expect(TemplatesBtn).ToBeVisibleAsync(),
            Expect(HeadingBtn).ToBeVisibleAsync(),
            Expect(SubHeadingBtn).ToBeVisibleAsync(),
            Expect(DividerBtn).ToBeVisibleAsync(),
            Expect(SingleLineBtn).ToBeVisibleAsync(),
            Expect(NumberBtn).ToBeVisibleAsync(),
            Expect(TextAreaBtn).ToBeVisibleAsync(),
            Expect(CurrencyBtn).ToBeVisibleAsync(),
            Expect(DecimalBtn).ToBeVisibleAsync(),
            Expect(EmailBtn).ToBeVisibleAsync(),
            Expect(DateBtn).ToBeVisibleAsync(),
            Expect(TimeBtn).ToBeVisibleAsync(),
            Expect(DateTimeBtn).ToBeVisibleAsync(),
            Expect(SingleImageBtn).ToBeVisibleAsync(),
            Expect(CheckboxBtn).ToBeVisibleAsync(),
            Expect(MultiSelectBtn).ToBeVisibleAsync(),
            Expect(FileUploadBtn).ToBeVisibleAsync(),
            Expect(DropdownBtn).ToBeVisibleAsync());

        public async Task<string> GetNameTextAsync()
        {
            await Expect(Name).ToBeVisibleAsync();
            return (await Name.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetStartedMessageTextAsync()
        {
            await Expect(GetStartedMessage).ToBeVisibleAsync();
            return (await GetStartedMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetBuilderMessageTextAsync()
        {
            await Expect(BuilderMessage).ToBeVisibleAsync();
            return (await BuilderMessage.TextContentAsync()) ?? string.Empty;
        }

        public ILocator GetName() => Name;
        public ILocator GetArchiveContextBtn() => ArchiveContextBtn;
        public Task ClickArchiveContextBtnAsync() => ArchiveContextBtn.ClickAsync();
        public ILocator GetContextSettingsBtn() => ContextSettingsBtn;
        public ILocator GetBackToContextOverviewBtn() => BackToContextOverviewBtn;
        public Task ClickBackToContextOverviewBtnAsync() => BackToContextOverviewBtn.ClickAsync();
        public ILocator GetSaveAsTemplateBtn() => SaveAsTemplateBtn;
        public ILocator GetBuildContextBtn() => BuildContextBtn;
        public Task ClickBuildContextBtnAsync() => BuildContextBtn.ClickAsync();
        public ILocator GetAddChildContextBtn() => AddChildContextBtn;

        public ILocator GetAddFieldsBtn() => AddFieldsBtn;
        public Task ClickAddFieldsBtnAsync() => AddFieldsBtn.ClickAsync();
        public ILocator GetTemplatesBtn() => TemplatesBtn;
        public Task ClickTemplatesBtnAsync() => TemplatesBtn.ClickAsync();
        public ILocator GetHeadingBtn() => HeadingBtn;
        public Task ClickHeadingBtnAsync() => HeadingBtn.ClickAsync();
        public ILocator GetSubHeadingBtn() => SubHeadingBtn;
        public Task ClickSubHeadingBtnAsync() => SubHeadingBtn.ClickAsync();
        public ILocator GetDividerBtn() => DividerBtn;
        public Task ClickDividerBtnAsync() => DividerBtn.ClickAsync();
        public ILocator GetSingleLineBtn() => SingleLineBtn;
        public Task ClickSingleLineBtnAsync() => SingleLineBtn.ClickAsync();
        public ILocator GetNumberBtn() => NumberBtn;
        public Task ClickNumberBtnAsync() => NumberBtn.ClickAsync();
        public ILocator GetTextAreaBtn() => TextAreaBtn;
        public Task ClickTextAreaBtnAsync() => TextAreaBtn.ClickAsync();
        public ILocator GetCurrencyBtn() => CurrencyBtn;
        public Task ClickCurrencyBtnAsync() => CurrencyBtn.ClickAsync();
        public ILocator GetDecimalBtn() => DecimalBtn;
        public Task ClickDecimalBtnAsync() => DecimalBtn.ClickAsync();
        public ILocator GetEmailBtn() => EmailBtn;
        public Task ClickEmailBtnAsync() => EmailBtn.ClickAsync();
        public ILocator GetDateBtn() => DateBtn;
        public Task ClickDateBtnAsync() => DateBtn.ClickAsync();
        public ILocator GetTimeBtn() => TimeBtn;
        public Task ClickTimeBtnAsync() => TimeBtn.ClickAsync();
        public ILocator GetDateTimeBtn() => DateTimeBtn;
        public Task ClickDateTimeBtnAsync() => DateTimeBtn.ClickAsync();
        public ILocator GetSingleImageBtn() => SingleImageBtn;
        public Task ClickSingleImageBtnAsync() => SingleImageBtn.ClickAsync();
        public ILocator GetCheckboxBtn() => CheckboxBtn;
        public Task ClickCheckboxBtnAsync() => CheckboxBtn.ClickAsync();
        public ILocator GetMultiSelectBtn() => MultiSelectBtn;
        public Task ClickMultiSelectBtnAsync() => MultiSelectBtn.ClickAsync();
        public ILocator GetFileUploadBtn() => FileUploadBtn;
        public Task ClickFileUploadBtnAsync() => FileUploadBtn.ClickAsync();
        public ILocator GetDropdownBtn() => DropdownBtn;
        public Task ClickDropdownBtnAsync() => DropdownBtn.ClickAsync();
    }
}
