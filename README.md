[![GitHub license](https://img.shields.io/github/license/kate-orlova/auth0-in-sitecore.svg)](https://github.com/kate-orlova/auth0-in-sitecore/blob/master/LICENSE)
![GitHub language count](https://img.shields.io/github/languages/count/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub top language](https://img.shields.io/github/languages/top/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub repo size](https://img.shields.io/github/repo-size/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub contributors](https://img.shields.io/github/contributors/kate-orlova/auth0-in-sitecore)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/kate-orlova/auth0-in-sitecore)

# Auth0 in Sitecore
Today almost every website requires some form of authentication to access its content and features. With the number of web portals and services rising exponentially nowadays, a Single Sign-On (SSO) authentication is now required more than ever.

The Auth0 in Sitecore module implements an identity provider allowing to authenticate visitors of a Sitecore website through [Auth0](https://auth0.com/docs/get-started/auth0-overview) identity management platform. The proposed solution is compatible with Sitecore versions 9 and 10, the current release is referenced to [Sitecore version 10.2](https://dev.sitecore.net/Downloads/Sitecore_Experience_Platform/102/Sitecore_Experience_Platform_102.aspx).


## SPA test app
The “SPA test app” is a Single Page JavaScript Application for testing purposes, it is based on [this code sample](https://github.com/auth0-samples/auth0-javascript-samples/tree/master/01-Login) from Auth0’s GitHub repository.

### How to configure a test SPA app locally 
1. Ensure that both `domain` and `clientId` are specified correctly in `auth_config.json` config file, use the `auth_config.example.json`provided by this repo as a template 
```
{
  "domain": "{YOUR_AUTH0_DOMAIN}",
  "clientId": "{YOUR_AUTH0_CLIENTID}"
}
```
2. Install supporting packages from the Windows command line (CMD) by running `npm install` command in the application folder 
3. Run the application by `npm run dev` command from CMD
4. View the app via http://localhost:3002; if required a port can be set to any with minimal changes in `\bin\www`, `Dockerfile`, `exec.ps1` and `exec.sh` files

## Sitecore
[Sitecore federated authentication](https://doc.sitecore.com/xp/en/developers/102/sitecore-experience-manager/using-federated-authentication-with-sitecore.html) allows users to log in to a Sitecore-based website through an external provider such as Auth0.

Auth0InSitecore solution performs the integration between a Sitecore website and Auth0 using [Auth0 Universal Login](https://auth0.com/docs/authenticate/login/auth0-universal-login), and consists of two projects:
1. **Foundation -> Auth0InSitecore** project implements a custom Identity Provider to support the Sitecore federated authentication with Auth0 and provides some examples of the required config files; 
2. **Feature -> MyAccount** project demonstrates the execution of a typical authentication feature in Sitecore and gives some examples of My Account components built as Controller Renderings. 

### Auth0 Identity Provider for Sitecore
Sitecore federated authentication expects an Identity Provider to be configured in a specific way to use [Owin]( https://docs.microsoft.com/en-us/aspnet/core/fundamentals/owin?view=aspnetcore-6.0) middleware to delegate authentication to third-party providers and then get claims back from a third-party provider.

This module comes with a `..\src\Sitecore\Foundation\Auth0InSitecore\Pipelines\IdentityProviders\Auth0IdentityProviderProcessor.cs` class that implements a custom Identity Provider Processor to let visitors of a Sitecore website authenticate through Auth0. Its `ProcessCore()` method establishes an OpenId connection to Auth0 and executes the external authentication via Auth0 identity management.


### Configuration
The module ships the following config files defined in `..\src\Sitecore\Foundation\Auth0InSitecore\App_Config\Include\Foundation\Auth0InSitecore` folder:
 - Foundation.Auth0InSitecore.config
 - `Foundation.Identity.config` declares settings of an Auth0 tenant for an OpenId connection; remember to update each setting as per your Auth0 application setup, see _Step 4_ of the installation guide detailed further down the page;
 - `Sitecore.Owin.Authentication.Enabler.config` enables the federated authentication in Sitecore; note, that the Sitecore federated authentication is expected to be enabled by default;
 
 
### Components
#### Login
`..\src\Sitecore\Feature\MyAccount\Controllers\LoginController.cs` controller and `..\src\Sitecore\Feature\MyAccount\Views\Feature\MyAccount\Login.cshtml` view implement the login functionality. Typically a login button is expected to stand out in a page layout, therefore, the Login component can be easily placed in the page header or footer.

### Sitecore Packages
Sitecore packages contain:
1. **Renderings**
   - _My Account -> Login_ Controller Rendering
 
 ### How to install
1. [Sign up](https://auth0.com/signup) for an Auth0 account;
2. Log in to the _Auth0 Dashboard_ and get your Auth0 tenant ready for SSO integration:
   - Create a new Auth0 application under the _Applications_ section, then fill the application properties in and specify your Sitecore URIs to be used for login, callback and logout functionality via Auth0;
   - Enable the _Universal Login Experience_ under the _User Management -> Branding -> Universal Login_ section; note, that this is a global setting shared across all your Auth0 applications configured with the same instance;
3. Add the _Auth0InSitecore_ project to your Visual Studio solution to your Sitecore Foundation layer;
4. Copy the config files shipped with this module from the `..\src\Sitecore\Foundation\Auth0InSitecore\App_Config\Include\Foundation\Auth0InSitecore\` folder to an  `..\App_Config\Include\` folder corresponding to your Sitecore website and ensure that all configuration settings are specified correctly:
   - Refer to your Auth0 application _Domain, Client ID, Client Secret, Application Login URI, Allowed Callback/Logout URLs_ properties under the _Settings_ tab and the back-end API for the audience to set the right values in `Foundation.Identity.config`;
   - Map properties, claims and roles in line with your specific profile attributes and user roles between Auth0 Users and Sitecore User Profiles in `Foundation.Auth0InSitecore.config`;
5. Implement your project specific Login, Logout and other My Account related components, see some examples in the attached _Renderings_ Sitecore package; for instance, call `Sitecore.Context.User.IsAuthenticated` method to check whether a user is authenticated or not and `Sitecore.Context.User.Profile.FullName` to get a value of a Name property;
6.	Add your Login/Logout components to pages that are in the authentication scope, for ease you can place them in the Page Layout or Sub Layout;
7.	All is ready and now you can try to authenticate in your Sitecore website with Auth0 accounts. Enjoy!


 
 
 
 
 # Contribution
Hope you found this module useful, your contributions and suggestions will be very much appreciated. Please submit a pull request.

 # License
The Auth0 in Sitecore module is released under the MIT license, this means that you can modify and use it how you want even for commercial use. Please give it a star if you like it and your experience was positive.

