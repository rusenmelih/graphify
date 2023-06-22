using Business.Abstract;
using Entity.FTM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graphify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkpageController : ControllerBase
    {
        private readonly IWorkPageService workPageService;
        private readonly IAuthenticationService authenticationService;

        public WorkpageController(IWorkPageService workPageService, IAuthenticationService authenticationService)
        {
            this.workPageService = workPageService;
            this.authenticationService = authenticationService;
        }


        [Authorize(Roles = "USER")]
        [HttpPost]
        [Route("create/new-workpage")]
        public async Task<IActionResult> CreateNewWorkpage([FromBody] NewWorkpageRequest workpage)
        {
            int ownerID = this.authenticationService.ReadUserIdentityData().Sid;
            var result = await this.workPageService.NewWorkPage(ownerID, workpage.PageName);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "USER")]
        [HttpPut]
        [Route("create/node")]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest node)
        {
            var result = await this.workPageService.CreateNode(node);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "USER")]
        [HttpPut]
        [Route("create/edge")]
        public async Task<IActionResult> CreateEdge([FromBody] CreateEdgeRequest edge)
        {
            var result = await this.workPageService.CreateEdge(edge);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "USER")]
        [HttpGet]
        [Route("get/workpages")]
        public async Task<IActionResult> GetWorkpages()
        {
            int ownerID = this.authenticationService.ReadUserIdentityData().Sid;
            var result = await this.workPageService.GetWorkpageTemplates(ownerID);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "USER")]
        [HttpGet]
        [Route("get/workpage")]
        public async Task<IActionResult> GetWorkPage([FromQuery] string identifier)
        {
            var result = await workPageService.GetWorkpageByIdentifier(identifier);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "USER")]
        [HttpGet]
        [Route("get/run")]
        public async Task<IActionResult> Run([FromQuery] string identifier)
        {
            var result = workPageService.RunGeneticAlgorithm(identifier);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

    }
}
