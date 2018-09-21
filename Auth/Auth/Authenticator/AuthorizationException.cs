using System;
using System.Collections.Generic;
using System.Json;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class AuthorizationException : Exception
    {

        /**
         * The extra string that used to store an {@link AuthorizationException} in an intent by
         * {@link #toIntent()}.
         */
        public const String EXTRA_EXCEPTION = "net.openid.appauth.AuthorizationException";

        /**
         * The OAuth2 parameter used to indicate the type of error during an authorization or
         * token request.
         *
         * @see "The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1
         * <https://tools.ietf.org/html/rfc6749#section-4.1.2.1>"
         * @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
         * <https://tools.ietf.org/html/rfc6749#section-5.2>"
         */
        public const String PARAM_ERROR = "error";

        /**
         * The OAuth2 parameter used to provide a human readable description of the error which
         * occurred.
         *
         * @see "The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1
         * <https://tools.ietf.org/html/rfc6749#section-4.1.2.1>"
         * @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
         * <https://tools.ietf.org/html/rfc6749#section-5.2>"
         */
        public const String PARAM_ERROR_DESCRIPTION = "error_description";

        /**
         * The OAuth2 parameter used to provide a URI to a human-readable page which describes the
         * error.
         *
         * @see "The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1
         * <https://tools.ietf.org/html/rfc6749#section-4.1.2.1>"
         * @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
         * <https://tools.ietf.org/html/rfc6749#section-5.2>"
         */
        public const String PARAM_ERROR_URI = "error_uri";


        /**
         * The error type used for all errors that are not specific to OAuth related responses.
         */
        public const int TYPE_GENERAL_ERROR = 0;

        /**
         * The error type for OAuth specific errors on the authorization endpoint. This error type is
         * used when the server responds to an authorization request with an explicit OAuth error, as
         * defined by [the OAuth2 specification, section 4.1.2.1](
         * https://tools.ietf.org/html/rfc6749#section-4.1.2.1). If the authorization response is
         * invalid and not explicitly an error response, another error type will be used.
         *
         * @see "The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1
         * <https://tools.ietf.org/html/rfc6749#section-4.1.2.1>"
         */
        public const int TYPE_OAUTH_AUTHORIZATION_ERROR = 1;

        /**
         * The error type for OAuth specific errors on the token endpoint. This error type is used when
         * the server responds with HTTP 400 and an OAuth error, as defined by
         * [the OAuth2 specification, section 5.2](https://tools.ietf.org/html/rfc6749#section-5.2).
         * If an HTTP 400 response does not parse as an OAuth error (i.e. no 'error' field is present
         * or the JSON is invalid), another error domain will be used.
         *
         * @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
         * <https://tools.ietf.org/html/rfc6749#section-5.2>"
         */
        public const int TYPE_OAUTH_TOKEN_ERROR = 2;

        /**
         * The error type for authorization errors encountered out of band on the resource server.
         */
        public const int TYPE_RESOURCE_SERVER_AUTHORIZATION_ERROR = 3;

        /**
         * The error type for OAuth specific errors on the registration endpoint.
         */
        public const int TYPE_OAUTH_REGISTRATION_ERROR = 4;


        const String KEY_TYPE = "type";


        const String KEY_CODE = "code";


        const String KEY_ERROR = "error";


        const String KEY_ERROR_DESCRIPTION = "errorDescription";


        const String KEY_ERROR_URI = "errorUri";

        /**
         * Prime number multiplier used to produce a reasonable hash value distribution.
         */
        private const int HASH_MULTIPLIER = 31;

        /**
         * Error codes related to failed authorization requests.
         *
         * @see "The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1
         * <https://tools.ietf.org/html/rfc6749#section-4.1.2.1>"
         */
        public sealed class AuthorizationRequestErrors
        {
            // codes in this group should be between 1000-1999

            /**
             * An `invalid_request` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_REQUEST = authEx(1000, "invalid_request");

            /**
             * An `unauthorized_client` OAuth2 error response.
             */
            public static readonly AuthorizationException UNAUTHORIZED_CLIENT = authEx(1001, "unauthorized_client");

            /**
             * An `access_denied` OAuth2 error response.
             */
            public static readonly AuthorizationException ACCESS_DENIED =
                    authEx(1002, "access_denied");

            /**
             * An `unsupported_response_type` OAuth2 error response.
             */
            public static readonly AuthorizationException UNSUPPORTED_RESPONSE_TYPE =
                    authEx(1003, "unsupported_response_type");

            /**
             * An `invalid_scope` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_SCOPE =
                    authEx(1004, "invalid_scope");

            /**
             * An `server_error` OAuth2 error response, equivalent to an HTTP 500 error code, but
             * sent via redirect.
             */
            public static readonly AuthorizationException SERVER_ERROR =
                    authEx(1005, "server_error");

            /**
             * A `temporarily_unavailable` OAuth2 error response, equivalent to an HTTP 503 error
             * code, but sent via redirect.
             */
            public static readonly AuthorizationException TEMPORARILY_UNAVAILABLE =
                    authEx(1006, "temporarily_unavailable");

            /**
             * An authorization error occurring on the client rather than the server. For example,
             * due to client misconfiguration. This error should be treated as unrecoverable.
             */
            public static readonly AuthorizationException CLIENT_ERROR =
                    authEx(1007, null);

            /**
             * Indicates an OAuth error as per RFC 6749, but the error code is not known to the
             * AppAuth for Android library. It could be a custom error or code, or one from an
             * OAuth extension. The {@link #error} field provides the exact error string returned by
             * the server.
             */
            public static readonly AuthorizationException OTHER =
                    authEx(1008, null);

            /**
             * Indicates that the response state param did not match the request state param,
             * resulting in the response being discarded.
             */
            public static readonly AuthorizationException STATE_MISMATCH =
                    generalEx(9, "Response state param did not match request state");

            private static readonly IDictionary<String, AuthorizationException> STRING_TO_EXCEPTION =
                    exceptionMapByString(
                            INVALID_REQUEST,
                            UNAUTHORIZED_CLIENT,
                            ACCESS_DENIED,
                            UNSUPPORTED_RESPONSE_TYPE,
                            INVALID_SCOPE,
                            SERVER_ERROR,
                            TEMPORARILY_UNAVAILABLE,
                            CLIENT_ERROR,
                            OTHER);

            /**
             * Returns the matching exception type for the provided OAuth2 error string, or
             * {@link #OTHER} if unknown.
             */

            public static AuthorizationException byString(String error)
            {
                AuthorizationException ex;
                if (STRING_TO_EXCEPTION.TryGetValue(error, out ex))
                {
                    return ex;
                }

                //AuthorizationException ex = STRING_TO_EXCEPTION.get(error);
                //if (ex != null)
                //{
                //    return ex;
                //}
                return OTHER;
            }
        }

        /**
         * Error codes related to failed token requests.
         *
         * @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
         * <https://tools.ietf.org/html/rfc6749#section-5.2>"
         */
        public sealed class TokenRequestErrors
        {
            // codes in this group should be between 2000-2999

            /**
             * An `invalid_request` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_REQUEST =
                    tokenEx(2000, "invalid_request");

            /**
             * An `invalid_client` OAuth2 error response.
             */
            public static AuthorizationException INVALID_CLIENT =
                    tokenEx(2001, "invalid_client");

            /**
             * An `invalid_grant` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_GRANT =
                    tokenEx(2002, "invalid_grant");

            /**
             * An `unauthorized_client` OAuth2 error response.
             */
            public static readonly AuthorizationException UNAUTHORIZED_CLIENT =
                    tokenEx(2003, "unauthorized_client");

            /**
             * An `unsupported_grant_type` OAuth2 error response.
             */
            public static readonly AuthorizationException UNSUPPORTED_GRANT_TYPE =
                    tokenEx(2004, "unsupported_grant_type");

            /**
             * An `invalid_scope` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_SCOPE =
                    tokenEx(2005, "invalid_scope");

            /**
             * An authorization error occurring on the client rather than the server. For example,
             * due to client misconfiguration. This error should be treated as unrecoverable.
             */
            public static readonly AuthorizationException CLIENT_ERROR =
                    tokenEx(2006, null);

            /**
             * Indicates an OAuth error as per RFC 6749, but the error code is not known to the
             * AppAuth for Android library. It could be a custom error or code, or one from an
             * OAuth extension. The {@link #error} field provides the exact error string returned by
             * the server.
             */
            public static readonly AuthorizationException OTHER =
                    tokenEx(2007, null);

            private static readonly IDictionary<String, AuthorizationException> STRING_TO_EXCEPTION =
                    exceptionMapByString(
                            INVALID_REQUEST,
                            INVALID_CLIENT,
                            INVALID_GRANT,
                            UNAUTHORIZED_CLIENT,
                            UNSUPPORTED_GRANT_TYPE,
                            INVALID_SCOPE,
                            CLIENT_ERROR,
                            OTHER);

            /**
             * Returns the matching exception type for the provided OAuth2 error string, or
             * {@link #OTHER} if unknown.
             */
            public static AuthorizationException byString(String error)
            {
                AuthorizationException ex;
                if (STRING_TO_EXCEPTION.TryGetValue(error, out ex))
                {
                    return ex;
                }
                
                return OTHER;
            }
        }

        /**
         * Error codes related to failed registration requests.
         */
        public sealed class RegistrationRequestErrors
        {
            // codes in this group should be between 4000-4999

            /**
             * An `invalid_request` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_REQUEST =
                    registrationEx(4000, "invalid_request");

            /**
             * An `invalid_client` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_REDIRECT_URI =
                    registrationEx(4001, "invalid_redirect_uri");

            /**
             * An `invalid_grant` OAuth2 error response.
             */
            public static readonly AuthorizationException INVALID_CLIENT_METADATA =
                    registrationEx(4002, "invalid_client_metadata");

            /**
             * An authorization error occurring on the client rather than the server. For example,
             * due to client misconfiguration. This error should be treated as unrecoverable.
             */
            public static readonly AuthorizationException CLIENT_ERROR =
                    registrationEx(4003, null);

            /**
             * Indicates an OAuth error as per RFC 6749, but the error code is not known to the
             * AppAuth for Android library. It could be a custom error or code, or one from an
             * OAuth extension. The {@link #error} field provides the exact error string returned by
             * the server.
             */
            public static readonly AuthorizationException OTHER =
                    registrationEx(4004, null);

            private static readonly IDictionary<String, AuthorizationException> STRING_TO_EXCEPTION =
                    exceptionMapByString(
                            INVALID_REQUEST,
                            INVALID_REDIRECT_URI,
                            INVALID_CLIENT_METADATA,
                            CLIENT_ERROR,
                            OTHER);

            /**
             * Returns the matching exception type for the provided OAuth2 error string, or
             * {@link #OTHER} if unknown.
             */
            public static AuthorizationException byString(String error)
            {
                AuthorizationException ex;
                if (STRING_TO_EXCEPTION.TryGetValue(error, out ex))
                {
                    return ex;
                }
                return OTHER;
            }
        }

        public sealed class GeneralErrors
        {
            // codes in this group should be between 0-999

            /**
             * Indicates a problem parsing an OpenID Connect Service Discovery document.
             */
            public static readonly AuthorizationException INVALID_DISCOVERY_DOCUMENT =
                generalEx(0, "Invalid discovery document");

            /**
             * Indicates the user manually canceled the OAuth authorization code flow.
             */
            public static readonly AuthorizationException USER_CANCELED_AUTH_FLOW =
                generalEx(1, "User cancelled flow");

            /**
             * Indicates an OAuth authorization flow was programmatically cancelled.
             */
            public static readonly AuthorizationException PROGRAM_CANCELED_AUTH_FLOW =
                generalEx(2, "Flow cancelled programmatically");

            /**
             * Indicates a network error occurred.
             */
            public static readonly AuthorizationException NETWORK_ERROR =
                generalEx(3, "Network error");

            /**
             * Indicates a server error occurred.
             */
            public static readonly AuthorizationException SERVER_ERROR =
                generalEx(4, "Server error");

            /**
             * Indicates a problem occurred deserializing JSON.
             */
            public static readonly AuthorizationException JSON_DESERIALIZATION_ERROR =
                generalEx(5, "JSON deserialization error");

            /**
             * Indicates a problem occurred constructing a {@link TokenResponse token response} object
             * from the JSON provided by the server.
             */
            public static readonly AuthorizationException TOKEN_RESPONSE_CONSTRUCTION_ERROR =
                generalEx(6, "Token response construction error");

            /**
             * Indicates a problem parsing an OpenID Connect Registration Response.
             */
            public static readonly AuthorizationException INVALID_REGISTRATION_RESPONSE =
                generalEx(7, "Invalid registration response");

            /**
             * Indicates that a received ID token could not be parsed
             */
            public static readonly AuthorizationException ID_TOKEN_PARSING_ERROR =
                generalEx(8, "Unable to parse ID Token");

            /**
             * Indicates that a received ID token is invalid
             */
            public static readonly AuthorizationException ID_TOKEN_VALIDATION_ERROR =
                generalEx(9, "Invalid ID Token");
        }

        private static AuthorizationException generalEx(int code, String errorDescription)
        {
            return new AuthorizationException(
                    TYPE_GENERAL_ERROR, code, null, errorDescription, null, null);
        }

        private static AuthorizationException authEx(int code, String error)
        {
            return new AuthorizationException(
                    TYPE_OAUTH_AUTHORIZATION_ERROR, code, error, null, null, null);
        }

        private static AuthorizationException tokenEx(int code, String error)
        {
            return new AuthorizationException(
                    TYPE_OAUTH_TOKEN_ERROR, code, error, null, null, null);
        }

        private static AuthorizationException registrationEx(int code, String error)
        {
            return new AuthorizationException(
                    TYPE_OAUTH_REGISTRATION_ERROR, code, error, null, null, null);
        }

        /**
         * Creates an exception based on one of the existing values defined in
         * {@link GeneralErrors}, {@link AuthorizationRequestErrors} or {@link TokenRequestErrors},
         * providing a root cause.
         */
        public static AuthorizationException fromTemplate(
                AuthorizationException ex,
                Exception rootCause)
        {
            return new AuthorizationException(
                    ex.type,
                    ex.code,
                    ex.error,
                    ex.errorDescription,
                    ex.errorUri,
                    rootCause);
        }

        /**
         * Creates an exception based on one of the existing values defined in
         * {@link AuthorizationRequestErrors} or {@link TokenRequestErrors}, adding information
         * retrieved from OAuth error response.
         */
        public static AuthorizationException fromOAuthTemplate(
                AuthorizationException ex,
                String errorOverride,
                String errorDescriptionOverride,
                Uri errorUriOverride)
        {
            return new AuthorizationException(
                    ex.type,
                    ex.code,
                    (errorOverride != null) ? errorOverride : ex.error,
                    (errorDescriptionOverride != null) ? errorDescriptionOverride : ex.errorDescription,
                    (errorUriOverride != null) ? errorUriOverride : ex.errorUri,
                    null);
        }

        private static IDictionary<String, AuthorizationException> exceptionMapByString(
                params AuthorizationException[] exceptions)
        {
            IDictionary<String, AuthorizationException> map =
                    new Dictionary<String, AuthorizationException>(exceptions != null ? exceptions.Length : 0);

            if (exceptions != null)
            {
                foreach (AuthorizationException ex in exceptions)
                {
                    if (ex.error != null)
                    {
                        map.Add(ex.error, ex);
                    }
                }
            }

            return map;
            //FIXME: return Collections.unmodifiableMap(map);
        }

        /**
         * The type of the error.
         * @see #TYPE_GENERAL_ERROR
         * @see #TYPE_OAUTH_AUTHORIZATION_ERROR
         * @see #TYPE_OAUTH_TOKEN_ERROR
         * @see #TYPE_RESOURCE_SERVER_AUTHORIZATION_ERROR
         */
        public readonly int type;

        /**
         * The error code describing the class of problem encountered from the set defined in this
         * class.
         */
        public readonly int code;

        /**
         * The error string as it is found in the OAuth2 protocol.
         */

        public readonly String error;

        /**
         * The human readable error message associated with this exception, if available.
         */

        public readonly String errorDescription;

        /**
         * A URI identifying a human-readable web page with information about this error.
         */

        public readonly Uri errorUri;

        /**
         * Instantiates an authorization request with optional root cause information.
         */
        public AuthorizationException(
                int type,
                int code,
                String error,
                String errorDescription,
                Uri errorUri,
            Exception rootCause) : base(errorDescription, rootCause)
        {
            this.type = type;
            this.code = code;
            this.error = error;
            this.errorDescription = errorDescription;
            this.errorUri = errorUri;
        }

        /**
         * Produces a JSON representation of the authorization exception, for transmission or storage.
         * This does not include any provided root cause.
         */
        public JsonObject toJson()
        {
            JsonObject json = new JsonObject();
            json[KEY_TYPE] = type;
            json[KEY_CODE] = code;
            if (error != null) json[KEY_ERROR] = error;
            if (errorDescription != null) json[KEY_ERROR_DESCRIPTION] = errorDescription;
            if (errorUri != null) json[KEY_ERROR_URI] = errorUri;
            //JsonUtil.put(json, KEY_TYPE, type);
            //JsonUtil.put(json, KEY_CODE, code);
            //JsonUtil.putIfNotNull(json, KEY_ERROR, error);
            //JsonUtil.putIfNotNull(json, KEY_ERROR_DESCRIPTION, errorDescription);
            //JsonUtil.putIfNotNull(json, KEY_ERROR_URI, errorUri);
            return json;
        }

        /**
         * Provides a JSON string representation of an authorization exception, for transmission or
         * storage. This does not include any provided root cause.
         */

        public String toJsonString()
        {
            return toJson().ToString();
        }

        public String toString()
        {
            return "AuthorizationException: " + toJsonString();
        }
    }

}
