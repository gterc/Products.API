using Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    public class APIControllerBase : ControllerBase
    {
        public ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return result.Value != null ? Ok(result.Value) : Ok(); //TODO:Add Created
            else if(!result.IsSuccess)
                return NotFound();
            else
                return BadRequest();
        }
    }
}