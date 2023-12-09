//using DataLogger_NetCore.Class;
//using Microsoft.AspNetCore.Cors;
//using Nancy;
//using Nancy.Extensions;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace DataLogger_NetCore.API
//{
//    [EnableCors("AllowAnyOrigin")]
//    public class LoginModule : NancyModule
//    {
//        public LoginModule() : base("/api/LoginAPI")
//        {
//            #region API POST: /api/LoginAPI/UsersLogin : User login
//            //POST /api/LoginAPI/UsersLogin
//            Post("/UsersLogin", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                string tokenkey;
//                try
//                {
//                    if (Program.Users.ContainsKey(request.username))
//                    {
//                        var user = Program.Users[request.username];
//                        if (user.password != "" && user.password == request.password)
//                        {
//                            user.listAreas.RemoveAll(x => x == null);
//                            if (user.permission == "admin")
//                            {
//                                //admin
//                                tokenkey = TokenGenerator.ToKeyUsers(request.username);
//                                user.tokenkey = tokenkey;
//                                user.lastLogin = Program.getTime().ToOADate();
//                                Program.User_replace(user);
//                                return Response.AsJson(user, HttpStatusCode.OK);
//                            }
//                            else
//                            {
//                                tokenkey = TokenGenerator.ToKeyUsers(request.username);
//                                user.tokenkey = tokenkey;
//                                user.lastLogin = Program.getTime().ToOADate();
//                                Program.User_replace(user);
//                                return Response.AsJson(user, HttpStatusCode.OK); //login successfully
//                            }
//                        }
//                        else
//                        {
//                            return Response.AsJson("Incorrect password", HttpStatusCode.OK);
//                        }
//                    }
//                    else
//                    {
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    }
//                }
//                catch (Exception Ex)
//                {
//                    Program.saveLog("API: /api/LoginAPI/UsersLogin -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion

//            #region API POST: /api/LoginAPI/UsersLoginGG : User login Google
//            //POST /api/LoginAPI/UsersLoginGG
//            Post("/UsersLoginGG", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                string tokenkey;
//                try
//                {
//                    string username = GGSignIn.verifyToken(request.username);
                    
//                    // Mục sửa từ chỗ này
//                    if (username == "") {
//                        var data = GGSignIn.verifyAccessTokenAsync(request.username);
//                        if(data == "Error")
//                        {
//                            return Response.AsJson("Sign in failed", HttpStatusCode.OK); 
//                        }
//                        else
//                        {
//                            username = data;
//                        }
//                    }
//                    // Kết thúc ở đây

//                    if (Program.Users.ContainsKey(username))
//                    {
//                        tokenkey = TokenGenerator.ToKeyUsers(username);
//                        Program.Users[username].tokenkey = tokenkey;
//                        Program.Users[username].lastLogin = Program.getTime().ToOADate();
//                        Program.User_replace(Program.Users[username]);
//                        return Response.AsJson(Program.Users[username], HttpStatusCode.OK); //login successfully
//                    }
//                    else
//                    {
//                        List<string> lsDevices = new List<string>();
//                        List<string> lsAreas = new List<string>();
//                        //List<string> lsmobile = new List<string>();
//                        double timenow = Program.getTime().ToOADate();
//                        Program.Users.Add(username, new User(username, "", "", timenow, -1, "user", lsDevices, lsAreas));
//                        tokenkey = TokenGenerator.ToKeyUsers(username);
//                        Program.Users[username].tokenkey = tokenkey;
//                        Program.User_Collection.InsertOne(Program.Users[username]);
//                        return Response.AsJson(Program.Users[username], HttpStatusCode.OK);
//                    }
//                }
//                catch (Exception Ex)
//                {
//                    Program.saveLog("API: /api/LoginAPI/UsersLogin -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/UsersLoginMS : User login Microsoft
//            //POST /api/LoginAPI/UsersLoginMS
//            Post("/UsersLoginMS", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                string tokenkey;
//                try
//                {
//                    string username = GGSignIn.verifyMSToken(request.username);
//                    if (username == "") return Response.AsJson("Sign in failed", HttpStatusCode.OK);

