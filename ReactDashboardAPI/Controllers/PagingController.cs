using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ReactDashboardAPI.Controllers
{
    [Route("api/paging")]
    [ApiController]
    public class PagingController : ControllerBase
    {
        private PaginationModel paginationModel;
        private List<DetailsListData> detailsList = new List<DetailsListData>();
        public PagingController()
        {
            for (int i = 0; i < 307; i++)
            {
                var detailsData = new DetailsListData
                {
                    Id = i.ToString(), //Guid.NewGuid().ToString(),
                    Name = "Test--" + i,
                    CreateDate = DateTime.UtcNow.AddHours(-i)
                };
                detailsList.Add(detailsData);
            }
        }

        [HttpGet]
        [Route("items")]
        public ActionResult<IEnumerable<PaginationModel>> PaginationRequest([FromQuery] int pageSize, int pageIndex, string direction)
        {
            var totalPage = detailsList.Count / pageSize;
            var totalItems = detailsList.Count;
            if (pageSize < 1 || pageIndex < 1)
            {
                return NotFound();
            }
            var paginationResult = detailsList.OrderBy(o => o.CreateDate).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            paginationModel = new PaginationModel()
            {
                content = paginationResult,
                last = true,
                totalPages = totalPage,
                totalElements = totalItems,
                sort = new List<Sort>()
                {
                    new Sort(){
                        direction = direction,
                        property = "id",
                        ascending = false,
                        descending = true,
                    }
                },
                number = 0,
                numberOfElement = 2,
                size = pageSize,
                first = true
            };

            return Ok(paginationModel);
        }
    }

    public class PaginationModel
    {
        public List<DetailsListData> content { get; set; }
        public bool last { get; set; }
        public int totalPages { get; set; }
        public int totalElements { get; set; }
        public List<Sort> sort { get; set; } = new List<Sort>();
        public int numberOfElement { get; set; }
        public bool first { get; set; }
        public int size { get; set; }
        public int number { get; set; }

    }
    public class DetailsListData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class Sort
    {
        public string direction { get; set; }
        public string property { get; set; }
        public bool ascending { get; set; }
        public bool descending { get; set; }
    }
}