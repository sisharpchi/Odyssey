using AuthService.Core.Entities;

namespace AuthService.Application.Contracts;

public interface IEmailService
{
    Task SendConfirmation(User user);
}
