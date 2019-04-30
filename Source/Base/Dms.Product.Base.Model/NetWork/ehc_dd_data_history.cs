using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Base.Model
{
    public class HeartBeat
    {
        /// <summary>
        /// 
        /// </summary>
        public int max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int min { get; set; }
    }

    public class Breath
    {
        /// <summary>
        /// 
        /// </summary>
        public int max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int min { get; set; }
    }

    public class Move
    {
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int times { get; set; }
    }

    public class OffBed
    {
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int times { get; set; }
    }

    public class SleepItems
    {
        /// <summary>
        /// 
        /// </summary>
        public string TotalTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SleepItem> sleepRecords { get; set; }
    }


    public class SleepItem
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime { get; set; }
    }
    public class SleepResult
    {
        /// <summary>
        /// 
        /// </summary>
        public SleepItems GoToSleep { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SleepItems LightSleep { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SleepItems DeepSleep { get; set; }
    }

    public class HistoryResult
    {
        /// <summary>
        /// 
        /// </summary>
        public HeartBeat heartBeat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Breath breath { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string moveTime { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int moveTimes { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string sleepTime { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string onBedTime { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string offBedTime { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int offBedTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Move move { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OffBed offBed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string onBedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sleepTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SleepResult sleepResult { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ehc_dd_data> data { get; set; } = new List<ehc_dd_data>();
        /// <summary>
        /// 
        /// </summary>
        public HistoryResult result { get; set; }
    }

    public class HistoryRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public bool succeed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }


        public string message { get; set; }
    }
}
