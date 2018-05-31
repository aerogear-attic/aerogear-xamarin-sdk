using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;

namespace Auth.Tests.Storage
{
    public class MockStorageManager : IUserPreferences
    {
        private readonly Dictionary<string, string> Store;

        public MockStorageManager()
        {
            Store = new Dictionary<string, string>();
        }

        public string GetString(string key, string defaultValue = null)
        {
            return Store.GetValueOrDefault(key, null);
        }

        public void RemoveValue(string key)
        {
            Store.Remove(key);
        }

        public void PutString(string key, string value)
        {
            Store.Add(key, value);
        }
    }
}
