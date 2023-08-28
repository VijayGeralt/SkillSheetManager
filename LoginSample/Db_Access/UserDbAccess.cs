using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SkillSheetManager.Models;
namespace LoginSample.Db_Access
{
    public class UserDbAccess
    {
        /// <summary>
        /// Adds user details to database.
        /// </summary>
        /// <param name="employeeDetailsModel">Employee details.</param>
        internal static void AddInfo(EmployeeDetailsModel employeeDetailsModel)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO UserDetailsTable (UserId, Name, Designation, DateOfBirth, Sex, JoiningDate, WorkedInJapan, Qualifications, Languages, DatabaseKnown, UserPhoto) VALUES (@UserId, @Name, @Designation, @DateOfBirth, @Sex, @JoiningDate, @WorkedInJapan, @Qualifications, @Languages, @DatabaseKnown, @UserPhoto)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserId", employeeDetailsModel.UserId);
                    sqlCmd.Parameters.AddWithValue("@Name", employeeDetailsModel.Name);
                    sqlCmd.Parameters.AddWithValue("@Designation", employeeDetailsModel.Designation);
                    sqlCmd.Parameters.AddWithValue("@DateOfBirth", employeeDetailsModel.DateOfBirth);
                    sqlCmd.Parameters.AddWithValue("@Sex", employeeDetailsModel.Sex);
                    sqlCmd.Parameters.AddWithValue("@JoiningDate", employeeDetailsModel.JoiningDate);
                    sqlCmd.Parameters.AddWithValue("@WorkedInJapan", employeeDetailsModel.WorkedInJapan);

                    // Check if Qualification is null or empty, and set it as DBNull.Value if true
                    sqlCmd.Parameters.Add("@Qualifications", SqlDbType.VarBinary).Value = string.IsNullOrWhiteSpace(employeeDetailsModel.Qualification)
                        ? (object)DBNull.Value
                        : Encoding.UTF8.GetBytes(employeeDetailsModel.Qualification);

                    // Check if Languages is null or empty, and set it as DBNull.Value if true
                    sqlCmd.Parameters.Add("@Languages", SqlDbType.VarBinary).Value = string.IsNullOrWhiteSpace(employeeDetailsModel.Languages)
                        ? (object)DBNull.Value
                        : Encoding.UTF8.GetBytes(employeeDetailsModel.Languages);

                    // Check if Database is null or empty, and set it as DBNull.Value if true
                    sqlCmd.Parameters.Add("@DatabaseKnown", SqlDbType.VarBinary).Value = string.IsNullOrWhiteSpace(employeeDetailsModel.Database)
                        ? (object)DBNull.Value
                        : Encoding.UTF8.GetBytes(employeeDetailsModel.Database);

                    // Check if Photo is null or empty, and set it as DBNull.Value if true
                    sqlCmd.Parameters.Add("@UserPhoto", SqlDbType.VarBinary).Value = employeeDetailsModel.Photo?.Length > 0
                        ? employeeDetailsModel.Photo
                        : (object)DBNull.Value;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Edits user information.
        /// </summary>
        /// <param name="employeeDetailsModel">Employee details.</param>
        internal static void EditInfo(EmployeeDetailsModel employeeDetailsModel)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE UserDetailsTable SET Name = @Name, Designation = @Designation, DateOfBirth = @DateOfBirth, Sex = @Sex, JoiningDate = @JoiningDate, WorkedInJapan = @WorkedInJapan, Qualifications = @Qualifications, Languages = @Languages, DatabaseKnown = @DatabaseKnown, UserPhoto = @UserPhoto WHERE UserId = @UserId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserId", employeeDetailsModel.UserId);
                sqlCmd.Parameters.AddWithValue("@Name", employeeDetailsModel.Name);
                sqlCmd.Parameters.AddWithValue("@Designation", employeeDetailsModel.Designation);
                sqlCmd.Parameters.AddWithValue("@DateOfBirth", employeeDetailsModel.DateOfBirth);
                sqlCmd.Parameters.AddWithValue("@Sex", employeeDetailsModel.Sex);
                sqlCmd.Parameters.AddWithValue("@JoiningDate", employeeDetailsModel.JoiningDate);
                sqlCmd.Parameters.AddWithValue("@WorkedInJapan", employeeDetailsModel.WorkedInJapan);
                //sqlCmd.Parameters.AddWithValue("@Qualifications", employeeDetailsModel.Qualification);
                if (string.IsNullOrWhiteSpace(employeeDetailsModel.Qualification))
                {
                    sqlCmd.Parameters.Add("@Qualifications", SqlDbType.VarChar).Value = (object)DBNull.Value;
                }
                else
                {
                    sqlCmd.Parameters.Add("@Qualifications", SqlDbType.VarChar).Value = employeeDetailsModel.Qualification;
                }
                //sqlCmd.Parameters.AddWithValue("@Languages", employeeDetailsModel.Languages);
                if (string.IsNullOrWhiteSpace(employeeDetailsModel.Languages))
                {
                    sqlCmd.Parameters.Add("@Languages", SqlDbType.VarChar).Value = (object)DBNull.Value;
                }
                else
                {
                    sqlCmd.Parameters.Add("@Languages", SqlDbType.VarChar).Value = employeeDetailsModel.Languages;
                }
                //sqlCmd.Parameters.AddWithValue("@DatabaseKnown", employeeDetailsModel.Database);
                if (string.IsNullOrWhiteSpace(employeeDetailsModel.Database))
                {
                    sqlCmd.Parameters.Add("@DatabaseKnown", SqlDbType.VarChar).Value = (object)DBNull.Value;
                }
                else
                {
                    sqlCmd.Parameters.Add("@DatabaseKnown", SqlDbType.VarChar).Value = employeeDetailsModel.Database;
                }

