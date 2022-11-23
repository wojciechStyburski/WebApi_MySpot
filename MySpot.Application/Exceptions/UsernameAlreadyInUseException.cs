using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class UsernameAlreadyInUseException : CustomException
{
    public string UserName { get; }

    public UsernameAlreadyInUseException(string userName) : base($"Username: '{userName}' is already in use.")
    {
        UserName = userName;
    }
}