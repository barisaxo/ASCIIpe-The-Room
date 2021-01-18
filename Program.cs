using System;
using System.Text;
using System.Collections.Generic;

namespace ASCIIpe_the_room
{
    public class Program
    {
        public enum Direction { North, South, East, West }

        public struct Room
        {
            public string Description;
            public HashSet<Direction> Doors;
            public HashSet<Direction> OpenedDoors;
            public bool HasVisited;
        }

        public struct Dungeon
        {
            public Room[,] Rooms;
            public Tuple<int, int> SpawnPoint;
        }

        public struct PlayerPosition
        { public int X, Y; }

        public static string[][] writingOnTheWall = new string[3][];

        static void Main()
        {
            string descriptionStartRoom = "What is this place... How did I get here?";

            writingOnTheWall[0] = new string[3];
            writingOnTheWall[0][0] = "You ";
            writingOnTheWall[0][1] = "I ";
            writingOnTheWall[0][2] = "They ";
            writingOnTheWall[1] = new string[3];
            writingOnTheWall[1][0] = "can't ";
            writingOnTheWall[1][1] = "wont ";
            writingOnTheWall[1][2] = "shouldn't ";
            writingOnTheWall[2] = new string[3];
            writingOnTheWall[2][0] = "even try.";
            writingOnTheWall[2][1] = "be allowed to live.";
            writingOnTheWall[2][2] = "touch this.";

            int rI;
            int rX, rY;
            int rA, rB, rC;

            Random RNG = new();
            rI = RNG.Next(1, 3);
            rX = RNG.Next(3, 10);
            rY = RNG.Next(3, 7);

            Dungeon dungeon = new();
            dungeon.Rooms = new Room[rX, rY];
            dungeon.SpawnPoint = new(0, 0);

            Direction startRoomDoor;
            if (rI == 1)
            { startRoomDoor = Direction.East; }
            else
            { startRoomDoor = Direction.South; }

            Room startRoom = new()
            {
                Description = descriptionStartRoom,
                Doors = new() { startRoomDoor },
                OpenedDoors = new(),
                HasVisited = true
            };

            for (int x = 0; x < dungeon.Rooms.GetLength(0); x++)
            {
                for (int y = 0; y < dungeon.Rooms.GetLength(1); y++)
                {
                    if (x == 0 && y == 0)
                    { dungeon.Rooms[x, y] = startRoom; }
                    else
                    {//New Room with Random Doors !!Some Rooms may not be accessable in current configuration. ie. not always solvable!!
                        rI = RNG.Next(11, 15);
                        Direction[] dirs;
                        dirs = IntToDirections(rI);

                        rA = RNG.Next(0, 3);
                        rB = RNG.Next(0, 3);
                        rC = RNG.Next(0, 3);

                        dungeon.Rooms[x, y] = new()
                        {
                            Description = new string("Theres some writing on the wall... " + "\"" +
                            writingOnTheWall[0][rA] + writingOnTheWall[1][rB] + writingOnTheWall[2][rC] + "\""),
                            Doors = new(),
                            OpenedDoors = new(),
                            HasVisited = false
                        };

                        foreach (Direction dir in dirs)
                        {
                            if ((dir == Direction.West && x == 0) ||
                                (dir == Direction.East && x == dungeon.Rooms.GetLength(0) - 1) ||
                                (dir == Direction.North && y == 0) ||
                                (dir == Direction.South && y == dungeon.Rooms.GetLength(1) - 1))
                            { /* No Doors around the perimeter */ }
                            else
                            { dungeon.Rooms[x, y].Doors.Add(dir); }
                        }
                    }
                }
            }

            PlayerPosition playerPos = new() { X = dungeon.SpawnPoint.Item1, Y = dungeon.SpawnPoint.Item2 };

            MapMaker.PrintMap(dungeon, playerPos);
        }

