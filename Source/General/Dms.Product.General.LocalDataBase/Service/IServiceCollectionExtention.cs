 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Product.General.LocalDataBase
{
    public static class IServiceCollectionExtention
    {

        /// <summary> 注册MySql数据上下文 </summary>
        public static void AddDbContextWithConnectString<T>(this IServiceCollection services,string conStr) where T: DbContext
        {
            services.AddDbContext<T>(options => options.UseMySQL(conStr));
        }

    }
}
