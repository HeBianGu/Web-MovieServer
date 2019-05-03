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
    public class CaseRespository : UserLoggerRepositoryBase<mbc_dc_case>, ICaseRespository
    {
        public CaseRespository(DataContext dbcontext, ILogger<CaseRespository> logger) : base(dbcontext, logger)
        {

        } 
    }
}
