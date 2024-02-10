using URCP.Domain;
using URCP.Core;
using URCP.Web.Binders;
using System;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using URCP.Web.Infrastructure.Encryption;
using Resources;
using AutoMapper;

namespace URCP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c => c.AddProfile<MapConfig>());

            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var binder = new DateTimeModelBinder(new string[] { "dd/MM/yyyy", "d/M/yyyy" });
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);

            //Important for localize default validation messages
            DefaultModelBinder.ResourceClassKey = "ValidationMessages";

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ValidationMessages";

            //Register ViewModelAttribute Metadata Provider
            ModelMetadataProviders.Current = new MetadataProvider(false, typeof(ViewModelAttribute));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //register our decrypter controller factory
            ControllerBuilder.Current.SetControllerFactory(typeof(DecryptingControllerFactory));

            Thread cleanThread = new Thread(() => Util.CleanTempFolder());
            cleanThread.Start();

        }

        protected void Session_End(Object sender, EventArgs e)
        {

            string tempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp");
            DirectoryInfo dir = new DirectoryInfo(tempPath);
            try
            {
                foreach (FileInfo fInfo in dir.GetFiles())
                {
                    if (fInfo.Name.Contains(Session.SessionID + "__"))
                        fInfo.Delete();
                }
            }
            catch (Exception ex) { }

            //clear the session
            Session.Clear();
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Add("X-Frame-Options", "DENY");
            HttpContext.Current.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            HttpContext.Current.Response.Headers.Remove("Server");

            //Uncommet this line if you are sure you do not use external scripts (like google maps)
            //HttpContext.Current.Response.Headers.Add("content-security-policy", "script-src 'self' 'unsafe-inline' 'unsafe-eval'");
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var oldPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (oldPrincipal != null)
            {
                var oldIdentity = oldPrincipal.Identity as ClaimsIdentity;

                if (oldIdentity != null && oldIdentity.IsAuthenticated)
                {
                    User userProfile = new User()
                    {
                        Id = int.Parse(oldIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                        UserName = oldIdentity.Claims.FirstOrDefault(x => x.Type == "Username").Value,
                        FullName = oldIdentity.Claims.FirstOrDefault(x => x.Type == "FullName").Value,
                        Email = oldIdentity.Claims.FirstOrDefault(x => x.Type == "Email").Value,
                        Mobile = oldIdentity.Claims.FirstOrDefault(x => x.Type == "Mobile").Value,
                        IdentityNumber = oldIdentity.Claims.FirstOrDefault(x => x.Type == "IdentityNumber").Value,
                        Birthdate = string.IsNullOrEmpty(oldIdentity.Claims.FirstOrDefault(x => x.Type == "Birthdate").Value) ? (DateTime?)null : DateTime.Parse(oldIdentity.Claims.FirstOrDefault(x => x.Type == "Birthdate").Value),
                        UserType = (UserType)int.Parse(oldIdentity.Claims.FirstOrDefault(x => x.Type == "UserType").Value)
                    };


                    if (userProfile != null)
                        Thread.CurrentPrincipal = HttpContext.Current.User = new UserProfilePrincipal(oldPrincipal, oldIdentity, userProfile);
                }
            }

        }

    }
}
