using System;
namespace RestaUm.Ultil
{
    public class NumberUtil
    {
        public static int Diff(int one, int two) {
            return one > two ? one - two : two - one;
        }
    }
}
