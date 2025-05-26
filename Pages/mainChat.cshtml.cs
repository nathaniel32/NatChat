using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using SignalRChat.Models;

namespace SignalRChat.Pages
{
    public class mainChatModel : PageModel
    {
        [BindProperty]
        public modelMainChat? MC { get; set; }
        private readonly string _connectionString = "Data Source=data.db";
        private readonly ILogger<IndexModel> _logger;

        public List<Dictionary<string, string>> Users { get; set; } = new List<Dictionary<string, string>>();

        public mainChatModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (!(Request.Cookies["cookie_auth"] != null)) {
                Response.Redirect("/authentication/addLogin?status_a=Please Login First");
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Retrieve all users from the database
                var command = connection.CreateCommand();
                var uID = Request.Cookies["cookie_auth"];
                command.CommandText = "SELECT photo, fname, lname, uID FROM user WHERE uID <> '" + uID + "'";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new Dictionary<string, string>();
                        user.Add("photo", reader.GetString(0));
                        user.Add("fname", reader.GetString(1));
                        user.Add("lname", reader.GetString(2));
                        user.Add("uID", reader.GetString(3));
                        Users.Add(user);
                    }
                }

                // Retrieve the number of users from the database
                command.CommandText = "SELECT COUNT(*) FROM user";
                var userCount = (long)command.ExecuteScalar();
                ViewData["userCount"] = userCount;

                connection.Close();
            }
        }
        public IActionResult OnPost()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT rID FROM room WHERE (id1 = @c_uID2 or id2 = @c_uID2) AND (id1 = @c_uID1 or id2 = @c_uID1)";
                command.Parameters.AddWithValue("@c_uID2", MC.uID_mChat);
                command.Parameters.AddWithValue("@c_uID1", @Request.Cookies["cookie_auth"]);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        //var rID = reader.GetString(0);
                        
                        //if (string.IsNullOrEmpty(rID)){
                        reader.Close();
                        command.CommandText = "INSERT INTO room (rID,id1,id2) VALUES (@rID,@id1,@id2)";
                        string rID = Guid.NewGuid().ToString("N").Substring(0, 20);
                        command.Parameters.AddWithValue("@rID", rID);
                        command.Parameters.AddWithValue("@id1", @Request.Cookies["cookie_auth"]);
                        command.Parameters.AddWithValue("@id2", MC.uID_mChat);
                        //command.Parameters.AddWithValue("@id2", MC.uID_mChat);
                        command.ExecuteNonQuery();
                        Response.Cookies.Append("cookie_room", rID);
                        //}
                    }
                    else {
                        var rID = reader.GetString(0);
                        Response.Cookies.Append("cookie_room", rID);
                    }
                }
                
                command.CommandText = "SELECT fname, lname FROM User WHERE uID = @c_uID";
                command.Parameters.AddWithValue("@c_uID", MC.uID_mChat);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var fname = reader.GetString(0);
                        var lname = reader.GetString(1);
                        return RedirectToPage("/chat", new { fnameChat = fname, lnameChat = lname, uIDChat = MC.uID_mChat});
                    }
                }
                connection.Close();

            }
            return RedirectToPage("/Index", new { Status_A = "Error" });
        }
    }
}

