using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class AuthHeaderProvider
    {
        public const String HEADER_TYPE = "Bearer ";
        public const String HEADER_KEY = "Authorization";

        private readonly IAuthService authService;

        private readonly IDictionary<String, String> EmptyDictionary = new Dictionary<String, String>();

        public AuthHeaderProvider(IAuthService authService)
        {
            this.authService = authService;
        }

        public IDictionary<String, String> GetHeaders()
        {
            var user = authService.CurrentUser();

            if (user != null && user.AccessToken != null)
            {
                return new Dictionary<String, String> { { HEADER_KEY, HEADER_TYPE + user.AccessToken } };
            }
            return EmptyDictionary;
        }
    }
}
