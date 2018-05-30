using System;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Metrics
{
    public interface IMetricsService : IServiceModule
    {
        Task SendAppAndDeviceMetrics();
        Task Publish(string type, params IMetrics[] metrics);
    }
}
