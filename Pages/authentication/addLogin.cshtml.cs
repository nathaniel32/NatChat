using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRChat.Models;
using Microsoft.Data.Sqlite;

namespace SignalRChat;

public class addLoginModel : PageModel
{
    private readonly string _connectionString = "Data Source=data.db";
    [BindProperty]
    public modelLogin? Login { get; set; }

    [BindProperty (SupportsGet = true)]
    public string Status_A { get; set; }

    public void OnGet()
    {
        if (string.IsNullOrEmpty(Status_A))
        {
            Status_A = "";
        }
    }

    public IActionResult OnPost()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT password, uID, fname FROM USER WHERE email = @email";
            command.Parameters.AddWithValue("@email", Login.email_login);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var password = reader.GetString(0);
                    var userID = reader.GetString(1);
                    var fname = reader.GetString(2);

                    if (password == Login.password_login)
                    {
                        Response.Cookies.Append("cookie_auth", userID);
                        Response.Cookies.Append("cookie_fname", fname);
                        return RedirectToPage("/Index", new { Status_A = "Success" });
                    }
                }
            }
            connection.Close();

        }
        return RedirectToPage("/authentication/addLogin", new { Status_A = "Invalid email or password." });
    }
}