using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Policy = "AccessPermission")]
    public class GroupController : ControllerBase
    {
    }
}
