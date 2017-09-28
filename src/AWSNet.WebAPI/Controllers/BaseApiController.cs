using AWSNet.Dtos;
using System.Web.Http;

namespace AWSNet.WebAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected object AWSNetResponse(object o, PaginationDto p = null)
        {
            dynamic response = new System.Dynamic.ExpandoObject();
            response.data = o;
            response.pagination = p;

            return response;
        }

        protected void LoadPagination(PaginationDto pagination, int count)
        {
            pagination.Total = count;

            if (pagination.PageNumber != null && pagination.PageSize != null)
            {
                if (pagination.PageNumber > 1)
                    pagination.Previous = string.Format("{0}?PageNumber={1}&PageSize={2}", Request.RequestUri.AbsolutePath, pagination.PageNumber - 1, pagination.PageSize);
                else
                    pagination.Previous = null;

                if (count > (pagination.PageNumber * pagination.PageSize))
                    pagination.Next = string.Format("{0}?PageNumber={1}&PageSize={2}", Request.RequestUri.AbsolutePath, pagination.PageNumber + 1, pagination.PageSize);
                else
                    pagination.Next = null;
            }
        }
    }
}