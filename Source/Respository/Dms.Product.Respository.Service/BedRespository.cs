using Dms.Prodoct.Domain.DataServce;
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
    public class BedRespositroy : UserLoggerRepositoryBase<ehc_dv_bed>, IBedRespository
    {
        public BedRespositroy(DataContext dbcontext, ILogger<BedRespositroy> logger) : base(dbcontext, logger)
        {

        }
        public async Task<IEnumerable<ehc_dd_device>> GetMats()
        {

            var result = await DataService.Instance.GetDevice();


            if (result.succeed)
            {
                return result.data;
            }
            else
            {
                _logger.LogInformation("Post- GetDevice遇到错误！");
                _logger.LogInformation(result.message);
                return null;
            }


        }
    }
}
