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

namespace OAuth2.PasswordGrant.Authentication.Providers
{
    public class SimpleOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //TOKEN İÇİN GELEN REQUEST'in İLK DURAĞI
            //1
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
            //2
            //Token için yapılan request'in check'i. Client bilgileri zaten kontrol edilmişti.
            context.Validated();

            return base.ValidateTokenRequest(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //3
            //Bu adıma geliyorsak request OK.
            //Artık authorize request'e göre ClaimsIdentity yaratılmalı ve Claim'ler eklenmeli.
            //Username ve Password kontrolü burada yapılıyor.
            //Pipeline'ı sonlandırıyor access token'ı veriyoruz.

            ClaimsIdentity identity = new ClaimsIdentity("Bearer");
            identity.AddClaim(new Claim("user", "Burak TUNGUT"));

            context.Validated(identity);

            return Task.FromResult(0);
        }
    }
}