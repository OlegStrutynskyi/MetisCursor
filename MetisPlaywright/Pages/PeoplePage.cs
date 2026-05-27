using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class PeoplePage : BasePage
    {
        public PeoplePage(IPage page) : base(page) { }

        private ILocator CreatePersonBtn => Page.Locator("//button[normalize-space()='Create Person']").First;
        private ILocator ManageTeamsBtn => Page.Locator("//button[normalize-space()='Manage Teams']");

        private ILocator GridPersonName => Page.Locator("//tr/td[1]//span[@class='ml-4']");
        private ILocator GridPersonEmail => Page.Locator("//tr/td[2]//span[@class='text-pmgrey-600']");
        private ILocator GridPersonEmailByEmail(string email) =>
            Page.Locator($"//span[@class='text-pmgrey-600' and normalize-space()='{email}']");
        private ILocator GridPersonStatus(string email) =>
            Page.Locator($"//span[normalize-space()='{email}']/ancestor::td/following-sibling::td[2]//span[@class='e-chip-text']");
        private ILocator GridPersonActions(string email) =>
            Page.Locator($"//span[normalize-space()='{email}']/ancestor::td/following-sibling::td[3]//span[contains(@class, 'e-icons')]");
        private ILocator GridPersonDeactivateAction => Page.Locator("//div[@class='e-tip-content']//p[normalize-space()='Deactivate']");
        private ILocator GridPersonActivateAction => Page.Locator("//div[@class='e-tip-content']//p[normalize-space()='Activate']");

        //Person Settings modal
        private ILocator PersonSettingsModal => Page.Locator("//div[@role='dialog']//h3[contains(text(),'Person Settings')]").First;
        private ILocator PersonSettingsFirstNameInput => Page.Locator("//label[normalize-space()='First Name']/following-sibling::div//input");
        private ILocator PersonSettingsFirstNameError => Page.Locator("//div[@role='dialog']//label[normalize-space()='First Name']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator PersonSettingsLastNameInput => Page.Locator("//label[normalize-space()='Last Name']/following-sibling::div//input");
        private ILocator PersonSettingsLastNameError => Page.Locator("//label[normalize-space()='Last Name']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator PersonSettingsEmailInput => Page.Locator("//label[normalize-space()='Email']/following-sibling::div//input");
        private ILocator PersonSettingsEmailError => Page.Locator("//label[normalize-space()='Email']/following-sibling::div//div[contains(@class,'e-error')]");
        private ILocator PersonSettingsTeamsInput => Page.Locator("//label[normalize-space()='Teams']/following-sibling::div//input");
        private ILocator PersonSettingsCancelBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Cancel']").First;
        private ILocator PersonSettingsSaveBtn => Page.Locator("//div[@role='dialog']//button[normalize-space()='Save']").First;
        private ILocator PersonSettingsCloseIcon => Page.Locator("//div[@role='dialog']//img[@src='/images/close.svg']").First;
        public async Task OpenForAutoTests1Async()
        {
            var leftMenu = new LeftMenuPage(Page);
            await leftMenu.OpenForAutoTests1Async();
            await leftMenu.ClickPeopleIconAsync();
            await GetPageTitleLocator().WaitForAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("People");
            await Expect(CreatePersonBtn).ToBeVisibleAsync();
        }
        
        public async Task OpenPersonSettingsModalAsync()
        {
            await CreatePersonBtn.ClickAsync();
            await Expect(PersonSettingsModal).ToBeVisibleAsync();
        }

        public async Task<TeamsOveviewPage> ClickManageTeamsBtnAsync()
        {
            await ManageTeamsBtn.ClickAsync();
            await Expect(GetPageTitleLocator()).ToHaveTextAsync("Teams Overview");
            return new TeamsOveviewPage(Page);
        }

        public async Task<PersonOverviewPage> ClickPersonByEmailAsync(string email)
        {
            var emailCell = GridPersonEmailByEmail(email);
            await emailCell.ScrollIntoViewIfNeededAsync();
            await emailCell.ClickAsync();
            await Page.WaitForURLAsync(url => url.Contains("people/"));
            return new PersonOverviewPage(Page);
        }

        public async Task<IReadOnlyList<string>> GetPersonNamesAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            return await GridPersonName.AllInnerTextsAsync();
        }

        public async Task<IReadOnlyList<string>> GetPersonEmailsAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            return await GridPersonEmail.AllInnerTextsAsync();
        }

        public async Task<int> GetPersonsCountAsync()
        {
            await Page.WaitForTimeoutAsync(1000);
            return await GridPersonName.CountAsync();
        }

        public async Task<string> GetPersonStatusAsync(string email)
        {
            var status = GridPersonStatus(email);
            await status.WaitForAsync();
            return (await status.TextContentAsync()) ?? string.Empty;
        }

        public async Task DeactivatePersonAsync(string email)
        {
            await GridPersonActions(email).HoverAsync();
            await GridPersonDeactivateAction.ClickAsync();
        }

        public async Task ActivatePersonAsync(string email)
        {
            await GridPersonActions(email).HoverAsync();
            await GridPersonActivateAction.ClickAsync();
        }

        public async Task ExpectPersonStatusAsync(string email, string expectedStatus)
        {
            await Expect(GridPersonStatus(email)).ToHaveTextAsync(expectedStatus);
        }

        public Task<string> GetPersonSettingsTitleAsync() => GetModalTitleTextAsync();

        public async Task<string> GetPersonSettingsFirstNameErrorAsync()
        {
            await PersonSettingsFirstNameError.WaitForAsync();
            return (await PersonSettingsFirstNameError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetPersonSettingsLastNameErrorAsync()
        {
            await PersonSettingsLastNameError.WaitForAsync();
            return (await PersonSettingsLastNameError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetPersonSettingsEmailErrorAsync()
        {
            await PersonSettingsEmailError.WaitForAsync();
            return (await PersonSettingsEmailError.TextContentAsync()) ?? string.Empty;
        }

        public Task FillPersonSettingsFirstNameAsync(string value) => PersonSettingsFirstNameInput.FillAsync(value);
        public Task FillPersonSettingsLastNameAsync(string value) => PersonSettingsLastNameInput.FillAsync(value);
        public Task FillPersonSettingsEmailAsync(string value) => PersonSettingsEmailInput.FillAsync(value);
        public Task ClickPersonSettingsCancelAsync() => PersonSettingsCancelBtn.ClickAsync();
        public Task ClickPersonSettingsSaveAsync() => PersonSettingsSaveBtn.ClickAsync();
        public Task ClickPersonSettingsCloseAsync() => PersonSettingsCloseIcon.ClickAsync();

        public async Task ExpectPersonSettingsModalVisibleAsync()
        {
            await Expect(PersonSettingsModal).ToBeVisibleAsync();
        }

        public async Task ExpectPersonSettingsControlsVisibleAsync()
        {
            await Expect(PersonSettingsFirstNameInput).ToBeVisibleAsync();
            await Expect(PersonSettingsLastNameInput).ToBeVisibleAsync();
            await Expect(PersonSettingsEmailInput).ToBeVisibleAsync();
            await Expect(PersonSettingsTeamsInput).ToBeVisibleAsync();
            await Expect(PersonSettingsCancelBtn).ToBeVisibleAsync();
            await Expect(PersonSettingsSaveBtn).ToBeVisibleAsync();
            await Expect(PersonSettingsCloseIcon).ToBeVisibleAsync();
        }
    }
}
