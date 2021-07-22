using System.Threading.Tasks;
using data.collection.entity.Entity;
using data.collection.entity.Model;

namespace data.collection.business.Interface
{
    public interface IOperatorBusiness
    {
        Task<LoginUser> GetOperatorInfo();
    }
}