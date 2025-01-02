using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryShipmentMgmtUI.Model
{
    public class ProductApiResponse
    {
        public int statusCode { get; set; }
        public bool status { get; set; }
        public string responseMessage { get; set; }
        public List<ProductResponse> data { get; set; }     
    }

    public class ProductResponse
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int Quantity { get; set; }
        public decimal price { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
