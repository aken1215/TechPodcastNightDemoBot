using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechPodcastNightDemoBot.Controllers;

namespace TechPodcastNightDemoBot.Modules
{
    public class MainModule: Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new LuisModelAttribute("bc75ce6d-1892-4829-869f-51098bffe1da", "cfff5bccbad84c6f95e8f50ef0cefc09")).AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LuisService>().Keyed<ILuisService>(FiberModule.Key_DoNotSerialize).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LUISDiaglog>().As<IDialog<object>>().InstancePerDependency();
        }
    }
}