using System;
using System.Linq;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using URCP.Core;
using URCP.Web.Models;
using URCP.Web.ViewModels;
using System.Web;
using System.Collections.Generic;
using URCP.Web.Areas.Admin.ViewModels;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace URCP.Web
{
    public static class Shared
    {
        public static bool HasActionPermission(string actionName, string controllerName, ControllerContext currentControlerContext)
        {
            HtmlHelper htmlHelper = new HtmlHelper(new ViewContext(currentControlerContext, new WebFormView(currentControlerContext, "omg"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            //if the controller name is empty the ASP.NET convention is:
            //"we are linking to a different controller
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                                                    ? htmlHelper.ViewContext.Controller
                                                    : GetControllerByName(htmlHelper, controllerName);

            var controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            var controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());

            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);
            if (actionDescriptor == null)
                actionDescriptor = controllerDescriptor.GetCanonicalActions().FirstOrDefault(a => a.ActionName == actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }
         
        private static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            // Instantiate the controller and call Execute
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(

                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));

            }

            return (ControllerBase)controller;
        }

        private static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false; // action does not exist so say yes - should we authorise this?!

            AuthorizationContext authContext = new AuthorizationContext(controllerContext, actionDescriptor);

            FilterInfo fi = new FilterInfo(FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor));
            // run each auth filter until on fails
            // performance could be improved by some caching
            foreach (IAuthorizationFilter authFilter in fi.AuthorizationFilters)
            {

                authFilter.OnAuthorization(authContext);

                if (authContext.Result != null)
                    return false;
            }

            return true;
        }

        //public static void RegisterError(string actionName, int? objectID, string objectName, Exception ex)
        //{
        //    ErrorLog error = new ErrorLog();
        //    error.ActionName = actionName;
        //    error.ErrorDate = DateTime.Now;
        //    if (ex != null)
        //        error.Message = string.Format("message:{0}.{1}StackTrace:{2}.", ex.Message, Environment.NewLine, ex.StackTrace);
        //    error.ObjectID = objectID;
        //    error.ObjectName = objectName;

        //    using (MOLInspectorsEntities db = new MOLInspectorsEntities())
        //    {
        //        try
        //        {
        //            db.ErrorLogs.Add(error);
        //            db.Configuration.ValidateOnSaveEnabled = false;
        //            db.SaveChanges();
        //        }
        //        catch
        //        {
        //            string message = string.Format("Action Name:{0}.{1}Error Time:{2}.{1}ObjectID:{3}.{1}Object Name:{4}.{1}message:{5}.{1}StackTrace:{6}.",
        //                actionName,
        //                Environment.NewLine,
        //                error.ErrorDate.Value.ToString("dd/MM/yyyy HH:mm")
        //                , objectID,
        //                objectName,
        //                ex.Message,
        //                ex.StackTrace);
        //            try
        //            {
        //                if (!EventLog.SourceExists("MOLInspectors"))
        //                    EventLog.CreateEventSource("MOLInspectors", "Application");

        //                EventLog.WriteEntry("MOLInspectors", message, EventLogEntryType.Error);
        //            }
        //            catch { }
        //        }
        //    }
        //}

        /// <summary>
        /// method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE: 
        /// Positive values will rotate clockwise
        /// negative values will rotate counter-clockwise
        /// </param>
        /// <returns></returns>
        public static String RotateImage(String imgUrl, float rotationAngle)
        {
            if (rotationAngle == 0)
            {
                using (Image image = Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl)))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            Image img = Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));

            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width + 5, img.Height + 5, PixelFormat.Format32bppArgb);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)img.Width / 2, (float)img.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)img.Width / 2, -(float)img.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(3, 1));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            string strBase64 = Convert.ToBase64String(byteArray);
            return strBase64;
        }
    }
}