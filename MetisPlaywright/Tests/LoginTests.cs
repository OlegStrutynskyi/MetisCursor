using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class LoginTests : BaseTest
    {
        [Test]
        public async Task T01_LoginPage_DefaultView()
        {
            const string expectedTitle = "Login";
            const string expectedMessage = "Welcome back! Please enter your details.";
            const string expectedEmailPlaceholder = "Enter your email";
            const string expectedPasswordPlaceholder = "Enter your password";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            var actualTitle = await loginPage.GetTitleAsync();
            var actualMessage = await loginPage.GetMessageAsync();
            var actualEmailPlaceholder = await loginPage.GetEmailPlaceholderAsync();
            var actualPasswordPlaceholder = await loginPage.GetPasswordPlaceholderAsync();

            actualTitle.Should().Be(expectedTitle, "Login page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "Login page message is not correct.");
            actualEmailPlaceholder.Should().Be(expectedEmailPlaceholder, "Email placeholder text is not correct.");
            actualPasswordPlaceholder.Should().Be(expectedPasswordPlaceholder, "Password placeholder text is not correct.");
            (await loginPage.IsRememberCheckboxCheckedAsync()).Should().BeFalse("'Remember me' checkbox should be unchecked by default.");
            await loginPage.ExpectDefaultControlsVisibleAsync();
        }

        [Test]
        public async Task T02_LoginPage_EmptyFields()
        {
            const string expectedEmailError = "'Email' is required.";
            const string expectedPasswordError = "'Password' is required.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            await loginPage.ClickLoginAsync();
            var actualEmailError = await loginPage.GetEmailErrorMessageAsync();
            var actualPasswordError = await loginPage.GetPasswordErrorMessageAsync();

            actualEmailError.Should().Be(expectedEmailError, "Email error should match the expected message when the email field is empty.");
            actualPasswordError.Should().Be(expectedPasswordError, "Password error should match the expected message when the password field is empty.");
        }

        [Test]
        public async Task T03_LoginPage_InvalidEmail()
        {
            const string expectedInvalidEmailError = "'Email' is not a valid email address.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.InvalidEmail);
            await loginPage.ClickLoginAsync();
            var actualEmailError = await loginPage.GetEmailErrorMessageAsync();
            
            actualEmailError.Should().Be(expectedInvalidEmailError, "An invalid email should show the correct validation message.");
        }

        [Test]
        public async Task T04_LoginPage_IncorrectEmail()
        {
            const string expectedFailureMessage = "We’re having trouble logging you in. The account may not exist or might be deactivated. Please double-check your login details and try again. If the issue persists, contact Metis Support for assistance.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.IncorrectEmail);
            await loginPage.FillPasswordAsync(Config.CorrectPassword);
            await loginPage.ClickLoginAsync();
            var actualFailure = await loginPage.GetGeneralErrorMessageAsync();

            actualFailure.Should().Be(expectedFailureMessage, "An incorrect account should show the general login failure message.");
        }

        [Test]
        public async Task T05_LoginPage_IncorrectPassword()
        {
            const string expectedFailureMessage = "We’re having trouble logging you in. The account may not exist or might be deactivated. Please double-check your login details and try again. If the issue persists, contact Metis Support for assistance.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.CorrectEmailEmptyAutoTests1);
            await loginPage.FillPasswordAsync(Config.IncorrectPassword);
            await loginPage.ClickLoginAsync();
            var actualFailure = await loginPage.GetGeneralErrorMessageAsync();

            actualFailure.Should().Be(expectedFailureMessage, "An incorrect account should show the general login failure message.");
        }

        [Test]
        public async Task T06_LoginPage_CorrectData()
        {
            const string expectedTitle = "Dashboard";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenAsync();
            await loginPage.FillEmailAsync(Config.CorrectEmailEmptyAutoTests1);
            await loginPage.FillPasswordAsync(Config.CorrectPassword);
            await loginPage.ClickLoginAsync();
            var dashboardPage = new DashboardPage(Fixture.Page);
            var actualTitle = await dashboardPage.GetDashboardTitleAsync();

            actualTitle.Should().Contain(expectedTitle, "Logging in with correct credentials should navigate to the Dashboard page.");
        }

        [Test]
        public async Task T07_ForgotPasswordPageDefaultView()
        {
            const string expectedTitle = "Forgot your password?";
            const string expectedMessage = "Please enter your email.";
            const string expectedEmailPlaceholder = "Enter your email";
           
            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenForgotPasswordAsync();
            var actualTitle = await loginPage.GetForgotPasswordTitleAsync();
            var actualMessage = await loginPage.GetForgotPasswordMessageAsync();
            var actualEmailPlaceholder = await loginPage.GetForgotPasswordEmailPlaceholderAsync();

            actualTitle.Should().Be(expectedTitle, "Forgot password page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "Forgot password page message is not correct.");
            actualEmailPlaceholder.Should().Be(expectedEmailPlaceholder, "Forgot password email placeholder is not correct.");
            await loginPage.ExpectForgotPasswordControlsVisibleAsync();
        }

        [Test]
        public async Task T08_ForgotPassword_EmptyEmail()
        {
            const string expectedEmailError = "'Email' is required.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenForgotPasswordAsync();
            await loginPage.FillForgotPasswordEmailAsync(string.Empty);
            await loginPage.ClickForgotPasswordResetAsync();
            var actualEmailError = await loginPage.GetForgotPasswordEmailErrorMessageAsync();

            actualEmailError.Should().Be(expectedEmailError, "Email error should be shown when Reset is clicked with empty email.");
        }

        [Test]
        public async Task T09_ForgotPassword_InvalidEmail()
        {
            const string expectedEmailError = "'Email' is not a valid email address.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenForgotPasswordAsync();
            await loginPage.FillForgotPasswordEmailAsync(Config.InvalidEmail);
            await loginPage.ClickForgotPasswordResetAsync();
            var actualEmailError = await loginPage.GetForgotPasswordEmailErrorMessageAsync();

            actualEmailError.Should().Be(expectedEmailError, "An invalid email on Forgot Password should show the correct validation message.");
        }

        [Test]
        public async Task T10_ForgotPassword_CorrectEmail()
        {
            const string expectedSuccessMessage = "We've sent a password reset email. Please follow the instructions in it.";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenForgotPasswordAsync();
            await loginPage.FillForgotPasswordEmailAsync(Config.CorrectEmailAutoTests1);
            await loginPage.ClickForgotPasswordResetAsync();
            var actualSuccess = await loginPage.GetForgotPasswordSuccessMessageAsync();

            actualSuccess.Should().Be(expectedSuccessMessage, "A valid email on Forgot Password should show the success message.");
        }

        [Test]
        public async Task T11_ForgotPassword_ClickLogin()
        {
            const string expectedTitle = "Login";

            var loginPage = new LoginPage(Fixture.Page);
            await loginPage.OpenForgotPasswordAsync();
            await loginPage.ClickForgotPasswordLoginAsync();
            var actualTitle = await loginPage.GetTitleAsync();

            actualTitle.Should().Be(expectedTitle, "Clicking 'Log in' on Forgot Password should navigate back to the Login page.");
        }      

    }
}
