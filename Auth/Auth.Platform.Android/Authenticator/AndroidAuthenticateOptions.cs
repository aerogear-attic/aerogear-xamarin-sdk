using System;
using Android.App;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class AndroidAuthenticateOptions : IAuthenticateOptions
    {
        /// <summary>
        /// The activity that the authentication request is triggered from
        /// </summary>
        /// <value>an instance of <see cref="Android.App.Activity"></value>
        public Activity FromActvity { get; }

        /// <summary>
        /// A unique integer that can be used to identity the authentication request in the fromActivity's <see cref="Android.App.Activity.OnActivityResult(int, Result, Android.Content.Intent)"> method
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Auth.Authenticator.AndroidAuthenticateOptions"/> class.
        /// </summary>
        /// <param name="from">tha activity that triggers the authentication request</param>
        /// <param name="resultCode">an integer to identify the authentication request</param>
        public AndroidAuthenticateOptions(Activity from, int resultCode)
        {
            this.FromActvity = NonNull(from, "from activity");
            this.ResultCode = NonNull(resultCode, "result code");
        }
    }
}
