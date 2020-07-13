using System;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map.Enums {
    public enum Direction {
        North,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    public static class DirectionExtensions {
        public static Direction Reverse(Direction direction) {
            switch (direction) {
                case North: return South;
                case South: return North;
                case East: return West;
                case West: return East;
                case NorthEast: return SouthWest;
                case SouthWest: return NorthEast;
                case NorthWest: return SouthEast;
                case SouthEast: return NorthWest;
            }

            return direction;
        }

        public static Direction[] GetCardinals() => new[] { North, East, South, West };

        public static bool IsVerticalDirection(Direction direction) {
            switch (direction) {
                case North: return true;
                case South: return true;
                case East: return false;
                case West: return false;
            }

            return false;
        }

        public static Direction GetDiagonalFromCardinals(Direction first, Direction second) {
            if (IsVerticalDirection(first))
                return (Direction)Enum.Parse(typeof(Direction), $"{first.ToString()}{second.ToString()}");
            else
                return (Direction)Enum.Parse(typeof(Direction), $"{second.ToString()}{first.ToString()}");
        }

        public static Direction[] GetRightAngleDirections(Direction direction) {
            switch (direction) {
                case North: return new []{ East, West };
                case South: return new []{ East, West };
                case East: return new []{ North, South };
                case West: return new []{ North, South };
            }

            return new []{ direction, direction };
        }
    }
}