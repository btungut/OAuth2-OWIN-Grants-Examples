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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OAuth2.AuthorizationCodeGrant.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public string AccessTokenCallback(string access_token, string token_type, string expires_in)
        {
            return $"Your token type is = '{token_type}' and access token is : '{access_token}' and it will be expired in {expires_in} seconds.";
        }
    }
}
