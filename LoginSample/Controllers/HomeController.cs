using LoginSample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace LoginSample.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Access database and query
        /// </summary>
        Db_Access.SsmLogInfo dbAccesser = new Db_Access.SsmLogInfo();

        /// <summary>
        /// Commiting link with database sql server
        /// </summary>
        String ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

        /// <summary>
        /// Index page action result
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                Session.Clear();
                HttpCookie userc = Request.Cookies["user"];
                HttpCookie userT = Request.Cookies["uType"];
                if (userc == null)
                {
                    // Model
                    var modelObj = new LoginInfo();

                    // User types
                    modelObj.UserType = new List<SelectListItem>();
                    modelObj.UserType.Add(new SelectListItem { Text = "--select--", Value = "0", Disabled = true, Selected = true });
                    modelObj.UserType.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
                    modelObj.UserType.Add(new SelectListItem { Text = "User", Value = "User" });
                    ViewBag.UserType = new SelectList(modelObj.UserType);
                    modelObj.UserName = new List<SelectListItem>();
                    ViewBag.UserName = new SelectList(modelObj.UserName);
                    Response.Cookies.Clear();
                    return View(modelObj);
                }
                if(userT.Value=="User")
                {
                    return RedirectToAction("EmployeeDetails", "EmployeeDetails");
                }
                else
                {
                    Session["IsFromMyAction"] = true;
                    return RedirectToAction("AdminPage", "Home");
                }

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        /// <summary>
        /// Admin page
        /// </summary>
        /// <returns>
        /// All users
        /// </returns>
        public ActionResult AdminPage()
        {
            try
            {
                HttpCookie userc = Request.Cookies["user"];
                HttpCookie userT = Request.Cookies["uType"];
                if(userc == null || userT == null)
                {
                    return RedirectToAction("PageError", "Home");
                }

                if(!userT.Value.Equals("Admin"))
                {
                    Session["IsFromMyAction"] = false;
                    if (Request.Cookies["user"] != null)
                    {
                        Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
                        Session.Abandon();
                    }
                    return RedirectToAction("PageError", "Home");
                }
                
                if (Request.Cookies["user"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if ((bool)Session["IsFromMyAction"] == true)
                {                    
                    List<LoginInfo> userData = new List<LoginInfo>();
                    userData = GetUsers();
                    userc = Request.Cookies["user"];
                    TempData["name"] = userc.Value;
                    return View(userData);
                }
                else
                {
                    return RedirectToAction("logout", "Home");
                }
            }
            catch
            {
                return RedirectToAction("logout", "Home");
            }
        }

        public ActionResult PageError()
        {
            return View();
        }

        /// <summary>
        /// Session logout
        /// </summary>
        /// <returns>Login page</returns>
        public ActionResult logout()
        {
            Session["IsFromMyAction"] = false;
            if (Request.Cookies["user"] != null)
            {
                Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["idCookie"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["uType"].Expires = DateTime.Now.AddDays(-1);
                Session.Abandon();
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="userId">Entered username</param>
        /// <param name="pass">Entered password</param>
        /// <returns>
        /// 1. If valid
        /// 2. if invalid
        /// </returns>
        public string ValidateUser(string userId, string pass)
        {
            try
            {
                pass = Security.PasswordSecurity.EncodePasswordToBase64(pass);
                /*Replace this query of code with you DB code.*/
                var logResult = GetDataFromDB(userId, pass);

                if (logResult.Count != 0)
                {
                    Session["IsFromMyAction"] = true;
                    // User Name stored in "user".
                    HttpCookie UserCookie = new HttpCookie("user", userId);
                    UserCookie.Expires.AddHours(1);
                    HttpContext.Response.SetCookie(UserCookie);
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Get all user according to type
        /// </summary>
        /// <param name="userType">Admin or user</param>
        /// <returns>Username</returns>
        public JsonResult UserNameBind(string userType)
        {
            try
            {
                HttpCookie UserTypeCookie = new HttpCookie("uType", userType);
                UserTypeCookie.Expires.AddHours(1);
                HttpContext.Response.SetCookie(UserTypeCookie);

                DataSet ds = dbAccesser.GetUsers(userType);
                var modelObj = new LoginInfo();
                modelObj.UserName = new List<SelectListItem>();
                List<SelectListItem> userList = new List<SelectListItem>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    modelObj.UserName.Add(new SelectListItem { Text = dr["UserName"].ToString(), Value = dr["UserName"].ToString() });
                }
                return Json(modelObj.UserName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Get user data
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="userPassword">Password</param>
        /// <returns>user data</returns>
        private List<LoginInfo> GetDataFromDB(string userName, string userPassword)
        {
            try
            {               
                DataSet ds = dbAccesser.GetLogPass(userName, userPassword);
                List<LoginInfo> userData = new List<LoginInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserPassword = dr["Password"].ToString(),UserPgId=dr["userid"].ToString() });
                    ViewData["UserIDToUpage"] = dr["userid"].ToString();
                    
                    // Vijay
                    HttpCookie UseridCookie = new HttpCookie("idCookie", dr["userid"].ToString());
                    UseridCookie.Expires.AddHours(1);
                    HttpContext.Response.SetCookie(UseridCookie);
                }
                return userData;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all user for datatable
        /// </summary>
        /// <returns>users details</returns>
        private List<LoginInfo> GetUsers()
        {
            try
            {
                DataSet ds = dbAccesser.GetAllUsers();
                List<LoginInfo> userData = new List<LoginInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserEmail = dr["UserEmail"].ToString(), UserSerial = dr["userid"].ToString() });
                }
                return userData;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add new users
        /// </summary>
        /// <param name="userid">User id</param>
        /// <param name="pass">Password</param>
        /// <param name="userEmail">User email</param>
        /// <returns>
        /// 1. Entered succesfully
        /// 0. Error in inputs
        /// </returns>
        public int AddNewUser(string userid, string pass, string userEmail)
        {
            try
            {
                HttpCookie userc = Request.Cookies["user"];
                HttpCookie userType = Request.Cookies["uType"];

                int val = dbAccesser.newUser(userid, pass, userEmail, userc.Value);
                if (val == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete existing data
        /// </summary>
        /// <param name="userid">User id</param>
        public void DeleteUser(string[] userid)
        {
            try
            {
                foreach (var user in userid)
                {
                    int userNumId = LoginSample.Db_Access.UserDbAccess.GetUserNumIdFromAdminDb(user);
                    dbAccesser.DeleteSelUser(user);
                   
                    if(userNumId == -1)
                    {
                        return;
                    }
                   LoginSample.Db_Access.UserDbAccess.DeleteUser(userNumId);
                }
            }
            catch(Exception exc)
            {
                throw (exc);
            }
        }

        /// <summary>
        /// Get user data to edit
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User data</returns>
        public JsonResult GetThisUser(string uid)
        {
            try
            {
                DataSet ds = dbAccesser.GetMeUser(uid);
                Security.PasswordSecurity decodePass = new Security.PasswordSecurity();
                List<LoginInfo> userData = new List<LoginInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserEmail = dr["UserEmail"].ToString(), UserPassword = decodePass.DecodeFrom64(dr["Password"].ToString()) });
                }
                if (userData.Count.Equals(0))
                {
                    return Json(new { result = false, error = "Selected user was deleted by another admin" });
                }
                return Json(userData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Update selected user
        /// </summary>
        /// <param name="SelectedUname">Checkbox selection with user name</param>
        /// <param name="newUname">New username</param>
        /// <param name="newPassword">New password</param>
        /// <param name="newEmail">New email</param>
        /// <returns>
        /// 1: if data updated
        /// 2: If user name already exits
        /// </returns>
        public int UpdateSelectedUser(string uid, string SelectedUname, string newUname, string newPassword, string newEmail)
        {
            try
            {
                DataSet ds = dbAccesser.GetUsernameForvalidation(newUname);
                newPassword = Security.PasswordSecurity.EncodePasswordToBase64(newPassword);
                Security.PasswordSecurity decodePass = new Security.PasswordSecurity();
                List<LoginInfo> userData = new List<LoginInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString() });
                }
                if (SelectedUname == newUname)
                {
                    dbAccesser.UpdateUserDetails(uid, newUname, newPassword, newEmail);
                    return 1;
                }
                if (userData.Count.Equals(0))
                {
                    dbAccesser.UpdateUserDetails(uid, newUname, newPassword, newEmail);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}