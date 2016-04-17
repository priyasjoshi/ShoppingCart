using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class InfoModel
    {
        public InfoModel() { }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public InfoModel(long Id, string Name, string Email) {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
        }
       
    }
}