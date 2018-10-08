using System;
using System.Collections.Generic;
using System.Json;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class AuthzException : Exception
    {
        /// <summary>
        /// The OAuth2 parameter used to indicate the type of error during an authorization or
        /// token request.
        /// 
        /// <see href="https://tools.ietf.org/html/rfc6749#section-4.1.2.1">The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1</see>
        /// <see href="https://tools.ietf.org/html/rfc6749#section-5.2">The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2</see>
        /// </summary>
        public const String PARAM_ERROR = "error";

        /// <summary>
        /// The OAuth2 parameter used to provide a human readable description of the error which
        /// occurred.
        /// 
        /// <see href="https://tools.ietf.org/html/rfc6749#section-4.1.2.1">The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1</see>
        /// <see href="https://tools.ietf.org/html/rfc6749#section-5.2">The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2</see>
        /// </summary>
        public const String PARAM_ERROR_DESCRIPTION = "error_description";

        /// <summary>
        /// The OAuth2 parameter used to provide a URI to a human-readable page which describes the
        /// error.
        ///
        /// <see href="https://tools.ietf.org/html/rfc6749#section-4.1.2.1">The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1</see>
        /// <see href="https://tools.ietf.org/html/rfc6749#section-5.2">The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2</see>
        /// </summary>
        public const String PARAM_ERROR_URI = "error_uri";


        /// <summary>
        /// The error type used for all errors that are not specific to OAuth related responses.
        /// </summary>
        public const int TYPE_GENERAL_ERROR = 0;

        /// <summary>
        /// The error type for OAuth specific errors on the authorization endpoint. This error type is
        /// used when the server responds to an authorization request with an explicit OAuth error, as
        /// defined by [the OAuth2 specification, section 4.1.2.1](
        /// https://tools.ietf.org/html/rfc6749#section-4.1.2.1). If the authorization response is
        /// invalid and not explicitly an error response, another error type will be used.
        ///
        /// <see href="https://tools.ietf.org/html/rfc6749#section-4.1.2.1">The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1</see>
        /// </summary>
        public const int TYPE_OAUTH_AUTHORIZATION_ERROR = 1;

        /// <summary>
        /// The error type for OAuth specific errors on the token endpoint. This error type is used when
        /// the server responds with HTTP 400 and an OAuth error, as defined by
        /// [the OAuth2 specification, section 5.2](https://tools.ietf.org/html/rfc6749#section-5.2).
        /// If an HTTP 400 response does not parse as an OAuth error (i.e. no 'error' field is present
        /// or the JSON is invalid), another error domain will be used.
        ///
        /// <see href="https://tools.ietf.org/html/rfc6749#section-5.2">The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2</see>
        /// </summary>
        public const int TYPE_OAUTH_TOKEN_ERROR = 2;

        /// <summary>
        /// The error type for authorization errors encountered out of band on the resource server.
        /// </summary>
        public const int TYPE_RESOURCE_SERVER_AUTHORIZATION_ERROR = 3;

        /// <summary>
        /// The error type for OAuth specific errors on the registration endpoint.
        /// </summary>
        public const int TYPE_OAUTH_REGISTRATION_ERROR = 4;


        const String KEY_TYPE = "type";


        const String KEY_CODE = "code";


        const String KEY_ERROR = "error";


        const String KEY_ERROR_DESCRIPTION = "errorDescription";


        const String KEY_ERROR_URI = "errorUri";

        /// <summary>
        /// Prime number multiplier used to produce a reasonable hash value distribution.
        /// </summary>
        private const int HASH_MULTIPLIER = 31;

        /// <summary>
        /// Error codes related to failed authorization requests.
        ///
        /// <see href="https://tools.ietf.org/html/rfc6749#section-4.1.2.1">The OAuth 2.0 Authorization Framework (RFC 6749), Section 4.1.2.1</see>
        /// </summary>
        public sealed class AuthorizationRequestErrors
        {
            // codes in this group should be between 1000-1999

            /// <summary>
            /// An `invalid_request` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_REQUEST = authEx(1000, "invalid_request");

            /// <summary>
            /// An `unauthorized_client` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException UNAUTHORIZED_CLIENT = authEx(1001, "unauthorized_client");

            /// <summary>
            /// An `access_denied` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException ACCESS_DENIED =
                    authEx(1002, "access_denied");

            /// <summary>
            /// An `unsupported_response_type` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException UNSUPPORTED_RESPONSE_TYPE =
                    authEx(1003, "unsupported_response_type");

            /// <summary>
            /// An `invalid_scope` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_SCOPE =
                    authEx(1004, "invalid_scope");

            /// <summary>
            /// An `server_error` OAuth2 error response, equivalent to an HTTP 500 error code, but
            /// sent via redirect.
            /// </summary>
            public static readonly AuthzException SERVER_ERROR =
                    authEx(1005, "server_error");

            /// <summary>
            /// A `temporarily_unavailable` OAuth2 error response, equivalent to an HTTP 503 error
            /// code, but sent via redirect.
            /// </summary>
            public static readonly AuthzException TEMPORARILY_UNAVAILABLE =
                    authEx(1006, "temporarily_unavailable");

            /// <summary>
            /// An authorization error occurring on the client rather than the server. For example,
            /// due to client misconfiguration. This error should be treated as unrecoverable.
            /// </summary>
            public static readonly AuthzException CLIENT_ERROR =
                    authEx(1007, null);

            /// <summary>
            /// Indicates an OAuth error as per RFC 6749, but the error code is not known to the
            /// AppAuth for Android library. It could be a custom error or code, or one from an
            /// OAuth extension. The {@link #error} field provides the exact error string returned by
            /// the server.
            /// </summary>
            public static readonly AuthzException OTHER =
                    authEx(1008, null);

            /// <summary>
            /// Indicates that the response state param did not match the request state param,
            /// resulting in the response being discarded.
            /// </summary>
            public static readonly AuthzException STATE_MISMATCH =
                    generalEx(9, "Response state param did not match request state");

            private static readonly IDictionary<String, AuthzException> STRING_TO_EXCEPTION =
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

            /// <summary>
            /// Returns the matching exception type for the provided OAuth2 error string, or
            /// OTHER if unknown.
            /// </summary>

            public static AuthzException byString(String error)
            {
                AuthzException ex;
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

        /// <summary>
        /// Error codes related to failed token requests.
        ///
        /// @see "The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2
        /// <see href="https://tools.ietf.org/html/rfc6749#section-5.2">The OAuth 2.0 Authorization Framework" (RFC 6749), Section 5.2</see>
        /// </summary>
        public sealed class TokenRequestErrors
        {
            // codes in this group should be between 2000-2999

            /// <summary>
            /// An `invalid_request` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_REQUEST =
                    tokenEx(2000, "invalid_request");

            /// <summary>
            /// An `invalid_client` OAuth2 error response.
            /// </summary>
            public static AuthzException INVALID_CLIENT =
                    tokenEx(2001, "invalid_client");

            /// <summary>
            /// An `invalid_grant` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_GRANT =
                    tokenEx(2002, "invalid_grant");

            /// <summary>
            /// An `unauthorized_client` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException UNAUTHORIZED_CLIENT =
                    tokenEx(2003, "unauthorized_client");

            /// <summary>
            /// An `unsupported_grant_type` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException UNSUPPORTED_GRANT_TYPE =
                    tokenEx(2004, "unsupported_grant_type");

            /// <summary>
            /// An `invalid_scope` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_SCOPE =
                    tokenEx(2005, "invalid_scope");

            /// <summary>
            /// An authorization error occurring on the client rather than the server. For example,
            /// due to client misconfiguration. This error should be treated as unrecoverable.
            /// </summary>
            public static readonly AuthzException CLIENT_ERROR =
                    tokenEx(2006, null);

            /// <summary>
            /// Indicates an OAuth error as per RFC 6749, but the error code is not known to the
            /// AppAuth for Android library. It could be a custom error or code, or one from an
            /// OAuth extension. The error field provides the exact error string returned by
            /// the server.
            /// </summary>
            public static readonly AuthzException OTHER =
                    tokenEx(2007, null);

            private static readonly IDictionary<String, AuthzException> STRING_TO_EXCEPTION =
                    exceptionMapByString(
                            INVALID_REQUEST,
                            INVALID_CLIENT,
                            INVALID_GRANT,
                            UNAUTHORIZED_CLIENT,
                            UNSUPPORTED_GRANT_TYPE,
                            INVALID_SCOPE,
                            CLIENT_ERROR,
                            OTHER);

            /// <summary>
            /// Returns the matching exception type for the provided OAuth2 error string, or
            /// OTHER if unknown.
            /// </summary>
            public static AuthzException byString(String error)
            {
                AuthzException ex;
                if (STRING_TO_EXCEPTION.TryGetValue(error, out ex))
                {
                    return ex;
                }

                return OTHER;
            }
        }

        /// <summary>
        /// Error codes related to failed registration requests.
        /// </summary>
        public sealed class RegistrationRequestErrors
        {
            // codes in this group should be between 4000-4999

            /// <summary>
            /// An `invalid_request` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_REQUEST =
                    registrationEx(4000, "invalid_request");

            /// <summary>
            /// An `invalid_client` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_REDIRECT_URI =
                    registrationEx(4001, "invalid_redirect_uri");

            /// <summary>
            /// An `invalid_grant` OAuth2 error response.
            /// </summary>
            public static readonly AuthzException INVALID_CLIENT_METADATA =
                    registrationEx(4002, "invalid_client_metadata");

            /// <summary>
            /// An authorization error occurring on the client rather than the server. For example,
            /// due to client misconfiguration. This error should be treated as unrecoverable.
            /// </summary>
            public static readonly AuthzException CLIENT_ERROR =
                    registrationEx(4003, null);

            /// <summary>
            /// Indicates an OAuth error as per RFC 6749, but the error code is not known to the
            /// AppAuth for Android library. It could be a custom error or code, or one from an
            /// OAuth extension. The error field provides the exact error string returned by
            /// the server.
            /// </summary>
            public static readonly AuthzException OTHER =
                    registrationEx(4004, null);

            private static readonly IDictionary<String, AuthzException> STRING_TO_EXCEPTION =
                    exceptionMapByString(
                            INVALID_REQUEST,
                            INVALID_REDIRECT_URI,
                            INVALID_CLIENT_METADATA,
                            CLIENT_ERROR,
                            OTHER);

            /// <summary>
            /// Returns the matching exception type for the provided OAuth2 error string, or
            /// OTHER if unknown.
            /// </summary>
            public static AuthzException byString(String error)
            {
                AuthzException ex;
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

            /// <summary>
            /// Indicates a problem parsing an OpenID Connect Service Discovery document.
            /// </summary>
            public static readonly AuthzException INVALID_DISCOVERY_DOCUMENT =
                generalEx(0, "Invalid discovery document");

            /// <summary>
            /// Indicates the user manually canceled the OAuth authorization code flow.
            /// </summary>
            public static readonly AuthzException USER_CANCELED_AUTH_FLOW =
                generalEx(1, "User cancelled flow");

            /// <summary>
            /// Indicates an OAuth authorization flow was programmatically cancelled.
            /// </summary>
            public static readonly AuthzException PROGRAM_CANCELED_AUTH_FLOW =
                generalEx(2, "Flow cancelled programmatically");

            /// <summary>
            /// Indicates a network error occurred.
            /// </summary>
            public static readonly AuthzException NETWORK_ERROR =
                generalEx(3, "Network error");

            /// <summary>
            /// Indicates a server error occurred.
            /// </summary>
            public static readonly AuthzException SERVER_ERROR =
                generalEx(4, "Server error");

            /// <summary>
            /// Indicates a problem occurred deserializing JSON.
            /// </summary>
            public static readonly AuthzException JSON_DESERIALIZATION_ERROR =
                generalEx(5, "JSON deserialization error");

            /// <summary>
            /// Indicates a problem occurred constructing a {@link TokenResponse token response} object
            /// from the JSON provided by the server.
            /// </summary>
            public static readonly AuthzException TOKEN_RESPONSE_CONSTRUCTION_ERROR =
                generalEx(6, "Token response construction error");

            /// <summary>
            /// Indicates a problem parsing an OpenID Connect Registration Response.
            /// </summary>
            public static readonly AuthzException INVALID_REGISTRATION_RESPONSE =
                generalEx(7, "Invalid registration response");

            /// <summary>
            /// Indicates that a received ID token could not be parsed
            /// </summary>
            public static readonly AuthzException ID_TOKEN_PARSING_ERROR =
                generalEx(8, "Unable to parse ID Token");

            /// <summary>
            /// Indicates that a received ID token is invalid
            /// </summary>
            public static readonly AuthzException ID_TOKEN_VALIDATION_ERROR =
                generalEx(9, "Invalid ID Token");
        }

        private static AuthzException generalEx(int code, String errorDescription)
        {
            return new AuthzException(
                    TYPE_GENERAL_ERROR, code, null, errorDescription, null, null);
        }

        private static AuthzException authEx(int code, String error)
        {
            return new AuthzException(
                    TYPE_OAUTH_AUTHORIZATION_ERROR, code, error, null, null, null);
        }

        private static AuthzException tokenEx(int code, String error)
        {
            return new AuthzException(
                    TYPE_OAUTH_TOKEN_ERROR, code, error, null, null, null);
        }

        private static AuthzException registrationEx(int code, String error)
        {
            return new AuthzException(
                    TYPE_OAUTH_REGISTRATION_ERROR, code, error, null, null, null);
        }

        /// <summary>
        /// Creates an exception based on one of the existing values defined in
        /// GeneralErrors, AuthorizationRequestErrors or TokenRequestErrors,
        /// providing a root cause.
        /// </summary>
        public static AuthzException fromTemplate(
                AuthzException ex,
                Exception rootCause)
        {
            return new AuthzException(
                    ex.type,
                    ex.code,
                    ex.error,
                    ex.errorDescription,
                    ex.errorUri,
                    rootCause);
        }

        /// <summary>
        /// Creates an exception based on one of the existing values defined in
        /// AuthorizationRequestErrors or TokenRequestErrors, adding information
        /// retrieved from OAuth error response.
        /// </summary>
        public static AuthzException fromOAuthTemplate(
                AuthzException ex,
                String errorOverride,
                String errorDescriptionOverride,
                Uri errorUriOverride)
        {
            return new AuthzException(
                    ex.type,
                    ex.code,
                    (errorOverride != null) ? errorOverride : ex.error,
                    (errorDescriptionOverride != null) ? errorDescriptionOverride : ex.errorDescription,
                    (errorUriOverride != null) ? errorUriOverride : ex.errorUri,
                    null);
        }

        private static IDictionary<String, AuthzException> exceptionMapByString(
                params AuthzException[] exceptions)
        {
            IDictionary<String, AuthzException> map =
                    new Dictionary<String, AuthzException>(exceptions != null ? exceptions.Length : 0);

            if (exceptions != null)
            {
                foreach (AuthzException ex in exceptions)
                {
                    if (ex.error != null)
                    {
                        map.Add(ex.error, ex);
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// The type of the error.
        /// @see #TYPE_GENERAL_ERROR
        /// @see #TYPE_OAUTH_AUTHORIZATION_ERROR
        /// @see #TYPE_OAUTH_TOKEN_ERROR
        /// @see #TYPE_RESOURCE_SERVER_AUTHORIZATION_ERROR
        /// </summary>
        public readonly int type;

        /// <summary>
        /// The error code describing the class of problem encountered from the set defined in this
        /// class.
        /// </summary>
        public readonly int code;

        /// <summary>
        /// The error string as it is found in the OAuth2 protocol.
        /// </summary>

        public readonly String error;

        /// <summary>
        /// The human readable error message associated with this exception, if available.
        /// </summary>

        public readonly String errorDescription;

        /// <summary>
        /// A URI identifying a human-readable web page with information about this error.
        /// </summary>

        public readonly Uri errorUri;

        /// <summary>
        /// Instantiates an authorization request with optional root cause information.
        /// </summary>
        public AuthzException(
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

        /// <summary>
        /// Produces a JSON representation of the authorization exception, for transmission or storage.
        /// This does not include any provided root cause.
        /// </summary>
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

        /// <summary>
        /// Provides a JSON string representation of an authorization exception, for transmission or
        /// storage. This does not include any provided root cause.
        /// </summary>

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
