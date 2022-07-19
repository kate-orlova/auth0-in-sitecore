using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using System.Web.Mvc;
using System.Linq;

namespace MyAccount.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            var corePipelineManager = (BaseCorePipelineManager)ServiceLocator.ServiceProvider.GetService(typeof(BaseCorePipelineManager));

            var args = new Sitecore.Pipelines.GetSignInUrlInfo.GetSignInUrlInfoArgs(Sitecore.Context.Site.Name, Settings.GetSetting("Foundation.Auth0InSitecore.Auth0RedirectUri"));
            Sitecore.Pipelines.GetSignInUrlInfo.GetSignInUrlInfoPipeline.Run(corePipelineManager, args);
            ViewBag.SignInUrl = args.Result.FirstOrDefault()?.Href;

            return View("~/Views/Feature/MyAccount/Login.cshtml");
		}

        public ActionResult Logout()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                Session.Abandon();
                Sitecore.Security.Authentication.AuthenticationManager.Logout();
            }

            return Redirect(Settings.GetSetting("Foundation.Auth0InSitecore.Auth0PostLogoutRedirectUri"));
        }
    }
}