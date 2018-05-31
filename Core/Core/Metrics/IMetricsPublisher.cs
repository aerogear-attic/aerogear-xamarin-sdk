using System;
using System.Json;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Metrics
{
    public interface IMetricsPublisher
    {
        Task Publish(string type, IMetrics[] metrics);
    }
}
