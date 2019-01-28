using System.Collections.Generic;
using RestaUm.Model;

namespace RestaUm.Stock
{
    public class StockContext
    {
        public static int Round { get; set; }
        public static Board Board { get; set; }
        public static List<Record> Records = new List<Record>();
    }
}
