using MetisPlaywright.Utils;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace MetisPlaywright.Pages
{
    public class StartOptimizationPage : BasePage
    {
        public StartOptimizationPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;

        public async Task GetStartOptimizationPage()
        {
            await Page.GotoAsync(Config.BaseUrl + "/start-optimization");
            await PageTitle.WaitForAsync();
        }

        public async Task<string> GetPageTitle()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }
    }
}