//                    if (Program.Users.ContainsKey(username))
//                    {
//                        tokenkey = TokenGenerator.ToKeyUsers(username);
//                        Program.Users[username].tokenkey = tokenkey;
//                        Program.Users[username].lastLogin = Program.getTime().ToOADate();
//                        Program.User_replace(Program.Users[username]);
//                        return Response.AsJson(Program.Users[username], HttpStatusCode.OK); //login successfully
//                    }
//                    else
//                    {
//                        List<string> lsDevices = new List<string>();
//                        List<string> lsAreas = new List<string>();
//                        //List<string> lsmobile = new List<string>();
//                        double timenow = Program.getTime().ToOADate();
//                        Program.Users.Add(username, new User(username, "", "", timenow, -1, "user", lsDevices, lsAreas));
//                        tokenkey = TokenGenerator.ToKeyUsers(username);
//                        Program.Users[username].tokenkey = tokenkey;
//                        Program.User_Collection.InsertOne(Program.Users[username]);
//                        return Response.AsJson(Program.Users[username], HttpStatusCode.OK);
//                    }
//                }
//                catch (Exception Ex)
//                {
//                    Program.saveLog("API: /api/LoginAPI/UsersLogin -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/Users : View all user
//            //POST /api/LoginAPI/Users
//            Post("/Users", args =>
//            {
//                // var tokenkey = JsonConvert.DeserializeObject<String>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                string tokenkey = Request.Body.AsString().Trim('"');
//                try
//                {
//                    TokenKey vKey = new TokenKey(tokenkey);
//                    if (!vKey.isOK)
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);

//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)

//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //  else if (vTmpUser.tokenkey != tokenkey)
//                    //     return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (vTmpUser.permission == "engineer" || vTmpUser.permission == "user" || vTmpUser.permission == "admin")
//                        {
//                            //  return Request.CreateResponse(HttpStatusCode.OK, Program.Users);
//                            Dictionary<string, object> usload = new Dictionary<string, object>(); //Users Load file
//                            foreach (var key in Program.Users.Keys)
//                            {
//                                    usload[key] = Program.Users[key];
//                            }
//                            //  var data = JsonConvert.SerializeObject(usload);
//                            // Console.WriteLine(data);\
//                            //  return new Response { StatusCode = HttpStatusCode.OK, Contents = usload };
//                            return Response.AsJson(usload, HttpStatusCode.OK);
//                        }
//                        else
//                        {
//                            // return Request.CreateResponse(HttpStatusCode.OK, "Permission denied");
//                            return Response.AsJson("Permission denied", HttpStatusCode.OK);
//                        }
//                    }

//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/Users -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/AddUsers : AddUsers
//            //POST /api/LoginAPI/Users
//            Post("/AddUsers", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                //string tokenkey = Request.Body.AsString();
//                try
//                {
//                    TokenKey vKey = new TokenKey(request.tokenkey);
//                    if (!vKey.isOK)
//                        //return Request.CreateResponse(HttpStatusCode.BadGateway, "Input Data is wrong!");
//                        //return Request.CreateResponse(HttpStatusCode.OK, "Incorrect input data");
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);
//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)
//                        // return Request.CreateResponse(HttpStatusCode.NotFound, "Account doesn't exist");
//                        // return Request.CreateResponse(HttpStatusCode.OK, "Account not exist");
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //   else if (vTmpUser.tokenkey != request.tokenkey)
//                    //      return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (vTmpUser.permission == "admin")
//                        {
//                            if (Program.Users.ContainsKey(request.username))
//                            {
//                                // return Request.CreateResponse(HttpStatusCode.OK, "Username đã tồn tại!");
//                                return Response.AsJson("Username đã tồn tại!", HttpStatusCode.OK);
//                            }
//                            else
//                            {
//                                List<string> lsDevices = new List<string>();
//                                List<string> lsAreas = new List<string>();
//                                //List<string> lsmobile = new List<string>();
//                                double timenow = Program.getTime().ToOADate();
//                                Program.Users.Add(request.username, new User(request.username, request.password, "", timenow, -1, request.permission, lsDevices, lsAreas));
//                                Program.User_Collection.InsertOne(Program.Users[request.username]);
//                                //  PacketProcessor.writeUsers(Program.Users);
//                                // return Request.CreateResponse(HttpStatusCode.OK, "Thêm 1 user thành công");
//                                Program.actionLog(vTmpUser.username + " create a new account: username=" + request.username + ", password=" + request.password);
//                                return Response.AsJson("Thêm 1 user thành công", HttpStatusCode.OK);
//                            }
//                        }
//                        else
//                        {
//                            //return Request.CreateResponse(HttpStatusCode.OK, "Permission denied");
//                            return Response.AsJson("Permission denied", HttpStatusCode.OK);
//                        }

