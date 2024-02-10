using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URCP.Core;
using URCP.Web.Models;
using System.Threading;
using URCP.Domain;
using URCP.RepositoryInterface;
using Unity;
using URCP.Domain.Interface;
using AutoMapper;
using URCP.Web.ViewModels;
using System.Text;

namespace URCP.Web
{
    public class BaseController : Controller
    {
        [Dependency]
       public ApplicationSignInManager _signInManager { get; set; } 
         

        public LayoutViewModel LayoutModel { get; private set; }

        public BaseController()
        {
            LayoutModel = new LayoutViewModel(); 
        }

        public TResult HandleExceptions<TResult>(Func<TResult> operationCallback)
        {
            try
            {
                return operationCallback();
            }
            catch (ApplicationException aex)
            {
                throw new Exception(aex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unhandled server exception was encountered.");
            }
        }

        public void TryCatch(Action action)
        {
            TryCatch(() => { action(); return 0; });
        }

        public T TryCatch<T>(Func<T> action)
        {
            try
            {
                action();
            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
            catch (PermissionException e)
            {
                throw e;
            }
            catch (ValidationException e)
            {
                throw e;
            }
            catch (BusinessRuleException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return default(T);
        }

        public string eFileAccessToken
        {
            get
            {
                //if (Session["AccessToken"] != null && !String.IsNullOrWhiteSpace(Session["AccessToken"].ToString()))
                //    return Session["AccessToken"].ToString();

                //Response.Redirect(Url.Action("Logout", "Account", new { area = "" }));
                return string.Empty;
            }
            set
            {
                Session["AccessToken"] = value;
            }
        }

        public Boolean VerifiyCaptcha(String captchaNumber)
        {
            if (String.IsNullOrWhiteSpace(captchaNumber))
                return false;

            if (String.IsNullOrWhiteSpace(Session["Captcha"].ToString()))
                return false;

            return Session["Captcha"].ToString().Equals(captchaNumber);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewResult viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
                viewResult.ViewBag.LayoutModel = LayoutModel;

            base.OnResultExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            Exception exception = filterContext.Exception;

            string errorTitle = string.Empty;
            if (exception is PermissionException)
                errorTitle = "Security Permission Required";

            ErrorModel model = new ErrorModel(exception, controller, action, errorTitle);
            var result = View("Error", model);
            result.ViewBag.LayoutModel = LayoutModel;
            filterContext.Result = result;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           if (filterContext.Exception != null)
            {
                LogException(filterContext.Exception);
                filterContext.ExceptionHandled = true;

                var response = filterContext.RequestContext.HttpContext.Response;
                response.RedirectToRoute("Default", new System.Web.Routing.RouteValueDictionary {
                        { "Controller",  "Shared"},{ "Action", "Error" }
                });
            }

            base.OnActionExecuted(filterContext);
        }

        #region Json
        public JsonResult JsonErrorMessage(string errorMessage)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.StatusDescription = errorMessage;

            return Json(new { message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonErrorMessage(string errorMessage, ModelStateDictionary modelState)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.StatusDescription = errorMessage;

            var modelStateErrors = modelState.Where(m => m.Value.Errors.Any()).ToList();
            //to avoid circular
            var cleanModelStateErrors = modelStateErrors.Select(m => new
            {
                key = m.Key,
                value = m.Value.Errors.FirstOrDefault().ErrorMessage
            });

            return Json(new { message = errorMessage, modelStateErrors = cleanModelStateErrors }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonSuccessMessage(string successMessage)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            response.StatusDescription = successMessage;

            return Json(new { message = successMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonSuccessMessage(string successMessage, ModelStateDictionary modelState)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            response.StatusDescription = successMessage;

            var modelStateErrors = modelState.Where(m => m.Value.Errors.Any()).ToList();
            //to avoid circular
            var cleanModelStateErrors = modelStateErrors.Select(m => new
            {
                key = m.Key,
                value = m.Value.Errors.FirstOrDefault().ErrorMessage
            });

            return Json(new { message = successMessage, modelStateErrors = cleanModelStateErrors }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// Get validation error of modelstate if not valid
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private List<String> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }

        /// <summary>
        /// Get validation error of modelstate if not valid
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected String GetErrorStringFromModelState(ModelStateDictionary modelState)
        {
            StringBuilder errorMessages = new StringBuilder();
            List<String> errorList = GetErrorListFromModelState(modelState);
            for (int i = 1; i <= errorList.Count; i++)
            {
                errorMessages.Append(errorList[(i - 1)].ToString());
                if (i % 2 == 0)
                    errorMessages.Append("<br />");
            }

            return errorMessages.ToString();
        }

        /// <summary>
        /// Get messages of validation exception
        /// </summary>
        /// <param name="vex"></param>
        /// <returns></returns>
        protected String GetValidationResults(ValidationException vex)
        {
            System.Text.StringBuilder errorMessages = new System.Text.StringBuilder();
            foreach (var item in vex.ValidationResults)
            {
                errorMessages.Append(item.ErrorMessage.ToString());
                errorMessages.Append("<br />");
            }

            return errorMessages.ToString();
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex)
        {
            String actionName = ex.TargetSite == null ? String.Empty : ex.TargetSite.ToString();
            String message = ex.Message.ToString();

            String innerException = ex.InnerException == null ? String.Empty : ex.InnerException.ToString();

            String innerExceptionMessage = ex.InnerException == null ? String.Empty : ex.InnerException.Message.ToString();
            String stackTrace = ex.StackTrace == null ? String.Empty : ex.StackTrace.ToString();
            String controllerName = String.Empty;
            try
            {
                controllerName = ((System.Reflection.MemberInfo)(ex.TargetSite)).DeclaringType.FullName;
            }
            catch { }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(String.Format("ControllerName: {0}", controllerName));
            sb.AppendLine(String.Format("ActionName: {0}", actionName));
            sb.AppendLine(String.Format("Message: {0}", message));
            sb.AppendLine(String.Format("InnerException: {0}", innerException));
            sb.AppendLine(String.Format("InnerExceptionMessage: {0}", innerExceptionMessage));
            sb.AppendLine(String.Format("StackTrace: {0}", stackTrace));
            sb.Append(String.Format("DateTime: {0}", DateTime.Now.ToString()));
            sb.AppendLine("=================================");


            System.Diagnostics.Trace.TraceError(sb.ToString());
        }


        public void AddMCIMessage(string message, MCIMessageType type = MCIMessageType.Info, int timeout = 5)
        {
            MCIAlert.AddMCIMessage(this, message, type, timeout);
        }

        protected ActionResult RegisterError(Exception ex, string actionName, int? objectID = null, string objectName = "", string toReturnViewName = "", object model = null, string userMessage = "عفواً .. حدث خطأ أثناء جلب  البيانات . الرجاء المحاولة لاحقاً.")
        {
            //
            //TODO: register Error in db here
            //

            string errorBody = string.Format("Exception: {1}.<br/>Exception Message:{0}.<br/>StackTrace: {2}.", ex.ToString(), ex.Message, ex.StackTrace);
            var iExeption = ex.InnerException;
            while (iExeption != null)
            {
                errorBody += string.Format("<br/>Inner Exception: {0}.", iExeption.Message);
                iExeption = iExeption.InnerException;
            }

            //developement mode
            if (Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["ExceptionDevelopmentMode"]))
                return Content(errorBody);

            // else return friendly message to the user
            if (!string.IsNullOrEmpty(userMessage))
            {
                if (!HttpContext.Request.IsAjaxRequest())
                {
                    if (!string.IsNullOrEmpty(toReturnViewName))
                    {
                        AddMCIMessage(userMessage, MCIMessageType.Danger, 15);
                        return View(toReturnViewName, model);
                    }
                    else
                        return Content(errorBody);
                }
                else
                    return JsonErrorMessage(userMessage);
            }
            return Content(errorBody);
        }

        protected ActionResult RedirectwithMCIMessage(string redirectTo, string message = "", MCIMessageType messageType = MCIMessageType.Info, int messageTimeOut = 8)
        {
            if (!string.IsNullOrWhiteSpace(message))
                AddMCIMessage(message, messageType, messageTimeOut);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new { url = redirectTo }, JsonRequestBehavior.AllowGet);
            else
                return Redirect(redirectTo);
        }

        protected User CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal.Identity as User;
            }
        }

        #region Lookups

        protected SelectList GetLookupByType(ILookupService lookupService, int lookupTypeId)
        {
            var lst = lookupService.FindBy(lookupTypeId).Items.ToList();
            return Util.GetDropDownData(Mapper.Map<List<SelectListViewModel>>(lst));
        }

        protected SelectList GetNationalities(ICountryService countryService)
        {
            var lst = countryService.FindBy(new Core.SearchEntities.CountrySearchCriteria()).Items.ToList();
            return Util.GetDropDownData(Mapper.Map<List<SelectListViewModel>>(lst));
        }
         

        protected List<string> GetSettingValuesByType(ISettingService settingService, int typeId)
        {
            var lst = settingService.FindBy(new Core.SearchEntities.SettingSearchCriteria { TypeId = typeId }).Items.ToList();
            return lst.Select(s => s.Value).ToList();
        }

        #endregion

    }
}