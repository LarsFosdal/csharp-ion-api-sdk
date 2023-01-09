using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Thinktecture.IdentityModel.Client;
using Thinktecture.IdentityModel.Extensions;
//using Thinktecture.IdentityModel.Http;

namespace Infor.OAuth2SampleConsoleResourceOwner
{
    /// <summary>
    /// Console application that obtains an access_token and makes a rest call to the service.
    /// 
    /// It is also able to get a new access_token out of the refresh token.
    /// 
    /// When the grant is not needed anymore it revokes the refresh token.
    /// 
    /// The implementation is inspired on the client projects from Thinktecture:
    /// https://github.com/IdentityServer/IdentityServer3.Samples
    /// 
    /// The implementation relies on the Thinktecture.IdentityModel.Client to do most of the hard lifting. 
    /// 
    /// </summary>
    class Program
    {

        private static OAuth2Client _oauth2;
        private static IonAPICredential IONAPI;


        static void Main(string[] args)
        {

            IONAPI = LoadCredentialsFromRegistry("TIP_FOSLAR_test_ONPREM");
            //IONAPI = LoadCredentialsFromRegistry("TIP_FOSLAR_test_CLOUD");
            //IONAPI = LoadCredentialsFromFile("C:\\Users\\foslar\\Downloads\\TIP_FOSLAR_test_ONPREM.ionapi");
            //IONAPI = LoadCredentialsFromFile("C:\\Users\\foslar\\Downloads\\TIP-FOSLAR-test_CLOUD.ionapi");


            _oauth2 = new OAuth2Client(
                new Uri(IONAPI.OAuth2TokenEndpoint),
                    IONAPI.ResourceOwnerClientId,
                    IONAPI.ResourceOwnerClientSecret);

            //Request a token with the provided ServiceAccountAccessKey and ServiceAccountSecretKey
            TokenResponse token = RequestToken();
            Console.WriteLine("Request Token");
            ShowResponse(token);

            if (!token.IsError)
            {
                //Use the access_token to make a call to ION API
                CallService(token.AccessToken);

                //If a refresh token is available the application can obtain new access_token after those have expired.
                if (token.RefreshToken != null)
                {
                    token = RefreshToken(token.RefreshToken);
                    Console.WriteLine("Refresh Token");
                    ShowResponse(token);

                    //It should be possible to continue calling the service with the new token.

                    if (!token.IsError)
                    {
                        CallService(token.AccessToken);
                    }
                }


                //When there is no need for the token it should be revoked so no further access is allowed.
                RevokeToken(token.AccessToken, OAuth2Constants.AccessToken);

                //If the refresh token is provided is recommended to revoke the refresh token.
                if (token.RefreshToken != null)
                {
                    RevokeToken(token.RefreshToken, OAuth2Constants.RefreshToken);
                }

                //It is not possible to use the access_token anymore...
                CallService(token.AccessToken);

                //It should not be possible to refresh the token again...
                token = RefreshToken(token.RefreshToken);
                Console.WriteLine("Refresh Token");
                ShowResponse(token);
            }
            Console.WriteLine("\n\nPress Enter");
            Console.ReadLine();
        }


        static IonAPICredential LoadCredentialsFromFile(string afilename)
        {
            using (var reader = new StreamReader(afilename))
                do
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<IonAPICredential>(json);
                } while (reader.EndOfStream);
        }


        static IonAPICredential LoadCredentialsFromRegistry(string akeyname)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TINE\IONAPI"))
            {
                object value = key.GetValue(akeyname);
                string json = value.ToString();
                return JsonConvert.DeserializeObject<IonAPICredential>(json);
            }
        }

        static void CallService(string token)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(IONAPI.IONAPIBaseUrl)
            };

            client.SetBearerToken(token);

            var cmd = IONAPI.IONAPIBaseUrl + "/M3/m3api-rest/execute/MMS200MI/GetServerTime";
            cmd.ConsoleYellow();

            var response = client.GetAsync(cmd).Result;

            if (response.IsSuccessStatusCode)
            {
                "\n\nWebService call response.".ConsoleGreen();
            }
            else
            {
                "\n\nWebService failed".ConsoleRed();
            }

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static TokenResponse RequestToken()
        {
            "\nUsing RequestToken:".ConsoleGreen();
            return _oauth2.RequestResourceOwnerPasswordAsync
                (IONAPI.ServiceAccountAccessKey, IONAPI.ServiceAccountSecretKey).Result;
        }

        private static TokenResponse RefreshToken(string refreshToken)
        {
            "\nUsing RefreshToken:".ConsoleGreen();
            Console.WriteLine(refreshToken);

            return _oauth2.RequestRefreshTokenAsync(refreshToken).Result;
        }

        private static void RevokeToken(string token, string tokenType)
        {
            var client = new HttpClient();
            client.SetBasicAuthentication(IONAPI.ResourceOwnerClientId, IONAPI.ResourceOwnerClientSecret);

            var postBody = new Dictionary<string, string>
            {
                { "token", token },
                { "token_type_hint", tokenType }
            };

            var result = client.PostAsync(IONAPI.OAuth2TokenRevocationEndpoint, new FormUrlEncodedContent(postBody)).Result;

            if (result.IsSuccessStatusCode)
            {
                "Succesfully revoked token.".ConsoleGreen();
            }
            else
            {
                "Error revoking token.".ConsoleRed();
            }

            Console.WriteLine("{1}, {0}", token, tokenType);
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "\nToken response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                "\nAccess Token:".ConsoleGreen();
                Console.WriteLine(response.AccessToken);
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleRed();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleRed();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleRed();
                    Console.WriteLine(response.Json);
                }
            }
        }
    }
}
