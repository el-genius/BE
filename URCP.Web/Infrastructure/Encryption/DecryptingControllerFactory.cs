using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace URCP.Web.Infrastructure.Encryption
{
    public class DecryptingControllerFactory : DefaultControllerFactory
    {
        // Used to provide the action descriptors to consider for attribute routing
        public const string Action = "action";

        // Used to indicate that a route is a controller-level attribute route.
        public const string Controller = "controller";

        public const string Area = "area";

        public const string ReturnUrl = "returnUrl";

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                if(!(StringEncrypter.ControlsEncrypter as SessionBasedStringEncrypter).IsKeyIVExist)
                {
                    throw new ArgumentNullException("Session timeout");
                }
           

                NameValueCollection parameters = requestContext.HttpContext.Request.Params;
                string[] encryptedParamKeys = parameters.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.StartsWith(StringEncrypter.ControlsEncrypter.Prefix)).ToArray();

                foreach (string key in encryptedParamKeys)
                {
                    string oldKey = key.Replace(StringEncrypter.ControlsEncrypter.Prefix, string.Empty);
                    string[] value = parameters.GetValues(key);
                    if (value.Length > 1)
                    {
                        var result = value.Select(s => string.IsNullOrEmpty(s) ? "" : StringEncrypter.ControlsEncrypter.Decrypt(s)).ToArray();
                        requestContext.RouteData.Values[oldKey] = result;
                    }
                    else if (value.Length == 1)
                    {
                        requestContext.RouteData.Values[oldKey] = string.IsNullOrEmpty(value[0]) ? "" : StringEncrypter.ControlsEncrypter.Decrypt(value[0]);
                    }
                } 
            }
            catch(ArgumentNullException ex)
            {
                return base.CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Client Form data Decryption failed: {0}", ex.ToString());

                return RedirectTo(requestContext, "Shared", "Error", String.Empty, String.Empty);
            }

            return base.CreateController(requestContext, controllerName);
        }

        private IController RedirectTo(RequestContext requestContext, string controller, string action, string area, string returnUrl)
        {
            requestContext.RouteData.Values[Action] = action;
            requestContext.RouteData.Values[Controller] = controller;
            requestContext.RouteData.Values[Area] = area;

            if (!String.IsNullOrWhiteSpace(returnUrl))
                requestContext.RouteData.Values[ReturnUrl] = returnUrl;


            return base.CreateController(requestContext, controller);
        }
    }
}