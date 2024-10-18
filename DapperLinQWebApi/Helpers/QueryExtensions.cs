using DapperLinQWebApi.Models;

namespace DapperLinQWebApi.Helpers
{
    public static class QueryExtensions
    {
        public static IEnumerable<User> WhereIsActive(this IEnumerable<User> users)
        {
            return users.Where(u => u.IsActive);
        }
    }
}
