using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace MetisPlaywright.Tests
{
    public class CreateCompanyTests : BaseTest
    {
        [Test]
        public async Task T01_CreateCompany_ValidData()
        {
            const string newCompanyName = "AutoTestsNewCompany1";
            const string newUserFirstName = "AutoTestUserOne";
            const string newUserLastName = "PlaywrightOne";
            var email = Config.CorrectEmailNewCompany1;
            var mailboxUrl = $"https://app.endtest.io/mailbox?email={email}";
            var expectedDashboardTitle = $"{Config.DefaultCoreLabel}s Dashboard";

            var postgresRepo = new PostgresRepository();
            var neo4jRepo = new Neo4jRepository();
            var createCompanyPage = new CreateCompanyPage(Fixture.Page);

            //ensure clean state up-front in case a previous failed run left rows behind
            await postgresRepo.DeleteCompanyByAdminEmailAsync(email);
            Thread.Sleep(2000);

            try
            {
                //capture mailbox baseline before POST so we can detect the newly-arrived invitation
                await Fixture.Page.GotoAsync(mailboxUrl);
                await Fixture.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                var baselineSetupEmails = await createCompanyPage.GetSetupEmailCountAsync();
                TestContext.Out.WriteLine($"Mailbox baseline: {baselineSetupEmails} 'Finish Setup' email(s) before POST /setup");

                var requestContext = await createCompanyPage.CreateRequestContextAsync();
                var response = await requestContext.PostAsync("setup", new APIRequestContextOptions
                {
                    DataObject = new
                    {
                        user = new
                        {
                            email,
                            firstName = newUserFirstName,
                            lastName = newUserLastName,
                            status = 1
                        },
                        company = new
                        {
                            name = newCompanyName,
                            coreLabel = Config.DefaultCoreLabel
                        }
                    }
                });

                response.Status.Should().Be(202,
                    "Creating a new company with valid data should return status code 202 Accepted.");

                //poll mailbox until a NEW invitation arrives — guarantees we won't click a stale,
                //expired link that endtest.io keeps from previous test runs
                await createCompanyPage.WaitForNewSetupEmailAsync(mailboxUrl, baselineSetupEmails, TimeSpan.FromMinutes(2));
                var afterCount = await createCompanyPage.GetSetupEmailCountAsync();
                TestContext.Out.WriteLine($"Mailbox after polling: {afterCount} 'Finish Setup' email(s) — taking the most recent");

                await createCompanyPage.ClickFinishLinkAsync();
                var setupLink = await createCompanyPage.GetFinishSetupLinkAsync();
                await Fixture.Page.GotoAsync(setupLink);

                var createPasswordPage = new CreatePasswordPage(Fixture.Page);
                await createPasswordPage.FillPasswordAsync(Config.CorrectPassword);
                await createPasswordPage.FillConfirmPasswordAsync(Config.CorrectPassword);
                await createPasswordPage.ClickSetPasswordAsync();

                //wait for the app to navigate away from /reset-password (it asynchronously redirects
                //either straight to /dashboard or to /login). NetworkIdle alone is unreliable here
                //because the click handler may run additional XHRs *after* the load state stabilizes.
                await Fixture.Page.WaitForURLAsync(url => !url.Contains("reset-password"),
                    new() { Timeout = 30_000 });
                var postSetPasswordUrl = Fixture.Page.Url;
                TestContext.Out.WriteLine($"After Set Password the app navigated to: {postSetPasswordUrl}");

                if (postSetPasswordUrl.Contains("login", StringComparison.OrdinalIgnoreCase))
                {
                    //classic flow: user must log in manually
                    var loginPage = new LoginPage(Fixture.Page);
                    await loginPage.FillEmailAsync(email);
                    await loginPage.FillPasswordAsync(Config.CorrectPassword);
                    await loginPage.ClickLoginAsync();
                }

                //the dashboard title initially renders as a generic "Dashboard" and is updated to
                //"{CoreLabel}s Dashboard" once the company data finishes loading, so we use the
                //auto-retrying Playwright assertion instead of a single point-in-time string check
                var dashboardTitle = Fixture.Page.Locator("//h1[contains(text(),'Dashboard')]").First;
                await Expect(dashboardTitle).ToHaveTextAsync(expectedDashboardTitle,
                    new() { Timeout = 30_000 });
            }
            finally
            {
                await postgresRepo.DeleteCompanyByAdminEmailAsync(email);
                await neo4jRepo.DeleteNodeByNameAsync(email);
                await neo4jRepo.DeleteNodeByNameAsync(newCompanyName);
            }
        }
    }
}
