using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KoujoNet
{
    
    public class Test_ActivatesFactory
    {
        [Fact]
        public void TestFactory()
        {
            var Half = Activates.Create(ActivateType.HalfActivate);
            Assert.True(Half.Type == ActivateType.HalfActivate);
            var Heviside = Activates.Create(ActivateType.Heviside);
            Assert.True(Heviside.Type == ActivateType.Heviside);
            var HyperbolicTangent = Activates.Create(ActivateType.HyperbolicTangent);
            Assert.True(HyperbolicTangent.Type == ActivateType.HyperbolicTangent);
            var InverseSquareRoot = Activates.Create(ActivateType.InverseSquareRoot);
            Assert.True(InverseSquareRoot.Type == ActivateType.InverseSquareRoot);
            var Sigmoid = Activates.Create(ActivateType.Sigmoid);
            Assert.True(Sigmoid.Type == ActivateType.Sigmoid);
        }
    }

}
