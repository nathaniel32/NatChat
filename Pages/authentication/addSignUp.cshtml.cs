using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRChat.Models;
using Microsoft.Data.Sqlite;

namespace SignalRChat;

public class addSignUpModel : PageModel
{
    private readonly string _connectionString = "Data Source=data.db";

    [BindProperty]
    public modelSignUp? SignUp { get; set; }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid == false)
        {
            return Page();
        }

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO USER (fname,lname,telp,email,password) VALUES (@fname_signup, @lname_signup, @telp_signup, @email_signup, @password_signup)";
            command.Parameters.AddWithValue("@fname_signup", SignUp.fname_signup);
            command.Parameters.AddWithValue("@lname_signup", SignUp.lname_signup);
            command.Parameters.AddWithValue("@telp_signup", SignUp.telp_signup);
            command.Parameters.AddWithValue("@email_signup", SignUp.email_signup);
            command.Parameters.AddWithValue("@password_signup", SignUp.password_signup);

            command.ExecuteNonQuery();

            command.CommandText = "SELECT uID, fname FROM USER WHERE email = @email";
            command.Parameters.AddWithValue("@email", SignUp.email_signup);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var authID = reader.GetString(0);
                    var fname = reader.GetString(1);
                    Response.Cookies.Append("cookie_auth", authID);
                    Response.Cookies.Append("cookie_fname", fname);
                    return RedirectToPage("/Index", new { Status_A = "Success" });
                }
            }
            connection.Close();
        }
        return RedirectToPage("/authentication/addSignUp", new { Status_A = "Error Try Again!" });
    }
}