using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using PreschollMvc;

[assembly: OwinStartup(typeof(Startup))]

namespace PreschollMvc
{
    public class Startup
    {
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44394",
                ClientId = "mitsos",
                ClientSecret = "1554db43-3015-47a8-a748-55bd76b6af48",

               // RedirectUri = "https://localhost:44375/signin-oidc", //Net4MvcClient's URL
               // PostLogoutRedirectUri = "https://localhost:44375/",

                // ResponseType = "code",
                ResponseType = "code",
                RequireHttpsMetadata = false,

                Scope = "app.api.weather",

                TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = "name"
                },

                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications {
                    SecurityTokenValidated = n =>
                    {
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("access_token",
                            n.ProtocolMessage.AccessToken));
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var id_token_claim =
                                n.OwinContext.Authentication.User.Claims.FirstOrDefault(x => x.Type == "id_token");
                            if (id_token_claim != null)
                            {
                                n.ProtocolMessage.IdTokenHint = id_token_claim.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });

            // app.UseNLog((eventType) => LogLevel.Debug);
        }
    }
}