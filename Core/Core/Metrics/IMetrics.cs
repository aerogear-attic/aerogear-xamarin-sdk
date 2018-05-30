using System;
using System.Json;

namespace AeroGear.Mobile.Core.Metrics
{
    public interface IMetrics
    {
        string Identifier();

        JsonObject Data();
    }
}
