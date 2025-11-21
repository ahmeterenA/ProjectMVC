using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class UserService : Service<User>, IService<UserRequest,UserResponse>
{
    public UserService(DbContext db) : base(db)
    {
    }

    protected override IQueryable<User> Query(bool isNoTracking = true)
    {
        return base.Query(isNoTracking)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Group)
            .OrderByDescending(u => u.IsActive).ThenBy(u => u.RegistrationDate).ThenBy(u => u.UserName);
    }

    public List<UserResponse> List()
    {
        return Query()
            .Select(u => new UserResponse()
            {
                Id = u.Id,
                Guid = u.Guid,
                UserName = u.UserName,
                FullName = u.FirstName + " " + u.LastName,
                Gender = u.Gender,
                GenderF = u.Gender.ToString(),
                BirthDate = u.BirthDate,
                BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("MM/dd/yyyy") : "",
                RegistrationDate = u.RegistrationDate,
                RegistrationDateF = u.RegistrationDate.ToString("MM/dd/yyyy"),
                Score = u.Score,
                ScoreF = u.Score.ToString("N1"),
                IsActive = u.IsActive,
                IsActiveF = u.IsActive ? "Active" : "Inactive",
                Address = u.Address,
                GroupId = u.GroupId,
                Group = u.Group != null ? u.Group.Title : "",
                RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList(),
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();
    }

    public UserResponse Item(int id)
    {
        var entity = Query()
            .SingleOrDefault(u => u.Id == id);
        if (entity is null)
            return null;
        return new UserResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            UserName = entity.UserName,
            Password = entity.Password,
            IsActive = entity.IsActive,
            IsActiveF = entity.IsActive ? "Active" : "Inactive",
            FullName = entity.FirstName + " " + entity.LastName,
            Gender = entity.Gender,
            GenderF = entity.Gender.ToString(),
            BirthDate = entity.BirthDate,
            BirthDateF = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToString("MM/dd/yyyy") : "",
            RegistrationDate = entity.RegistrationDate,
            RegistrationDateF = entity.RegistrationDate.ToString("MM/dd/yyyy"),
            Score = entity.Score,
            ScoreF = entity.Score.ToString("N1"),
            Address = entity.Address,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? entity.Group.Title : "",
            RoleIds = entity.UserRoles.Select(ur => ur.RoleId).ToList(),
            Roles = entity.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }

    public UserRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(u => u.Id == id);
        if (entity is null)
            return null;
        return new UserRequest
        {
            Id = entity.Id,
            UserName = entity.UserName,
            Password = entity.Password,
            IsActive = entity.IsActive,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            BirthDate = entity.BirthDate,
            Score = entity.Score,
            Address = entity.Address,
            GroupId = entity.GroupId,
            RoleIds = entity.RoleIds,
        };
    }

    public CommandResponse Create(UserRequest request)
    {
        if (Query().Any(u => u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
            return Error("User with the same username exists");
        var entity = new User
        {
            UserName = request.UserName,
            Password = request.Password,
            FirstName = request.FirstName?.Trim(),
            LastName = request.LastName?.Trim(),
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            RegistrationDate = DateTime.Now,
            Score = request.Score ?? 0,
            IsActive = request.IsActive,
            Address = request.Address?.Trim(),
            GroupId = request.GroupId,
            RoleIds = request.RoleIds,
        };
        Create(entity);
        return Success("User created successfully.", entity.Id);
    }

    public CommandResponse Update(UserRequest request)
    {
        if (Query().Any(u => u.Id != request.Id && u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
            return Error("User with the same user name already exists!");
        var entity = Query(false)
            .SingleOrDefault(u => u.Id == request.Id);
        if (entity is null)
            return Error("User not found!");
        
        Delete(entity.UserRoles);

        entity.UserName = request.UserName;
        entity.Password = request.Password;
        entity.FirstName = request.FirstName?.Trim();
        entity.LastName = request.LastName?.Trim();
        entity.Gender = request.Gender;
        entity.BirthDate = request.BirthDate;
        entity.Score = request.Score ?? 0;
        entity.IsActive = request.IsActive;
        entity.Address = request.Address?.Trim();
        entity.GroupId = request.GroupId;
        entity.RoleIds = request.RoleIds;
        
        Update(entity);
        return Success("User updated successfully", entity.Id);
    }


    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(u => u.Id == id);
        if (entity is null)
            return Error("User not found!");
        Delete(entity.UserRoles);
        Delete(entity);
        return Success("User deleted successfully.", entity.Id);
    }
}