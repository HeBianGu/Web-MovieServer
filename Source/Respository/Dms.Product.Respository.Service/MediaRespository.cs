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
    public class MediaRespository : UserLoggerRepositoryBase<mbc_db_mediatype>, IMediaRespository
    {
        public MediaRespository(DataContext dbcontext, ILogger<MediaRespository> logger) : base(dbcontext, logger)
        {

        } 
    }
}
