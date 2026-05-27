using MetisPlaywright.Utils;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace MetisPlaywright.Pages
{
    public class ReportsPage : BasePage
    {
        public ReportsPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;

        public async Task GetReportsPage()
        {
            await Page.GotoAsync(Config.BaseUrl + "/reports");
            await PageTitle.WaitForAsync();
        }

        public async Task<string> GetPageTitle()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }
    }
}
