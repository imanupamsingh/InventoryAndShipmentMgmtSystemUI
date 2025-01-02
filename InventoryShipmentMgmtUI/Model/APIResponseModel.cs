using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryShipmentMgmtUI.Model
{
    public class APIResponseModel
    {
        public int statusCode { get; set; }
        public bool status { get; set; }
        public string responseMessage { get; set; }
        public string data { get; set; }
    }
}
