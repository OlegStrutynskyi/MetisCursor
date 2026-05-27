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
        public async Task T04_ContextBuilder_ExistingContext_ClickContextSettings()
        {
            const string expectedModalTitle = "Context Settings";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();

            var contextName = (await builderPage.GetNameTextAsync()).Trim();
            var contextSettingsPage = await builderPage.ClickContextSettingsBtnAsync();

            var actualModalTitle = await contextSettingsPage.GetContextSettingsTitleAsync();
            actualModalTitle.Trim().Should().Be(expectedModalTitle, "Context Settings modal title is not correct.");

            var actualContextTitle = (await contextSettingsPage.GetContextTitleAsync()).Trim();
            actualContextTitle.Should().Be(contextName, "Context Title in settings should match the Builder context name.");
        }

        [Test]
        public async Task T05_ContextBuilder_ExistingContext_ClickBackToOverview()
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

        [Test]
        public async Task T06_ContextBuilder_ExistingContext_ClickSaveAsTemplate()
        {
            const string expectedModalTitle = "Save as a Template";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);

            var contextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            var contextDescription = (await contextOverviewPage.GetDescriptionTextAsync()).Trim();

            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();
            await builderPage.ClickSaveAsTemplateBtnAsync();

            var actualModalTitle = (await builderPage.GetSaveAsTemplateModalTitleAsync()).Trim();
            actualModalTitle.Should().Be(expectedModalTitle, "Save as a Template modal title is not correct.");

            var actualTemplateName = (await builderPage.GetSaveAsTemplateModalNameAsync()).Trim();
            actualTemplateName.Should().Be(contextName, "Template Name should match Context Overview name.");

            var actualTemplateDescription = (await builderPage.GetSaveAsTemplateModalDescriptionAsync()).Trim();
            actualTemplateDescription.Should().Be(contextDescription, "Template Description should match Context Overview description.");

            await builderPage.ExpectSaveAsTemplateModalControlsVisibleAsync();
        }

        [Test]
        public async Task T07_ContextBuilder_ExistingContext_ClickBuildContext()
        {
            const string expectedOverviewTitle = "Context Overview";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);

            var contextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();
            await builderPage.ClickBuildContextBtnAsync();

            await contextOverviewPage.ExpectOpenedAsync();

            var actualOverviewTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            actualOverviewTitle.Should().Be(expectedOverviewTitle, "Context Overview page title is not correct.");

            var actualContextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            actualContextName.Should().Be(contextName, "Context Overview name should match the name before opening Builder.");
        }

        [Test]
        public async Task T08_ContextBuilder_ExistingContext_ClickAddChildContext()
        {
            const string expectedModalTitle = "Context Settings";
            const string expectedCreateBtnText = "Create Child";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();

            var contextSettingsPage = await builderPage.ClickAddChildContextBtnAsync();

            var actualModalTitle = (await contextSettingsPage.GetContextSettingsTitleAsync()).Trim();
            actualModalTitle.Should().Be(expectedModalTitle, "Context Settings modal title is not correct.");

            var actualCreateBtnText = (await contextSettingsPage.GetCreateBtnTextAsync()).Trim();
            actualCreateBtnText.Should().Be(expectedCreateBtnText, "Create button text should be 'Create Child' for child context.");
        }
    }
}
