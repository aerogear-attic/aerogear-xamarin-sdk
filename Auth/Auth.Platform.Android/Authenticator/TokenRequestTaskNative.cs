using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenId.AppAuth;
using Org.Json;
using AuthException = AeroGear.Mobile.Auth.Authenticator.AuthorizationException;

namespace Auth.Platform.Authenticator
{
    public class TokenRequestTaskNative
    {
        private readonly TokenRequest tokenRequest;

        public TokenRequestTaskNative(TokenRequest request)
        {
            this.tokenRequest = request;
        }

        private HttpWebRequest openConnection(string url)
        {

            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Timeout = 15000;
            webReq.ReadWriteTimeout = 10000;
            webReq.AllowAutoRedirect = false;

            return webReq;
        }


        public async Task<TokenResponse> RequestAsync()
        {
            JsonValue json = null;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var parameters = new Dictionary<string, string> { { "scope", "openid" }, { "refresh_token", tokenRequest.RefreshToken }, { "grant_type", "refresh_token" } };

                var clientAuthParams = new Dictionary<string, string> { { "client_id", tokenRequest.ClientId } };
                if (clientAuthParams != null)
                {
                    foreach (KeyValuePair<string, string> clientParam in clientAuthParams)
                    {
                        parameters.Add(clientParam.Key, clientParam.Value);
                    }
                }

                var requestBody = new FormUrlEncodedContent(parameters);

                var responseMessage = await client.PostAsync(tokenRequest.Configuration.TokenEndpoint.ToString(), requestBody).ConfigureAwait(false);
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

            TokenResponse response = null;
            try
            {
                response = new TokenResponse.Builder(tokenRequest).FromResponseJson(new JSONObject(json.ToString())).Build();
                //response = new TokenResponse.Builder(tokenRequest).FromResponseJson(json).Build();
            }
            catch (Exception je)
            {
                throw AuthException.fromTemplate(
                    AuthException.GeneralErrors.JSON_DESERIALIZATION_ERROR, je);
            }

            return response;
        }

        public TokenResponse RequestSync()
        {
            return RequestAsync().GetAwaiter().GetResult();
        }

        private String urlEncode(IDictionary<String, String> keyValuePairs)
        {
            String[] ary = new String[keyValuePairs.Count];
            int i = 0;
            foreach (var pair in keyValuePairs)
            {
                ary[i++] = $"{pair.Key}={pair.Value}";
            }

            return String.Join("&", ary);
        }

        private void addJsonToAcceptHeader(HttpWebRequest conn)
        {
            if (System.String.IsNullOrEmpty(conn.Accept))
            {
                conn.Accept = "application/json";
            }
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