using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IPage page) : base(page) { }

        private ILocator LoginPageTitle => Page.Locator("h1").First;
        private ILocator LoginPageMessage => Page.Locator("//h1/following-sibling::p");
        private ILocator LoginPageEmailInput => Page.Locator("//input[@type='text']");
        private ILocator LoginPagePasswordInput => Page.Locator("//input[@type='password']");
        private ILocator LoginPageEmailError => Page.Locator("//input[@type='text']/../following-sibling::div");
        private ILocator LoginPagePasswordError => Page.Locator("//input[@type='password']/../following-sibling::div");
        private ILocator LoginPageRememberCheckbox => Page.Locator("//input[@type='checkbox']");
        private ILocator LoginPageForgotPasswordLink => Page.Locator("//a[normalize-space()='Forgot password']");
        private ILocator LoginPageLoginBtn => Page.Locator("//button[normalize-space()='Log in']");
        private ILocator LoginPageGeneralError => Page.Locator("//li[@class='validation-message']");

        private ILocator ForgotPasswordTitle => Page.Locator("//h1[contains(text(),'Forgot')]").First;
        private ILocator ForgotPasswordMessage => Page.Locator("//p[contains(text(),'Please')]");
        private ILocator ForgotPasswordEmailInput => Page.Locator("//input[@type='text']");
        private ILocator ForgotPasswordEmailError => Page.Locator("//li[@class='validation-message']");
        private ILocator ForgotPasswordLoginBtn => Page.Locator("//a[normalize-space()='Log in']");
        private ILocator ForgotPasswordResetBtn => Page.Locator("//button[normalize-space()='Reset password']");
        private ILocator ForgotPasswordSuccessMessage => Page.Locator("//form[@method='post']/following-sibling::div");


        public async Task OpenAsync()
        {
            await Page.GotoAsync(Config.BaseUrl + "login");
            await LoginPageTitle.WaitForAsync();
        }

        public async Task<string> GetTitleAsync()
        {
            await LoginPageTitle.WaitForAsync();
            return (await LoginPageTitle.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetMessageAsync()
        {
            await LoginPageMessage.WaitForAsync();
            return (await LoginPageMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetEmailPlaceholderAsync()
        {
            return (await LoginPageEmailInput.GetAttributeAsync("placeholder")) ?? string.Empty;
        }

        public async Task<string> GetPasswordPlaceholderAsync()
        {
            return (await LoginPagePasswordInput.GetAttributeAsync("placeholder")) ?? string.Empty;
        }

        public async Task<string> GetEmailErrorMessageAsync()
        {
            await LoginPageEmailError.WaitForAsync();
            return (await LoginPageEmailError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetPasswordErrorMessageAsync()
        {
            await LoginPagePasswordError.WaitForAsync();
            return (await LoginPagePasswordError.TextContentAsync()) ?? string.Empty;
        }

        public async Task<bool> IsRememberCheckboxCheckedAsync()
        {
            return await LoginPageRememberCheckbox.IsCheckedAsync();
        }

        public Task FillEmailAsync(string email) => LoginPageEmailInput.FillAsync(email);
        public Task FillPasswordAsync(string password) => LoginPagePasswordInput.FillAsync(password);
        public Task ClearEmailAsync() => LoginPageEmailInput.ClearAsync();
        public Task ClearPasswordAsync() => LoginPagePasswordInput.ClearAsync();
        public Task ClickRememberCheckboxAsync() => LoginPageRememberCheckbox.ClickAsync();
        public Task ClickForgotPasswordLinkAsync() => LoginPageForgotPasswordLink.ClickAsync();
        public Task ClickLoginAsync() => LoginPageLoginBtn.ClickAsync();

        public async Task<string> GetGeneralErrorMessageAsync()
        {
            await LoginPageGeneralError.WaitForAsync();
            return (await LoginPageGeneralError.TextContentAsync()) ?? string.Empty;
        }

        public async Task ExpectDefaultControlsVisibleAsync()
        {
            await Expect(LoginPageEmailInput).ToBeVisibleAsync();
            await Expect(LoginPagePasswordInput).ToBeVisibleAsync();
            await Expect(LoginPageRememberCheckbox).ToBeVisibleAsync();
            await Expect(LoginPageForgotPasswordLink).ToBeVisibleAsync();
            await Expect(LoginPageLoginBtn).ToBeVisibleAsync();
        }

        public async Task OpenForgotPasswordAsync()
        {
            await OpenAsync();
            await ClickForgotPasswordLinkAsync();
            await ForgotPasswordTitle.WaitForAsync();
        }

        public async Task<string> GetForgotPasswordTitleAsync()
        {
            await ForgotPasswordTitle.WaitForAsync();
            return (await ForgotPasswordTitle.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetForgotPasswordMessageAsync()
        {
            await ForgotPasswordMessage.WaitForAsync();
            return (await ForgotPasswordMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task<string> GetForgotPasswordEmailPlaceholderAsync()
        {
            return (await ForgotPasswordEmailInput.GetAttributeAsync("placeholder")) ?? string.Empty;
        }

        public async Task<string> GetForgotPasswordEmailErrorMessageAsync()
        {
            await ForgotPasswordEmailError.WaitForAsync();
            return (await ForgotPasswordEmailError.TextContentAsync()) ?? string.Empty;
        }

        public Task FillForgotPasswordEmailAsync(string email) => ForgotPasswordEmailInput.FillAsync(email);

        public async Task ClickForgotPasswordLoginAsync()
        {
            await ForgotPasswordLoginBtn.ClickAsync();
            await Page.WaitForSelectorAsync("h1:has-text(\"Login\")");
        }

        public Task ClickForgotPasswordResetAsync() => ForgotPasswordResetBtn.ClickAsync();

        public async Task<string> GetForgotPasswordSuccessMessageAsync()
        {
            await ForgotPasswordSuccessMessage.WaitForAsync();
            return (await ForgotPasswordSuccessMessage.TextContentAsync()) ?? string.Empty;
        }

        public async Task ExpectForgotPasswordControlsVisibleAsync()
        {
            await Expect(ForgotPasswordEmailInput).ToBeVisibleAsync();
            await Expect(ForgotPasswordLoginBtn).ToBeVisibleAsync();
            await Expect(ForgotPasswordResetBtn).ToBeVisibleAsync();
        }
    }
}
