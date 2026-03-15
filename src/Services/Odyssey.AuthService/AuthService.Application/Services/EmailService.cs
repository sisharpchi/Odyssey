using AuthService.Application.Contracts;
using AuthService.Application.Helpers;
using AuthService.Core.Entities;
using AuthService.Core.Persistence;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class EmailService : IEmailService
{
    private readonly UserManager<User> _userManager;
    private readonly IDateTimeService _dateTimeService;
    private readonly IAuthRepository _authRepository;

    public EmailService(UserManager<User> userManager, IDateTimeService dateTimeService, IAuthRepository authRepository)
    {
        _userManager = userManager;
        _dateTimeService = dateTimeService;
        _authRepository = authRepository;
    }

    public async Task SendConfirmation(User user)
    {
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailOtpCodeMassage = new EmailOtpCodeMessage
        {
            Email = user.Email,
            Code = emailToken,
            SendedAt = _dateTimeService.DateTime
        };

        await _authRepository.AddAsync(emailOtpCodeMassage);
        await _authRepository.UnitOfWork.CommitAsync();

        // send notification service
    }
}
