using Microsoft.AspNetCore.Mvc;
using Model1.Models;
using System.Net;

namespace Model1.Controllers
{
    [ApiController]
    public class RestaurantController(MapDbContext db) : ControllerBase
    {
        [HttpGet("system/api/restaurant/list")]
        public ApiResult List(int pageNum = 1
            ,int pageSize = 10
            ,string? spotName = null
            ,string? address = null)
        {
            var q = db.ScenicSpots.AsQueryable(); //ใช้เชื่อมต่อกับตาราง Restaurant
            if (!string.IsNullOrEmpty(spotName))
                q = q.Where(x => x.SpotName!.Contains(spotName));
            if (!string.IsNullOrEmpty(address)) q = q.Where(x => x.Address!.Contains(address));
            int total = q.Count();
            var rows = q.OrderBy(x => x.SpotId)
            .Skip((pageNum - 1) * pageSize).Take(pageSize).ToList()
            .Select(x => new {
                x.SpotId,
                x.SpotName,
                x.Address,
                x.Phone,
                latitude = x.Latitude.ToString(),
                longitude = x.Longitude.ToString(),
                rating = x.Rating.ToString(),
                ticketPrice = x.TicketPrice.ToString(),
                x.OpeningHours,
                x.Description,
                x.ImageUrl,
                distance = x.Distance.ToString(),
                businessType = "scenic"
            });
            return ApiResult.Ok(new { total, rows });
        }
    }
}
