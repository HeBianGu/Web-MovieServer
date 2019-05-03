
using Dms.Product.Base.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Product.General.LocalDataBase
{
    /// <summary> 增加一个泛型集合 </summary>
    public class BaseDataContext<T> : DataContext where T : class
    {

        public BaseDataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        /// <summary> 泛型集合 </summary>
        public DbSet<T> Collection { get; set; }
    }

    public class ConsoleContext : DbContext
    {
        Action<DbContextOptionsBuilder> _action;

        public ConsoleContext(string constr)
        {
            _action = l => l.UseMySQL(constr);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _action?.Invoke(optionsBuilder);
        }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<ehc_dv_user> Users { get; set; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public DbSet<jw_add_data> Datas { get; set; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public DbSet<ehc_dv_customer> Customers { get; set; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public DbSet<ehc_dv_bed> Beds { get; set; }

        ///// <summary>
        ///// 报表数据
        ///// </summary>
        //public DbSet<JCSJ_MAT> Mats { get; set; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public DbSet<ehc_dv_monitor> Moniters { get; set; }

        /// <summary>
        /// 监控项扩展
        /// </summary>
        public DbSet<ehc_dv_monitorextention> ehc_dv_monitorextentions { get; set; }

        /// <summary>
        /// 监控项类型表
        /// </summary>
        public DbSet<ehc_dv_monitortype> ehc_dv_monitortypes { get; set; }

        /// <summary>
        /// 用户操作日志
        /// </summary>
        public DbSet<ehc_dv_userlogger> ehc_dv_userloggers { get; set; }


        /// <summary>
        /// 报警记录
        /// </summary>
        public DbSet<ehc_dv_warn> ehc_dv_warns { get; set; }

        /// <summary>
        /// 权限表
        /// </summary>
        public DbSet<ehc_dv_role> ehc_dv_roles { get; set; }

        /// <summary>
        /// 报警记录
        /// </summary>
        public DbSet<ehc_dv_alarm> ehc_dv_alarms { get; set; }


        /// <summary>
        /// 实时信息
        /// </summary>
        public DbSet<ehc_dv_realdata> ehc_dv_realdatas { get; set; }


        /// <summary>
        /// 历史信息
        /// </summary>
        public DbSet<ehc_dv_history> ehc_dv_historys { get; set; }


        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<ehc_dv_realcache> ehc_dv_realcaches { get; set; }



        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_dv_movie> mbc_dv_movies { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_areatype> mbc_db_areatypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_articulationtype> mbc_db_articulationtypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_extendtype> mbc_db_extendtypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_fromtype> mbc_db_fromtypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_mediatype> mbc_db_mediatypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_tagtype> mbc_db_tagtypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_db_viptype> mbc_db_viptypes { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_dc_case> mbc_dc_cases { get; set; }

        /// <summary>
        /// 实时缓存数据 用于绘制实时曲线
        /// </summary>
        public DbSet<mbc_dv_movieimage> mbc_dv_movieimages { get; set; }
    }


}
