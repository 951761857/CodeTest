using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Test.CoreAppLifeTime.BaseCore;
using Test.CoreAppLifeTime.LifeTimes;

namespace Test.CoreAppLifeTime.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TestScope _testScope;
        private readonly TestSingleton _testSingleton;
        private readonly TestTransient _testTransient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _service;

        public IndexModel(ILogger<IndexModel> logger, TestScope _testScope, TestSingleton _testSingleton, TestTransient _testTransient, IServiceProvider provider)
        {
            _logger = logger;
            this._testScope = _testScope;
            this._testSingleton = _testSingleton;
            this._testTransient = _testTransient;

            //此处拿到的IServiceProvider是每个请求生成的IServiceProvider，所有的scope和transient生命周期的都由这个类生成
            this._serviceProvider = provider;
        }

        public void OnGet()
        {

            HttpContext.Response.ContentType = "text/html; charset=utf-8";

            HttpContext.Response.WriteAsync($"RequestProvier与上次请求是否同一个:{object.ReferenceEquals(LifeTimeConfig.RequestProvier, _serviceProvider)} <br>");
            HttpContext.Response.WriteAsync($"RequestProvier与根是否同一个:{object.ReferenceEquals(LifeTimeConfig.RootProvide, _serviceProvider)} <br>");
            HttpContext.Response.WriteAsync($"构造函数ServiceCollection与根ServiceCollection是否同一个:{object.ReferenceEquals(LifeTimeConfig.ConfigServiceCollection, _service)} <br>");

            LifeTimeConfig.RequestProvier = _serviceProvider; 

            HttpContext.Response.WriteAsync($"_testScope:{_testScope.CreateTime} <br>");
            HttpContext.Response.WriteAsync($"_testSingleton:{_testSingleton.CreateTime} <br>");
            HttpContext.Response.WriteAsync($"_testTransient:{_testTransient.CreateTime} <br>");


            //RequestProvider与RootProvider生成singleton是否同一个
            //如果相同说明RequestProvider获取singleton跟RootProvider指向同一个生成方法
            HttpContext.Response.WriteAsync($"RequestProvider与RootProvider生成singleton是否同一个:{object.ReferenceEquals(_serviceProvider.GetRequiredService<TestSingleton>(), LifeTimeConfig.RootProvide.GetRequiredService<TestSingleton>())} <br>");

            //RequestProvider重新获取的Scope与构造函数是否同一个,是同一个说明：
            //1.构造函数获取的是该请求的ServiceProvider
            //2.单个请求获取到的Scope都是同一个对象
            HttpContext.Response.WriteAsync($"RequestProvider重新获取的Scope与构造函数是否同一个:{object.ReferenceEquals(_serviceProvider.GetRequiredService<TestScope>(), _testScope)} <br>");

            //RequestProvider获取的Transient与构造函数是否同一个
            HttpContext.Response.WriteAsync($"RequestProvider获取的Transient与构造函数是否同一个:{object.ReferenceEquals(_serviceProvider.GetRequiredService<TestTransient>(), _testTransient)} <br>");

            //RequestProvider两次获取的Transient是否同一个
            HttpContext.Response.WriteAsync($"RequestProvider两次获取的Transient是否同一个:{object.ReferenceEquals(_serviceProvider.GetRequiredService<TestTransient>(), _serviceProvider.GetRequiredService<TestTransient>())} <br>");


            //从根provider获取对象
            var rootSingleton = LifeTimeConfig.RootProvide.GetRequiredService<TestSingleton>();
            try
            {
                var rootScope = LifeTimeConfig.RootProvide.GetRequiredService<TestScope>();
            }
            catch (Exception e)
            {
                HttpContext.Response.WriteAsync($"无法从根provider获取scope：{e.Message} <br>");
                
            }
            var rootTransient = LifeTimeConfig.RootProvide.GetRequiredService<TestTransient>();

            rootSingleton.CreateTime = DateTime.Parse("2015-01-01 02:02:02");
          //  rootScope.CreateTime = DateTime.Parse("2015-01-01 02:02:02");
            rootTransient.CreateTime = DateTime.Parse("2015-01-01 02:02:02");

            HttpContext.Response.WriteAsync($"rootSingleton:{rootSingleton.CreateTime} <br>");
            HttpContext.Response.WriteAsync($"rootScope:{rootSingleton.CreateTime} <br>");
            HttpContext.Response.WriteAsync($"rootTransient:{rootSingleton.CreateTime} <br>");

            HttpContext.Response.WriteAsync($"_testSingleton:{_testSingleton.CreateTime} 证明Singleton都是从root provider获取的<br>");
            HttpContext.Response.WriteAsync($"_testScope:{_testScope.CreateTime} 证明Scope都是从各自的请求request provider获取的<br>");
            HttpContext.Response.WriteAsync($"_testTransient:{_testTransient.CreateTime} 证明Transient都是从各自的请求request provider获取的<br>");

            //从根获取transient 请求完毕后没有被释放
            for (int i = 0; i < 5; i++)
            {
                var empty = LifeTimeConfig.RootProvide.GetRequiredService<TestTransient>();
                empty.IsWrite = true;
                empty.WriteMessage = $"Root Transient关闭";
            }

            //从request获取transient 每次请求完毕后都是释放
            for (int i = 0; i < 5; i++)
            {
                var empty = _serviceProvider.GetRequiredService<TestTransient>();
                empty.IsWrite = true;
                empty.WriteMessage = $"Request Transient关闭";
            }

        }
    }
}
