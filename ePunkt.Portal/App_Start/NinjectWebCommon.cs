using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ePunkt.Portal.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(ePunkt.Portal.NinjectWebCommon), "Stop")]

namespace ePunkt.Portal
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<CustomSettings>().ToMethod(context => LoadCustomSettings());
            kernel.Bind<ApiHttpClient>().ToMethod(context => GetHttpClient());
        }

        private static ApiHttpClient GetHttpClient()
        {
            var customSettings = LoadCustomSettings();
            var apiClient = new ApiHttpClient(new Uri(customSettings.ApiEndpoint), LoadApiKey, () => _tokenCache)
                {
                    Timeout = TimeSpan.FromSeconds(500) //increase the timeout significantly to 500s (default is 100s), to compensate for our lame staging server upstream
                };

            return apiClient;
        }

        private static readonly ApiTokenCache _tokenCache = new ApiTokenCache();

        private static CustomSettings LoadCustomSettings()
        {
            return new CustomSettingsService().LoadCustomSettings(HttpContext.Current);
        }

        private static ApiKey LoadApiKey()
        {
            var customSettings = LoadCustomSettings();
            if (customSettings == null)
                throw new Exception("No custom settings found.");
            return new ApiKey
                {
                    ClientInfo = "ePunkt.Portal v" + typeof (NinjectWebCommon).Assembly.GetName().Version,
                    Key = customSettings.ApiKey,
                    MandatorId = customSettings.MandatorId
                };
        }
    }
}
