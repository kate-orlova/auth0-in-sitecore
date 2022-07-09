[![GitHub license](https://img.shields.io/github/license/kate-orlova/auth0-in-sitecore.svg)](https://github.com/kate-orlova/auth0-in-sitecore/blob/master/LICENSE)
![GitHub language count](https://img.shields.io/github/languages/count/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub top language](https://img.shields.io/github/languages/top/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub repo size](https://img.shields.io/github/repo-size/kate-orlova/auth0-in-sitecore.svg?style=flat)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/kate-orlova/auth0-in-sitecore)

# Auth0 in Sitecore
SSO with Auth0 in Sitecore.

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
Auth0InSitecore solution implements the integration between Sitecore and Auth0.

### Configuration
The module ships the following config files defined in `..\src\Sitecore\Foundation\Auth0InSitecore\App_Config\Include\Foundation\Auth0InSitecore` folder:
 - Foundation.Auth0InSitecore.config
 - Foundation.Identity.config
 - Sitecore.Owin.Authentication.Enabler.config
 
 ### Pipelines
 TBC
 
 ### Components
 TBC
 
 ### How to install
 1. Add the Auth0InSitecore project to your Visual Studio solution to your Sitecore Foundation layer;
 
 TBC
 # Contribution
Hope you found this module useful, your contributions and suggestions will be very much appreciated. Please submit a pull request.

 # License
The Auth0 in Sitecore module is released under the MIT license, this means that you can modify and use it how you want even for commercial use. Please give it a star if you like it and your experience was positive.

