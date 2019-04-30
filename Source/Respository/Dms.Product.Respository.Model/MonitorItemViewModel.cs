
using Dms.Product.Base.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Respository.Model
{
    public class MonitorItemViewModel
    {
        public ehc_dv_monitor Monitor { get; set; }

        [Display(Name = "身份证号")]
        public string CardID { get; set; }

        [Display(Name = "床垫编码")]
        public string MatCode { get; set; }

        [Display(Name = "心率监测区间")]
        [MonitorType("5d107bfa-3784-4e7c-8d40-5c9a38309cd6")]
        [AlarmType("144c5f8b-64de-4a88-8621-965a6bb4e7cd")]
        public Splite2Template HeartRange { get; set; } = new Splite2Template();

        [Display(Name = "呼吸监测区间")]
        [MonitorType("e126274b-1bc4-4724-8c84-6123a506615d")]
        [AlarmType("9b4bf881-191c-4319-bdf8-2b46f1d5dc2f")]
        public Splite2Template BreathRange { get; set; } = new Splite2Template();

        [Display(Name = "监测时间一")]
        [MonitorType("5e4d6b02-1c05-4630-bfda-9c71fc031408")]
        public TimeTemplate TimeRange1 { get; set; } = new TimeTemplate();

        [Display(Name = "监测时间二")]
        [MonitorType("ee2579bd-111b-457c-8fd1-befb12bbe64e")]
        public TimeTemplate TimeRange2 { get; set; } = new TimeTemplate();

        [Display(Name = "监测时间三")]
        [MonitorType("c1a239fe-5326-4bd0-8bb3-38b9e59e36e5")]
        public TimeTemplate TimeRange3 { get; set; } = new TimeTemplate();

        [Display(Name = "监测时间四")]
        [MonitorType("1b77e4ae-81b9-45c0-bcfe-867746582dbf")]
        public TimeTemplate TimeRange4 { get; set; } = new TimeTemplate();

        [Display(Name = "监测时间五")]
        [MonitorType("67216ec5-c33c-4965-b201-c7889e46d9ef")]
        public TimeTemplate TimeRange5 { get; set; } = new TimeTemplate();

        [Display(Name = "尿湿")]
        //[MonitorType("5d107bfa-3784-4e7c-8d40-5c9a38309cd6")]
        [AlarmType("3f3290ee-4967-405f-a39e-ff73909ebf0b")]
        public bool Wet { get; set; }

        public bool IsFormVisble()
        {
            if (Monitor == null) return false;
            if (Monitor.CUSTOMID == null) return false;
            if (Monitor.BEDID == null) return false;

            if (Monitor.CUSTOMID == null) return false;

            if (!this.HeartRange.Visible()) return false;

            if (!this.BreathRange.Visible()) return false;

            if (this.TimeRange1.IsEnbled)
            {
                if (!this.TimeRange1.Visible()) return false;
            }
            if (this.TimeRange2.IsEnbled)
            {
                if (!this.TimeRange2.Visible()) return false;
            }
            if (this.TimeRange3.IsEnbled)
            {
                if (!this.TimeRange3.Visible()) return false;
            }
            if (this.TimeRange4.IsEnbled)
            {
                if (!this.TimeRange4.Visible()) return false;
            }
            if (this.TimeRange5.IsEnbled)
            {
                if (!this.TimeRange5.Visible()) return false;
            }

            return true;
        }

    }


    public abstract class TemplateBase
    {
        /// <summary> 获取模板名称 </summary>
        public string GetTempName()
        {
            var result = this.GetType().GetCustomAttributes(false);

            return (result[0] as DescriptionAttribute).Description;
        }

        public abstract bool Match(double? value);

        public abstract bool Visible();
    }

    [Description("Splite2Template")]
    public class Splite2Template : TemplateBase
    { 
        [Display(Name = "请输入最小值")]
        [Required(ErrorMessage = "请输入最小值")]
        public double? MinValue { get; set; } 


        [Display(Name = "请输入最大值")]
        [Required(ErrorMessage = "请输入最大值")]
        public double? MaxVlalue { get; set; }


        public override bool Match(double? value)
        {
            if (!value.HasValue) return false;

            return value < this.MinValue || value > this.MaxVlalue;
        }

        public override string ToString()
        {
            return $"{MinValue}-{MaxVlalue}";
        }

        public override bool Visible()
        {
            return this.MaxVlalue != double.NaN && this.MinValue != double.NaN && this.MaxVlalue > this.MinValue;
        }
    }

    [Description("TimeTemplate")]
    public class TimeTemplate : Splite2Template
    {
        [Required(ErrorMessage = "请输入离床持续时间")]
        [Display(Name = "离床持续时间")]
        [AlarmType("e6a7982b-54b7-4d1d-8cee-2db9e7f36348")]
        public int? LeaveBedTime { get; set; }

        [Required(ErrorMessage = "请输入体动持续时间")]
        [Display(Name = "体动持续时间")]
        [AlarmType("7c36158b-abf8-48ef-80fc-d7bebb26758a")]
        public int? MoveTime { get; set; }

        /// <summary> 是否可用 </summary>
        public bool IsEnbled { get; set; }

        [AlarmType("7c36158b-abf8-48ef-80fc-d7bebb26758a")]
        public override bool Match(double? value)
        {
            if (!IsEnbled) return false;

            return value < this.MinValue || value > this.MaxVlalue;
        }

        public override bool Visible()
        {
            bool minc = this.MinValue > 0 && this.MinValue <= 24;
            bool maxc = this.MaxVlalue > 0 && this.MaxVlalue <= 24;

            return base.Visible() && this.LeaveBedTime > 0 && this.MoveTime > 0 && minc && maxc;
        }

    }



}
