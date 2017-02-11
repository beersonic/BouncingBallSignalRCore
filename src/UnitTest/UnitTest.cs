using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MoveShapeDemoCore;

namespace UnitTest
{
    public class UnitTest
    {
        [Fact]
        public void TestAddBall()
        {
            IPositionCalculator pc = PositionCalculatorFactory.Instance.GetPositionCalculator();
            int nBall1 = pc.GetBallCount();
            pc.AddBall();
            int nBall2 = pc.GetBallCount();

            Assert.Equal(1, nBall2 - nBall1);
        }

        [Fact]
        public void TestRemoveBall()
        {
            IPositionCalculator pc = PositionCalculatorFactory.Instance.GetPositionCalculator();
            int id = pc.AddBall();

            int nBall1 = pc.GetBallCount();

            pc.RemoveBall(id);

            int nBall2 = pc.GetBallCount();

            Assert.Equal(1, nBall1 - nBall2);
        }
    }
}