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
TBC
