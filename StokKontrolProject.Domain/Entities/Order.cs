using StokKontrolProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public Status Status { get; set; }

        //Navigation Property
        //Bir siparişin bir kullanıcı olur
        //Bir siparişin birden fazla sipariş detayı olur.
        public virtual User? User { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
