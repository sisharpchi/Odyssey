using AuthService.Application.Dtos.Requests;
using AuthService.Application.Dtos.Responses.Common;
using AuthService.Shared;

namespace AuthService.Application.Contracts
{
    public interface IAuthService
    {
        Task<ResponseResult> RegisterAsync(RegisterRequestDto request);

        Task ConfirmEmailAsync(string email, string code);

        Task ConfirmPhoneAsync(string phoneNumber, string code);
    }
}
