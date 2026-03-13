using OpenIddict.EntityFrameworkCore.Models;

namespace AuthService.Core.Entities.OpenId;

public class OpenIdAuthorization : OpenIddictEntityFrameworkCoreAuthorization<long, OpenIdApplication, OpenIdToken>
{
}
