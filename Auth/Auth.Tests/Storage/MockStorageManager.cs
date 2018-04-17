using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Storage;

namespace Auth.Tests.Storage
{
	public class MockStorageManager : IStorageManager
    {
		private readonly Dictionary<string, string> Store;

        public MockStorageManager()
        {
			Store = new Dictionary<string, string>();
        }

		public string Read(string key)
		{
			return Store.GetValueOrDefault(key, null);
		}

		public void Remove(string key)
		{
			Store.Remove(key);
		}

		public void Save(string key, string value)
		{
			Store.Add(key, value);
		}
	}
}
