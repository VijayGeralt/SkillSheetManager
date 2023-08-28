using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillSheetManager.Models;

namespace LoginSample.Controllers

{
    public class EmployeeDetailsController : Controller
    {
        // GET: EmployeeDetails
        public ActionResult EmployeeDetails()
        {
            HttpCookie userc = Request.Cookies["idCookie"];
            HttpCookie userT = Request.Cookies["uType"];
            if (userc == null || userT == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (userT.Value.ToLower() == "admin")
            {
                return RedirectToAction("PageError", "Home");
            }
            int id = int.Parse(userc.Value);
            EmployeeDetailsModel employeeDetailsModel = new EmployeeDetailsModel();
            bool isOldUser = LoginSample.Db_Access.UserDbAccess.GetUserDetails(id, out employeeDetailsModel);

            // New user.
            if (isOldUser == false)
            {
                ViewBag.IsOldUser = isOldUser;
                return View(employeeDetailsModel);
            }
            // Old User
            ViewBag.IsOldUser = isOldUser;
            return View(employeeDetailsModel);
        }


        public ActionResult AddUserData(EmployeeDetailsModel employeeDetailsModel, string userPhoto, string isOldUser)
        {
            try
            {
                HttpCookie userc = Request.Cookies["user"];
                HttpCookie userT = Request.Cookies["uType"];
                if (userc == null || userT == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (userT.Value.ToLower() == "admin")
                {
                    return RedirectToAction("PageError", "Home");
                }

                bool isOldUserGot = bool.Parse(isOldUser);
                // Converting photo from base 64 string to byte array.
                employeeDetailsModel.Photo = Convert.FromBase64String(userPhoto);


                // Adding new user to the Database.
                if (isOldUserGot == false)
                {
                    LoginSample.Db_Access.UserDbAccess.AddInfo(employeeDetailsModel);
                }
                else
                {
                    // Updating user info.
                    LoginSample.Db_Access.UserDbAccess.EditInfo(employeeDetailsModel);

                }
                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error Encountered" + ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOverview(string sortOrder)
        {
            try
            {
                HttpCookie userc = Request.Cookies["user"];
                HttpCookie userT = Request.Cookies["uType"];
                if (userc == null || userT == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (userT.Value.ToLower() == "user")
                {
                    return RedirectToAction("PageError", "Home");
                }

                DataTable userDetailTbl = new DataTable();
                string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM UserDetailsTable", sqlCon);
                    sqlDa.Fill(userDetailTbl);
                }

                // Sorting parameters
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                // Sorting logic
                switch (sortOrder)
                {
                    case "name_desc":
                        userDetailTbl.DefaultView.Sort = "Name DESC";
                        break;
                    default:
                        userDetailTbl.DefaultView.Sort = "Name"; // Default sorting by Name ascending
                        break;
                }

                return View(userDetailTbl.DefaultView.ToTable()); // Pass the sorted DataView to the view
            }
            catch
            {
                return RedirectToAction("PageError", "Home");
            }
        }

        public ActionResult ChangePasswordView()
        {
            HttpCookie userT = Request.Cookies["uType"];
            if (userT==null)
            {
                return RedirectToAction("PageError", "Home");
            }
            return View();
        }

        public JsonResult ChangePassword(string enteredPassword, string newPassword)
        {
            HttpCookie userc = Request.Cookies["idCookie"];
            if (userc == null)
            {
                //return Json(false, JsonRequestBehavior.AllowGet);
                return Json(new { success = false, message = "User not found" });
            }
            int id = int.Parse(userc.Value);

            string userOldPassword = LoginSample.Db_Access.UserDbAccess.GetUserPassword(id);

            if(userOldPassword != enteredPassword)
            {
                //return Json("Your current password doesn't match old password", JsonRequestBehavior.AllowGet);
                return Json(new { success = false, message = "Your current password doesn't match your old password" });
            }

            bool isPasswordUpdated = false;
            isPasswordUpdated = LoginSample.Db_Access.UserDbAccess.UpdateNewPassword(id, newPassword);

            if(isPasswordUpdated == true)
            {
                return Json(new { success = true, message = "Password changed successfully!!" });
            }
            return Json(new { success = false, message = "Failed to change password !!" });
        }
    }
}