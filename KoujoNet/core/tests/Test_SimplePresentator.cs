using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KoujoNet
{
    public class Test_SimplePresentator
    {
        [Fact]
        public void TestPresentator()
        {
            double[] Numbers = new double[] { 0.5, 0.1, 0.3, 0.8 };
            var output = SimplePresentator.Out(Numbers);
            Assert.True(output == 3);
        }
    }
}
