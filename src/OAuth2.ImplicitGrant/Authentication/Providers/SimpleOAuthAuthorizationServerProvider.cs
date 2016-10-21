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

namespace OAuth2.ImplicitGrant.Authentication.Providers
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
            //4
            //Köprüden önceki son çıkış
            //Logging, last check'ler yapılmalı.

            return base.AuthorizationEndpointResponse(context);
        }
    }
}