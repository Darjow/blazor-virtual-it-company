using Bogus;
using Domain.Projecten;
using Domain.Users;
using Domain.VirtualMachines;
using Domain.VirtualMachines.VirtualMachine;
using Fakers.User;
using Fakers.VirtualMachines;
using System.Collections.Generic;
using System.Linq;

namespace Fakers.Projects
{
    public class ProjectFaker : Faker<Project>
    {
        private int id = 1;


        private List<Project> _projects = new();


        private static ProjectFaker? _instance;

        public static ProjectFaker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProjectFaker();
                }
                return _instance;
            }
        }


        public ProjectFaker()
        {

            CustomInstantiator(e => new Project($"Project: {e.Company.CompanyName()}"));
            RuleFor(x => x.Id, _ => id++);
            RuleFor(x => x.VirtualMachines, _ => VirtualMachineFaker.Instance.Generate(5));
            RuleFor(x => x.Klant, _ => UserFaker.Klant.Instance.Generate(1)[0]);

        }



        public override List<Project> Generate(int count, string ruleSets = null)
        {
            List<Project> output = new();




            if (_projects.Count() < count)
            {
                output = base.Generate(count, ruleSets);
                output.ForEach(e => _projects.Add(e));
            }
            else
            {
                output = _projects.GetRange(0, count);
            }

            return output;
        }

    }
}