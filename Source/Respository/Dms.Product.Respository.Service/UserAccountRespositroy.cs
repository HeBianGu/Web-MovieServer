
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{
    public class UserAccountRespositroy : UserLoggerRepositoryBase<ehc_dv_user>, IUserAccountRespositroy
    {
        public UserAccountRespositroy(DataContext dbcontext, ILogger<UserAccountRespositroy> logger) : base(dbcontext, logger)
        {

        }

        public async Task<IEnumerable<ehc_dv_role>> GetRoles()
        {
            return await _dbContext.ehc_dv_roles.ToListAsync();
        }

        public async Task<IEnumerable<Tuple<ehc_dv_user, string>>> GetUsers()
        {

            List<Tuple<ehc_dv_user, string>> result = new List<Tuple<ehc_dv_user, string>>();

            var collection = await _dbContext.Users.ToListAsync();


            var role = await _dbContext.ehc_dv_roles.ToListAsync();

            foreach (var item in collection)
            {

                var find = Tuple.Create(item, role?.FirstOrDefault(l => l.ID == item.ROLEID)?.NAME);

                result.Add(find);
            }

            return result;
        }
    }
}
