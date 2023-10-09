using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Controllers
{
  public class TagsController : Controller
  {
    private readonly ToDoListContext _db;

    public TagsController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Tags.ToList()); //take database full of tags and make it into a list
    }

    public ActionResult Details(int id)
    {
      Tag thisTag = _db.Tags //for this tag in the list of tags in the datatable.
          .Include(tag => tag.JoinEntities) //find its join table properties 
          .ThenInclude(join => join.Item) //Find the items corresponding to this tag based on the ids found 
          .FirstOrDefault(tag => tag.TagId == id); // find the item with the tags that are found in the join table
      return View(thisTag);
    }

  }
}