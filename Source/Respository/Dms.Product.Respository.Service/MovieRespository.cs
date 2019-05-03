using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using HeBianGu.Product.General.FFmpegService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{
    public class MovieRespository : UserLoggerRepositoryBase<mbc_dv_movie>, IMovieRespository
    {
        public MovieRespository(DataContext dbcontext, ILogger<MovieRespository> logger) : base(dbcontext, logger)
        {

        }

        public async Task<IEnumerable<mbc_dc_case>> GetCases()
        {
            return await _dbContext.mbc_dc_cases.Where(l => l.ISENBLED == 1).ToListAsync();
        }

        public async Task<IEnumerable<mbc_db_extendtype>> GetExtends()
        {
            return await _dbContext.mbc_db_extendtypes.Where(l => l.ISENBLED == 1).ToListAsync();
        }

        public async Task<Tuple<mbc_dv_movie, List<mbc_dv_movieimage>>> GetMovieWIthDetial(string id)
        {
            var movie = await this.GetByIDAsync(id);

            //foreach (var item in this._dbContext.mbc_dv_movieimages)
            //{

            //    Debug.WriteLine(item.Text);

            //}

            var images = await this._dbContext.mbc_dv_movieimages.Where(l => l.MovieID == id)?.ToListAsync();

            return Tuple.Create(movie, images);
        }


        public async Task<int> Insert(FileInfo file, string caseid)
        {

            mbc_dv_movie movie = new mbc_dv_movie();
            movie.Name = file.Name;
            movie.Url = file.FullName;
            movie.ExtendType = file.Extension;
            movie.CaseType = caseid;
            movie.Size = file.Length;
            movie.FromType = "local";

            var detial = FFmpegService.Instance.GetMediaEntity(file.FullName);

            var tags = this._dbContext.mbc_db_tagtypes.Where(l => file.Name.Contains(l.Value));

            var list = tags.ToList();

            //foreach (var item in list)
            //{

            //    System.Diagnostics.Debug.WriteLine(item.Value);

            //}

            if (list != null && list.Count > 0)
            {
                movie.TagTypes = list.Select(l => l.Value).Aggregate((l, k) => l + "," + k);
            }



            if (detial != null)
            {
                movie.Duration = detial.Duration;
                movie.Bitrate = detial.Bitrate;
                movie.MediaCode = detial.MediaCode;
                movie.VedioType = detial.MediaType;
                movie.Resoluction = detial.Resoluction;
                movie.Aspect = detial.Aspect;
                movie.Rate = detial.Rate;
            }

            return await this.InsertAsync(movie, false);
        }

        public async Task<List<string>> UpdateImage(string id, string timespan = "00:01:00")
        {

            return null;

            //var find = await this.GetByIDAsync(id);

            //string shootcutpath = Path.Combine(Path.GetDirectoryName(find.Url), Path.GetFileNameWithoutExtension(find.Url) + "_shootcut.png");

            //Action<int> exitAction = k =>
            //{
            //    if (k != 0) return;

            //    if (!File.Exists(shootcutpath)) return;

            //    find.Image = "data:image/jpeg;base64," + EncodeImageToString(shootcutpath);

            //    this.UpdateAsync(find);

            //    List<string> result = new List<string>();
            //    result.Add(shootcutpath);

            //    return result;
            //};

            //FFmpegService.Instance.ShootCut(find.Url, shootcutpath, timespan, exitAction);

            //find.Image = "data:image/jpeg;base64," + EncodeImageToString(shootcutpath);


        }

        //将图片以二进制流
        public byte[] SaveImage(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中
            return imgBytesIn;
        }

        public string EncodeImageToString(string imageFilePath)
        {
            byte[] image = null;
            FileStream fstrm = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fstrm))
            {
                image = new byte[reader.BaseStream.Length];
                for (int i = 0; i < reader.BaseStream.Length; i++)
                {
                    image[i] = reader.ReadByte();
                }
            }
            return Base64EncodeBytes(image);
        }

        public string Base64EncodeBytes(byte[] inputBytes)
        {
            return Convert.ToBase64String(inputBytes);
        }
    }
}
