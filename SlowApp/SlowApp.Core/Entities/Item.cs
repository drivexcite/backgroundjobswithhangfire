using System;

namespace SlowApp.Core.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemText { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
