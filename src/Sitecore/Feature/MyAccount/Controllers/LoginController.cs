using Sitecore.Abstractions;
using Sitecore.Configuration;
using System.Web.Mvc;
using System.Linq;

namespace MyAccount.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            var corePipelineManager = DependencyResolver.Current.GetService<BaseCorePipelineManager>();

            var args = new Sitecore.Pipelines.GetSignInUrlInfo.GetSignInUrlInfoArgs(Sitecore.Context.Site.Name, Settings.GetSetting("Foundation.Auth0InSitecore.Auth0RedirectUri"));
            Sitecore.Pipelines.GetSignInUrlInfo.GetSignInUrlInfoPipeline.Run(corePipelineManager, args);
            ViewBag.SignInUrl = args.Result.FirstOrDefault()?.Href;

			return View();
		}
    }
}