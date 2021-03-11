using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour
{

    /******** This script controls the creation of the map based on the random fill percentage
    that can be accessed within the editior, this will base the amount of area that is wall and
    that is playable area. *************/

    public int width;
    public int height;

    public string seed;
    public bool randomSeed;

    [Range(0, 100)]
    public int randomFill;

    int[,] map;

    public GameObject easy;
    //public GameObject hard;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        //if (easy != null)
        //{
            RandomFillMapEasy();
        //}
        /*if (easy == null)
        {
            RandomFillMapHard();
        }*/

        for (int i = 0; i < 5; i++)
        {
            SmoothingMap();
        }

        Process();

        int borderSize = 1;
        int[,] mapWithBorder = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < mapWithBorder.GetLength(0); x++)
        {
            for (int y = 0; y < mapWithBorder.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    mapWithBorder[x, y] = map[x - borderSize, y - borderSize];
                }
                else {
                    mapWithBorder[x, y] = 1;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateWallMesh(mapWithBorder, 1);
    }

    void Process()
    {
        List<List<Coord>> wallZones = GetZones(1);
        int wallThresholdSize = 50;

        foreach (List<Coord> wallZone in wallZones)
        {
            if (wallZone.Count < wallThresholdSize)
            {
                foreach (Coord piece in wallZone)
                {
                    map[piece.pieceX, piece.pieceY] = 0;
                }
            }
        }

        List<List<Coord>> roomZones = GetZones(0);
        int roomThresholdSize = 50;
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomZone in roomZones)
        {
            if (roomZone.Count < roomThresholdSize)
            {
                foreach (Coord piece in roomZone)
                {
                    map[piece.pieceX, piece.pieceY] = 1;
                }
            }
            else {
                survivingRooms.Add(new Room(roomZone, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isReachableFromMain = true;

        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceRouteFromMain = false)
    {

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceRouteFromMain)
        {
            foreach (Room room in allRooms)
            {
                if (room.isReachableFromMain)
                {
                    roomListB.Add(room);
                }
                else {
                    roomListA.Add(room);
                }
            }
        }
        else {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int shortDistance = 0;
        Coord bestPieceA = new Coord();
        Coord bestPieceB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnection = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceRouteFromMain)
            {
                possibleConnection = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int pieceIndexA = 0; pieceIndexA < roomA.edgePiece.Count; pieceIndexA++)
                {
                    for (int pieceIndexB = 0; pieceIndexB < roomB.edgePiece.Count; pieceIndexB++)
                    {
                        Coord pieceA = roomA.edgePiece[pieceIndexA];
                        Coord pieceB = roomB.edgePiece[pieceIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(pieceA.pieceX - pieceB.pieceX, 2) + Mathf.Pow(pieceA.pieceY - pieceB.pieceY, 2));

                        if (distanceBetweenRooms < shortDistance || !possibleConnection)
                        {
                            shortDistance = distanceBetweenRooms;
                            possibleConnection = true;
                            bestPieceA = pieceA;
                            bestPieceB = pieceB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnection && !forceRouteFromMain)
            {
                CreateRoute(bestRoomA, bestRoomB, bestPieceA, bestPieceB);
            }
        }

        if (possibleConnection && forceRouteFromMain)
        {
            CreateRoute(bestRoomA, bestRoomB, bestPieceA, bestPieceB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceRouteFromMain)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreateRoute(Room roomA, Room roomB, Coord pieceA, Coord pieceB)
    {
        Room.ConnectRooms(roomA, roomB);
        Debug.DrawLine(CoordToWorldPoint(pieceA), CoordToWorldPoint(pieceB), Color.green, 100);

        List<Coord> line = GetLine(pieceA, pieceB);
        foreach (Coord c in line)
        {
            DrawCircle(c, 5);
        }
    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.pieceX + x;
                    int drawY = c.pieceY + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.pieceX;
        int y = from.pieceY;

        int dx = to.pieceX - from.pieceX;
        int dy = to.pieceY - from.pieceY;

        bool upsideDown = false;
        int step = Math.Sign(dx);
        int gradient = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            upsideDown = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradient = Math.Sign(dx);
        }

        int gradientTotal = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (upsideDown)
            {
                y += step;
            }
            else {
                x += step;
            }

            gradientTotal += shortest;
            if (gradientTotal >= longest)
            {
                if (upsideDown)
                {
                    x += gradient;
                }
                else {
                    y += gradient;
                }
                gradientTotal -= longest;
            }
        }

        return line;
    }

    Vector3 CoordToWorldPoint(Coord piece)
    {
        return new Vector3(-width / 2 + .5f + piece.pieceX, 2, -height / 2 + .5f + piece.pieceY);
    }

    List<List<Coord>> GetZones(int pieceType)
    {
        List<List<Coord>> zones = new List<List<Coord>>();
        int[,] mapID = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapID[x, y] == 0 && map[x, y] == pieceType)
                {
                    List<Coord> newZone = GetZonePieces(x, y);
                    zones.Add(newZone);

                    foreach (Coord piece in newZone)
                    {
                        mapID[piece.pieceX, piece.pieceY] = 1;
                    }
                }
            }
        }

        return zones;
    }

    List<Coord> GetZonePieces(int startX, int startY)
    {
        List<Coord> pieces = new List<Coord>();
        int[,] mapIDs = new int[width, height];
        int pieceType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapIDs[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord piece = queue.Dequeue();
            pieces.Add(piece);

            for (int x = piece.pieceX - 1; x <= piece.pieceX + 1; x++)
            {
                for (int y = piece.pieceY - 1; y <= piece.pieceY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == piece.pieceY || x == piece.pieceX))
                    {
                        if (mapIDs[x, y] == 0 && map[x, y] == pieceType)
                        {
                            mapIDs[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return pieces;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }


    void RandomFillMapEasy()
    {
        if (randomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFill) ? 1 : 0;
                }
            }
        }
    }


    void SmoothingMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourPieces = GetSurroundingWallCount(x, y);

                if (neighbourPieces > 4)
                    map[x, y] = 1;
                else if (neighbourPieces < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    struct Coord
    {
        public int pieceX;
        public int pieceY;

        public Coord(int x, int y)
        {
            pieceX = x;
            pieceY = y;
        }
    }


    class Room : IComparable<Room>
    {
        public List<Coord> pieces;
        public List<Coord> edgePiece;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isReachableFromMain;
        public bool isMainRoom;

        public Room()
        {
        }

        public Room(List<Coord> roomPieces, int[,] map)
        {
            pieces = roomPieces;
            roomSize = pieces.Count;
            connectedRooms = new List<Room>();

            edgePiece = new List<Coord>();
            foreach (Coord piece in pieces)
            {
                for (int x = piece.pieceX - 1; x <= piece.pieceX + 1; x++)
                {
                    for (int y = piece.pieceY - 1; y <= piece.pieceY + 1; y++)
                    {
                        if (x == piece.pieceX || y == piece.pieceY)
                        {
                            if (map[x, y] == 1)
                            {
                                edgePiece.Add(piece);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isReachableFromMain)
            {
                isReachableFromMain = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isReachableFromMain)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isReachableFromMain)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}