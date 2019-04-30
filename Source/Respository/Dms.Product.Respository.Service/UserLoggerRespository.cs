
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using Dms.Product.Respository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{
    public class UserLoggerRespository : UserLoggerRepositoryBase<ehc_dv_userlogger>, IUserLoggerRespository
    {
        public UserLoggerRespository(DataContext dbcontext, ILogger<UserLoggerRespository> logger) : base(dbcontext, logger)
        {

        }

        public async Task<Tuple<List<UserLoggerViewModel>, int>> GetLoggersAsync(int start, int pageCount, Expression<Func<ehc_dv_userlogger, bool>> where = null, Expression<Func<ehc_dv_userlogger, object>> order = null)
        {
            var collection = await this.LoadPageList(start, pageCount, where, order);

            List<UserLoggerViewModel> result = new List<UserLoggerViewModel>();

            foreach (var item in collection.Item1)
            {
                UserLoggerViewModel viewModel = new UserLoggerViewModel();

                viewModel.User = await this._dbContext.Users.FindAsync(item.USERID);

                viewModel.TIME = item.TIME;

                viewModel.MESSAGE = item.MESSAGE;

                viewModel.TYPE = item.TYPE;

                result.Add(viewModel);

            }

            return Tuple.Create(result, collection.Item2);
        }

        public async Task<List<UserLoggerViewModel>> GetAllLoggersAsync()
        {
            var collection = await this.GetListAsync();

            List<UserLoggerViewModel> result = new List<UserLoggerViewModel>();

            foreach (var item in collection)
            {
                UserLoggerViewModel viewModel = new UserLoggerViewModel();

                viewModel.User = await this._dbContext.Users.FindAsync(item.USERID);

                viewModel.TIME = item.TIME;

                viewModel.MESSAGE = item.MESSAGE;

                viewModel.TYPE = item.TYPE;

                result.Add(viewModel);

            }

            return result;
        } 

        public async Task<List<UserLoggerViewModel>> GetLoggersBy(Predicate<ehc_dv_userlogger> match)
        { 
            var collection = await this.GetListAsync(l => match(l));

            List<UserLoggerViewModel> result = new List<UserLoggerViewModel>();

            foreach (var item in collection)
            {
                UserLoggerViewModel viewModel = new UserLoggerViewModel();

                viewModel.User = await this._dbContext.Users.FindAsync(item.USERID);

                viewModel.TIME = item.TIME;

                viewModel.MESSAGE = item.MESSAGE;

                viewModel.TYPE = item.TYPE;

                result.Add(viewModel); 
            }

            return result;
        }

        public async Task<List<ehc_dv_user>> GetUsers()
        {
            return await this._dbContext.Users.ToListAsync();
        }
    }
}
