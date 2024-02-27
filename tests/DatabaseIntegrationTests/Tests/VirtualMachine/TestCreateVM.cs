using NUnit.Framework;
using Shared.Authentication;
using Shared.VirtualMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseIntegrationTests.Tests.VirtualMachine
{
    public class TestCreateVM : BaseTest
    {


        [Test]
        public async Task WithWrongParams()
        {
            await LoginAsBilly();

            var response = await HttpClient.PostAsJsonAsync("virtualmachine", new VirtualMachineRequest.Create
            {
                CustomerId = 1,
                VirtualMachine = new VirtualMachineDto.Mutate
                {
                    Backup = null,
                    Hardware = null,
                    Name = "Heu",
                    OperatingSystem = Domain.VirtualMachines.VirtualMachine.OperatingSystemEnum.WINDOWS_10
                }
            });


            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task WithCorrectParams()
        {
            await LoginAsBilly();

            var response = await HttpClient.PostAsJsonAsync("virtualmachine", new VirtualMachineRequest.Create
            {
                CustomerId = 4,
                VirtualMachine = new VirtualMachineDto.Mutate
                {
                    Backup = new Domain.VirtualMachines.BackUp.Backup(Domain.VirtualMachines.BackUp.BackUpType.GEEN, null),
                    Hardware = new Domain.Common.Hardware(5000, 50000, 5),
                    Name = "Project",
                    OperatingSystem = Domain.VirtualMachines.VirtualMachine.OperatingSystemEnum.WINDOWS_10,
                    End = DateTime.Parse("2023-12-12"),
                    ProjectId = 3,
                    Start = DateTime.Now
                }
            });


            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
