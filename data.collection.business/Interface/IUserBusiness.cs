using System.Threading.Tasks;
using data.collection.entity.Entity;
using data.collection.entity.Request;

namespace data.collection.business.Interface
{
    public interface IUserBusiness
    {
        Task AddUserAsync(User request);
        Task<string> GetUserByFieldAsync(UserCommonRequest request);
        Task<string> GetUserLoginTokenAsync(UserCommonRequest request);
    }
}