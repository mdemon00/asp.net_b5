using Autofac;
using DataImporter.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "Member")]
    [ApiController]
    public class GroupApiController : ControllerBase
    {
        private readonly ILogger<GroupApiController> _logger;
        private readonly ILifetimeScope _scope;

        public GroupApiController(ILogger<GroupApiController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var model = _scope.Resolve<GroupListModel>();

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
