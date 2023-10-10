using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ToDoList.Models
{
  public class Item
  {
    public int ItemId { get; set; }
    [Required(ErrorMessage = "The item's description can't be empty!")]
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "You must add your item to a category. Have you created a category yet?")] //sets a minimum value for the category id as 1, meaning that it must be a nonzero number. If it, the we have not made any categories to add to our item.
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<ItemTag> JoinEntities { get; }
  }
}