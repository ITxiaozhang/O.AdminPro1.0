using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Http;

namespace Common
{
    /// <summary>
    /// 非WebApi使用
    /// </summary>
    public class AutoFacConfig
    {
        public static string _DefaultName = "O.AdminPro";//默认项目命名空间/项目名
        public static string _DefaultApiName = "O.AdminProApi";//默认WebApi命名空间/项目名
        /// <summary>
        /// Web项目配置
        /// </summary>
        /// <param name="projectName">WebApi项目名（命名空间）</param>
        public static void Init(string projectName="")
        {
            //默认项目名称
            projectName = string.IsNullOrEmpty(projectName) ? _DefaultName : projectName;

            //构造一个AutoFac的builder容器  
            ContainerBuilder builder = new Autofac.ContainerBuilder();

            //第二步：告诉AutoFac控制器工厂，控制器类的创建去哪些程序集中查找（默认控制器工厂是去扫描bin目录下的所有程序集）  
            Assembly controllerAss = Assembly.Load(projectName);

            builder.RegisterControllers(controllerAss);

            //加载业务逻辑层程序集。  
            Assembly servicesAss = Assembly.Load("Services");
            Type[] stypes = servicesAss.GetTypes();
            builder.RegisterTypes(stypes).AsImplementedInterfaces(); //指明创建的stypes这个集合中所有类的对象实例，以其接口的形式保存  

            //创建一个真正的AutoFac的工作容器  
            var container = builder.Build();

            //将当前容器中的控制器工厂替换掉MVC默认的控制器工厂。
            //（即：不要MVC默认的控制器工厂了，用AutoFac容器中的控制器工厂替代）此处使用的是将AutoFac工作容器交给MVC底层 (需要using System.Web.Mvc;)  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        /// <summary>
        /// WebApi的AutoFac配置
        /// </summary>
        /// <param name="projectName">WebApi项目名（命名空间）</param>
        public static void Init_WebApi(string projectName="")
        {
            //默认项目名称
            projectName = string.IsNullOrEmpty(projectName) ? _DefaultApiName : projectName;
            //构造一个AutoFac的builder容器  
            ContainerBuilder builder = new Autofac.ContainerBuilder();
            var configuration = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(configuration);

            Assembly apicontrollerAss = Assembly.Load(projectName);
            builder.RegisterApiControllers(apicontrollerAss);

            Assembly repositoryAss = Assembly.Load("Services");   //加载数据层程序集。  
            Type[] rtypes = repositoryAss.GetTypes();
            builder.RegisterTypes(rtypes)
                .AsImplementedInterfaces().InstancePerApiRequest();

            //创建AutoFac工作容器  
            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);//注册api容器
        }

    }
}
