
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{
    public class UserLoggerRepositoryBase<T> : RepositoryBase<T>, IUserLoggerRepositoryBase<T> where T : StringEntityBase
    {
        public UserLoggerRepositoryBase(DataContext dbcontext, ILogger<UserLoggerRepositoryBase<T>> logger) : base(dbcontext, logger)
        {

        }

        public async  Task<int> WriteUserLogger(string userID,string type, string message)
        {
            ehc_dv_userlogger logger = new ehc_dv_userlogger();

            logger.USERID = userID;

            logger.TIME = DateTime.Now.ToDateTimeString();
            logger.TYPE = type;

            logger.MESSAGE = message;

            _dbContext.ehc_dv_userloggers.Add(logger);

           return await _dbContext.SaveChangesAsync();
        }
    }

 
}
