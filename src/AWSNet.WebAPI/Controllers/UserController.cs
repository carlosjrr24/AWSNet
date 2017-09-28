using AWSNet.WebAPI.Extensions;
using AWSNet.Utils.Solr;
using AWSNet.Dtos;
using AWSNet.Managers;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AWSNet.WebAPI.Controllers
{
    [AWSNetAuthorize]
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {
        private IUserManager _manager;

        public UserController(IUserManager manager)
        {
            _manager = manager;
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var user = await _manager.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(AWSNetResponse(user));
        }

        public async Task<IHttpActionResult> GetAll([FromUri] PaginationDto pagination)
        {
            LoadPagination(pagination, await _manager.Count());
            return Ok(AWSNetResponse(await _manager.GetAll(pagination.Skip(), pagination.Take()), pagination));
        }

        [HttpGet]
        [Route("GetAllSolr")]
        public async Task<IHttpActionResult> GetAllSolr()
        {
            return Ok(await SolrHelper.ExecuteQuery(SolrCore.USER, HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.ToString())));
        }

        public async Task<IHttpActionResult> Put(CreateUserDtoBindingModel userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _manager.Update(userDto);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateProfileImage")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var userId = RequestContext.Principal.Identity.GetUserId<int>();
            var user = await _manager.GetById(userId);

            var httpRequest = System.Web.HttpContext.Current.Request;

            if (httpRequest.Files != null && httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                user.ProfileImagePath = ImageStorage.ImageStoreHelper.UpdateUserProfileImage(user.ProfileImagePath, postedFile);
                await _manager.SetProfileImagePath(user);

                return Ok();
            }

            ModelState.AddModelError("Image", "image is required");
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("Enable")]
        public async Task<IHttpActionResult> Enable(UpdateUserDtoBindingModel userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _manager.Enable(userDto.Id);

            return Ok();
        }

        [HttpPut]
        [Route("Disable")]
        public async Task<IHttpActionResult> Disable(UpdateUserDtoBindingModel userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = RequestContext.Principal.Identity.GetUserId<int>();

            if (currentUserId == userDto.Id)
            {
                ModelState.AddModelError("Model", "You can not disable yourself");
                return BadRequest(ModelState);
            }

            await _manager.Disable(userDto.Id);

            return Ok();
        }

        public async Task<IHttpActionResult> Delete(UpdateUserDtoBindingModel userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = RequestContext.Principal.Identity.GetUserId<int>();

            if (currentUserId == userDto.Id)
            {
                ModelState.AddModelError("Model", "You can not delete yourself");
                return BadRequest(ModelState);
            }

            await _manager.Delete(userDto);

            return Ok();
        }
    }
}
