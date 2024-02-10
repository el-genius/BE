using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.RepositoryInterface;

namespace URCP.Domain
{
    public class BaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Commit()
        {
            //using (var uow = _unitOfWork)
            //{
            int erc = _unitOfWork.Commit();

            //if (erc == 0)
            //    throw new InvalidOperationException("UOW could not Commit.");
            //}
        }

        public async Task CommitAsync()
        {
            //using (var uow = _unitOfWork)
            //{
            await _unitOfWork.CommitAsync();
            //if (erc.Exception != null || erc.Status == TaskStatus.Faulted) 
            //    throw new InvalidOperationException("UOW could not Commit."); 
            //}

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
    }
}
