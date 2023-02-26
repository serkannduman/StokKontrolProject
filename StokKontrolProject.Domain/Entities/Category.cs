using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products= new List<Product>();
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation Property
        //Bir kategorinin birden fazla ürünü olabilir.
        public virtual List<Product> Products { get; set; }
    }
}
