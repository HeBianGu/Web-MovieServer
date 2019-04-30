﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Base.Model
{
    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class EntityBase<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey ID { get; set; }
    }

    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public abstract class GuidEntityBase: EntityBase<Guid>
    {

    }



    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public abstract class StringEntityBase : EntityBase<string>
    {
        public StringEntityBase()
        {
            this.ID = Guid.NewGuid().ToString();

            this.CDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        [Display(Name = "创建时间")]
        public string CDATE { get; set; }

        [Display(Name = "修改时间")]
        public string UDATE { get; set; }

        [Display(Name = "是否可用")]
        public int ISENBLED { get; set; } = 1; 
    }
}
