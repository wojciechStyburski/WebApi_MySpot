using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class UserNotFoundException : CustomException
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId) 
        : base($"User with id {userId} not found")
    {
        UserId = userId;
    }
}