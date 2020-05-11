using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map.Enums
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public static class DirectionExtensions
    {
        public static Direction Reverse(Direction direction)
        {
            switch (direction)
            {
                case North : return South;
                case South : return North;
                case East : return West;
                case West : return East;
            }

            return direction;
        }
    }
}

