using Microsoft.AspNetCore.Antiforgery;
using ZCYX.FRMSCore.Controllers;

namespace ZCYX.FRMSCore.Web.Host.Controllers
{
    public class AntiForgeryController : FRMSCoreControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
