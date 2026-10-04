using System;
using System.Linq.Expressions;
using AutoMapper;

namespace EC226.Utility.Library.Extensions
{
    public class AutoMapperInstance
    {
        /// <summary>
        /// Mapper config
        /// </summary>
        private static MapperConfiguration config = null;

        /// <summary>
        /// 
        /// </summary>
        public static void InitAutoMapperConfig(MapperConfiguration mapperConfiguration)
        {
            config = mapperConfiguration;
        }

        /// <summary>
        /// get auto mapper only one config in dal layer
        /// </summary>
        /// <returns>MapperConfiguration config object</returns>
        public static MapperConfiguration GetAutoMapperConfigInstance()
        {
            return config;
        }

        /// <summary>
        /// 適合更新資料
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static To ConvertModel<From, To>(From source, To destination)
        {
            var config = AutoMapperInstance.GetAutoMapperConfigInstance();
            IMapper mapper = config.CreateMapper();
            return mapper.Map<From, To>(source, destination);
        }

        /// <summary>
        /// 適合新增和查詢資料
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static To ConvertModel<From, To>(From source)
        {
            var config = AutoMapperInstance.GetAutoMapperConfigInstance();
            IMapper mapper = config.CreateMapper();
            return mapper.Map<From, To>(source);
        }

        //public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
        //    this IMappingExpression<TSource, TDestination> map,
        //    Expression<Func<TDestination, object>> selector)
        //{
        //    map.ForMember(selector, config => config.Ignore());
        //    return map;
        //}
    }
}
