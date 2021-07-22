using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using data.collection.util.AOP;
using data.collection.util.DI;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace data.collection.util.Extentions
{
    public static partial class Extention
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        /// <summary>
        /// 自动注入拥有ITransientDependency,IScopeDependency或ISingletonDependency的类
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static IServiceCollection AddFxServices(this IServiceCollection services)
        {
            Dictionary<Type, ServiceLifetime> lifeTimeMap = new Dictionary<Type, ServiceLifetime>
            {
                {typeof(ITransientDependency), ServiceLifetime.Transient},
                {typeof(IScopedDependency), ServiceLifetime.Scoped},
                {typeof(ISingletonDependency), ServiceLifetime.Singleton}
            };

            GlobalData.AllFxTypes.ForEach(aType =>
            {
                lifeTimeMap.ToList().ForEach(aMap =>
                {
                    var theDependency = aMap.Key;
                    if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract &&
                        aType.IsClass)
                    {
                        //注入实现
                        services.Add(new ServiceDescriptor(aType, aType, aMap.Value));

                        var interfaces = GlobalData.AllFxTypes
                            .Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                        //有接口则注入接口
                        if (interfaces.Count > 0)
                        {
                            interfaces.ForEach(aInterface =>
                            {
                                //注入AOP
                                services.Add(new ServiceDescriptor(aInterface, serviceProvider =>
                                {
                                    CastleInterceptor castleInterceptor = new CastleInterceptor(serviceProvider);

                                    return _generator.CreateInterfaceProxyWithTarget(aInterface,
                                        serviceProvider.GetService(aType), castleInterceptor);
                                }, aMap.Value));
                            });
                        }
                        //无接口则注入自己
                        else
                        {
                            services.Add(new ServiceDescriptor(aType, aType, aMap.Value));
                        }
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddFxServicesForHostedService(this IServiceCollection services)
        {
            Dictionary<Type, ServiceLifetime> lifeTimeMap = new Dictionary<Type, ServiceLifetime>
            {
                {typeof(ITransientDependency), ServiceLifetime.Transient},
                {typeof(IScopedDependency), ServiceLifetime.Scoped},
                {typeof(ISingletonDependency), ServiceLifetime.Singleton}
            };

            GlobalData.AllFxTypes.ForEach(aType =>
            {
                lifeTimeMap.ToList().ForEach(aMap =>
                {
                    var theDependency = aMap.Key;
                    if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract &&
                        aType.IsClass)
                    {
                        //注入实现
                        services.Add(new ServiceDescriptor(aType, aType, ServiceLifetime.Singleton));

                        var interfaces = GlobalData.AllFxTypes
                            .Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                        //有接口则注入接口
                        if (interfaces.Count > 0)
                        {
                            interfaces.ForEach(aInterface =>
                            {
                                //注入AOP
                                services.Add(new ServiceDescriptor(aInterface, serviceProvider =>
                                {
                                    CastleInterceptor castleInterceptor = new CastleInterceptor(serviceProvider);

                                    return _generator.CreateInterfaceProxyWithTarget(aInterface,
                                        serviceProvider.GetService(aType), castleInterceptor);
                                }, ServiceLifetime.Singleton));
                            });
                        }
                        //无接口则注入自己
                        else
                        {
                            services.Add(new ServiceDescriptor(aType, aType, ServiceLifetime.Singleton));
                        }
                    }
                });
            });

            return services;
        }

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

        /// <summary>
        /// 注入 MongoDb
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoClient(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var _db = client.GetDatabase(configuration["MongoDB:Database"]);
            services.AddSingleton(_db);
            return services;
        }
    }
}