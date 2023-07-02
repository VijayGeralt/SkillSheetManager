using LoginSample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LoginSample.Db_Access
{
    /// <summary>
    /// Acccess Database and table
    /// </summary>
    public class SsmLogInfo
    {
        /// <summary>
        /// Access data using webConfig
        /// </summary>
        String ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Dataset of login</returns>
        public DataSet GetLogInfo()
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select * from LogInfo", sqlConnector);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Get user according to usertype
        /// </summary>
        /// <param name="UserType">Admin or user</param>
        /// <returns>Dataset of users</returns>
        public DataSet GetUsers(string UserType)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select * from LogInfo where UserType=@uId", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uId", UserType);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }
        // aW5kaWE=

        /// <summary>
        /// Get login info
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="userPassword">user password</param>
        /// <returns>User dataset</returns>
        public DataSet GetLogPass(string userName, string userPassword)
        {
            Security.PasswordSecurity secPass = new Security.PasswordSecurity();
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            // userPassword = secPass.DecodeFrom64(userPassword);
            SqlCommand sqlCmd = new SqlCommand("Select UserName,Password,userid from LogInfo where UserName=@uId and password=@uPass ", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uId", userName);
            sqlCmd.Parameters.AddWithValue("uPass", userPassword);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Get data of all user for datatable
        /// </summary>
        /// <returns>User dataset</returns>
        public DataSet GetAllUsers()
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select UserName,UserEmail,userid from LogInfo where UserType=@utype", sqlConnector);
            sqlCmd.Parameters.AddWithValue("utype", "user");
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Get new user
        /// </summary>
        /// <param name="userid">Username</param>
        /// <param name="password">User password</param>
        /// <param name="userEmail">User email</param>
        /// <param name="adminName">Admin editor</param>
        /// <returns>
        /// 1. If new user addes
        /// 0. Error in inputs
        /// </returns>
        public int newUser(string userid, string password, string userEmail, string adminName)
        {
            password = Security.PasswordSecurity.EncodePasswordToBase64(password);
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand validateUser = new SqlCommand("select UserName from LogInfo where UserName=@uname", sqlConnector);
            validateUser.Parameters.AddWithValue("uname", userid);
            SqlDataAdapter validIsUser = new SqlDataAdapter(validateUser);
            DataSet ds = new DataSet();
            validIsUser.Fill(ds);
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString() });
            }
            if (userData.Count.Equals(0))
            {
                SqlCommand sqlCmd = new SqlCommand("insert into LogInfo (UserName,password,UserEmail,RegisterAdmin,UserType,RegisterDate) values (@uname,@upass,@uemail,@uAdmin,@uType,@rDate)", sqlConnector);
                sqlCmd.Parameters.AddWithValue("uname", userid);
                sqlCmd.Parameters.AddWithValue("upass", password);
                sqlCmd.Parameters.AddWithValue("uemail", userEmail);
                sqlCmd.Parameters.AddWithValue("uAdmin", adminName);
                sqlCmd.Parameters.AddWithValue("uType", "user");
                sqlCmd.Parameters.AddWithValue("rDate", DateTime.Now);
                SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                DataSeq.Fill(ds);
                return 1;
            }
            else
            {               
                return 0;
            }
        }

        /// <summary>
        /// Delete selected users
        /// </summary>
        /// <param name="username">Username</param>
        public void DeleteSelUser(string username)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Delete from LogInfo where UserName=@uname", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", username);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
        }

        /// <summary>
        /// Get seletected user details
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User data details</returns>
        public DataSet GetMeUser(string uid)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select UserName, UserEmail, password from LogInfo where userid=@uid ", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uid", uid);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public DataSet GetUsernameForvalidation(string username)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select UserName, UserEmail, password from LogInfo where UserName=@uname ", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", username);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Update selected users
        /// </summary>
        /// <param name="selectedUser">Selected user</param>
        /// <param name="newUname">New user</param>
        /// <param name="newUpassword">New user password</param>
        /// <param name="newUemail">New user email</param>
        public void UpdateUserDetails(string uid,string newUname,string newUpassword,string newUemail)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("update LogInfo set UserName=@nUname,password=@nUpass,UserEmail=@nUemail where userid=@uname", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", uid);
            sqlCmd.Parameters.AddWithValue("nUname", newUname);
            sqlCmd.Parameters.AddWithValue("nUpass", newUpassword);
            sqlCmd.Parameters.AddWithValue("nUemail", newUemail);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
        }
    }
}