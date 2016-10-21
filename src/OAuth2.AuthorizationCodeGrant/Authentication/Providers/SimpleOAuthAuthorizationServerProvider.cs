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
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OAuth2.AuthorizationCodeGrant.Authentication.Providers
{
    public class SimpleOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            //1
            //Uri check yapabiliriz
            //Ya da tüm bu check'ler bir sonraki method'da (ValidateAuthorizeRequest) yapılabilir. 
            context.Validated();

            return base.ValidateClientRedirectUri(context);
        }

        public override Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            //2
            //Request data'ların istediğimiz gibi olup olmadığı kontrol edilebilir.
            //scope ve state bilgileri burada check edilebilir.
            context.Validated();

            return base.ValidateAuthorizeRequest(context);
        }

        public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            //3
            //Bu adıma geliyorsak request OK.
            //Artık authorize request'e göre ClaimsIdentity yaratılmalı ve Claim'ler eklenmeli.
            //Pipeline'ı sonlandırıyor Auth. Code'un yaratılmasına geçiyoruz.

            ClaimsIdentity identity = new ClaimsIdentity("Bearer");
            identity.AddClaim(new Claim("user", "Burak TUNGUT"));

            context.OwinContext.Authentication.SignIn(identity);
            context.RequestCompleted();

            return Task.FromResult(0);
        }

        public override Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
        {
            //5
            //Köprüden önceki son çıkış
            //Logging, last check'ler yapılmalı.

            return base.AuthorizationEndpointResponse(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //TOKEN İÇİN GELEN REQUEST'in İLK DURAĞI
            //6
            //clientId, clientSecret ve clientType check yapılır.
            //Client bilgileri yanlış ise pipeline durdurulmalı yoksa ticket deseralize edilmek üzere.
            string clientId;
            string clientSecret;

            if (context.TryGetBasicCredentials(out clientId, out clientSecret) || context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                string clientType = context.Parameters.Get("client_type");
            }

            context.Validated();

            return Task.FromResult(0);
        }

        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            //8
            //Token için yapılan request'in check'i. Client bilgileri zaten kontrol edilmişti.
            context.Validated();

            return base.ValidateTokenRequest(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            return base.TokenEndpoint(context);
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            return base.TokenEndpointResponse(context);
        }
    }
}