//                    }
//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/AddUsers -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/RootConfig : RootConfig
//            //POST /api/LoginAPI/RootConfig
//            Post("/RootConfig", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                //string tokenkey = Request.Body.AsString();
//                try
//                {
//                    TokenKey vKey = new TokenKey(request.tokenkey);
//                    if (!vKey.isOK)
//                        //return Request.CreateResponse(HttpStatusCode.BadGateway, "Input Data is wrong!");
//                        //return Request.CreateResponse(HttpStatusCode.OK, "Incorrect input data");
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);
//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)
//                        // return Request.CreateResponse(HttpStatusCode.NotFound, "Account doesn't exist");
//                        // return Request.CreateResponse(HttpStatusCode.OK, "Account not exist");
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //   else if (vTmpUser.tokenkey != request.tokenkey)
//                    //      return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (vTmpUser.permission == "root")
//                        {
//                            if (Program.Users.ContainsKey(request.username))
//                            {
//                                // return Request.CreateResponse(HttpStatusCode.OK, "Username đã tồn tại!");
//                                return Response.AsJson("Username đã tồn tại!", HttpStatusCode.OK);
//                            }
//                            else
//                            {
//                                List<string> lsDevices = new List<string>();
//                                List<string> lsAreas = new List<string>();
//                                double timenow = Program.getTime().ToOADate();
//                                string permission = request.permission == "null" ? "user" : request.permission;
//                                Program.Users.Add(request.username, new User(request.username, request.password, "", timenow, -1, permission, lsDevices, lsAreas));
                                
//                                //  PacketProcessor.writeUsers(Program.Users);
//                                // return Request.CreateResponse(HttpStatusCode.OK, "Thêm 1 user thành công");
                              
//                                Program.actionLog("[RootConfig] CREATE username=" + request.username + ", password=" + request.password + " permission=" + request.permission);
//                                Program.User_Collection.InsertOne(Program.Users[request.username]);
//                                return Response.AsJson("Thêm thành công", HttpStatusCode.OK);
//                            }
//                        }
//                        else
//                        {
//                            //return Request.CreateResponse(HttpStatusCode.OK, "Permission denied");
//                            return Response.AsJson("Permission denied", HttpStatusCode.OK);
//                        }

//                    }
//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/RootConfig -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/DeleteUsers : DeleteUsers
//            //POST /api/LoginAPI/DeleteUsers
//            Post("/DeleteUsers", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                //string tokenkey = Request.Body.AsString();
//                try
//                {
//                    TokenKey vKey = new TokenKey(request.tokenkey);
//                    if (!vKey.isOK)
//                        //return Request.CreateResponse(HttpStatusCode.BadGateway, "Input Data is wrong!");
//                        //return Request.CreateResponse(HttpStatusCode.OK, "Incorrect input data");
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);
//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)
//                        // return Request.CreateResponse(HttpStatusCode.NotFound, "Account doesn't exist");
//                        // return Request.CreateResponse(HttpStatusCode.OK, "Account not exist");
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //   else if (vTmpUser.tokenkey != request.tokenkey)
//                    //      return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (vTmpUser.permission == "admin")
//                        {
//                            if (Program.Users.ContainsKey(request.username) && Program.Users[request.username].permission != "admin")
//                            {
//                                Program.Users.Remove(request.username);
//                                Program.actionLog("[DELETE USER] "+vTmpUser.username + " remove an account: username=" + request.username);
//                                Program.User_delete(request.username);
//                                return Response.AsJson("Xóa username thành công!", HttpStatusCode.OK);
//                            }
//                            else
//                            {
//                                //return Request.CreateResponse(HttpStatusCode.Conflict, "Username doesn't exist!");
//                                return Response.AsJson("Username không tồn tại!", HttpStatusCode.OK);
//                            }
//                        }
//                        else
//                        {
//                            return Response.AsJson("Permission denied", HttpStatusCode.OK);
//                        }                     
//                    }
//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/DeleteUsers -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/UserChangePassword : UserChangePassword
//            //POST /api/LoginAPI/UserChangePassword
//            Post("/UserChangePassword", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                //string tokenkey = Request.Body.AsString();
//                try
//                {
//                    TokenKey vKey = new TokenKey(request.tokenkey);
//                    if (!vKey.isOK)
//                        //return Request.CreateResponse(HttpStatusCode.BadGateway, "Input Data is wrong!");
//                        //return Request.CreateResponse(HttpStatusCode.OK, "Incorrect input data");
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);
//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)
//                        // return Request.CreateResponse(HttpStatusCode.NotFound, "Account doesn't exist");
//                        // return Request.CreateResponse(HttpStatusCode.OK, "Account not exist");
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //   else if (vTmpUser.tokenkey != request.tokenkey)
//                    //      return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (request.password != vTmpUser.password)
//                            //  return Request.CreateResponse(HttpStatusCode.NotFound, "Old password is wrong!");
//                            // return Request.CreateResponse(HttpStatusCode.OK, "Mật khẩu cũ sai!");
//                            return Response.AsJson("Mật khẩu cũ sai!", HttpStatusCode.OK);
//                        else
//                        {
//                            Program.Users[vTmpUser.username].password = request.newpassword;
//                            //return Request.CreateResponse(HttpStatusCode.OK, Program.Users[vTmpUser.username]);
//                            Program.actionLog("[USER_CHANGE_PASSWORD] " +vTmpUser.username + " change a password to " + request.newpassword);
//                            Program.User_replace(Program.Users[vTmpUser.username]);
//                            return Response.AsJson(Program.Users[vTmpUser.username], HttpStatusCode.OK);
//                        }

