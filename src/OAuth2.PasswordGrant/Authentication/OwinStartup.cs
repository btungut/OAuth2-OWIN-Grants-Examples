// Copyright (C) 2016 Collaborators of https://github.com/btungut/OAuth2-OWIN-Grants-Examples repository.
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see;
// https://github.com/btungut/OAuth2-OWIN-Grants-Examples/blob/master/LICENSE.md
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(OAuth2.PasswordGrant.Authentication.OwinStartup))]
namespace OAuth2.PasswordGrant.Authentication
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            ConfigureOAuth(appBuilder);

            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }

        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            OAuthAuthorizationServerOptions serverOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(10),
                AllowInsecureHttp = true,

                Provider = new Providers.SimpleOAuthAuthorizationServerProvider()
            };

            OAuthBearerAuthenticationOptions bearerOptions = new OAuthBearerAuthenticationOptions()
            {
                Provider = new Providers.SimpleOAuthBearerAuthenticationProvider()
            };

            appBuilder.UseOAuthAuthorizationServer(serverOptions);
            appBuilder.UseOAuthBearerAuthentication(bearerOptions);
        }
    }
}