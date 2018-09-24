using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Authenticator;
using Foundation;
using OpenId.AppAuth;

namespace Auth.Platform.Authenticator.extensions
{
    /// <summary>
    /// Adds platform specific implementation to the TokenLifecycleManager class
    /// </summary>
    internal static class TokenLifecycleManagerExtensions
    {
        /// <summary>
        /// Performs the token refresh request returning a TokenResponse object.
        /// </summary>
        /// <returns>The token response (async)</returns>
        /// <param name="tlcm">The token lifecycle manager class.</param>
        /// <param name="tokenRequest">The request to be executed.</param>
        public async static Task<TokenResponse> RefreshTokenAsync(this TokenLifecycleManager tlcm, TokenRequest tokenRequest)
        {
            try
            {
                var json = await tlcm.RefreshAsync(tokenRequest.Configuration.TokenEndpoint.ToString(), tokenRequest.ClientId, tokenRequest.RefreshToken).ConfigureAwait(false);

                var parameters = new NSDictionary<NSString, NSCopying>(
                    new NSString[]{ new NSString("access_token"), new NSString("id_token"), new NSString("expires_in"), new NSString("token_type") }, 
                    new NSCopying[]{ new CopyableNSString(json["access_token"]), new CopyableNSString(json["id_token"]), new CopyableNSNumber(json["expires_in"]), new CopyableNSString(json["token_type"]) });

                return new TokenResponse(tokenRequest, parameters);
            }
            catch (AuthzException ae) 
            {
                throw ae;
            }
            catch (Exception je)
            {
                throw AuthzException.fromTemplate(
                    AuthzException.GeneralErrors.JSON_DESERIALIZATION_ERROR, je);
            }
        }
    }

    /// <summary>
    /// For some reason the NSString object provided by Xamarin does not implements the NSCopying protocol.
    /// This class is a simple wrapper for the NSString object implementing the NSCopying protocol.
    /// </summary>
    internal class CopyableNSString : NSCopying, INSCopying
    {
        private readonly String val;

        public CopyableNSString(String s)
        {
            this.val = s;
        }

        public override NSObject Copy(NSZone zone)
        {
            return new NSString(val);
        }
    }

    /// <summary>
    /// For some reason the NSNumber object provided by Xamarin does not implements the NSCopying protocol.
    /// This class is a simple wrapper for the NSNumber object implementing the NSCopying protocol.
    /// </summary>
    internal class CopyableNSNumber : NSCopying, INSCopying
    {
        private readonly long val;

        public CopyableNSNumber(long l)
        {
            this.val = l;
        }

        public override NSObject Copy(NSZone zone)
        {
            return new NSNumber(val);
        }
    }
}
