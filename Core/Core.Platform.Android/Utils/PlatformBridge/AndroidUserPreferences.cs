using System;
using Android.Content;

namespace AeroGear.Mobile.Core.Utils
{
    public class AndroidUserPreferences : IUserPreferences
    {
        private readonly ISharedPreferences sharedPreferences;

        public AndroidUserPreferences(Context appContext, string storageName)
        {
            sharedPreferences = appContext.GetSharedPreferences(storageName, FileCreationMode.Private);
        }

        public string GetString(string key, string defaultValue = null)
        {
            return sharedPreferences.GetString(key, defaultValue);
        }

        public void PutString(string key, string value)
        {
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.PutString(key, value);
            editor.Apply();
        }
    }
}
