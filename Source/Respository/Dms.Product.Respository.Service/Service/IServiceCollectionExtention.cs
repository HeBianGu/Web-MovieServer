
using Dms.Product.Respository.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Product.Respository.Service
{
    public static class IServiceCollectionExtention
    {

        /// <summary> 依赖注入Respository</summary>
        public static void AddRespositorys(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountRespositroy, UserAccountRespositroy>();

            services.AddScoped<IMonitorSetRespository, MonitorSetRespository>();

            services.AddScoped<ICustomerRespository, CustomerRespository>();

            services.AddScoped<IBedRespository, BedRespositroy>();

            services.AddScoped<IUserLoggerRespository, UserLoggerRespository>();

            services.AddScoped<IMovieRespository, MovieRespository>();

        }

      
    }
}
