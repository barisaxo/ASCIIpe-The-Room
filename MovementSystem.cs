using System;
using System.Text;
using System.Collections.Generic;

namespace ASCIIpe_the_room
{
    public static class MovementSystem
    {

        /// <summary>
        /// Moves the player in the given direction, if possible.
        /// </summary>
        /// <param name="playerPos">Current Player Position</param>
        /// <param name="direction">Direction</param>
        /// <param name="dungeon">Map</param>
        /// <returns>New Player position</returns>
        public static Program.PlayerPosition NewPlayerPosition(Program.PlayerPosition playerPos, Program.Direction direction, Program.Dungeon dungeon)
        {
            if (MoveIsLegal(playerPos, direction, dungeon))
            {
                switch (direction)
                {
                    case Program.Direction.North:
                        playerPos.Y -= 1; break;
                    case Program.Direction.South:
                        playerPos.Y += 1; break;
                    case Program.Direction.East:
                        playerPos.X += 1; break;
                    case Program.Direction.West:
                        playerPos.X -= 1; break;
                }
            }
            return playerPos;
        }

        /// <summary>
        /// Check if the move is legal.
        /// </summary>
        /// <param name="playerPos">Player Position</param>
        /// <param name="direction">Selected Direction</param>
        /// <param name="dungeon">Map</param>
        /// <returns></returns>
        public static bool MoveIsLegal(Program.PlayerPosition playerPos, Program.Direction direction, Program.Dungeon dungeon)
        {
            switch (direction)
            {
                case Program.Direction.North:
                    if (0 > playerPos.Y - 1) { return false; }
                    break;
                case Program.Direction.South:
                    if (dungeon.Rooms.GetLength(1) - 1 < playerPos.Y + 1) { return false; }
                    break;
                case Program.Direction.East:
                    if (dungeon.Rooms.GetLength(0) - 1 < playerPos.X + 1) { return false; }
                    break;
                case Program.Direction.West:
                    if (0 > playerPos.X - 1) { return false; }
                    break;
            }
            return dungeon.Rooms[playerPos.X, playerPos.Y].Doors.Contains(direction);
        }


    }
}

