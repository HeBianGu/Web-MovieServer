
using Dms.Product.Base.Model;
using Dms.Product.Respository.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    public interface IUserLoggerRespository : IUserLoggerRepositoryBase<ehc_dv_userlogger>
    {

        Task<Tuple<List<UserLoggerViewModel>, int>> GetLoggersAsync(int start, int pageCount, Expression<Func<ehc_dv_userlogger, bool>> where = null, Expression<Func<ehc_dv_userlogger, object>> order = null);


        Task<List<UserLoggerViewModel>> GetAllLoggersAsync();


        Task<List<UserLoggerViewModel>> GetLoggersBy(Predicate<ehc_dv_userlogger> match);


        Task<List<ehc_dv_user>> GetUsers();
    }
}