                if (employeeDetailsModel.Photo.Length == 0)
                {
                    sqlCmd.Parameters.Add("@UserPhoto", SqlDbType.VarBinary).Value = (object)DBNull.Value;
                }
                else
                {
                    sqlCmd.Parameters.Add("@UserPhoto", SqlDbType.VarBinary).Value = employeeDetailsModel.Photo;
                }

                sqlCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets user details.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <param name="employeeDetailsModel">Employee details</param>
        /// <returns>True, if user details fetched successfully from database otherwise false.</returns>
        internal static bool GetUserDetails(int id, out EmployeeDetailsModel employeeDetailsModel)
        {
            //EmployeeDetailsModel employeeDetailsModel = new EmployeeDetailsModel();
            employeeDetailsModel = new EmployeeDetailsModel();
            DataTable dataTableDetails = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM UserDetailsTable WHERE UserId = @UserId";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlCon);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@UserId", id);
                sqlDataAdapter.Fill(dataTableDetails);
            }

            // User found in database
            if (dataTableDetails.Rows.Count == 1)
            {
                employeeDetailsModel.UserId = id/*Convert.ToInt32(dataTableDetails.Rows[0][0].ToString())*/;
                employeeDetailsModel.Name = dataTableDetails.Rows[0][1].ToString();
                employeeDetailsModel.Designation = dataTableDetails.Rows[0][2].ToString();
                employeeDetailsModel.DateOfBirth = DateTime.Parse(dataTableDetails.Rows[0][3].ToString());
                employeeDetailsModel.Sex = dataTableDetails.Rows[0][4].ToString();
                employeeDetailsModel.JoiningDate = DateTime.Parse(dataTableDetails.Rows[0][5].ToString());
                employeeDetailsModel.WorkedInJapan = dataTableDetails.Rows[0][6].ToString();
                employeeDetailsModel.Qualification = (dataTableDetails.Rows[0][7] == DBNull.Value) ? null : dataTableDetails.Rows[0][7].ToString();
                employeeDetailsModel.Languages = (dataTableDetails.Rows[0][8] == DBNull.Value) ? null : dataTableDetails.Rows[0][8].ToString();
                employeeDetailsModel.Database = (dataTableDetails.Rows[0][9] == DBNull.Value) ? null : dataTableDetails.Rows[0][9].ToString();
                employeeDetailsModel.Photo = (dataTableDetails.Rows[0][10] == DBNull.Value) ? null : (byte[])dataTableDetails.Rows[0][10];
                return true;
            }
            else
            {
                employeeDetailsModel.UserId = id;
                return false;
            }
        }

        /// <summary>
        /// Gets the password stored in database.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <returns>Password stored in database.</returns>
        internal static string GetUserPassword(int id)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
            string password = string.Empty;

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                string query = "SELECT password FROM LogInfo WHERE userid = @userid";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@userid", id);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataSet pwdDS = new DataSet();
                sqlDataAdapter.Fill(pwdDS);

                if (pwdDS.Tables[0].Rows.Count > 0)
                {
                    password = pwdDS.Tables[0].Rows[0]["password"].ToString();

                    // Decoding password
                    Security.PasswordSecurity passwordSecurity = new Security.PasswordSecurity();
                    password = passwordSecurity.DecodeFrom64(password);
                }
            }

            return password;
        }

        /// <summary>
        /// Updates the password of user or admin based on their id.
        /// </summary>
        /// <param name="id">User or admin ID.</param>
        /// <param name="newPassword">New entered password.</param>
        /// <returns>True, when password updated successfully otherwise false.</returns>
        internal static bool UpdateNewPassword(int id, string newPassword)
        {
            newPassword = Security.PasswordSecurity.EncodePasswordToBase64(newPassword);
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE LogInfo SET password=@npwd WHERE userid = @userid";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@userid", id);
                    sqlCmd.Parameters.AddWithValue("@npwd", newPassword);

                    sqlCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the user id with the specified user name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>User ID</returns>
        internal static int GetUserNumIdFromAdminDb(string userName)
        {
            try
            {
                int userNumId = 0;
                string ConnectionStringForAdminDb = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
                using (SqlConnection sqlConnector = new SqlConnection(ConnectionStringForAdminDb))
                {
                    sqlConnector.Open();
                    // userPassword = secPass.DecodeFrom64(userPassword);
                    SqlCommand sqlCmd = new SqlCommand("Select userid from LogInfo where UserName=@UserName", sqlConnector);
                    sqlCmd.Parameters.AddWithValue("UserName", userName);
                    SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
                    DataSet dataSet = new DataSet();
                    DataSeq.Fill(dataSet);
                    userNumId = int.Parse(dataSet.Tables[0].Rows[0]["userid"].ToString());
                }

                return userNumId;
            }
            

            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Deletes the user with the specific user id.
        /// </summary>
        /// <param name="id">User id.</param>
        internal static void DeleteUser(int id)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM UserDetailsTable WHERE UserId = @UserId";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("UserId", id);
                sqlCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Converts the image into binary.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}