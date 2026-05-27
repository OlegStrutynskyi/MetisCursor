using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class TemplatesTests : BaseTest
    {
        [Test]
        public async Task T01_Templates_DefaultView()
        {
            const string expectedTitle = "Templates Overview";

            var templatesPage = new TemplatesPage(Fixture.Page);
            await templatesPage.OpenForAutoTests1Async();
            var actualTitle = await templatesPage.GetTemplatesPageTitleAsync();

            actualTitle.Should().Be(expectedTitle, "Templates page title is not correct.");
        }

        [Test]
        public async Task T02_Templates_ClickCreateNewTemplate()
        {
            var templatesPage = new TemplatesPage(Fixture.Page);
            await templatesPage.OpenForAutoTests1Async();
            await templatesPage.ClickCreateNewTemplateBtnAsync();
            await templatesPage.ExpectTemplateSettingsModalVisibleAsync();
        }

        [Test]
        public async Task T03_Templates_CreateTemplateModal_View()
        {
            const string expectedTitle = "Create Template";

            var templatesPage = new TemplatesPage(Fixture.Page);
            await templatesPage.OpenForAutoTests1Async();
            await templatesPage.ClickCreateNewTemplateBtnAsync();
            var actualTitle = await templatesPage.GetTemplateSettingsTitleAsync();
            await templatesPage.ExpectTemplateSettingsControlsVisibleAsync();

            actualTitle.Should().Be(expectedTitle, "Template Settings modal title is not correct.");
        }

        [Test]
        public async Task T04_Templates_CreateTemplateModal_EmptyFields()
        {
            const string expectedNameError = "'Name' is required";

            var templatesPage = new TemplatesPage(Fixture.Page);
            await templatesPage.OpenForAutoTests1Async();
            await templatesPage.ClickCreateNewTemplateBtnAsync();
            await templatesPage.ClickTemplateSettingsSaveAsync();
            var nameError = await templatesPage.GetTemplateSettingsNameErrorAsync();

            nameError.Should().Contain(expectedNameError, "Name error message should be displayed when trying to save Template with empty Name.");
        }

        [Test]
        public async Task T05_Templates_CreateTemplate()
        {
            const string expectedName = "AutoTests Template 1";
            var neo4jRepo = new Neo4jRepository();

            //ensure clean state in case a previous run left the Template behind
            await neo4jRepo.DeleteTemplateByNameAsync(expectedName);

            try
            {
                var templatesPage = new TemplatesPage(Fixture.Page);
                await templatesPage.OpenForAutoTests1Async();
                var numberBefore = await templatesPage.GetGridTotalRecordsAsync();
                await templatesPage.ClickCreateNewTemplateBtnAsync();
                await templatesPage.FillTemplateSettingsNameAsync(expectedName);
                await templatesPage.ClickTemplateSettingsSaveAsync();

                var leftMenuPage = new LeftMenuPage(Fixture.Page);
                await leftMenuPage.ClickTemplatesIconAsync();

                var templatesPage2 = new TemplatesPage(Fixture.Page);
                var numberAfter = await templatesPage2.GetGridTotalRecordsAsync();
                var templateNames = await templatesPage2.GetTemplateNamesAsync();

                numberAfter.Should().Be(numberBefore + 1, "Total number of Templates should increase by 1 after creating a new Template.");
                templateNames.Should().Contain(expectedName, "Newly created Template should be present in the grid.");
            }
            finally
            {
                await neo4jRepo.DeleteTemplateByNameAsync(expectedName);
            }
        }
    }
}
