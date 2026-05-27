using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class PeopleTests : BaseTest
    {
        [Test]
        public async Task T01_People_DefaultView()
        {
            const string expectedTitle = "People";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            var actualTitle = await peoplePage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "People page title is not correct.");
        }

        [Test]
        public async Task T02_People_ClickManageTeamsBtn()
        {
            const string expectedPageTitle = "Teams Overview";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            var teamsOverviewPage = await peoplePage.ClickManageTeamsBtnAsync();
            var actualPageTitle = await teamsOverviewPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Teams Overview page title is not correct after clicking Manage Teams.");
        }

        [Test]
        public async Task T03_People_ClickCreatePersonBtn()
        {
            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            await peoplePage.OpenPersonSettingsModalAsync();
            await peoplePage.ExpectPersonSettingsModalVisibleAsync();
        }

        [Test]
        public async Task T04_People_PersonSettingsModal_View()
        {
            const string expectedModalTitle = "Person Settings";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            await peoplePage.OpenPersonSettingsModalAsync();
            var actualModalTitle = await peoplePage.GetPersonSettingsTitleAsync();

            actualModalTitle.Should().Be(expectedModalTitle, "Person Settings modal title is not correct.");
            await peoplePage.ExpectPersonSettingsControlsVisibleAsync();
        }

        [Test]
        public async Task T05_People_PersonSettingsModal_EmptyFields()
        {
            const string expectedFirstNameError = "'First Name' is required.";
            const string expectedLastNameError = "'Last Name' is required.";
            const string expectedEmailError = "'Email' is required.";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            await peoplePage.OpenPersonSettingsModalAsync();
            await peoplePage.ClickPersonSettingsSaveAsync();
            var firstNameError = await peoplePage.GetPersonSettingsFirstNameErrorAsync();
            var lastNameError = await peoplePage.GetPersonSettingsLastNameErrorAsync();
            var emailError = await peoplePage.GetPersonSettingsEmailErrorAsync();

            firstNameError.Should().Be(expectedFirstNameError, "First Name error message is not correct.");
            lastNameError.Should().Be(expectedLastNameError, "Last Name error message is not correct.");
            emailError.Should().Be(expectedEmailError, "Email error message is not correct.");            
        }

        [Test]
        public async Task T06_People_PersonSettingsModal_TakenEmail()
        {
            var expectedEmailError = $"Email '{Config.CorrectEmailAutoTests1}' is already taken.";
            var autoTests1Email = Config.CorrectEmailAutoTests1;
            const string autoTests1FirstName = "AutoTestsFirstName";
            const string autoTests1LastName = "AutoTestsLastName";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            await peoplePage.OpenPersonSettingsModalAsync();
            await peoplePage.FillPersonSettingsFirstNameAsync(autoTests1FirstName);
            await peoplePage.FillPersonSettingsLastNameAsync(autoTests1LastName);
            await peoplePage.FillPersonSettingsEmailAsync(autoTests1Email);
            await peoplePage.ClickPersonSettingsSaveAsync();
            var emailError = await peoplePage.GetPersonSettingsEmailErrorAsync();

            emailError.Should().Be(expectedEmailError, "Email error message should indicate that the email is already taken.");
        }

        [Test]
        public async Task T07_People_PersonSettingsModal_IncorrectDataType()
        {
            const string input = "1";
            const string expectedFirstNameError = "'First Name' must be a valid name.";
            const string expectedLastNameError = "'Last Name' must be a valid name.";
            const string expectedEmailError = "Email must be a valid email address.";

            var peoplePage = new PeoplePage(Fixture.Page);
            await peoplePage.OpenForAutoTests1Async();
            await peoplePage.OpenPersonSettingsModalAsync();
            await peoplePage.FillPersonSettingsFirstNameAsync(input);
            await peoplePage.FillPersonSettingsLastNameAsync(input);
            await peoplePage.FillPersonSettingsEmailAsync(input);
            await peoplePage.ClickPersonSettingsSaveAsync();
            var firstNameError = await peoplePage.GetPersonSettingsFirstNameErrorAsync();
            var lastNameError = await peoplePage.GetPersonSettingsLastNameErrorAsync();
            var emailError = await peoplePage.GetPersonSettingsEmailErrorAsync();

            firstNameError.Should().Be(expectedFirstNameError, "First Name error message should indicate that the value must be valid.");
            lastNameError.Should().Be(expectedLastNameError, "Last Name error message should indicate that the value must be valid.");
            emailError.Should().Be(expectedEmailError, "Email error message should indicate that the value must be valid.");
        }

        [Test]
        public async Task T08_People_PersonSettingsModal_CreateNewUser_FullFlow()
        { 
            const string firstName = "AutoTestsFirstName";
            const string lastName = "AutoTestsLastName";
            var email = $"{Config.NewAutotestsUser}";
            const string expectedStatus1 = "Invited";
            const string expectedStatus2 = "Active";
            const string expectedStatus3 = "Deactivated";
            var mailboxUrl = $"https://app.endtest.io/mailbox?email={email}";
            const string expectedDashboardTitle = "Dashboard";
            const string expectedErrorMessage = "This account is currently deactivated. Access is restricted. Please reach out to Metis Support for further guidance.";
            var postgresRepo = new PostgresRepository();
            var neo4jRepo = new Neo4jRepository();

            var createCompanyPage = new CreateCompanyPage(Fixture.Page);
            await Fixture.Page.GotoAsync(mailboxUrl);
            var baselineSetupEmails = await createCompanyPage.GetSetupEmailCountAsync();

            await postgresRepo.DeleteUserByEmailAsync(email);
            await neo4jRepo.DeleteNodeByNameAsync(email);

            try
            {
                var peoplePage = new PeoplePage(Fixture.Page);
                await peoplePage.OpenForAutoTests1Async();
                var initialPersonCount = await peoplePage.GetPersonsCountAsync();
                await peoplePage.OpenPersonSettingsModalAsync();
                await peoplePage.FillPersonSettingsFirstNameAsync(firstName);
                await peoplePage.FillPersonSettingsLastNameAsync(lastName);
                await peoplePage.FillPersonSettingsEmailAsync(email);
                await peoplePage.ClickPersonSettingsSaveAsync();
                await peoplePage.ExpectPersonStatusAsync(email, expectedStatus1);

                var finalPersonCount = await peoplePage.GetPersonsCountAsync();
                finalPersonCount.Should().Be(initialPersonCount + 1, "A new person should be added to the grid after saving.");

                //set up a password for the new user to change their status to 'Active'
                var leftMenuPage = new LeftMenuPage(Fixture.Page);
                await leftMenuPage.ClickLogOutIconAsync();
                await createCompanyPage.WaitForNewSetupEmailAsync(mailboxUrl, baselineSetupEmails);
                await createCompanyPage.ClickFinishLinkAsync();
                var setupLink = await createCompanyPage.GetFinishSetupLinkAsync();
                await Fixture.Page.GotoAsync(setupLink);

                var createPasswordPage = new CreatePasswordPage(Fixture.Page);
                await createPasswordPage.FillPasswordAsync(Config.CorrectPassword);
                await createPasswordPage.FillConfirmPasswordAsync(Config.CorrectPassword);
                await createPasswordPage.ClickSetPasswordAsync();
                await Fixture.Page.WaitForURLAsync(url => !url.Contains("reset-password"), new() { Timeout = 30_000 });

                var loginPage = new LoginPage(Fixture.Page);
                await loginPage.FillEmailAsync(email);
                await loginPage.FillPasswordAsync(Config.CorrectPassword);
                await loginPage.ClickLoginAsync();
                var dashboardPage = new DashboardPage(Fixture.Page);
                var actualDashboardTitle = await dashboardPage.GetDashboardTitleAsync();
                actualDashboardTitle.Should().Contain(expectedDashboardTitle, "Logging in with correct credentials should navigate to the Dashboard page.");

                var leftMenuPage2 = new LeftMenuPage(Fixture.Page);
                await leftMenuPage2.ClickLogOutIconAsync();
                var peoplePage2 = new PeoplePage(Fixture.Page);
                await peoplePage2.OpenForAutoTests1Async();
                await peoplePage2.ExpectPersonStatusAsync(email, expectedStatus2);

                await peoplePage2.DeactivatePersonAsync(email);
                await peoplePage2.ExpectPersonStatusAsync(email, expectedStatus3);

                var leftMenuPage3 = new LeftMenuPage(Fixture.Page);
                await leftMenuPage3.ClickLogOutIconAsync();
                var loginPage2 = new LoginPage(Fixture.Page);
                await loginPage2.FillEmailAsync(email);
                await loginPage2.FillPasswordAsync(Config.CorrectPassword);
                await loginPage2.ClickLoginAsync();
                var actualErrorMessage = await loginPage2.GetGeneralErrorMessageAsync();
                actualErrorMessage.Should().Be(expectedErrorMessage, "An incorrect account should show the general login failure message.");

                await loginPage2.ClearEmailAsync();
                await loginPage2.ClearPasswordAsync();
                var peoplePage3 = new PeoplePage(Fixture.Page);
                await peoplePage3.OpenForAutoTests1Async();
                await peoplePage3.ActivatePersonAsync(email);
                await peoplePage3.ExpectPersonStatusAsync(email, expectedStatus2);

                var leftMenuPage4 = new LeftMenuPage(Fixture.Page);
                await leftMenuPage4.ClickDashboardIconAsync();
                await leftMenuPage4.ClickLogOutIconAsync();
                var loginPage3 = new LoginPage(Fixture.Page);
                await loginPage3.FillEmailAsync(email);
                await loginPage3.FillPasswordAsync(Config.CorrectPassword);
                await loginPage3.ClickLoginAsync();
                var dashboardPage2 = new DashboardPage(Fixture.Page);
                var actualDashboardTitle2 = await dashboardPage2.GetDashboardTitleAsync();
                actualDashboardTitle2.Should().Contain(expectedDashboardTitle, "Logging in with correct credentials should navigate to the Dashboard page.");
            }
            finally
            {
                await postgresRepo.DeleteUserByEmailAsync(email);
                await neo4jRepo.DeleteNodeByNameAsync(email);
            }
        }
    }
}
