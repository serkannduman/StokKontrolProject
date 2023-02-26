using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }

        //Navigation Properties
        //Bir ürünün bir kategorisi olur
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }

        //Bir ürünün bir tedarikçisi olur.

        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; } // virtual'ı ilerde lazy loading'de kullanırsak diye yaptık yapmasakta olur.

        //Bir ürün birden fazla sipariş detayında geçebilir
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
