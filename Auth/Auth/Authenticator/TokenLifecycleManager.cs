using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Auth.Authenticator
{
    /// <summary>
    /// Class for managing the toke lifecycle.
    /// </summary>
    public class TokenLifecycleManager
    {
        public TokenLifecycleManager()
        {
        }

        /// <summary>
        /// Performs a token refresh.
        /// </summary>
        /// <returns>The JSOn answer received by the server.</returns>
        /// <param name="url">The url of the openid serevr</param>
        /// <param name="clientId">Client identifier.</param>
        /// <param name="refreshToken">Refresh token.</param>
        public async Task<JsonValue> RefreshAsync(String url, String clientId, String refreshToken) 
        {
            JsonValue json = null;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var parameters = new Dictionary<string, string> { { "scope", "openid" }, { "refresh_token", refreshToken }, { "grant_type", "refresh_token" } };

                var clientAuthParams = new Dictionary<string, string> { { "client_id", clientId } };
                if (clientAuthParams != null)
                {
                    foreach (KeyValuePair<string, string> clientParam in clientAuthParams)
                    {
                        parameters.Add(clientParam.Key, clientParam.Value);
                    }
                }

                var requestBody = new FormUrlEncodedContent(parameters);
                var responseMessage = await client.PostAsync(url, requestBody).ConfigureAwait(false);
                var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                json = JsonValue.Parse(responseString);
            }
            catch (IOException e)
            {
                throw AuthzException.fromTemplate(AuthzException.GeneralErrors.NETWORK_ERROR, e);
            }
            catch (ArgumentException e)
            {
                throw AuthzException.fromTemplate(AuthzException.GeneralErrors.JSON_DESERIALIZATION_ERROR, e);
            }
            catch (Exception e)
            {
                throw AuthzException.fromTemplate(AuthzException.GeneralErrors.NETWORK_ERROR, e);
            }

            if (json.ContainsKey(AuthzException.PARAM_ERROR))
            {
                AuthzException ex;
                try
                {
                    string error = json[AuthzException.PARAM_ERROR];
                    ex = AuthzException.fromOAuthTemplate(
                            AuthzException.TokenRequestErrors.byString(error), error,
                        json.ContainsKey(AuthzException.PARAM_ERROR_DESCRIPTION) ? json[AuthzException.PARAM_ERROR_DESCRIPTION] : null,
                        parseUriIfAvailable(json.ContainsKey(AuthzException.PARAM_ERROR_URI) ? json[AuthzException.PARAM_ERROR_URI] : null));
                }
                catch (Exception e)
                {
                    ex = AuthzException.fromTemplate(AuthzException.GeneralErrors.JSON_DESERIALIZATION_ERROR, e);
                }

                throw ex;
            }

            return json;
        }

        private static Uri parseUriIfAvailable(String uri)
        {
            if (uri == null)
            {
                return null;
            }

            return new Uri(uri);
        }
    }
}
