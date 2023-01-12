using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Extensions;

namespace Infor.OAuth2SampleConsoleResourceOwner
{

    public class IonAPICredential
    {
        public static IonAPICredential FromRegistry(string aKeyName)
        {
            string json;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINE\IONAPI"))
            {
                object value = key.GetValue(aKeyName);
                json = value.ToString();
            };
            return JsonConvert.DeserializeObject<IonAPICredential>(json);
        }

        public static IonAPICredential FromFile(string afilename)
        {
            using (var reader = new StreamReader(afilename))
                do
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<IonAPICredential>(json);
                } while (reader.EndOfStream);
        }

        public static IonAPICredential FromKeyVault(string asecretname)
        {
            //string json;

            //string CLIENT_ID = "2875e55c-13ec-430d-b0e8-acce760b8c21"; 
            //string BASE_URI = "https://testkeyvault-demo.vault.azure.net/"; 
            //string CLIENT_SECRET = "nqj.X.k6zwX290~7WK7pguDZ85huSzn.1x";

            //var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (string auth, string res, string scope) =>
            //{
            //var authContext = new AuthenticationContext(auth);
            //var credential = new ClientCredential(CLIENT_ID, CLIENT_SECRET);
            //AuthenticationResult result = await authContext.AcquireTokenAsync(res, credential);
            //if (result == null)
            //{
            //    throw new InvalidOperationException("Failed to retrieve token");
            //}


            // https://www.c-sharpcorner.com/blogs/fetching-secrets-from-key-vault-in-net-console-app

            //{
            //    object value = ?.GetValue(asecretname);
            //    string json = value.ToString();
            //}
            return null;
            //return JsonConvert.DeserializeObject<IonAPICredential>(json);
        }

        public static string Combine(string uri1, string uri2)
        { 
            return uri1.TrimEnd('/') + "/" + uri2.TrimStart('/');
        }

        public void Dump()
        {
            Console.Write("Client ID\t");
            ResourceOwnerClientId.ConsoleGreen();
            Console.Write("Client secret\t");
            ResourceOwnerClientSecret.ConsoleGreen();
            Console.Write("Auth endpoint\t");
            OAuth2AuthorizationEndpoint.ConsoleGreen();
            Console.Write("Token endpoint\t");
            OAuth2TokenEndpoint.ConsoleGreen();
            Console.Write("API base URL\t");
            IONAPIBaseUrl.ConsoleGreen();
        }

        public string ResourceOwnerClientId { get { return ci; } } 
        public string ResourceOwnerClientSecret { get { return cs; } } 
        public string OAuth2TokenEndpoint { get { return Combine(pu, ot); } } 
        public string OAuth2TokenRevocationEndpoint { get { return Combine(pu, or); } } 
        public string OAuth2AuthorizationEndpoint { get { return Combine(pu, oa); } } 
        public string IONAPIBaseUrl { get { return Combine(iu, ti); } } 
        public string ServiceAccountAccessKey { get { return saak; } } 
        public string ServiceAccountSecretKey { get { return sask; } }


        public string ti { get; set; }
        public string cn { get; set; }
        public string dt { get; set; }
        public string ci { get; set; }
        public string cs { get; set; }
        public string iu { get; set; }
        public string pu { get; set; }
        public string oa { get; set; }
        public string ot { get; set; }
        public string or { get; set; }
        public string ev { get; set; }
        public string v { get; set; }
        public string saak { get; set; }
        public string sask { get; set; }
    }

}
