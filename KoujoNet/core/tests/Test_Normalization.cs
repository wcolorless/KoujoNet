using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace KoujoNet
{
    public class Test_Normalization
    {
        [Fact]
        public void TestNormalization()
        {
            double[] Numbers = new double[] { 2, 5, 6, 8, 9 };
            var norm = Normalization.Go(Numbers);
            for (int i = 0; i < norm.Length; i++) Assert.True(norm[i] <= 1);
        }
    }
}