//                    }
//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/UserChangePassword -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//            #region API POST: /api/LoginAPI/AdminResetPassword : UserChangePassword
//            //POST /api/LoginAPI/AdminResetPassword
//            Post("/AdminResetPassword", args =>
//            {
//                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
//                //HttpRequest new_req = new HttpRequest();
//                //string tokenkey = Request.Body.AsString();
//                try
//                {
//                    TokenKey vKey = new TokenKey(request.tokenkey);
//                    if (!vKey.isOK)
//                        //return Request.CreateResponse(HttpStatusCode.BadGateway, "Input Data is wrong!");
//                        //return Request.CreateResponse(HttpStatusCode.OK, "Incorrect input data");
//                        return Response.AsJson("Incorrect input data", HttpStatusCode.OK);
//                    User vTmpUser = Program.Users.Values.Where(p => p.username == vKey.UserName && p.password == vKey.Password).FirstOrDefault();
//                    if (vTmpUser == null)
//                        // return Request.CreateResponse(HttpStatusCode.NotFound, "Account doesn't exist");
//                        // return Request.CreateResponse(HttpStatusCode.OK, "Account not exist");
//                        return Response.AsJson("Account not exist", HttpStatusCode.OK);
//                    //   else if (vTmpUser.tokenkey != request.tokenkey)
//                    //      return Request.CreateResponse(HttpStatusCode.OK, "Token key đã hết hạn");
//                    else
//                    {
//                        if (vTmpUser.permission == "admin")
//                        {
//                            if (Program.Users.ContainsKey(request.username))
//                            {
//                                Program.Users[request.username].password = request.newpassword;
//                                // return Request.CreateResponse(HttpStatusCode.OK, "Đổi mật khẩu user thành công");
//                                Program.actionLog("[ADMIN_RESET_PASSWORD] reset password: username=" + request.username + ", newpassword=" + request.newpassword);
//                                Program.User_replace(Program.Users[request.username]);
//                                return Response.AsJson("Đổi mật khẩu user thành công", HttpStatusCode.OK);
//                            }
//                            else
//                            {
//                                //return Request.CreateResponse(HttpStatusCode.OK, "Tài khoản username không tồn tại");
//                                return Response.AsJson("Tài khoản username không tồn tại", HttpStatusCode.OK);
//                            }
//                        }
//                        else
//                        {
//                            //return Request.CreateResponse(HttpStatusCode.OK, "Bạn không có quyền thực hiện điều này");
//                            return Response.AsJson("Bạn không có quyền thực hiện điều này", HttpStatusCode.OK);
//                        }

//                    }
//                }
//                catch (Exception Ex)
//                {
//                    // return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message);
//                    Program.saveLog("API: /api/LoginAPI/AdminResetPassword -> " + Ex.Message);
//                    return Response.AsJson(Ex.Message, HttpStatusCode.OK);
//                }
//            });
//            #endregion
//        }
//    }
//}
