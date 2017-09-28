using AWSNet.WebAPI.Extensions;
using AWSNet.Utils.Solr;
using AWSNet.Dtos;
using AWSNet.Managers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AWSNet.WebAPI.Controllers
{
    [AWSNetAuthorize]
    [RoutePrefix("api/Category")]
    public class CategoryController : BaseApiController
    {
        private ICategoryManager _manager;

        public CategoryController(ICategoryManager manager)
        {
            _manager = manager;
        }

        public async Task<IHttpActionResult> Get(int id, bool includeMedia = false)
        {
            var category = await _manager.GetById(id, includeMedia);

            if (category == null)
                return NotFound();

            return Ok(AWSNetResponse(category));
        }

        public async Task<IHttpActionResult> GetAll([FromUri] PaginationDto pagination, bool includeMedia = false)
        {
            LoadPagination(pagination, await _manager.Count());
            return Ok(AWSNetResponse(await _manager.GetAll(pagination.Skip(), pagination.Take(), includeMedia), pagination));
        }

        [HttpGet]
        [Route("GetAllSolr")]
        public async Task<IHttpActionResult> GetAllSolr()
        {
            return Ok(await SolrHelper.ExecuteQuery(SolrCore.CATEGORY, HttpUtility.UrlDecode(HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.ToString()))));
        }

        public async Task<IHttpActionResult> Post(CreateCategoryDtoBindingModel categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _manager.Add(categoryDto);

            return Ok(AWSNetResponse(result));
        }

        public async Task<IHttpActionResult> Put(UpdateCategoryDtoBindingModel categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _manager.Update(categoryDto);

            return Ok();
        }

        public async Task<IHttpActionResult> Delete(DeleteCategoryDtoBindingModel categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _manager.Delete(categoryDto);

            return Ok();
        }
    }
}
