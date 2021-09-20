using DataImporter.Areas.Member.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupApiController : ControllerBase
    {
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var model = new GroupListModel();

                var names = model.GetAllGroups();
                var data = names.Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
                return Ok(data);
            }
            else
            {
                return Ok();
            }
        }
    }
}
