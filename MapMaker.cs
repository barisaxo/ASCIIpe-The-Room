using System;
using System.Text;

namespace ASCIIpe_the_room
{
    public static class MapMaker
    {

        public struct RoomGraphics
        {
            public string Top;
            public string Middle;
            public string Bottom;
        }

        /// <summary>
        /// Builds an ASCII dungeon of the explored area
        /// </summary>
        /// <param name="dungeon">Program.Map</param>
        /// <param name="playerPos">Program.PlayerPosition</param>
        public static void PrintMap(Program.Dungeon dungeon, Program.PlayerPosition playerPos)
        {
            bool foundTheKey = true;
            RoomGraphics[,] graphics = new RoomGraphics[dungeon.Rooms.GetLength(0), dungeon.Rooms.GetLength(1)];

            for (int x = 0; x < dungeon.Rooms.GetLength(0); x++)
            {
                for (int y = 0; y < dungeon.Rooms.GetLength(1); y++)
                {
                    char north, east, south, west, player;
                    north = '-'; east = '|'; south = '-'; west = '|'; player = ' ';

                    if (dungeon.Rooms[x, y].Doors.Contains(Program.Direction.North)) { north = '='; }
                    if (dungeon.Rooms[x, y].Doors.Contains(Program.Direction.East)) { east = '‡'; }
                    if (dungeon.Rooms[x, y].Doors.Contains(Program.Direction.West)) { west = '‡'; }
                    if (dungeon.Rooms[x, y].Doors.Contains(Program.Direction.South)) { south = '='; }

                    if (dungeon.Rooms[x, y].OpenedDoors.Contains(Program.Direction.North)) { north = '\\'; }
                    if (dungeon.Rooms[x, y].OpenedDoors.Contains(Program.Direction.East)) { east = '_'; }
                    if (dungeon.Rooms[x, y].OpenedDoors.Contains(Program.Direction.West)) { west = '_'; }
                    if (dungeon.Rooms[x, y].OpenedDoors.Contains(Program.Direction.South)) { south = '/'; }

                    if (playerPos.X == x && playerPos.Y == y) { player = 'P'; }

                    if (dungeon.Rooms[x, y].HasVisited)
                    {
                        graphics[x, y] = CreateRoomString(north: north, east: east,
                                                          south: south, west: west,
                                                          playerPos: player);
                    }
                    else
                    {
                        graphics[x, y] = InvisibleRoomString();
                        foundTheKey = false;
                    }
                }
            }

            StringBuilder sb = new();
            for (int y = 0; y < graphics.GetLength(1); y++)
            {
                for (int x = 0; x < graphics.GetLength(0); x++)
                {
                    sb.Append(graphics[x, y].Top);
                }
                sb.Append('\n');
                for (int x = 0; x < graphics.GetLength(0); x++)
                {
                    sb.Append(graphics[x, y].Middle);
                }
                sb.Append('\n');
                for (int x = 0; x < graphics.GetLength(0); x++)
                {
                    sb.Append(graphics[x, y].Bottom);
                }
                sb.Append('\n');
            }

            if (foundTheKey)
            {
                if (playerPos.X == 0 && playerPos.Y == 0)
                {
                    Console.Clear();
                    Console.Write("The magical orb in your hand starts to glow as " +
                        "the world around you slowly disolves..." +
                        "\nThere is no escape.");
                    Console.ReadKey();
                    Console.Clear();

                    //fine(end).
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("KEY: W = North, A = West, S = South, D = East, Q = QUIT.\n\n");
                    Console.WriteLine(sb.ToString());
                    Console.WriteLine($"\n\n{dungeon.Rooms[playerPos.X, playerPos.Y].Description}");
                    Console.WriteLine("\nYou found a magical orb!");
                    Program.Input(dungeon, playerPos);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("KEY: W = North, A = West, S = South, D = East, Q = QUIT.\n\n");
                Console.WriteLine(sb.ToString());
                Console.WriteLine($"\n\n{dungeon.Rooms[playerPos.X, playerPos.Y].Description}");
                Program.Input(dungeon, playerPos);
            }
        }

        public static RoomGraphics CreateRoomString(char north, char east, char south, char west, char playerPos)
        {
            return new() { Top = $"┌─{north}─┐", Middle = $"{west} {playerPos} {east}", Bottom = $"└─{south}─┘" };
        }
        public static RoomGraphics InvisibleRoomString()
        {
            return new() { Top = $"     ", Middle = $"     ", Bottom = $"     " };
        }
    }
}
