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
    public class MovieRespository : UserLoggerRepositoryBase<mbc_dv_movie>, IMovieRespository
    {
        public MovieRespository(DataContext dbcontext, ILogger<MovieRespository> logger) : base(dbcontext, logger)
        {

        } 
    }
}
