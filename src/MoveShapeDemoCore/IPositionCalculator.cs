using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{
    public interface IPositionCalculator
    {

        Dictionary<int, BallInfo> GetBalls();
        int AddBall();
        void RemoveBall(int id);
        int GetBallCount();
        void UpdateBallPosition();

        event PositionCalculator.OnBallRemove OnBallRemoveCallback;
        event PositionCalculator.OnUpdateBallInfo OnBallInfoUpdateCallback;
    }
}
