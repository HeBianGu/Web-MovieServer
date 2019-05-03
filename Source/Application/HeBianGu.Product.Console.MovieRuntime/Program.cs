using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using HeBianGu.Product.General.FFmpegService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeBianGu.Product.Console.MovieRuntime
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            ConsoleContext context = new ConsoleContext("server=localhost;userid=root;pwd=123456;port=3306;database=test;sslmode=none;");


            foreach (var item in context.mbc_dc_cases.Where(l => l.ISENBLED == 1))
            {

                System.Console.WriteLine("正在生成案例:" + item.Name + "-" + item.BaseUrl);
                var extends = context.mbc_db_extendtypes;

                List<string> allextends = new List<string>();

                if (extends != null)
                {
                    foreach (var item1 in extends)
                    {
                        allextends.AddRange(item1.Value.Trim().ToLower().Split('/'));
                    }
                }

                Predicate<FileInfo> match = l =>
                {
                    if (allextends.Count == 0) return true;

                    return allextends.Exists(k => k == l.Extension);
                };

                if (!Directory.Exists(item.BaseUrl))
                {
                    Directory.CreateDirectory(item.BaseUrl);
                }

                var movies = context.mbc_dv_movies.Where(l => l.CaseType == item.ID).ToList();

                Action<FileInfo> action = l =>
                {
                    if (movies != null)
                    {
                        if (movies.Exists(k => k.Url == l.FullName)) return;
                    }

                    if (!match(l)) return;

                    System.Console.WriteLine("正在加载文件:" + l.FullName);

                    mbc_dv_movie movie = new mbc_dv_movie();
                    //  Message：基础数据
                    movie.Name = l.Name;
                    movie.Url = l.FullName;
                    movie.ExtendType = l.Extension;
                    movie.CaseType = item.ID;
                    movie.Size = l.Length;
                    movie.FromType = "local";


                    var tags = context.mbc_db_tagtypes.Where(k => l.Name.Contains(k.Value));
                    var list = tags.ToList();

                    if (list != null && list.Count > 0)
                    {
                        movie.TagTypes = list.Select(k => k.Value).Aggregate((m, k) => m + "," + k);
                    }

                    System.Console.WriteLine("加载文件详情:" + l.FullName);
                    try
                    {
                        //  Message：ffmpeg数据
                        var detial = FFmpegService.Instance.GetMediaEntity(l.FullName);

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
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("获取ffmpeg详情信息错误:" + ex);
                    }

                    System.Console.WriteLine("加载缩略图:" + l.FullName);
                    //  Message：缩略图和预览图
                    string shootcutpath = Path.Combine(Path.GetDirectoryName(movie.Url), Path.GetFileNameWithoutExtension(movie.Url) + "_shootcut.png");

                    //Action<int> exitAction = k =>
                    //  {
                    //      if (k != 0) return;

                    //      if (!File.Exists(shootcutpath)) return;

                    //      movie.Image = "data:image/jpeg;base64," + EncodeImageToString(shootcutpath);

                    //      context.mbc_dv_movies.Add(movie);
                    //  };
                    //  Message：默认一分钟图片作为缩略图
                    FFmpegService.Instance.ShootCut(movie.Url, shootcutpath, "00:01:00");


                    if(File.Exists(shootcutpath))
                    {
                        movie.Image = "data:image/jpeg;base64," + EncodeImageToString(shootcutpath);

                        File.Delete(shootcutpath);
                    } 

                    context.mbc_dv_movies.Add(movie);

                    System.Console.WriteLine("加载预览图:" + l.FullName);

                    string shootcutbatpath = Path.Combine(Path.GetDirectoryName(movie.Url), Path.GetFileNameWithoutExtension(movie.Url) + "_shootcut");

                    //  Message：默认一分钟图片作为缩略图
                    var images = FFmpegService.Instance.ShootCutBat(movie.Url, shootcutbatpath);

                    foreach (var m in images)
                    {
                        if (!File.Exists(m)) continue;

                        mbc_dv_movieimage image = new mbc_dv_movieimage();

                        image.MovieID = movie.ID;
                        image.Image = "data:image/jpeg;base64," + EncodeImageToString(m);
                        image.Text = Path.GetFileName(m);

                        context.mbc_dv_movieimages.Add(image);

                        //  Message：保存完删除图片
                        File.Delete(m);
                    }

                    //  Message：一个文件一保存
                    context.SaveChanges();

                    System.Console.WriteLine("完成加载文件:" + l.FullName);
                };

                DoAllFiles(item.BaseUrl, action);

                context.SaveChangesAsync();

                System.Console.WriteLine("完成生成案例:" + item.Name + "-" + item.BaseUrl);

            }

            System.Console.WriteLine("全部完成");

            System.Console.Read();
        }

        static void DoAllFiles(string dirs, Action<FileInfo> action)
        {
            //绑定到指定的文件夹目录
            DirectoryInfo dir = new DirectoryInfo(dirs);

            //检索表示当前目录的文件和子目录
            FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();

            //遍历检索的文件和子目录
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                //判断是否为空文件夹　　
                if (fsinfo is DirectoryInfo)
                {
                    //递归调用
                    DoAllFiles(fsinfo.FullName, action);
                }
                else if (fsinfo is FileInfo)
                {
                    action(fsinfo as FileInfo);
                }
            }
        }


        static string EncodeImageToString(string imageFilePath)
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

        static string Base64EncodeBytes(byte[] inputBytes)
        {
            return Convert.ToBase64String(inputBytes);
        }
    }
}
