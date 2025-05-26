using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRChat.Pages
{
    public class chatModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public chatModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty (SupportsGet = true)]
        public string Status_A { get; set; }

        [BindProperty (SupportsGet = true)]
        public string fnameChat { get; set; }

        [BindProperty (SupportsGet = true)]
        public string lnameChat { get; set; }

        [BindProperty (SupportsGet = true)]
        public string uIDChat { get; set; }

        public void OnGet()
        {
            if (!(Request.Cookies["cookie_auth"] != null)) {
                Response.Redirect("/authentication/addLogin?status_a=Please Login First");
            }
            
            if (string.IsNullOrEmpty(Status_A))
            {
                Status_A = "";
            }
            if (string.IsNullOrEmpty(fnameChat))
            {
                fnameChat = "";
            }

            if (string.IsNullOrEmpty(lnameChat))
            {
                lnameChat = "";
            }

            if (string.IsNullOrEmpty(uIDChat))
            {
                uIDChat = "";
            }
        }
    }
}
