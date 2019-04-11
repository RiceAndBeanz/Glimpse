using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebServices.Models;
using Glimpse.Core.Contracts.Services;
//using Plugin.RestClient;
using Glimpse.Core.Services.General;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebServices.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly string WebServiceUrl = "http://glimpseofficial.azurewebsites.net/api/vendors/";
        private Vendor currentVendor;
        //RestClient<Vendor> restClient = new RestClient<Vendor>();

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
        }

        public async Task<Vendor> GetByKeyword(string keyword, bool slashRequired = false)
        {

            var httpClient = new HttpClient();

            string request = WebServiceUrl + "Search/" + keyword;

            if (slashRequired)
                request = request + "/";

            var json = await httpClient.GetStringAsync(request);

            var taskModel = JsonConvert.DeserializeObject<Vendor>(json);

            return taskModel;
        }

        public async Task<bool> Authenticate(Vendor t)
        {
            string request = WebServiceUrl.Substring(0, WebServiceUrl.IndexOf("api")) + "authenticate";
            //string request = "http://localhost/Glimpse/authenticate/";

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(request, httpContent);

            return result.IsSuccessStatusCode;

        }

        public async Task<bool> AuthenticateVendor(string email, string enteredPassword)
        {
            Vendor vendor = new Vendor { Email = email, Password = enteredPassword };

            var response = await Authenticate(vendor);

            if (response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            currentVendor = await GetByKeyword(context.UserName, true);
            if (currentVendor!=null){
                bool response = await AuthenticateVendor(context.UserName, context.Password);
                if (response) {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                    identity.AddClaim(new Claim("username", "user"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, ""+ currentVendor.VendorId));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, ""+ currentVendor.VendorId));
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}