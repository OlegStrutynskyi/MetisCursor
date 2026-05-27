using MetisPlaywright.Fixtures;
using NUnit.Framework;

namespace MetisPlaywright.Tests
{
    public class BaseTest
    {
        // Assigned in [SetUp] before any test body runs; NUnit guarantees the lifecycle, so the
        // null-forgiving initializer is safe and lets consumers treat Fixture as non-nullable.
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
