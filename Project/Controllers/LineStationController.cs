//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Project.Models;

//public class LineStationsController : Controller
//{
//    private readonly HanoiMetroDbContext _context;

//    public LineStationsController(HanoiMetroDbContext context)
//    {
//        _context = context;
//    }

 
//    public async Task<IActionResult> Index(string? lineId)
//    {
//        var query = _context.LineStations
//            .Include(ls => ls.Line)
//            .Include(ls => ls.Station)
//            .AsQueryable();

//        if (!string.IsNullOrEmpty(lineId))
//        {
//            query = query.Where(ls => ls.LineID == lineId);
//        }

//        var lineStations = await query
//            .OrderBy(ls => ls.LineID)
//            .ThenBy(ls => ls.OrderIndex)
//            .ToListAsync();

//        return View(lineStations);
//    }
//}