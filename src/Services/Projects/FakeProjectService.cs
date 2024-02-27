using Domain.Projecten;
using Shared.VirtualMachines;
using Domain.Users;
using Domain.VirtualMachines.VirtualMachine;
using Services.Projects;
using Shared.Projects;
using Shared.Users;
using Fakers.Projects;

namespace Services.Projects
{
    public class FakeProjectService : IProjectService
    {
        private List<Project> _projects = new();

        public FakeProjectService()
        {
            _projects = ProjectFaker.Instance.Generate(15);

        }

        public async Task<ProjectResponse.Create> CreateAsync(ProjectRequest.Create request)
        {
            await Task.Delay(100);

            ProjectResponse.Create response = new();
            int id = _projects.Max(x => x.Id) + 1;

            Project p = new Project(request.Name) { Id = id };
            _projects.Add(p);



            response.ProjectId = id;

            return response;
        }


        public async Task<ProjectResponse.Delete> DeleteAsync(ProjectRequest.Delete request)
        {
            await Task.Delay(100);

            var proj = _projects.Single(x => x.Id == request.ProjectId);

            _projects.Remove(proj);

            return new ProjectResponse.Delete { Id= proj.Id };
        }

        public async Task<ProjectResponse.Edit> EditAsync(ProjectRequest.Edit request)
        {
            await Task.Delay(100);
            ProjectResponse.Edit response = new();

            var proj = _projects.SingleOrDefault(x => x.Id == request.ProjectId);

            if (proj == null)
            {
                response.ProjectId = -1;
                return response;
            }


            proj.Name = request.Project.Name;
          

            response.ProjectId = proj.Id;
            return response;


        }

        public async Task<ProjectResponse.Detail> GetDetailAsync(ProjectRequest.Detail request)
        {
            ProjectResponse.Detail response = new();


            Project project = _projects.Single(e => e.Id == request.Id);
            List<VirtualMachineDto.Index> vms = new();

            project.VirtualMachines.ForEach(e => vms.Add(new VirtualMachineDto.Index() { Id = e.Id, Mode = e.Mode, Name = e.Name }));

            response.Id = project.Id;
            response.Name = project.Name;
            response.User = new UserDto.Index
            {
                PhoneNumber = project.Klant.PhoneNumber,
                Name = project.Klant.Name,
                Email = project.Klant.Email,
                FirstName = project.Klant.FirstName,
                Id = project.Klant.Id
            };
            response.VirtualMachines = vms;



            return response;
        }

        public async Task<ProjectResponse.All> GetIndexAsync(ProjectRequest.All request)
        {

            await Task.Delay(100);

            ProjectResponse.All response = new();
            List<Project> projects;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                projects = _projects.FindAll(e => e.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || e.Klant.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                projects = _projects;
            }

            response.Total = projects.Count();
            response.Projects = projects.Select(x => new ProjectDto.Index
            {
                Id = x.Id,
                Name = x.Name,
                User = new UserDto.Index
                {
                    Email = x.Klant.Email,
                    FirstName = x.Klant.FirstName,
                    Id = x.Klant.Id,
                    Name = x.Klant.Name,
                    PhoneNumber = x.Klant.PhoneNumber
                }

            }).ToList();

            return response;
        }
    }
}
