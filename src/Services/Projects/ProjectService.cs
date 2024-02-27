using System.Linq;
using Persistence.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Projecten;
using System;
using Domain.Common;
using Shared.Projects;
using Shared.VirtualMachines;
using Domain.Users;
using Shared.Users;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Azure.Core;

namespace Services.Projecten
{
    public class ProjectService : IProjectService
    {
        private readonly DotNetDbContext _dbContext;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(DotNetDbContext dbContext, ILogger<ProjectService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<ProjectResponse.Create> CreateAsync(ProjectRequest.Create request)
        {
            _logger.LogInformation($"Request received: CreateAsync(...)");
            var customer = await _dbContext.Klanten.FirstOrDefaultAsync(e => e.Id == request.CustomerId);

            if (customer == null)
            {
                _logger.LogWarning($"Request failed: No customer found with id: {request.CustomerId}.");
                throw new EntityNotFoundException("Customer", "CustomerId", request.CustomerId);
            }

            
            var proj = new Project(request.Name);
            proj.Klant = customer;


            await _dbContext.Projecten.AddAsync(proj);
            await _dbContext.SaveChangesAsync();


            _logger.LogInformation("Request was successful");
            return new ProjectResponse.Create { ProjectId = proj.Id };
        }


        public async Task<ProjectResponse.Delete> DeleteAsync(ProjectRequest.Delete request)
        {
            _logger.LogInformation($"Request received: DeleteAsync({request.ProjectId})");

            _dbContext.Projecten.RemoveIf(e => e.Id == request.ProjectId);

            _logger.LogInformation("Request was successful");
            return new ProjectResponse.Delete { Id = request.ProjectId };
        }

        public async Task<ProjectResponse.Edit> EditAsync(ProjectRequest.Edit request)
        {
            _logger.LogInformation($"Request received: EditAsync({request.ProjectId}, ...)");
            ProjectResponse.Edit response = new();

            var proj = await _dbContext.Projecten.Include(e => e.Klant).SingleOrDefaultAsync(x => x.Id == request.ProjectId);

            if (proj == null)
            {
                _logger.LogWarning($"Request failed: No project found with id: {request.ProjectId}.");
                throw new EntityNotFoundException("Project", "projectId", request.Project);
            }


            proj.Name = request.Project.Name;
            proj.Klant = proj.Klant;

            response.ProjectId = proj.Id;
            _logger.LogInformation("Request was successful");
            return response;


        }

        public async Task<ProjectResponse.Detail> GetDetailAsync(ProjectRequest.Detail request)
        {
            _logger.LogInformation($"Request received: GetDetailsAsync({request.Id})");

            ProjectResponse.Detail response = new();


            Project project = await _dbContext.Projecten.Include(e => e.VirtualMachines).ThenInclude(e => e.Contract).SingleOrDefaultAsync(e => e.Id == request.Id);

            if ( project == null)
            {
                _logger.LogWarning($"Request failed: No project found with id: {request.Id}.");
                throw new EntityNotFoundException("Project", "id", request.Id);

            }

            List<VirtualMachineDto.Index> vms = new();

            project.VirtualMachines.ForEach(e => vms.Add(new VirtualMachineDto.Index() { Id = e.Id, Mode = e.Mode, Name = e.Name }));

            var customer = await GetUserAssociatedWithProject(project);
            
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

            _logger.LogInformation("Request was successful");
            return response;
        }
        public async Task<ProjectResponse.All> GetIndexAsync(ProjectRequest.All request)
        {
            _logger.LogInformation($"Request received: GetIndexAsync({request.SearchTerm})");

            ProjectResponse.All response = new();
            response.Projects = new();
            List<Project> projects;


            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                projects = await _dbContext.Projecten.Include(e => e.VirtualMachines).ThenInclude(e => e.Contract).ToListAsync();
                projects = projects.FindAll(e => e.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || e.Klant.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                projects = await _dbContext.Projecten.Include(e => e.VirtualMachines).ThenInclude(e => e.Contract).ToListAsync();
            }

            foreach(var project in projects)
            {
                var user = await GetUserAssociatedWithProject(project);
                response.Projects.Add(new ProjectDto.Index { Id = project.Id, Name = project.Name, User = user }) ;
            }



            response.Total = projects.Count;
            _logger.LogInformation("Request was successful");

            return response;
        }

        private async Task<UserDto.Index> GetUserAssociatedWithProject(Project project)
        {
            _logger.LogInformation($"Method invocation received: GetUserAssociatedWithProject(...)");

            var response = new UserDto.Index();
            int? id = null;
           
            id = project.VirtualMachines.FirstOrDefault(e => e.Contract != null && e.Contract.CustomerId > 0)?.Contract?.CustomerId;

            if (id == null)
            {
                var res = await _dbContext.Projecten.Include(e => e.Klant).FirstOrDefaultAsync(e => e.Id == project.Id);
                id = res.Klant.Id;
            }
            
                var customer = await _dbContext.Klanten.FirstOrDefaultAsync(u => u.Id == id);
                response.FirstName = customer.FirstName;
                response.PhoneNumber = customer.PhoneNumber;
                response.Name = customer.Name;
                response.Id = customer.Id;
                response.Email = customer.Email;

            _logger.LogInformation("Method was successfully executed");

            return response;

        }
    }



}
