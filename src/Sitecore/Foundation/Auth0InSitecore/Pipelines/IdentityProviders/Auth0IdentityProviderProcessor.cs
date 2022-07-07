using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Extensions;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;
using System;
using System.Threading.Tasks;


namespace Auth0InSitecore.Pipelines.IdentityProviders
{
    public class Auth0IdentityProviderProcessor : IdentityProvidersProcessor
    {
        private readonly string _auth0Domain;
        private readonly string _auth0ClientId;
        private readonly string _auth0ClientSecret;
        private readonly string _auth0Audience;
        private readonly string _auth0RedirectUri;
        private readonly string _auth0PostLogoutRedirectUri;

        private IdentityProvider IdentityProvider => GetIdentityProvider();

        public Auth0IdentityProviderProcessor(FederatedAuthenticationConfiguration federatedAuthenticationConfiguration) : base(federatedAuthenticationConfiguration)
        {
            _auth0Domain = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0Domain");
            _auth0ClientId = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0ClientId");
            _auth0ClientSecret = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0ClientSecret");
            _auth0RedirectUri = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0RedirectUri");
            _auth0PostLogoutRedirectUri = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0PostLogoutRedirectUri");
            _auth0Audience = Settings.GetSetting("Foundation.Auth0InSitecore.Auth0Audience");
        }

        /// <summary>
        /// IdentityProvider name. Has to match the configuration
        /// </summary>
        protected override string IdentityProviderName
        {
            get { return "Auth0"; }
        }

        protected override void ProcessCore(IdentityProvidersArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            var authenticationType = GetAuthenticationType();
            var options = new OpenIdConnectAuthenticationOptions(authenticationType)
            {
                AuthenticationType = "Auth0",
                Authority = $"https://{_auth0Domain}",
                ClientId = _auth0ClientId,
                RedirectUri = _auth0RedirectUri,
                PostLogoutRedirectUri = _auth0PostLogoutRedirectUri,
                ClientSecret = _auth0ClientSecret,
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name"
                },
                Scope = "read:users",
                ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.AuthenticationRequest)
                        {
                            notification.ProtocolMessage.SetParameter("audience", _auth0Audience);
                        }
                        else if (notification.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var logoutUri = $"https://{_auth0Domain}/v2/logout?client_id={_auth0ClientId}";

                            var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    // transform to absolute
                                    var request = notification.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                }
                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            notification.Response.Redirect(logoutUri);
                            notification.HandleResponse();
                        }
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = notification =>
                    {
                        notification.AuthenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("id_token", notification.ProtocolMessage.IdToken));
                        notification.AuthenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("access_token", notification.ProtocolMessage.AccessToken));

                        notification.AuthenticationTicket.Identity.ApplyClaimsTransformations(
                            new TransformationContext(FederatedAuthenticationConfiguration, IdentityProvider));
                        notification.AuthenticationTicket = new AuthenticationTicket(notification.AuthenticationTicket.Identity, notification.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },
                    AuthenticationFailed = notification =>
                    {
                        return Task.FromResult(0);
                    },
                    AuthorizationCodeReceived = notification =>
                    {
                        return Task.FromResult(0);
                    },
                    MessageReceived = notification =>
                    {
                        return Task.FromResult(0);
                    },
                    SecurityTokenReceived = notifaction =>
                    {
                        return Task.FromResult(0);
                    }
                }
            };

            args.App.UseCookieAuthentication(new CookieAuthenticationOptions());
            args.App.UseOpenIdConnectAuthentication(options);
        }
    }
}