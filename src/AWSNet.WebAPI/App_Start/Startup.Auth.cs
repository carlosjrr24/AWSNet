using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using AWSNet.WebAPI.Providers;
using Microsoft.Owin.Security.DataProtection;
using AWSNet.Utils.Authentication;

namespace AWSNet.WebAPI
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            if (AuthProviderManager.Microsoft.IsEnabled)
                app.UseMicrosoftAccountAuthentication(
                    clientId: AuthProviderManager.Microsoft.Key,
                    clientSecret: AuthProviderManager.Microsoft.Secret);

            if (AuthProviderManager.Twitter.IsEnabled)
                app.UseTwitterAuthentication(
                    consumerKey: AuthProviderManager.Twitter.Key,
                    consumerSecret: AuthProviderManager.Twitter.Secret);

            if (AuthProviderManager.Facebook.IsEnabled)
                app.UseFacebookAuthentication(
                    appId: AuthProviderManager.Facebook.Key,
                    appSecret: AuthProviderManager.Facebook.Secret);

            if (AuthProviderManager.Google.IsEnabled)
            {
                var google = new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = AuthProviderManager.Google.Key,
                    ClientSecret = AuthProviderManager.Google.Secret

                };

                google.Scope.Add("email");
                google.Scope.Add("profile");
                app.UseGoogleAuthentication(google);
            }
        }
    }
}
