using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AuthException = AeroGear.Mobile.Auth.Authenticator.AuthorizationException;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class TokenLifecycleManager
    {
        public TokenLifecycleManager()
        {
        }

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
                throw AuthException.fromTemplate(AuthException.GeneralErrors.NETWORK_ERROR, e);
            }
            catch (ArgumentException e)
            {
                throw AuthException.fromTemplate(AuthException.GeneralErrors.JSON_DESERIALIZATION_ERROR, e);
            }
            catch (Exception e)
            {
                throw AuthException.fromTemplate(AuthException.GeneralErrors.NETWORK_ERROR, e);
            }

            if (json.ContainsKey(AuthException.PARAM_ERROR))
            {
                AuthException ex;
                try
                {
                    string error = json[AuthException.PARAM_ERROR];
                    ex = AuthException.fromOAuthTemplate(
                            AuthException.TokenRequestErrors.byString(error), error,
                        json.ContainsKey(AuthException.PARAM_ERROR_DESCRIPTION) ? json[AuthException.PARAM_ERROR_DESCRIPTION] : null,
                        parseUriIfAvailable(json.ContainsKey(AuthException.PARAM_ERROR_URI) ? json[AuthException.PARAM_ERROR_URI] : null));
                }
                catch (Exception e)
                {
                    ex = AuthException.fromTemplate(AuthException.GeneralErrors.JSON_DESERIALIZATION_ERROR, e);
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
