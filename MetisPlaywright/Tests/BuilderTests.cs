using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class BuilderTests : BaseTest
    {
        [Test]
        public async Task T01_ContextBuilder_EmptyView()
        {
            const string expectedTitle = "Builder";
            const string expectedGetStartedMessage = "Get Started";
            const string expectedBuilderMessage =
                "To start, drag and drop items from the right to this area and start building your context.";

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.GetContextBuilderPageAutoTests1Async();

            var actualTitle = await builderPage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "Builder page title is not correct.");

            var actualGetStartedMessage = await builderPage.GetStartedMessageTextAsync();
            actualGetStartedMessage.Trim().Should().Be(expectedGetStartedMessage, "Get Started message is not correct.");

            var actualBuilderMessage = await builderPage.GetBuilderMessageTextAsync();
            actualBuilderMessage.Trim().Should().Be(expectedBuilderMessage, "Builder message is not correct.");

            await builderPage.ExpectDefaultControlsVisibleAsync();
            await builderPage.ExpectConstructorButtonsVisibleAsync();
        }

        [Test]
        public async Task T02_ContextBuilder_ClickCreateNewContext()
        {
            const string expectedModalTitle = "Context Settings";

            var builderPage = await new BuilderPage(Fixture.Page).GetContextBuilderPageAutoTests1Async();
            var contextSettingsPage = await builderPage.ClickCreateNewContextBtnAsync();
            await contextSettingsPage.ExpectContextSettingsModalVisibleAsync();

            var actualModalTitle = await contextSettingsPage.GetContextSettingsTitleAsync();
            actualModalTitle.Trim().Should().Be(expectedModalTitle, "Context Settings modal title is not correct.");
        }

        [Test]
        public async Task T03_ContextBuilder_ExistingContext_View()
        {
            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();
            await builderPage.ExpectExistingContextControlsVisibleAsync();
        }

        [Test]
        public async Task T04_ContextBuilder_ExistingContext_ClickBackToOverview()
        {
            const string expectedOverviewTitle = "Context Overview";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);

            var contextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();
            await builderPage.ClickBackToContextOverviewBtnAsync();

            await contextOverviewPage.ExpectOpenedAsync();

            var actualOverviewTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            actualOverviewTitle.Should().Be(expectedOverviewTitle, "Context Overview page title is not correct.");

            var actualContextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            actualContextName.Should().Be(contextName, "Context Overview name should match the name before opening Builder.");
        }
    }
}
