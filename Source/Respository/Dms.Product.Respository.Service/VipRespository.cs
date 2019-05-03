using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using HeBianGu.Product.General.FFmpegService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{
    public class VipRespository : UserLoggerRepositoryBase<mbc_db_viptype>, IVipRespository
    {
        public VipRespository(DataContext dbcontext, ILogger<VipRespository> logger) : base(dbcontext, logger)
        {

        } 
    }
}
