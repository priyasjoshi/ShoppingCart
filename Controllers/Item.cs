using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models;
namespace ShoppingCart.Controllers
{
    public class Item
    {
        private ProductTable product = new ProductTable();
        private int quantity;
        public Item() { }
        public Item(ProductTable product, int quantity) {
            this.Product = product;
            this.quantity = quantity;

        }
        public ProductTable Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
            }
        }
        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }
    }
}