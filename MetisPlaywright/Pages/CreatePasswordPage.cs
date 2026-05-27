using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Pages
{
    public class CreatePasswordPage : BasePage
    {
        public CreatePasswordPage(IPage page) : base(page) { }

        //single-use reset-password token shared by all error-flow tests. None of them submits a
        //valid password, so the token is never consumed and the same URL keeps rendering the form.
        private const string ResetPasswordToken =
            "CfDJ8OVzOhF4FltNjcGDh9rNApW7lFdevSUuqU0YBY-m0GgIXphVmHhrCqOF4mHUTO7NYWmDyFARHOywQ9_xq" +
            "GIABxI3AwLR8m2ao8JJUeT35SvkPvM1yYIZRIST79NYfs8KtF1X96Q2LUPlEUNIYVJwco3dFbeGATVu6ZiO_6" +
            "cuiGA1s91Yw2PfeaZEVKm1YltMKPl1eKT1E2vO8YNVuhYVzsPFjm065LWG2JQ9PPbHTHemkumykR2cQrRX2J2" +
            "j8jbpcGXNXmX8buuIUh8jd048zpGsXYoTiE5KKUn7Mjy0l0owvx_l1XeKZ5LnlBwIfgZsi8kjGlPMwnED7VCE" +
            "zeGZ0SZ6RsabvoWoqtVKYuKCYwnzjXFFnGP1yDa7_edua71adnzLE-4I9WWFMZ4MdMF0Y0ntBYJfyfnhHPEgp" +
            "fasBG0-ucQhL2x0F6OMftLe4fNPX2pezwRcu1JFVLnoKKjg3ANoGNgecj1XQEcR3cwbiz87";

        private ILocator CreatePasswordTitle => Page.Locator("//h1").First;
        private ILocator CreatePasswordMessage => Page.Locator("//h1/following-sibling::p").First;
        private ILocator PasswordInput => Page.Locator("//label[normalize-space()='Password']/following-sibling::span/input").First;
        private ILocator ConfirmPasswordInput => Page.Locator("//label[normalize-space()='Confirm Password']/following-sibling::span/input").First;
        private ILocator PasswordErrorMessage => Page.Locator("//label[normalize-space()='Password']/following-sibling::div/div[@class='text-base e-error']").First;
        private ILocator ConfirmPasswordErrorMessage => Page.Locator("//label[normalize-space()='Confirm Password']/following-sibling::div/div[@class='text-base e-error']").First;
        private ILocator SetPasswordBtn => Page.Locator("//button[normalize-space()='Set Password']").First;

        public async Task OpenAsync()
        {
            await Page.GotoAsync($"{Config.BaseUrl}reset-password?token={ResetPasswordToken}");
            await CreatePasswordTitle.WaitForAsync();
        }

        //read-only text/attributes
        public async Task<string> GetTitleAsync() =>
            (await CreatePasswordTitle.TextContentAsync()) ?? string.Empty;

        public async Task<string> GetMessageAsync()
        {
            var text = (await CreatePasswordMessage.TextContentAsync()) ?? string.Empty;
            return text.Trim();
        }

        public async Task<string> GetPasswordPlaceholderAsync() =>
            (await PasswordInput.GetAttributeAsync("placeholder")) ?? string.Empty;

        public async Task<string> GetConfirmPasswordPlaceholderAsync() =>
            (await ConfirmPasswordInput.GetAttributeAsync("placeholder")) ?? string.Empty;

        public async Task<string> GetPasswordErrorMessageAsync() =>
            (await PasswordErrorMessage.TextContentAsync()) ?? string.Empty;

        public async Task<string> GetConfirmPasswordErrorMessageAsync() =>
            (await ConfirmPasswordErrorMessage.TextContentAsync()) ?? string.Empty;

        //actions
        public Task FillPasswordAsync(string value) => PasswordInput.FillAsync(value);
        public Task FillConfirmPasswordAsync(string value) => ConfirmPasswordInput.FillAsync(value);
        public Task ClickSetPasswordAsync() => SetPasswordBtn.ClickAsync();

        //expectations
        public Task ExpectDefaultControlsVisibleAsync() => Task.WhenAll(
            Expect(PasswordInput).ToBeVisibleAsync(),
            Expect(ConfirmPasswordInput).ToBeVisibleAsync(),
            Expect(SetPasswordBtn).ToBeVisibleAsync());
    }
}
