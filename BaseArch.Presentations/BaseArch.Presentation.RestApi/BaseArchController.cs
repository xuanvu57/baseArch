using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseArch.Presentation.RestApi
{
    /// <summary>
    /// Base controller
    /// </summary>
    [ApiController]
    [Authorize]
    public abstract class BaseArchController : ControllerBase
    {
    }
}
