using OpenIddict.EntityFrameworkCore.Models;

namespace AuthService.Core.Entities.OpenId;

public class OpenIdApplication : OpenIddictEntityFrameworkCoreApplication<long, OpenIdAuthorization, OpenIdToken>
{
}
