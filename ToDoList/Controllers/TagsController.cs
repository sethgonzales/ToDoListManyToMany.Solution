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
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Tag tag)
    {
      _db.Tags.Add(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddItem(int id) //get action when we are directed to the page. Pass in the id of the tag
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id); //find the tag based on its id
      ViewBag.ItemId = new SelectList(_db.Items, "ItemId", "Description"); //collect the item id and its description to create a dropdown menu of items and their ids that we can add to our tag.
      return View(thisTag); //association with the tag and the item, passed through to the page 
    }

    [HttpPost] //submit the action to the database
    public ActionResult AddItem(Tag tag, int itemId)
    {
#nullable enable
      ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.ItemId == itemId && join.TagId == tag.TagId)); //enable the ability for our joinEntity variable to be null. State that our variable is ItemTag, add the ? to again state that this can be nullable. The joinEntity variable will either be an itemtag object (meaning the relationship between the item and tag exists in the join table, with an item id and tag id), or it will be returned as null, meaning that there were no entries. disable nullability once finished.
#nullable disable
      if (joinEntity == null && itemId != 0) //itemId != 0 means the user has selected an item from the dropdown. joinEntity is the variable created before. It has to be null meaning the relationship must not exist.
      {
        _db.ItemTags.Add(new ItemTag() { ItemId = itemId, TagId = tag.TagId }); //make a new item tag entry into the database, with the item id and tag id
        _db.SaveChanges(); //save the entry into the database
      }
      return RedirectToAction("Details", new { id = tag.TagId }); //do to the details page passing in the id of the tag we were looking at
    }

    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult Edit(Tag tag)
    {
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      _db.Tags.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      ItemTag joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
      _db.ItemTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}