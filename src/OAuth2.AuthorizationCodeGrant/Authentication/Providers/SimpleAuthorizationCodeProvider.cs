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
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.AuthorizationCodeGrant.Authentication.Providers
{
    public class SimpleAuthorizationCodeProvider : AuthenticationTokenProvider
    {
        public SimpleAuthorizationCodeProvider() : base()
        {
            this.OnCreate = CreateCode;
            this.OnReceive = ReceiveCode;
        }

        private readonly ConcurrentDictionary<string, string> _authenticationCodes =
                new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        public void CreateCode(AuthenticationTokenCreateContext context)
        {
            //4
            //Auth code yaratıyoruz.
            //Daha sonra bu Auth code ile bize gelinip token istenilecek. Onun için saklıyoruz.
            context.SetToken(Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"));

            _authenticationCodes[context.Token] = context.SerializeTicket();
        }


        public void ReceiveCode(AuthenticationTokenReceiveContext context)
        {
            //7
            //Client bilgileri OK.
            //gelen Auth code'u kontrol ediyoruz. Gerçekten biz yarattık mı? Elimizde var mı?
            //Var ise token'ı oluşturuyoruz. Yoksa zaten value=null olacak. Ticket deserialize olmayacak.
            string value;
            _authenticationCodes.TryGetValue(context.Token, out value);

            context.DeserializeTicket(value);
        }
    }
}