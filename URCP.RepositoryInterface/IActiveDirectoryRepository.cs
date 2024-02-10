using URCP.Core;

namespace URCP.RepositoryInterface
{
    public interface IActiveDirectoryRepository
    {
        bool AuthenticateUser(string userName, string password);
        ActiveDirectoryUserInfo GetUserInfo(string userName);
    }
}
