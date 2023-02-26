using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }

        //Navigation Properties
        //Bir sipariş detayınınn 1 ürünü olur
        //Bir sipariş detayınınn 1 siparişi olur olur
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
