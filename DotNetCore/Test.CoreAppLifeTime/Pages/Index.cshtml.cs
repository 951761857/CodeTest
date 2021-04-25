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
            HttpContext.Response.WriteAsync($"RequestProvier与Config是否同一个:{object.ReferenceEquals(LifeTimeConfig.ProviderInstance, _serviceProvider)} <br>");
            HttpContext.Response.WriteAsync($"RequestProvier与根是否同一个:{object.ReferenceEquals(LifeTimeConfig.RootProvide, _serviceProvider)} <br>");
            HttpContext.Response.WriteAsync($"Config与根是否同一个:{object.ReferenceEquals(LifeTimeConfig.RootProvide, LifeTimeConfig.ProviderInstance)} <br>");
            HttpContext.Response.WriteAsync($"ServiceColletion重新buildService与Config是否同一个:{object.ReferenceEquals(LifeTimeConfig.ConfigServiceCollection.BuildServiceProvider(), LifeTimeConfig.ProviderInstance)} <br>");

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

            //RequestProvider与Config的Scope是否同一个
            HttpContext.Response.WriteAsync($"RequestProvider与Config的Scope是否同一个:{object.ReferenceEquals(_serviceProvider.GetRequiredService<TestScope>(), LifeTimeConfig.ProviderInstance.GetRequiredService<TestScope>())} <br>");

        }
    }
}
