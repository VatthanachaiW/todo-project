using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Todo.API.Controllers.V2_0
{
    /// <summary>
    /// Todo API Controller
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class TodoController
    {
        /// <summary>
        /// Method นี้ยังไม่ได้เขียนโค๊ดเลยนะ 
        /// </summary>
        /// <returns>NotImplementedException</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}