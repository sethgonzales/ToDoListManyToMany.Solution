namespace ToDoList.Models
{
  public class ItemTag //join entity that relates the item and tag objects 
    {       
        public int ItemTagId { get; set; }  //id of each row in the itemtag table
        public int ItemId { get; set; } //get item id
        public Item Item { get; set; } //find item object
        public int TagId { get; set; } //get tag id
        public Tag Tag { get; set; } //find tag object
    }
}