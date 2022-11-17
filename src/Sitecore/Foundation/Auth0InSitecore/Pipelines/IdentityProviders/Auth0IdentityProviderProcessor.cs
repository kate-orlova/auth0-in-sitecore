using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Extensions;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Sitecore.Web.UI.Sheer;


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

        public Auth0IdentityProviderProcessor(
            FederatedAuthenticationConfiguration federatedAuthenticationConfiguration, ICookieManager cookieManager, BaseSettings settings)
            : base(federatedAuthenticationConfiguration, cookieManager, settings)

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

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },
                Scope = "openid profile email",
                ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        {
                            notification.ProtocolMessage.SetParameter("audience", _auth0Audience);
                        }
                        else if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var revokeProperties = notification.OwinContext.Authentication.AuthenticationResponseRevoke?.Properties?.Dictionary;
                            if (revokeProperties != null && revokeProperties.ContainsKey("nonce"))
                            {
                                var uri = new Uri(notification.ProtocolMessage.PostLogoutRedirectUri);
                                var host = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                                var path = "/" + uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                                var nonce = revokeProperties["nonce"];

                                notification.ProtocolMessage.PostLogoutRedirectUri = $"{host}/identity/postexternallogout?ReturnUrl={path}&nonce={nonce}";
                            }
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