        /// <summary>
        /// Converts the random int to an array of directions
        /// </summary>
        /// <param name="i">Random Interger</param>
        /// <returns>Array of Directions</returns>
        public static Direction[] IntToDirections(int i)
        {
            Direction[] dir = new Direction[3];
            switch (i)
            {
                #region NotVeryGoodDirections - Might Delete Later
                //case 1: dir[0] = Direction.North; break;
                //case 2: dir[0] = Direction.East; break;
                //case 3: dir[0] = Direction.South; break;
                //case 4: dir[0] = Direction.West; break;

                //case 5:
                //    dir = new Direction[2];
                //    dir[0] = Direction.North;
                //    dir[1] = Direction.East;
                //    break;
                //case 6:
                //    dir = new Direction[2];
                //    dir[0] = Direction.North;
                //    dir[1] = Direction.South;
                //    break;
                //case 7:
                //    dir = new Direction[2];
                //    dir[0] = Direction.North;
                //    dir[1] = Direction.West;
                //    break;
                //case 8:
                //    dir = new Direction[2];
                //    dir[0] = Direction.East;
                //    dir[1] = Direction.South;
                //    break;
                //case 9:
                //    dir = new Direction[2];
                //    dir[0] = Direction.East;
                //    dir[1] = Direction.West;
                //    break;
                //case 10:
                //    dir = new Direction[2];
                //    dir[0] = Direction.South;
                //    dir[1] = Direction.West;
                //    break;
                #endregion

                case 11:
                    //dir = new Direction[3];
                    dir[0] = Direction.North;
                    dir[1] = Direction.East;
                    dir[2] = Direction.South;
                    break;
                case 12:
                    // dir = new Direction[3];
                    dir[0] = Direction.North;
                    dir[1] = Direction.East;
                    dir[2] = Direction.West;
                    break;
                case 13:
                    //dir = new Direction[3];
                    dir[0] = Direction.North;
                    dir[1] = Direction.West;
                    dir[2] = Direction.South;
                    break;
                case 14:
                    // dir = new Direction[3];
                    dir[0] = Direction.West;
                    dir[1] = Direction.East;
                    dir[2] = Direction.South;
                    break;
            }
            return dir;
        }

        /// <summary>
        /// Waits for user input and then issues the appropriate command.
        /// </summary>
        /// <param name="dungeon">Dungeon</param>
        /// <param name="playerPos">Player Position</param>
        public static void Input(Dungeon dungeon, PlayerPosition playerPos)
        {
            var c = Console.ReadKey();
            var oldPos = playerPos;
            switch (c.KeyChar)
            {
                case 'w':
                    playerPos = MovementSystem.NewPlayerPosition(playerPos, Direction.North, dungeon); break;
                case 'a':
                    playerPos = MovementSystem.NewPlayerPosition(playerPos, Direction.West, dungeon); break;
                case 's':
                    playerPos = MovementSystem.NewPlayerPosition(playerPos, Direction.South, dungeon); break;
                case 'd':
                    playerPos = MovementSystem.NewPlayerPosition(playerPos, Direction.East, dungeon); break;
                case 'q':
                    Console.Clear();
                    Console.Write("There is no escape...");
                    Console.ReadKey();
                    Console.Clear();
                    return;

                    //fine(end)
            }

            #region OpenDoorHandler
            if (oldPos.X < playerPos.X)
            {
                if (!dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Contains(Direction.East))
                { dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Add(Direction.East); }

                if (dungeon.Rooms[playerPos.X, playerPos.Y].Doors.Contains(Direction.West))
                {
                    if (!dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Contains(Direction.West))
                    { dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Add(Direction.West); }
                }
            }
            else if (oldPos.X > playerPos.X)
            {
                if (!dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Contains(Direction.West))
                { dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Add(Direction.West); }

                if (dungeon.Rooms[playerPos.X, playerPos.Y].Doors.Contains(Direction.East))
                {
                    if (!dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Contains(Direction.East))
                    { dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Add(Direction.East); }
                }
            }
            else if (oldPos.Y < playerPos.Y)
            {
                if (!dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Contains(Direction.South))
                { dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Add(Direction.South); }

                if (dungeon.Rooms[playerPos.X, playerPos.Y].Doors.Contains(Direction.North))
                {
                    if (!dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Contains(Direction.North))
                    { dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Add(Direction.North); }
                }
            }
            else if (oldPos.Y > playerPos.Y)
            {
                if (!dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Contains(Direction.North))
                { dungeon.Rooms[oldPos.X, oldPos.Y].OpenedDoors.Add(Direction.North); }

                if (dungeon.Rooms[playerPos.X, playerPos.Y].Doors.Contains(Direction.South))
                {
                    if (!dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Contains(Direction.South))
                    { dungeon.Rooms[playerPos.X, playerPos.Y].OpenedDoors.Add(Direction.South); }
                }
            }
            #endregion 

            dungeon.Rooms[playerPos.X, playerPos.Y].HasVisited = true;

            MapMaker.PrintMap(dungeon, playerPos);
        }



    }
}
