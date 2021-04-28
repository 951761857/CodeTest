using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Test.CoreAppLifeTime.LifeTimes;

namespace Test.CoreAppLifeTime.BaseCore
{
    public class LifeTimeConfig
    {
        public static IServiceProvider RootProvide { get; set; }


        public static IServiceCollection ConfigServiceCollection { get; set; }

        public static IServiceProvider RequestProvier { get; set; }
    }
}
