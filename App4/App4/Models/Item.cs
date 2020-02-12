using SQLite;
using System;

namespace App4.Models
{
    public class Item
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}