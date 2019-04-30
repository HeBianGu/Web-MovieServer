
using Dms.Product.Base.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    public interface IUserAccountRespositroy : IUserLoggerRepositoryBase<ehc_dv_user>
    {
        /// <summary> 获取所有角色 </summary> 
        Task<IEnumerable<ehc_dv_role>> GetRoles();

        /// <summary> 获取所有系统用户数据 </summary>
        Task<IEnumerable<Tuple<ehc_dv_user, string>>> GetUsers();
    }
}
