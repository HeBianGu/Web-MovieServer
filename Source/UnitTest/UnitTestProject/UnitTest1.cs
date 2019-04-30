
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
using System.Data;
using Dms.Product.General.NetWorkDataBase;
using Newtonsoft.Json.Linq;
using Dms.Product.Base.Model;
using Dms.Product.Respository.Model;
using System.Reflection;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod()
        {
            //  Message：根据表达式获取对应属性的值  
            PersonModel model = new PersonModel();



            //Expression<Func<IGrouping<PersonModel>, double>> ex =  l => l.Max(k => k.InCome);

            Expression<Func<IQueryable<PersonModel>, double>> ex = l => l.Max(k => k.Pay);
            Test(ex);
        }

        void Test(Expression<Func<IQueryable<PersonModel>, double>> ex)
        {
            MethodCallExpression dynamicExpression = ex.Body as MethodCallExpression;

            UnaryExpression unaryexpression = dynamicExpression.Arguments[1] as UnaryExpression;

            LambdaExpression LambdaExpression = unaryexpression.Operand as LambdaExpression;

            MemberExpression memberExpression = LambdaExpression.Body as MemberExpression;

            string ss = memberExpression.Member.Name;


        }
        [TestMethod]
        public void TestMethod1()
        {


            string v = Guid.NewGuid().ToString();

            //  Message：根据表达式获取对应属性的值  
            PersonModel model = new PersonModel();
            model.ID = "1";
            model.Name = "王杰";
            model.Value = 90;
            model.InCome = 100;
            model.Pay = 200;
            model.Age = 33;

            var result = this.GetPropertyValue(model, l => l.Name);

            Debug.WriteLine($"Description：{result.Item1} - Name:{result.Item2}");

        }

        [TestMethod]
        public void TestMethod2()
        {
            //  Message：根据表达式获取对应属性的值  
            List<PersonModel> models = new List<PersonModel>();

            Random r = new Random();

            string[] names = { "张学友", "王杰", "刘德华", "张曼玉", "李连杰", "孙悟空" };

            //  Message：构造测试数据
            for (int i = 0; i < 80; i++)
            {
                PersonModel model = new PersonModel();
                model.ID = i.ToString();
                model.Name = names[r.Next(6)];
                model.Value = r.Next(20, 100);
                model.InCome = r.Next(20, 100);
                model.Pay = r.Next(20, 100);
                model.Age = r.Next(20, 100);
                models.Add(model);
            }

            //  Message：生成自定义报表
            DataTable dt = this.GetSum(models.AsQueryable(), l => l.ID, l => l.Value, l => l.Age, l => l.InCome, l => l.Pay);

            WriteTable(dt);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //  Message：根据表达式获取对应属性的值  
            List<PersonModel> models = new List<PersonModel>();

            Random r = new Random();

            string[] names = { "张学友", "王杰", "刘德华", "张曼玉", "李连杰", "孙悟空" };

            //  Message：构造测试数据
            for (int i = 0; i < 80; i++)
            {
                PersonModel model = new PersonModel();
                model.ID = i.ToString();
                model.Name = names[r.Next(6)];
                model.Value = r.Next(20, 100);
                model.InCome = r.Next(20, 100);
                model.Pay = r.Next(20, 100);
                model.Age = r.Next(20, 100);
                models.Add(model);
            }

            Func<DesignerCategoryAttribute, string> toHeader = l => l.Category;

            //  Message：生成自定义报表
            DataTable dt = this.GetSum(models.AsQueryable(), toHeader, l => l.Name, l => l.Value);

            WriteTable(dt);
        }

        public void WriteTable(DataTable dt)
        {
            string colums = string.Empty;
            ;
            foreach (DataColumn item in dt.Columns)
            {
                colums += item.ColumnName.PadRight(5, ' ') + " ";
            }

            Debug.WriteLine(colums);

            foreach (DataRow item in dt.Rows)
            {
                string rows = string.Empty;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    rows += item[i].ToString().PadRight(5, ' ') + " ";
                }

                Debug.WriteLine(rows);
            }
        }

        /// <summary>
        /// 获取汇总求和数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="groupby"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public DataTable GetSum<T>(IQueryable<T> collection, Expression<Func<T, String>> groupby, params Expression<Func<T, double>>[] expressions)
        {
            DataTable table = new DataTable();

            //  Message：利用表达式设置列名称
            MemberExpression memberExpression = groupby.Body as MemberExpression;

            var displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;

            table.Columns.Add(new DataColumn(displayName));

            foreach (var expression in expressions)
            {
                memberExpression = expression.Body as MemberExpression;

                displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;

                table.Columns.Add(new DataColumn(displayName));

            }

            //  Message：通过表达式设置数据体 
            var groups = collection.GroupBy(groupby);

            foreach (var group in groups)
            {
                //  Message：设置分组列头
                DataRow dataRow = table.NewRow();
                dataRow[0] = group.Key;

                //  Message：设置分组汇总数据
                for (int i = 0; i < expressions.Length; i++)
                {
                    var expression = expressions[i];

                    Func<T, double> fun = expression.Compile();

                    dataRow[i + 1] = group.Sum(fun);
                }

                table.Rows.Add(dataRow);
            }

            return table;

        }


        [TestMethod]
        public void TestMethod6()
        {
            //  Message：根据表达式获取对应属性的值  
            List<PersonModel> models = new List<PersonModel>();

            Random r = new Random();

            string[] names = { "张学友", "王杰", "刘德华", "张曼玉", "李连杰", "孙悟空" };

            //  Message：构造测试数据
            for (int i = 0; i < 80; i++)
            {
                PersonModel model = new PersonModel();
                model.ID = i.ToString();
                model.Name = names[r.Next(6)];
                model.Value = r.Next(20, 100);
                model.InCome = r.Next(20, 100);
                model.Pay = r.Next(20, 100);
                model.Age = r.Next(20, 100);
                models.Add(model);
            }


            //  Message：生成自定义报表
            DataTable dt = this.GetReport(models.AsQueryable(), l => l.Name, l => l.Max(k => k.InCome), l => l.Min(k => k.InCome), l => l.Sum(k => k.InCome));

            WriteTable(dt);
        }
        /// <summary>
        /// 获取汇总求和数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="groupby"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        DataTable GetReport<T>(IQueryable<T> collection, Expression<Func<T, String>> groupby, params Expression<Func<IQueryable<T>, double>>[] expressions)
        {

            DataTable table = new DataTable();

            //  Message：利用表达式设置列名称 

            MemberExpression memberExpression = groupby.Body as MemberExpression;

            var displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;

            table.Columns.Add(new DataColumn(displayName));

            foreach (var expression in expressions)
            {
                MethodCallExpression dynamicExpression = expression.Body as MethodCallExpression;

                string groupName = dynamicExpression.Method.Name;

                UnaryExpression unaryexpression = dynamicExpression.Arguments[1] as UnaryExpression;

                LambdaExpression LambdaExpression = unaryexpression.Operand as LambdaExpression;


                memberExpression = LambdaExpression.Body as MemberExpression;

                displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;

                table.Columns.Add(new DataColumn(displayName + $"({groupName})"));

            }

            //  Message：通过表达式设置数据体 
            var groups = collection.GroupBy(groupby);

            foreach (var group in groups)
            {
                //  Message：设置分组列头
                DataRow dataRow = table.NewRow();
                dataRow[0] = group.Key;

                //  Message：设置分组汇总数据
                for (int i = 0; i < expressions.Length; i++)
                {
                    var expression = expressions[i];

                    Func<IQueryable<T>, double> fun = expression.Compile();

                    dataRow[i + 1] = fun(group.AsQueryable());
                }

                table.Rows.Add(dataRow);
            }

            return table;

        }


        /// <summary>
        /// 获取汇总求和数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="groupby"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public DataTable GetSum<TModel, TAttr>(IQueryable<TModel> collection, Func<TAttr, string> toHeader, Expression<Func<TModel, String>> groupby, params Expression<Func<TModel, double>>[] expressions)
        {
            DataTable table = new DataTable();

            //  Message：利用表达式设置列名称
            MemberExpression memberExpression = groupby.Body as MemberExpression;

            var displayName = toHeader((TAttr)memberExpression.Member.GetCustomAttributes(typeof(TAttr), false).First());

            table.Columns.Add(new DataColumn(displayName));

            foreach (var expression in expressions)
            {
                memberExpression = expression.Body as MemberExpression;

                displayName = toHeader((TAttr)memberExpression.Member.GetCustomAttributes(typeof(TAttr), false).First());

                table.Columns.Add(new DataColumn(displayName));

            }

            //  Message：通过表达式设置数据体 
            var groups = collection.GroupBy(groupby);

            foreach (var group in groups)
            {
                //  Message：设置分组列头
                DataRow dataRow = table.NewRow();
                dataRow[0] = group.Key;

                //  Message：设置分组汇总数据
                for (int i = 0; i < expressions.Length; i++)
                {
                    var expression = expressions[i];

                    Func<TModel, double> fun = expression.Compile();

                    dataRow[i + 1] = group.Sum(fun);
                }

                table.Rows.Add(dataRow);
            }

            return table;

        }



        /// <summary>
        /// 通过Linq表达式获取成员属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Tuple<string, string> GetPropertyValue<T>(T instance, Expression<Func<T, string>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;

            string propertyName = memberExpression.Member.Name;

            string attributeName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;

            var property = typeof(T).GetProperties().Where(l => l.Name == propertyName).First();

            return new Tuple<string, string>(attributeName, property.GetValue(instance).ToString());

        }




        [TestMethod]
        public void TestMethodPost()
        {
            NetWorkService httpPostHelper = new NetWorkService();

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("key", "chzhjkkj123456./");
            keyValuePairs.Add("sn", "Z55693");
            keyValuePairs.Add("startTime", "2019-04-01");
            keyValuePairs.Add("endTime", "2019-04-11");

            string err;

            string url = "http://121.40.75.174:7777/DaPaRe/FindPastRecordsData";

            var result = httpPostHelper.Post(url, keyValuePairs, out err);


            Debug.WriteLine(result);


            string ss = result;
        }

        [TestMethod]
        public void TestMethodGetHistPost()
        {
            // string err;

            //var result=  DataService.Instance.GetHistDataCollection("Z55693", "2019-04-01", "2019-04-11",out err);

            // Debug.WriteLine(result);

            // var result1 = DataService.Instance.GetRealDataCollection(out err);

            // Debug.WriteLine(result1);


        }


        [TestMethod]
        public void TestMethodDateTime()
        {

            //DateTime time = "01/01/2016 12:00 AM".ToDateTime("MM/dd/yyyy HH:mm tt");


            //Debug.WriteLine(time);

            //DateTime dt = DateTime.Now; string aa = string.Format("{0:yyyy-mm-dd hh:mm:ss tt }", dt);
            //string cc = aa.Replace("上午", "am").Replace("下午", "pm");


            DateTime dt = DateTime.Parse("01/01/2016 12:00 PM"); // No error checking


        }

        [TestMethod]
        public void TestMethod111()
        {
            MonitorItemViewModel model = new MonitorItemViewModel();

            foreach (PropertyInfo p in typeof(MonitorItemViewModel).GetProperties())
            {
                //object[] Attribute1 = p.GetCustomAttributes(true);
                object[] Attribute2 = p.GetCustomAttributes(typeof(DescriptionAttribute), false);


                //if (Attribute1 != null)
                //{
                //    foreach (var item in Attribute1)
                //    {
                //        if (item is DescriptionAttribute)
                //            Debug.WriteLine((item as DescriptionAttribute).Description);
                //    }
                //}

                if (Attribute2 != null)
                {
                    foreach (var item in Attribute2)
                    {
                        if (item is DescriptionAttribute)
                            Debug.WriteLine((item as DescriptionAttribute).Description);
                    }
                }
            }


            string str = string.Empty;

        }

        [TestMethod]
        public void TestMethod222()
        {
            Expression<Func<MonitorItemViewModel, bool>> expression = l => l.Wet;

            MemberExpression memberExpression = expression.Body as MemberExpression;

            AlarmTypeAttribute obj = (AlarmTypeAttribute)memberExpression.Member.GetCustomAttributes(false).Where(l=>l is AlarmTypeAttribute).FirstOrDefault();

            string displayName = obj.AlarmTypeID;

            string sss = string.Empty; 
        }

        [TestMethod]
        public void TestMethod333()
        {
            TimeTemplate timeTemplate = new TimeTemplate();

            var methodname = MethodBase.GetCurrentMethod().Name; 

            Debug.WriteLine(methodname);

            var method = timeTemplate.GetType().GetMethod("Match");

            object[] attr = method.GetCustomAttributes(typeof(AlarmTypeAttribute), false); 


            string ss = string.Empty;

        }

        [TestMethod]
        public void TestMethod444()
        {
          var mes=  typeof(UserLoggerConfiger).GetFields();

            foreach (var item in mes)
            {

                Debug.WriteLine(item.GetRawConstantValue().ToString());

            }

            string ss = string.Empty;

        }


        
    }

    [Description("唯一标识")]
    class PersonModel
    {
        [Description("唯一标识")]
        [DesignerCategory("分组一")]
        public string ID { get; set; }

        [Description("名称")]
        [DesignerCategory("分组二")]
        public string Name { get; set; }

        [Description("值")]
        [DesignerCategory("分组三")]
        public double Value { get; set; }

        [Description("年齡")]
        [DesignerCategory("分组四")]
        public double Age { get; set; }

        [Description("收入")]
        [DesignerCategory("分组五")]
        public double InCome { get; set; }

        [Description("支出")]
        [DesignerCategory("分组六")]
        public double Pay { get; set; }
    }

}
