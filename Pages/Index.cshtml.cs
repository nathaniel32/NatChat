﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty (SupportsGet = true)]
        public string Status_A { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Status_A))
            {
                Status_A = "";
            }
        }

    }
}
