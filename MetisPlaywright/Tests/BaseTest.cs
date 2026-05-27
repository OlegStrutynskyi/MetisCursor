using MetisPlaywright.Fixtures;
using NUnit.Framework;

namespace MetisPlaywright.Tests
{
    public class BaseTest
    {
        protected PlaywrightFixture Fixture { get; private set; } = null!;

        [SetUp]
        public async Task SetUp()
        {
            Fixture = new PlaywrightFixture();
            await Fixture.InitializeAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await Fixture.DisposeAsync();
        }
    }
}
