using System;
namespace AeroGear.Mobile.Core.Metrics
{
    public interface IMetrics<T>
    {
        string Identifier();

        T Data();
    }
}
