
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    public interface IUserLoggerRepositoryBase<T> : IStringRepository<T> where T : StringEntityBase
    {
        Task<int> WriteUserLogger(string userID,string type, string message);
    }
}
