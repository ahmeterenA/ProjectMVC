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

    public List<UserResponse> List()
    {
        return Query().Select(u => new UserResponse()
        {
            Id = u.Id,
            Guid = u.Guid,
            UserName = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Gender = u.Gender,
            BirthDate = u.BirthDate,
            RegistrationDate = u.RegistrationDate,
            Score = u.Score,
            IsActive = u.IsActive,
            Address = u.Address,
            GroupId = u.GroupId,
            RoleIds = u.RoleIds
        }).ToList();
    }

    public UserResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(u => u.Id == id);
        if (entity is null)
            return null;
        return new UserResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            UserName = entity.UserName,
            Password = entity.Password,
            IsActive = entity.IsActive,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            BirthDate = entity.BirthDate,
            RegistrationDate = entity.RegistrationDate,
            Score = entity.Score,
            Address = entity.Address,
            GroupId = entity.GroupId,
            RoleIds = entity.RoleIds,
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
        var entity = Query(false).SingleOrDefault(u => u.Id == request.Id);
        if (entity is null)
            return Error("User not found!");
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
            return Error("User not found with the id"+id);
        Delete(entity);
        return Success("User deleted successfully", entity.Id);
    }
}