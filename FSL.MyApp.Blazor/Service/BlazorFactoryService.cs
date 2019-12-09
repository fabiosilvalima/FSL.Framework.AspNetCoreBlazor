using FSL.Framework.Core.Factory.Service;
using System;

namespace FSL.MyApp.Blazor.Service
{
    public class BlazorFactoryService : IFactoryService
    {
        private readonly IServiceProvider _serviceProvider;

        public BlazorFactoryService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T InstanceOf<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}
