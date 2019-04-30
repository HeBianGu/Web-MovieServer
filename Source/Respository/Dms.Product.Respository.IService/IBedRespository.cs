﻿
using Dms.Product.Base.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    public interface IBedRespository : IUserLoggerRepositoryBase<ehc_dv_bed>
    {

        Task<IEnumerable<ehc_dd_device>> GetMats();
    }
}
