using System;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Metrics
{
    public interface IMetricsService : IServiceModule
    {
        Task SendAppAndDeviceMetrics();
        Task Publish<T>(string type, params IMetrics<T>[] metrics);
    }
}
