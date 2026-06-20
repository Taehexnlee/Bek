using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> SearchItems([FromQuery] SearchParams searchParams)
        {
            var query = DB.PagedSearch<Item, Item>();
            query.Sort(x => x.Ascending(i => i.Make));

            if(!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(i => i.Make)),
                "new" => query.Sort(x => x.Ascending(i => i.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)) 
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(i => i.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(i => i.AuctionEnd < DateTime.UtcNow.AddHours(6) 
                    && i.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
            };

            if(!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(i => i.Seller.ToLower() == searchParams.Seller.ToLower());
            }
            if(!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(i => i.Winner.ToLower() == searchParams.Winner.ToLower());
            }

            query.PageNumber(searchParams.PageNumber).PageSize(searchParams.PageSize);
            var result = await query.ExecuteAsync();

            return Ok(new
            { 
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
                
            });
                
        }
    }
}
