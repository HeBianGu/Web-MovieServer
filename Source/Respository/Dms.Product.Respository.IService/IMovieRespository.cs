
using Dms.Product.Base.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    public interface IMovieRespository : IUserLoggerRepositoryBase<mbc_dv_movie>
    {
        /// <summary> 获取所有床列表 </summary>
        Task<IEnumerable<mbc_dc_case>> GetCases();

        Task<IEnumerable<mbc_db_extendtype>> GetExtends();

        Task<int> Insert(FileInfo file, string caseid);

        Task<List<string>> UpdateImage(string id, string timespan = "00:01:00");

        Task<Tuple<mbc_dv_movie, List<mbc_dv_movieimage>>> GetMovieWIthDetial(string id);
    }
}
