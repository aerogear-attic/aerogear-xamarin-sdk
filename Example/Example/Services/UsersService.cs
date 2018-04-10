using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Http;
using Example.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Example.Services
{
    class UsersService
    {
        public async Task<List<PlaceholderUser>> GetUsers()
        {
            var httpLayer = MobileCore.Instance.HttpLayer;

            var request = httpLayer.NewRequest();
            Stopwatch sw = new Stopwatch();
            var url = "https://jsonplaceholder.typicode.com/users";
            sw.Start();
            var response = await request.Get(url).Execute();
            sw.Stop();
            if (response.Successful)
            {
                return JsonConvert.DeserializeObject<List<PlaceholderUser>>(response.Body);
            }
            else
            {
                MobileCore.Instance.Logger.Error($"failed to get users from {url}", response.Error);
                return null;
            }
        }
    }
}
