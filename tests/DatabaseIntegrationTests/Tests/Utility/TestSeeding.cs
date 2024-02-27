using NUnit.Framework;
using Persistence.Data;
using System.Linq;

namespace DatabaseIntegrationTests.Tests.Utility
{
    public class TestSeeding : BaseTest
    {

        [Test]
        public void TestConnection()
        {
            Assert.IsNotNull(DbContext);

            Assert.IsTrue(DbContext.Gebruikers.Any());
            Assert.IsTrue(DbContext.Projecten.Any());
            Assert.IsTrue(DbContext.Connections.Any());
            Assert.IsTrue(DbContext.FysiekeServers.Any());
            Assert.IsTrue(DbContext.VirtualMachines.Any());
            Assert.IsTrue(DbContext.VMContracts.Any());
            Assert.IsTrue(DbContext.Klanten.Any());
            Assert.IsTrue(DbContext.Admins.Any());
            Assert.IsTrue(DbContext.BackUps.Any());
        }

    }
}
