using Hangfire;
using LightInject;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace WebShop.LightInjector
{
    public class LightInjectJobActivator : JobActivator
    {
        private readonly ServiceContainer _container;

        public LightInjectJobActivator(ServiceContainer container, bool selfReferencing = false)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public override object ActivateJob(Type jobType)
        {
            Context.InitializeFakeHttpContext();

            // this will fail if you do self referencing job queues on a class with an interface:
            //  BackgroundJob.Enqueue(() => this.SendSms(message)); 
            var instance = _container.TryGetInstance(jobType);

            // since it fails we can try to get the first interface and request from container
            if (instance == null && jobType.GetInterfaces().Any())
                instance = _container.GetInstance(jobType.GetInterfaces().FirstOrDefault());

            return instance;
        }

        [Obsolete("Please implement/use the BeginScope(JobActivatorContext) method instead. Will be removed in 2.0.0.")]
        public override JobActivatorScope BeginScope()
        {
            Context.InitializeFakeHttpContext();
            return new LightInjectScope(_container);
        }
    }

    internal class LightInjectScope : JobActivatorScope
    {
        private readonly ServiceContainer _container;
        private readonly Scope _scope;

        public LightInjectScope(ServiceContainer container)
        {
            _container = container;
            _scope = _container.BeginScope();
        }

        public override object Resolve(Type jobType)
        {
            Context.InitializeFakeHttpContext();

            var instance = _container.TryGetInstance(jobType);

            // since it fails we can try to get the first interface and request from container
            if (instance == null && jobType.GetInterfaces().Any())
                instance = _container.GetInstance(jobType.GetInterfaces().FirstOrDefault());

            return instance;
        }

        public override void DisposeScope()
        {
            Context.InitializeFakeHttpContext();
            _scope?.Dispose();
        }
    }

    internal static class Context
    {
        public static void InitializeFakeHttpContext()
        {
            // IMPORTANT: HACK to create fake http context for job to allow the LightInject PerWebRequestScopeManager to work correctly when running in background jobs
            // Umbraco is hardcoded to using MixedLightInjectScopeManagerProvider so its really really hard to get around so this hack is the easiest way to handle this.
            if (HttpContext.Current == null)
            {
                HttpContext.Current = new HttpContext(
                    new HttpRequest("PerWebRequestScopeManager", "https://localhost/PerWebRequestScopeManager",
                        string.Empty),
                    new HttpResponse(new StringWriter()));
            }
        }
    }
}