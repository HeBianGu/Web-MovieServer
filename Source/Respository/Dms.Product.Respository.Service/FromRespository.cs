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
    public class FromRespository : UserLoggerRepositoryBase<mbc_db_fromtype>, IFromRespository
    {
        public FromRespository(DataContext dbcontext, ILogger<FromRespository> logger) : base(dbcontext, logger)
        {

        } 
    }
}
