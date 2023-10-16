using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, ToDoListContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
    //these actions will only happen if there is currently a logged in user. If not, these wont happen.
      // Category logic
      Category[] cats = _db.Categories.ToArray();
      Dictionary<string, object[]> model = new Dictionary<string, object[]>(); //dictionaty will hold our category and item information
      model.Add("categories", cats);
      // Item logic
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId); //find the current user
      if (currentUser != null) //if there is a current user, show the items in the categories
      {
        Item[] items = _db.Items
                    .Where(entry => entry.User.Id == currentUser.Id)
                    .ToArray();
        model.Add("items", items);
      }
      return View(model);
    }
  }
}