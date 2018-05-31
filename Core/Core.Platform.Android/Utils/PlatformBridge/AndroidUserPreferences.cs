using System;
using Android.Content;

namespace AeroGear.Mobile.Core.Utils
{
    public class AndroidUserPreferences : AbstractUserPreferences
    {
        private readonly ISharedPreferences sharedPreferences;

        public AndroidUserPreferences(Context appContext, string storageName)
        {
            sharedPreferences = appContext.GetSharedPreferences(storageName, FileCreationMode.Private);
        }

        protected override string doGetString(string key, string defaultValue = null)
        {
            return sharedPreferences.GetString(key, defaultValue);
        }

        protected override void doPutString(string key, string value)
        {
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.PutString(key, value);
            editor.Apply();
        }

        protected override void doRemoveValue(string key)
        {
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.Remove(key);
            editor.Apply();
        }
    }
}
