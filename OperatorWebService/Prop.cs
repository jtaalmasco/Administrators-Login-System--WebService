using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperatorWebService
{
    public class Prop
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public string OperatorName { get; set; }
        public string Time { get; set; }
        public string OperatingType { get; set; }
        public string Domain { get; set; }
    }
}