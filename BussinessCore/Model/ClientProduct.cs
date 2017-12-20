using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessCore.Model
{
    public class ClientProduct
    {
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
