//using Google.Apis.Auth;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Requests;
//using Google.Apis.Oauth2.v2;
//using Newtonsoft.Json;
//using RestSharp.Validation;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataLogger_NetCore.Class
//{
//    public static class GGSignIn
//    {
//        // Chú ý sau đổi sang của thầy
//        static string appIdGG = "1084294794528-qioi5hj1b3tj6k99r7dlh8umgdet4i57.apps.googleusercontent.com";
//        static GoogleJsonWebSignature.ValidationSettings GGValidationSettings = new GoogleJsonWebSignature.ValidationSettings()
//        {
//            Audience = new List<string>() { appIdGG }
//        };
//        public static string verifyToken(string tokenId)
//        {
//            try
//            {
//                Program.saveLog("GG signing in " + tokenId);

//                GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(tokenId, GGValidationSettings).Result;
//                var email = payload.Email;
//                if (email != null)
//                {
//                    Program.saveLog("GG token check ok " + email);
//                    return email;
//                }
//                else
//                {
//                    return "";
//                }
//            }
//            catch (Exception e)
//            {
//                Program.saveLog(e.ToString());
//                return "";
//            }
//        }
//        public static string verifyAccessTokenAsync(string token) // Phần mới đây nhá! VerifyAccessToken
//        {
//            string accessToken = token; // Thay YOUR_ACCESS_TOKEN bằng Access Token bạn đã nhận được

//            try
//            {
//                GoogleCredential credential = GoogleCredential.FromAccessToken(accessToken);
//                Oauth2Service service = new Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer
//                {
//                    HttpClientInitializer = credential
//                });

//                var request = service.Userinfo.Get();
//                var userInfo = request.Execute();
//                return userInfo.Email;
//            }catch(Exception e)
//            {
//                return "Error";
//            }
//        }
//        //static string appIdMS = "62b29b8b-1dda-4e0e-9fdf-d52ec63137e0";
//        static string appIdMS = "3197c144-9fdf-41b4-ace6-32e549e2dd5c";

//        public static string verifyMSToken(string tokenId)
//        {
            
//            try
//            {
//                Program.saveLog("MS signing in " + tokenId);
//                var token = new JwtSecurityToken(jwtEncodedString: tokenId);
//                if (token.Payload.Aud.First() != appIdMS) return "";
//                var now = DateTimeOffset.Now.ToUnixTimeSeconds();
//                if (token.Payload.NotBefore > now || token.Payload.Expiration < now) return "";
//                object email = "";
//                token.Payload.TryGetValue("preferred_username", out email);
//                if (email != null)
//                {
//                    Program.saveLog("MS token check ok " + email);
//                    return (string)email;
//                }
//                else return "";
//            }
//            catch (Exception e)
//            {
//                Program.saveLog(e.ToString());
//                return "";
//            }
//        }
//    }
//}
