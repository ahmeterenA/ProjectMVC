using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class ProjectService : Service<Project>, IService<ProjectRequest, ProjectResponse>
{
    private readonly DbContext _context;

    public ProjectService(DbContext db) : base(db)
    {
        _context = db;
    }

    protected override IQueryable<Project> Query(bool isNoTracking = true)
    {
        return base.Query(isNoTracking)
            .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
            .OrderBy(p => p.Name);
    }

    public List<ProjectResponse> List()
    {
        return Query().Select(p => new ProjectResponse()
        {
            Id = p.Id,
            Guid = p.Guid,
            Name = p.Name,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            UserIds = p.UserIds
        }).ToList();
    }

    public ProjectResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return null;
        return new ProjectResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            UserIds = entity.UserIds
        };
    }

    public ProjectRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return null;
        return new ProjectRequest
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            UserIds = entity.UserIds
        };
    }

    public CommandResponse Create(ProjectRequest request)
    {
        if (Query().Any(p => p.Name == request.Name.Trim()))
            return Error("Project with the same name exists");
        var entity = new Project
        {
            Name = request.Name.Trim(),
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            UserIds = request.UserIds
        };
        Create(entity);
        return Success("Project created successfully.", entity.Id);
    }

    public CommandResponse Update(ProjectRequest request)
    {
        if (Query().Any(p => p.Id != request.Id && p.Name == request.Name.Trim()))
            return Error("Project with the same name already exists!");
        var entity = Query(false).SingleOrDefault(p => p.Id == request.Id);
        if (entity is null)
            return Error("Project not found!");
        
        var existingProjectUsers = _context.Set<ProjectUser>().Where(pu => pu.ProjectId == request.Id);
        _context.Set<ProjectUser>().RemoveRange(existingProjectUsers);

        entity.Name = request.Name.Trim();
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.UserIds = request.UserIds;
        
        Update(entity);
        return Success("Project updated successfully", entity.Id);
    }

    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return Error("Project not found with the id " + id);
        
        var existingProjectUsers = _context.Set<ProjectUser>().Where(pu => pu.ProjectId == id);
        _context.Set<ProjectUser>().RemoveRange(existingProjectUsers);
        
        Delete(entity);
        return Success("Project deleted successfully", entity.Id);
    }
}
