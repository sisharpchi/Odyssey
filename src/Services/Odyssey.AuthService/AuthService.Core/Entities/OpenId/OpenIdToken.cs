using OpenIddict.EntityFrameworkCore.Models;

namespace AuthService.Core.Entities.OpenId;

public class OpenIdToken : OpenIddictEntityFrameworkCoreToken<long, OpenIdApplication, OpenIdAuthorization>
{
}