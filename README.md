[![GitHub license](https://img.shields.io/github/license/kate-orlova/auth0-in-sitecore.svg)](https://github.com/kate-orlova/auth0-in-sitecore/blob/master/LICENSE)
![GitHub language count](https://img.shields.io/github/languages/count/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub top language](https://img.shields.io/github/languages/top/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub repo size](https://img.shields.io/github/repo-size/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub contributors](https://img.shields.io/github/contributors/kate-orlova/auth0-in-sitecore)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/kate-orlova/auth0-in-sitecore)

# Auth0 in Sitecore
Today almost every website requires some form of authentication to access its content and features. With the number of web portals and services rising exponentially nowadays, a Single Sign-On (SSO) authentication is now required more than ever.

The Auth0 in Sitecore module implements an identity provider allowing to authenticate visitors of a Sitecore website through [Auth0](https://auth0.com/docs/get-started/auth0-overview) identity management platform.


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
Auth0InSitecore solution implements the integration between a Sitecore website and Auth0 using [Auth0 Universal Login](https://auth0.com/docs/authenticate/login/auth0-universal-login).

### Configuration
The module ships the following config files defined in `..\src\Sitecore\Foundation\Auth0InSitecore\App_Config\Include\Foundation\Auth0InSitecore` folder:
 - Foundation.Auth0InSitecore.config
 - Foundation.Identity.config
 - Sitecore.Owin.Authentication.Enabler.config
 
### Pipelines
 TBC
 
### Components
#### Login
`..\src\Sitecore\Feature\MyAccount\Controllers\LoginController.cs` controller and `..\src\Sitecore\Feature\MyAccount\Views\Feature\MyAccount\Login.cshtml` view implement the login functionality. Typically a login button is expected to stand out in a page layout, therefore, the Login component can be easily placed in the page header or footer.

### Sitecore Packages
Sitecore packages contain:
1. **Renderings**
   - My Account -> Login controller rendering
 
 ### How to install
 1. Add the Auth0InSitecore project to your Visual Studio solution to your Sitecore Foundation layer;
 2. Config
 3. Implement your project specific Login, Logout and other My Account related components, see some examples in the attached _Renderings_ Sitecore package;
 4. Add your Login/Logout components to the concernated pages, for ease you can place them in the Page Layout / Sub Layout;
 5. TBC
 
 TBC
 # Contribution
Hope you found this module useful, your contributions and suggestions will be very much appreciated. Please submit a pull request.

 # License
The Auth0 in Sitecore module is released under the MIT license, this means that you can modify and use it how you want even for commercial use. Please give it a star if you like it and your experience was positive.

