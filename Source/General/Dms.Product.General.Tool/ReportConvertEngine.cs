using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Dms.Product.General.Tool
{

    public class ReportConvertEngine<TModel, RType>
    {
        public Func<TModel, RType> Convert { get; set; }

        public Predicate<RType> MatchValue { get; set; }

        public string Name { get; set; } = "未知";

        public string Type { get; set; } = "line";

        public List<RType> Datas { get; set; } = new List<RType>();

        public dynamic ToDynamic()
        {
            dynamic dynamic = new ExpandoObject();

            dynamic.name = this.Name;

            dynamic.type = this.Type;

            dynamic.data = this.Datas.ToArray();

            //dynamic.markPoint=

            //dynamic dynamic1 = new ExpandoObject();

            //dynamic1.data = new ExpandoObject();

            //dynamic1.data.xAxis = 19;

            //dynamic1.data.yAxis = 40;

            //dynamic1.data.name = "测试";

            //dynamic.markPoint =  dynamic1;

            //           @"data: [{

            //                   name: '呼吸',
            //	value: 112.2,
            //	xAxis: 19,
            //	yAxis: 110,
            //}, {
            //	type: 'max',
            //	name: '最大值'
            //}, {
            //	type: 'min',
            //	name: '最小值'
            //}]";

            return dynamic;
        }
    }
}
