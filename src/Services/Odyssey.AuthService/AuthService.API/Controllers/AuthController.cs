using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

public class AuthController : ControllerBase
{
    //[HttpPost("register")]
    //public async Task<IActionResult> Register(RegisterRequest request)
    //{
    //    //if (string.IsNullOrEmpty(request.Email) &&
    //    //    string.IsNullOrEmpty(request.PhoneNumber) &&
    //    //    string.IsNullOrEmpty(request.Username))
    //    //{
    //    //    return BadRequest("Email, phone or username required.");
    //    //}

    //    //var user = new User
    //    //{
    //    //    UserName = request.Username ?? request.Email ?? request.PhoneNumber,
    //    //    Email = request.Email,
    //    //    PhoneNumber = request.PhoneNumber
    //    //};

    //    //var result = await _userManager.CreateAsync(user, request.Password);

    //    //if (!result.Succeeded)
    //    //    return BadRequest(result.Errors);

    //    //// EMAIL tasdiqlash
    //    //if (!string.IsNullOrEmpty(user.Email))
    //    //{
    //    //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    //    //    // send email
    //    //}

    //    //// PHONE tasdiqlash
    //    //if (!string.IsNullOrEmpty(user.PhoneNumber))
    //    //{
    //    //    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
    //    //    // send sms
    //    //}

    //    return Ok();
    //}
}
