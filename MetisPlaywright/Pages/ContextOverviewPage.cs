using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public enum ContextStatus
    {
        Draft,
        Pending,
        InProgress,
        Delayed,
        Completed
    }

    public class ContextOverviewPage : BasePage
    {
        public ContextOverviewPage(IPage page) : base(page) { }

        private ILocator ContextOverviewTitle => Page.Locator("//h1[contains(text(),'Overview')]").First;
        private ILocator ContextName => Page.Locator("//div[@class='skeleton-wrapper ']/div/h1");
        private ILocator CustomerName => Page.Locator("//div[@class='skeleton-wrapper ']/p[contains(@class,'truncate')]");
        private ILocator PathfinderBtn => Page.Locator("//span[@class='leading-none']");
        private ILocator OptimizeBtn => Page.Locator("//button[normalize-space()='Optimize']");
        private ILocator DuplicateBtn => Page.Locator("//button[normalize-space()='Duplicate']");
        private ILocator PrintViewToggle => Page.Locator("//span[contains(@class,'e-switch-off')]");
        private ILocator QRCodeBtn => Page.Locator("//button[contains(@id,'qr-btn')]");
        private ILocator OpenContextBuilderBtn => Page.Locator("//button[contains(@title,'Context Builder')]");
        private ILocator OverviewTab => Page.Locator("//div[@class='skeleton-wrapper ']/span[normalize-space()='Overview']");
        private ILocator RequirementsTab => Page.Locator("//div[@class='skeleton-wrapper ']/span[normalize-space()='Requirements']");
        private ILocator ConsumablesTab => Page.Locator("//div[@class='skeleton-wrapper ']/span[normalize-space()='Consumables']");
        private ILocator AttachmentsTab => Page.Locator("//div[@class='skeleton-wrapper ']/span[normalize-space()='Attachments']");
        private ILocator TimelineTab => Page.Locator("//div[@class='skeleton-wrapper ']/span[normalize-space()='Timeline']");
        private ILocator StatusDropdown => Page.Locator("//label[normalize-space()='Status']/../following-sibling::div//span[@role='combobox']");
        private ILocator StatusDropdownValue => Page.Locator("//span[contains(@class,'e-input-value')]/div");
        private ILocator StatusDropdownArrow => Page.Locator("//label[normalize-space()='Status']/../following-sibling::div//span[contains(@class,'e-icons')]");
        private ILocator DescriptionText => Page.Locator("//label[normalize-space()='Description']/../following-sibling::div/p");
        private ILocator DescriptionReadMoreLink => Page.Locator("//label[normalize-space()='Description']/../following-sibling::div/span");
        private ILocator StartDate => Page.Locator("//label[normalize-space()='Start Date']/following-sibling::div//span[@class='e-editable-value']");
        private ILocator StartDateEditIcon => Page.Locator("//label[normalize-space()='Start Date']/following-sibling::div//span[@title='Click to edit']");
        private ILocator Duration => Page.Locator("//label[normalize-space()='Duration']/following-sibling::div//span[@class='e-editable-value']");
        private ILocator DurationEditIcon => Page.Locator("//label[normalize-space()='Duration']/following-sibling::div//span[@title='Click to edit']");
        private ILocator EndDate => Page.Locator("//label[normalize-space()='End Date']/following-sibling::div//span[@class='e-editable-value']");
        private ILocator EndDateEditIcon => Page.Locator("//label[normalize-space()='End Date']/following-sibling::div//span[@title='Click to edit']");
        private ILocator ChildContextsSectionTitle => Page.Locator("//h1[normalize-space()='Child Contexts']");
        private ILocator SelectLabelDropdown => Page.Locator("//input[@placeholder='Select label']");
        private ILocator SelectLabelDropdownValue => Page.Locator("//input[@placeholder='Select label']/preceding-sibling::span//span");
        private ILocator SelectLabelDropdownArrow => Page.Locator("//input[@placeholder='Select label']/following-sibling::span[contains(@class,'e-icons')]");
        private ILocator SelectLabelDropdownOptionJob => Page.Locator("//li[@data-value='Job']");
        private ILocator CreateChildContextBtn => Page.Locator("//button[normalize-space()='Create Child Context']");
        private ILocator ChildContextsGrid => Page.Locator("//div[@class='e-content e-yscroll']");
        private ILocator ContextFieldsSectionTitle => Page.Locator("//h2[normalize-space()='Context Fields']");
        private ILocator ContextFieldsEditBtn => Page.Locator("//button[normalize-space()='Edit']");
        private ILocator ContextFieldsEmptyText => Page.Locator("//h2[normalize-space()='Context Fields']/../following-sibling::div//p/strong");
        private ILocator ValidationBtn => Page.Locator("//button[contains(@class,'bottom')]");

        private ILocator StatusDropdownOption(ContextStatus status) =>
            Page.Locator($"//li[@data-value='{status}']");

        public async Task OpenForContextAsync(string contextName)
        {
            var dashboardPage = new DashboardPage(Page);
            await dashboardPage.OpenForAutoTests1Async();
            await dashboardPage.ClickGridContextAsync(contextName);
            await ContextOverviewTitle.WaitForAsync();
        }

        //read-only text
        public async Task<string> GetContextOverviewTitleAsync()
        {
            await ContextOverviewTitle.WaitForAsync();
            return (await ContextOverviewTitle.TextContentAsync()) ?? string.Empty;
        }
        public async Task<string> GetContextNameAsync() =>
            (await ContextName.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetCustomerNameAsync() =>
            (await CustomerName.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetStatusValueAsync() =>
            (await StatusDropdownValue.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetDescriptionTextAsync() =>
            (await DescriptionText.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetStartDateAsync() =>
            (await StartDate.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetDurationAsync() =>
            (await Duration.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetEndDateAsync() =>
            (await EndDate.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetSelectLabelValueAsync() =>
            (await SelectLabelDropdownValue.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetChildContextsSectionTitleAsync() =>
            (await ChildContextsSectionTitle.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetContextFieldsSectionTitleAsync() =>
            (await ContextFieldsSectionTitle.TextContentAsync()) ?? string.Empty;
        public async Task<string> GetContextFieldsEmptyTextAsync() =>
            (await ContextFieldsEmptyText.TextContentAsync()) ?? string.Empty;

        //actions
        public Task ClickPathfinderAsync() => PathfinderBtn.ClickAsync();
        public Task ClickOptimizeAsync() => OptimizeBtn.ClickAsync();
        public Task ClickDuplicateAsync() => DuplicateBtn.ClickAsync();
        public Task ClickPrintViewToggleAsync() => PrintViewToggle.ClickAsync();
        public Task ClickQRCodeAsync() => QRCodeBtn.ClickAsync();
        public Task ClickOpenContextBuilderAsync() => OpenContextBuilderBtn.ClickAsync();
        public Task ClickOverviewTabAsync() => OverviewTab.ClickAsync();
        public Task ClickRequirementsTabAsync() => RequirementsTab.ClickAsync();
        public Task ClickConsumablesTabAsync() => ConsumablesTab.ClickAsync();
        public Task ClickAttachmentsTabAsync() => AttachmentsTab.ClickAsync();
        public Task ClickTimelineTabAsync() => TimelineTab.ClickAsync();
        public Task ClickDescriptionReadMoreAsync() => DescriptionReadMoreLink.ClickAsync();
        public Task ClickStartDateEditAsync() => StartDateEditIcon.ClickAsync();
        public Task ClickDurationEditAsync() => DurationEditIcon.ClickAsync();
        public Task ClickEndDateEditAsync() => EndDateEditIcon.ClickAsync();
        public Task ClickCreateChildContextAsync() => CreateChildContextBtn.ClickAsync();
        public Task ClickContextFieldsEditAsync() => ContextFieldsEditBtn.ClickAsync();
        public Task ClickValidationAsync() => ValidationBtn.ClickAsync();

        public async Task<ContextSettingsPage> ClickCreateChildContextAndOpenContextSettingsAsync()
        {
            await ClickCreateChildContextAsync();
            var contextSettingsPage = new ContextSettingsPage(Page);
            await contextSettingsPage.ExpectContextSettingsModalVisibleAsync();
            return contextSettingsPage;
        }

        public async Task SelectStatusAsync(ContextStatus status)
        {
            await StatusDropdownArrow.ClickAsync();
            await StatusDropdownOption(status).ClickAsync();
        }

        public async Task SelectLabelJobAsync()
        {
            await SelectLabelDropdownArrow.ClickAsync();
            await SelectLabelDropdownOptionJob.ClickAsync();
        }

        //expectations
        public Task ExpectOpenedAsync() => Expect(ContextOverviewTitle).ToBeVisibleAsync();
        public Task ExpectContextNameAsync(string expectedContextName) =>
            Expect(ContextName).ToHaveTextAsync(expectedContextName, new() { Timeout = 15_000 });

        public Task ExpectChildContextVisibleInGridAsync(string childContextName) =>
            Expect(ChildContextsGrid).ToContainTextAsync(childContextName, new() { Timeout = 15_000 });

        public Task ClickChildContextInGridAsync(string childContextName) =>
            ChildContextsGrid.GetByText(childContextName, new() { Exact = true }).ClickAsync();

        public Task ExpectDefaultControlsVisibleAsync() => Task.WhenAll(
            Expect(PathfinderBtn).ToBeVisibleAsync(),
            Expect(OptimizeBtn).ToBeVisibleAsync(),
            Expect(DuplicateBtn).ToBeVisibleAsync(),
            Expect(PrintViewToggle).ToBeVisibleAsync(),
            Expect(QRCodeBtn).ToBeVisibleAsync(),
            Expect(OpenContextBuilderBtn).ToBeVisibleAsync(),
            Expect(OverviewTab).ToBeVisibleAsync(),
            Expect(RequirementsTab).ToBeVisibleAsync(),
            Expect(ConsumablesTab).ToBeVisibleAsync(),
            Expect(AttachmentsTab).ToBeVisibleAsync(),
            Expect(TimelineTab).ToBeVisibleAsync(),
            Expect(StatusDropdown).ToBeVisibleAsync(),
            Expect(DescriptionText).ToBeVisibleAsync(),
            Expect(StartDate).ToBeVisibleAsync(),
            Expect(Duration).ToBeVisibleAsync(),
            Expect(EndDate).ToBeVisibleAsync(),
            Expect(SelectLabelDropdown).ToBeVisibleAsync(),
            Expect(CreateChildContextBtn).ToBeVisibleAsync(),
            Expect(ChildContextsGrid).ToBeVisibleAsync(),
            Expect(ContextFieldsSectionTitle).ToBeVisibleAsync(),
            Expect(ContextFieldsEditBtn).ToBeVisibleAsync(),
            Expect(ValidationBtn).ToBeVisibleAsync());
    }
}
