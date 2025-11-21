using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class TaskService : Service<APP.Domain.Task>, IService<TaskRequest, TaskResponse>
{
    public TaskService(DbContext db) : base(db)
    {
    }

    public List<TaskResponse> List()
    {
        return Query().Select(t => new TaskResponse()
        {
            Id = t.Id,
            Guid = t.Guid,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            Status = t.Status,
            ProjectId = t.ProjectId,
            UserId = t.UserId
        }).ToList();
    }

    public TaskResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(t => t.Id == id);
        if (entity is null)
            return null;
        return new TaskResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            Title = entity.Title,
            Description = entity.Description,
            DueDate = entity.DueDate,
            Status = entity.Status,
            ProjectId = entity.ProjectId,
            UserId = entity.UserId
        };
    }

    public TaskRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(t => t.Id == id);
        if (entity is null)
            return null;
        return new TaskRequest
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueDate = entity.DueDate,
            Status = entity.Status,
            ProjectId = entity.ProjectId,
            UserId = entity.UserId
        };
    }

    public CommandResponse Create(TaskRequest request)
    {
        var entity = new APP.Domain.Task
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Status = request.Status,
            ProjectId = request.ProjectId,
            UserId = request.UserId
        };
        Create(entity);
        return Success("Task created successfully.", entity.Id);
    }

    public CommandResponse Update(TaskRequest request)
    {
        var entity = Query(false).SingleOrDefault(t => t.Id == request.Id);
        if (entity is null)
            return Error("Task not found!");
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.DueDate = request.DueDate;
        entity.Status = request.Status;
        entity.ProjectId = request.ProjectId;
        entity.UserId = request.UserId;
        Update(entity);
        return Success("Task updated successfully", entity.Id);
    }

    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(t => t.Id == id);
        if (entity is null)
            return Error("Task not found with the id " + id);
        Delete(entity);
        return Success("Task deleted successfully", entity.Id);
    }
}
