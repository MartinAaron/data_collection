using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using data.collection.util;
using ZR.DataHunter.Util;

namespace ZR.DataHunter.Api.AutoMapperProfile
{
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 使用AutoMapper自动映射拥有MapAttribute的类
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">自定义配置</param>
        public static IServiceCollection AddAutoMapperFx(this IServiceCollection services,
            Action<IMapperConfigurationExpression> configure = null)
        {
            services.AddAutoMapper(GlobalData.AllFxAssemblies);
            return services;
        }
    }
}