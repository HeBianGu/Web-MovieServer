using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dms.Product.General.Tool
{
    public class ReportAutoCreateService
    {
        /// <summary>
        /// 获取类型TModel所有匹配RType的字段值作为画线
        /// </summary>
        /// <typeparam name="TModel"> 模型的类型 </typeparam>
        /// <typeparam name="RType"> 匹配字段的类型 </typeparam>
        /// <param name="models"> 模型集合 </param>
        /// <param name="toxAxis"> 生成x轴的转换规则 </param>
        /// <returns> 绘图Series对象 </returns>
        public dynamic CreateWithAllProperty<TModel, RType>(List<TModel> models, Func<TModel, string> toxAxis, Predicate<PropertyInfo> except = null)
        {
            //  Do：获取类型TModel所有匹配RType的字段值作为画线
            List<ReportConvertEngine<TModel, RType>> allMacthType = new List<ReportConvertEngine<TModel, RType>>();

            var vaildIntCollection = typeof(TModel).GetProperties().ToList().FindAll(l => l.PropertyType == typeof(RType));

            foreach (var item in vaildIntCollection)
            {
                if (except(item)) continue;

                ReportConvertEngine<TModel, RType> engine = new ReportConvertEngine<TModel, RType>();

                engine.Convert = l =>
                {
                    return (RType)item.GetValue(l);
                };

                DisplayAttribute display = item.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

                engine.Name = display == null ? item.Name : display.Name;

                engine.Type = "line";

                //  Do：如果字段取值为null不添加值
                engine.MatchValue = l => l != null;

                //if(typeof(RType)==typeof(int?)|| typeof(RType) == typeof(double?) || typeof(RType) == typeof(float?))

                allMacthType.Add(engine);
            }

            return this.Create(models, toxAxis, allMacthType);
        }

        /// <summary>
        /// 获取类型TModel指定匹配RType的字段值作为画线
        /// </summary>
        /// <typeparam name="TModel"> 模型的类型 </typeparam>
        /// <typeparam name="RType"> 匹配字段的类型 </typeparam>
        /// <param name="models"> 模型集合 </param>
        /// <param name="toxAxis"> 生成x轴的转换规则 </param>
        /// <param name="matchs"> 指定匹配 </param>
        /// <returns> 绘图Series对象 </returns>
        public dynamic Create<TModel, RType>(List<TModel> models, Func<TModel, string> toxAxis, List<ReportConvertEngine<TModel, RType>> matchs)
        {
            if (models == null) return null;
            //  Do：生成x坐标
            List<string> pxAxis = new List<string>();

            foreach (var c in models)
            {
                string v = toxAxis(c);

                if (!pxAxis.Contains(v))
                {
                    pxAxis.Add(toxAxis(c));
                }
            }

            //  Do：根据属性动态生成数据
            foreach (var x in pxAxis)
            {
                //  Do：获取当前天的数据
                var find = models.Where(l => toxAxis(l) == x);

                //  Do：根据定义规则动态加载到数据集合
                if (find.Count() > 0)
                {
                    foreach (var item in matchs)
                    {
                        RType c = item.Convert(find.First());

                        if (item.MatchValue(c))
                        {
                            item.Datas.Add(c);
                        }
                    }
                }
            }

            List<string> Legend = new List<string>();

            foreach (var item in matchs)
            {
                Legend.Add(item.Name);
            }

            List<dynamic> pseries = matchs.Select(l => l.ToDynamic()).ToList();

            return new
            {
                xAxis = new { data = pxAxis.ToArray() },
                legend = new { data = Legend.ToArray() },
                series = pseries

            };
        }

        /// <summary>
        /// 根据数据设置分组和汇总数据
        /// </summary>
        /// <typeparam name="TModel"> 模型类型 </typeparam>
        /// <typeparam name="TResult"> 汇总数据类型 </typeparam>
        /// <param name="models"> 模型数据列表 </param>
        /// <param name="groupBy"> 分组规则 </param>
        /// <param name="groupFunc"> 汇总规则 </param>
        /// <returns></returns>
        public dynamic GroupBy<TModel, TResult>(IQueryable<TModel> models, Func<TModel, string> groupBy, params ReportGroupEngine<TModel, TResult>[] groupFuncs)
        {

            var groups = models.GroupBy(groupBy);

            //  Do：创建x坐标
            List<string> pxAxis = new List<string>();

            foreach (var c in groups)
            {
                var v = c.Key;

                if (!pxAxis.Contains(v))
                {
                    pxAxis.Add(v);
                }
            }

            //List<TResult> datas = new List<TResult>();

            //  Do：根据属性动态生成数据
            foreach (var x in pxAxis)
            {
                foreach (var item in groupFuncs)
                {
                    //  Do：获取当前天的数据 
                    TResult find = item.GroupBy(models.Where(l => groupBy(l) == x));

                    if (item.MatchValue == null || item.MatchValue(find))
                    {
                        item.Datas.Add(find);
                    }
                }
            }

            List<string> Legend = new List<string>();

            foreach (var item in groupFuncs)
            {
                Legend.Add(item.Name);
            }

            List<dynamic> pseries = groupFuncs.Select(l => l.ToDynamic()).ToList();

            return new
            {
                xAxis = new { data = pxAxis.ToArray() },
                legend = new { data = Legend.ToArray() },
                series = pseries
            };

            ////  Do：创建返回集合
            //List<dynamic> collection = new List<dynamic>();

            //dynamic dynamic = new ExpandoObject();

            //dynamic.name = "0-6岁儿童建档总数汇总";

            //dynamic.type = "line";

            //collection.Add(dynamic);

            //List<string> Legend = new List<string>();

            //Legend.Add(dynamic.name);

            //dynamic.data = datas.ToArray();

            ////  Do：返回的Json对象
            //return new
            //{
            //    xAxis = new { data = pxAxis.ToArray() },
            //    legend = new { data = Legend.ToArray() },
            //    series = collection.ToArray()
            //};
        }


        /// <summary>
        /// 获取汇总求和数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="groupby"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public dynamic GroupByExpression<T, TResult>(IQueryable<T> collection, Expression<Func<T, String>> groupby, params Expression<Func<IQueryable<T>, TResult>>[] expressions)
        {

            List<string> Legend = new List<string>();

            ////  Message：利用表达式设置列名称 
            //MemberExpression memberExpression = groupby.Body as MemberExpression;

            //var displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DisplayAttribute).Description;

            //Legend.Add(displayName);


            var datas = new List<ReportGroupEngine<T, TResult>>();

            foreach (var expression in expressions)
            {
                //  Message：构造Legend  
                Func<UnaryExpression, MethodCallExpression> getMethodCall = null;

                //  Message：地柜获取MethodCallExpression表达式
                getMethodCall = l =>
                   {
                       if (l.Operand is UnaryExpression)
                       {
                           UnaryExpression unaryExpression = l.Operand as UnaryExpression;

                           return getMethodCall(unaryExpression);
                       }
                       else if (l.Operand is MethodCallExpression)
                       {
                           return l.Operand as MethodCallExpression;
                       }
                       else
                       {
                           return null;
                       }
                   };

                MethodCallExpression methodCallExpression = null;

                if (expression.Body is UnaryExpression)
                {
                    methodCallExpression = getMethodCall(expression.Body as UnaryExpression);
                }
                else if (expression.Body is MethodCallExpression)
                {
                    methodCallExpression = expression.Body as MethodCallExpression;
                }

                string groupName = methodCallExpression.Method.Name;

                string displayName = string.Empty;

                //  Message：无参数聚合函数不取DisplayName 如Count
                if (methodCallExpression.Arguments.Count == 2)
                {
                    UnaryExpression unaryexpression = methodCallExpression.Arguments[1] as UnaryExpression;

                    LambdaExpression LambdaExpression = unaryexpression.Operand as LambdaExpression;

                    MemberExpression memberExpression = LambdaExpression.Body as MemberExpression;

                    displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DisplayAttribute).Name;
                }

                Legend.Add(displayName + $"({groupName})");



                //  Message：构造Serise
                Func<IQueryable<T>, TResult> fun = expression.Compile();

                ReportGroupEngine<T, TResult> data = new ReportGroupEngine<T, TResult>();

                data.Name = displayName + $"({groupName})";

                data.GroupBy = fun;

                data.Type = "line";

                datas.Add(data);

                //}
                //else if (expression.Body is UnaryExpression)
                //{
                //    UnaryExpression unaryExpression = expression.Body as UnaryExpression;

                //    MethodCallExpression dynamicExpression = unaryExpression.Operand as MethodCallExpression;

                //    string groupName = dynamicExpression.Method.Name;

                //    UnaryExpression unaryexpression = dynamicExpression.Arguments[0] as UnaryExpression;

                //    LambdaExpression LambdaExpression = unaryexpression.Operand as LambdaExpression;

                //    MemberExpression memberExpression = LambdaExpression.Body as MemberExpression;

                //    string displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DisplayAttribute).Name;

                //    Legend.Add(groupName);

                //    //  Message：构造Serise
                //    Func<IQueryable<T>, TResult> fun = expression.Compile();

                //    ReportGroupEngine<T, TResult> data = new ReportGroupEngine<T, TResult>();

                //    data.Name = groupName;

                //    data.GroupBy = fun;

                //    data.Type = "line";

                //    datas.Add(data);

                //}




            }

            //  Message：通过表达式设置数据体 
            var groups = collection.GroupBy(groupby);

            //  Do：创建x坐标
            List<string> pxAxis = new List<string>();

            foreach (var group in groups)
            {
                //  Message：设置分组列头 
                pxAxis.Add(group.Key);

                //  Message：设置分组汇总数据
                for (int i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];

                    data.Datas.Add(data.GroupBy(group.AsQueryable()));
                }
            }

            List<dynamic> pseries = datas.Select(l => l.ToDynamic()).ToList();

            return new
            {
                xAxis = new { data = pxAxis.ToArray() },
                legend = new { data = Legend.ToArray() },
                series = pseries
            };

        }

    }
}
