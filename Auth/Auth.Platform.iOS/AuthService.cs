﻿using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Auth;

[assembly: AeroGear.Mobile.Core.Utils.Service(typeof(AuthService))]
namespace AeroGear.Mobile.Auth
{
    public class AuthService : AbstractAuthService
    {
        public AuthService(MobileCore mobileCore = null, ServiceConfiguration serviceConfig = null) : base(mobileCore, serviceConfig)
        {
            var storageManager = new StorageManager("AeroGear.Mobile.Auth.Credentials");
            CredentialManager = new CredentialManager(storageManager);
        }

        /// <summary>
        /// Provide authentication configuration to the service.
        /// </summary>
        /// <param name="authConfig">Authentication config.</param>
        public override void Configure(AuthenticationConfig authConfig)
        {
            Authenticator = new OIDCAuthenticator(authConfig, KeycloakConfig, CredentialManager, MobileCore.HttpLayer, MobileCore.Logger);
        }

        /// <summary>
        /// Retrieve the current user.
        /// </summary>
        /// <returns>The current user.</returns>
        public override User CurrentUser()
        {
            var serializedCredential = CredentialManager.LoadSerialized();
            if (serializedCredential == null)
            {
                return null;
            }
            var parsedCredential = new OIDCCredential(serializedCredential);
            return User.NewUser().FromUnverifiedCredential(parsedCredential, KeycloakConfig.ResourceId);
        }
       
    }
}