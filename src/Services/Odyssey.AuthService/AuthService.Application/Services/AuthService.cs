using AuthService.Application.Contracts;
using AuthService.Application.Dtos.Requests;
using AuthService.Application.Dtos.Responses.Common;
using AuthService.Application.Helpers;
using AuthService.Core.Entities;
using AuthService.Core.Persistence;
using AuthService.Shared;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthRepository _authRepository;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<User> userManager, IAuthRepository authRepository, IEmailService emailService)
    {
        _userManager = userManager;
        _authRepository = authRepository;
        _emailService = emailService;
    }

    public async Task<ResponseResult> RegisterAsync(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) &&
            string.IsNullOrWhiteSpace(request.PhoneNumber) &&
            string.IsNullOrWhiteSpace(request.Username))
        {
            return ResponseResult.CreateError("At least one of Username, Email or PhoneNumber must be provided.", ErrorConstant.MissingIdentifier);
        }

        var username = request.Username ?? request.Email ?? request.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                return ResponseResult.CreateError("Username already taken.", ErrorConstant.UsernameAlreadyTaken);
            }
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailUser = await _userManager.FindByEmailAsync(request.Email);
            if (emailUser != null)
            {
                return ResponseResult.CreateError("Email already in use.", ErrorConstant.EmailAlreadyUsed);
            }
        }

        var user = new User
        {
            UserName = username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return ResponseResult.CreateError(string.Join("; ", result.Errors.Select(e => e.Description)), ErrorConstant.InvalidRequest);
        }

        if (!string.IsNullOrEmpty(user.Email))
        {
            await _emailService.SendConfirmation(user);
        }

        /* if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            var phoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

            await _smsService.SendCode(user.PhoneNumber, phoneToken);
            sms yuborish
        } */

        return ResponseResult.CreateSuccess();
    }

    public async Task<ResponseResult> ConfirmEmailAsync(string email, string code)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
        {
            return ResponseResult.CreateError("Email and confirmation code must be provided.", ErrorConstant.MissingIdentifier);
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return ResponseResult.CreateError("User not found.", ErrorConstant.UserNotFound);
        }

        if (user.EmailConfirmed)
        {
            return ResponseResult.CreateError("Email is already confirmed.", ErrorConstant.EmailNotConfirmed);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            return ResponseResult.CreateError(string.Join("; ", result.Errors.Select(e => e.Description)), ErrorConstant.InvalidEmailConfirmationCode);
        }

        return ResponseResult.CreateSuccess();
    }

    public Task ConfirmPhoneAsync(string phoneNumber, string code)
    {
        throw new NotImplementedException();
    }
}
