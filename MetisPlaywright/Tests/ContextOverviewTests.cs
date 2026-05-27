using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class ContextOverviewTests : BaseTest
    {
        [Test]
        public async Task T01_ContextOverview_DefaultView()
        {
            const string expectedTitle = "Context Overview";
            var expectedContextName = $"{Config.AutoTestsContext1}";
            var expectedCustomerName = $"{Config.AutoTestsCustomer1}";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(expectedContextName);

            var actualTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            var actualContextName = await contextOverviewPage.GetContextNameAsync();
            var actualCustomerName = await contextOverviewPage.GetCustomerNameAsync();

            actualTitle.Should().Be(expectedTitle, "The page title is not correct.");
            actualContextName.Should().Be(expectedContextName, "The context name in the overview is not correct.");
            actualCustomerName.Should().Be(expectedCustomerName, "The customer name in the overview is not correct.");

            await contextOverviewPage.ExpectDefaultControlsVisibleAsync();
        }

        [Test]
        public async Task T02_ContextOverview_ClickOpenContextBuilder()
        {
            const string expectedBuilderTitle = "Builder";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);

            var contextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();

            var actualBuilderTitle = await builderPage.GetPageTitleTextAsync();
            actualBuilderTitle.Should().Be(expectedBuilderTitle, "Builder page title is not correct.");

            var actualContextName = (await builderPage.GetNameTextAsync()).Trim();
            actualContextName.Should().Be(contextName, "Builder context name should match Context Overview name.");
        }
    }
}
