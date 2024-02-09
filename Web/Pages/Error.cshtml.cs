using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        #region Fields
        private readonly ILogger<ErrorModel> _logger;
        #endregion

        #region Properties
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
        #endregion

        #region Constructors
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Publics
        public void OnGet()
        {
            this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
        }
        #endregion
    }
}