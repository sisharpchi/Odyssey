using AuthService.Core.Entities.Common;
using Microsoft.AspNetCore.Identity;
namespace AuthService.Core.Entities;

public class User : IdentityUser, IEntity<string>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => LastName + " " + FirstName;
}
