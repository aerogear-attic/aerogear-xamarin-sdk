using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Java.IO;
using Java.Net;
using Net.Openid.Appauth.Internal;
using OpenId.AppAuth;
using Org.Json;

namespace Auth.Platform.Authenticator
{
    public class TokenRequestTask
    {
        private readonly TokenRequest tokenRequest;
        private readonly IClientAuthentication clientAuthentication;

        private readonly AppAuthConfiguration clientConfiguration = new AppAuthConfiguration.Builder().Build();

        public TokenRequestTask(TokenRequest request, IClientAuthentication clientAuthentication)
        {
            this.tokenRequest = request;
            this.clientAuthentication = clientAuthentication;
        }

        private TokenResponse request() {
            Stream inStream = null;
            JSONObject json = null;

            try {
                var conn = clientConfiguration.ConnectionBuilder.OpenConnection(tokenRequest.Configuration.TokenEndpoint);
                conn.RequestMethod = "POST";
                conn.SetRequestProperty("Content-Type", "application/x-www-form-urlencoded");
                addJsonToAcceptHeader(conn);
                conn.DoOutput = true;

                var headers = clientAuthentication.GetRequestHeaders(tokenRequest.ClientId);

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        conn.SetRequestProperty(header.Key, header.Value);
                    }
                }

                var parameters = tokenRequest.RequestParameters;
                var clientAuthParams = clientAuthentication.GetRequestParameters(tokenRequest.ClientId);

                if (clientAuthParams != null)
                {
                    foreach (KeyValuePair<string, string> clientParam in clientAuthParams)
                    {
                        parameters.Add(clientParam.Key, clientParam.Value);
                    }
                }

                String queryData = UriUtil.FormUrlEncode(parameters);
                conn.SetRequestProperty("Content-Length", queryData.Length.ToString());
                OutputStreamWriter wr = new OutputStreamWriter(conn.OutputStream);

                wr.Write(queryData);
                wr.Flush();

                if (conn.ResponseCode >= HttpURLConnection.HttpOk
                    && conn.ResponseCode < HttpURLConnection.HttpMultChoice)
                {
                    inStream = conn.InputStream;
                }
                else
                {
                    inStream = conn.ErrorStream;
                }

                json = new JSONObject(new StreamReader(inStream).ReadToEnd());
            }
            catch (Java.IO.IOException e) 
            {
                throw AuthorizationException.FromTemplate(AuthorizationException.GeneralErrors.NetworkError, e);
            }
            catch (JSONException je) {
                throw AuthorizationException.FromTemplate(AuthorizationException.GeneralErrors.JsonDeserializationError, je);
            }
            finally 
            {
                if (inStream != null)
                {
                    try { inStream.Close(); } catch (Exception) {}
                }
            }

            if (json.Has(AuthorizationException.ParamError)) {
                AuthorizationException ex;
                try
                {
                    string error = json.GetString(AuthorizationException.ParamError);
                    ex = AuthorizationException.FromOAuthTemplate(
                        AuthorizationException.TokenRequestErrors.ByString(error), error,
                        json.Has(AuthorizationException.ParamErrorDescription) ? json.GetString(AuthorizationException.ParamErrorDescription) : null,
                        UriUtil.ParseUriIfAvailable(json.Has(AuthorizationException.ParamErrorUri) ? json.GetString(AuthorizationException.ParamErrorUri) : null));
                }
                catch (JSONException e)
                {
                    ex = AuthorizationException.FromTemplate(
                        AuthorizationException.GeneralErrors.JsonDeserializationError, e);
                }

                throw ex;
            }

            TokenResponse response = null;
            try 
            {
                response = new TokenResponse.Builder(tokenRequest).FromResponseJson(json).Build();
            } catch (JSONException je) {
                throw AuthorizationException.FromTemplate(
                    AuthorizationException.GeneralErrors.JsonDeserializationError, je);
            }

            return response;
        }


        public TokenResponse RequestSync()
        {
            return RequestAsync().GetAwaiter().GetResult();
        }

        public Task<TokenResponse> RequestAsync()
        {
            return Task.Run(() =>
            {
                return request();
            });
        }


        void HandleAction()
        {
        }


        private void addJsonToAcceptHeader(HttpURLConnection conn)
        {
            if (System.String.IsNullOrEmpty(conn.GetRequestProperty("Accept")))
            {
                conn.SetRequestProperty("Accept", "application/json");
            }
        }
    }
}
