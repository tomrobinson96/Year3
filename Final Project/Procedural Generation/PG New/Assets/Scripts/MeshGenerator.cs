using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{

    /******* This script contols the generation of the mesh that is based around the map this is created in the 'MapGenrator' script ********/

    public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;

    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleArchive = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    public void GenerateWallMesh(int[,] map, float squareSize)
    {

        triangleArchive.Clear();
        outlines.Clear();
        checkedVertices.Clear();

        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        int pieceCount = 10;
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * pieceCount;
            float percentY = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].z) * pieceCount;
            uvs[i] = new Vector2(percentX, percentY);
        }
        mesh.uv = uvs;

        CreateWMeshes();
    }

    void CreateWMeshes()
    {

        CalculateMeshOutlines();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        float wallHeight = 5;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;
                wallVertices.Add(vertices[outline[i]]); // left
                wallVertices.Add(vertices[outline[i + 1]]); // right
                wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bottom left
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); // bottom right

                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
            }
        }
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.mesh = wallMesh;

        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = wallMesh;
    }

    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0:
                break;

            // 1 points:
            case 1:
                MeshFromPoints(square.leftCentre, square.bottomCentre, square.leftBottom);
                break;
            case 2:
                MeshFromPoints(square.rightBottom, square.bottomCentre, square.rightCentre);
                break;
            case 4:
                MeshFromPoints(square.rightTop, square.rightCentre, square.centreTop);
                break;
            case 8:
                MeshFromPoints(square.leftTop, square.centreTop, square.leftCentre);
                break;

            // 2 points:
            case 3:
                MeshFromPoints(square.rightCentre, square.rightBottom, square.leftBottom, square.leftCentre);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.rightTop, square.rightBottom, square.bottomCentre);
                break;
            case 9:
                MeshFromPoints(square.leftTop, square.centreTop, square.bottomCentre, square.leftBottom);
                break;
            case 12:
                MeshFromPoints(square.leftTop, square.rightTop, square.rightCentre, square.leftCentre);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.rightTop, square.rightCentre, square.bottomCentre, square.leftBottom, square.leftCentre);
                break;
            case 10:
                MeshFromPoints(square.leftTop, square.centreTop, square.rightCentre, square.rightBottom, square.bottomCentre, square.leftCentre);
                break;

            // 3 point:
            case 7:
                MeshFromPoints(square.centreTop, square.rightTop, square.rightBottom, square.leftBottom, square.leftCentre);
                break;
            case 11:
                MeshFromPoints(square.leftTop, square.centreTop, square.rightCentre, square.rightBottom, square.leftBottom);
                break;
            case 13:
                MeshFromPoints(square.leftTop, square.rightTop, square.rightCentre, square.bottomCentre, square.leftBottom);
                break;
            case 14:
                MeshFromPoints(square.leftTop, square.rightTop, square.rightBottom, square.bottomCentre, square.leftCentre);
                break;

            // 4 point:
            case 15:
                MeshFromPoints(square.leftTop, square.rightTop, square.rightBottom, square.leftBottom);
                checkedVertices.Add(square.leftTop.vertexIndex);
                checkedVertices.Add(square.rightTop.vertexIndex);
                checkedVertices.Add(square.rightBottom.vertexIndex);
                checkedVertices.Add(square.leftBottom.vertexIndex);
                break;
        }

    }

    void MeshFromPoints(params Nodes[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);

    }

    void AssignVertices(Nodes[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Nodes a, Nodes b, Nodes c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToArchive(triangle.vertexIndexA, triangle);
        AddTriangleToArchive(triangle.vertexIndexB, triangle);
        AddTriangleToArchive(triangle.vertexIndexC, triangle);
    }

    void AddTriangleToArchive(int vertexIndexKey, Triangle triangle)
    {
        if (triangleArchive.ContainsKey(vertexIndexKey))
        {
            triangleArchive[vertexIndexKey].Add(triangle);
        }
        else {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleArchive.Add(vertexIndexKey, triangleList);
        }
    }

    void CalculateMeshOutlines()
    {

        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }

    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleArchive[vertexIndex];

        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleArchive[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }

    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }


        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNodes[,] controlNodes = new ControlNodes[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNodes(pos, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    public class Square
    {

        public ControlNodes leftTop, rightTop, rightBottom, leftBottom;
        public Nodes centreTop, rightCentre, bottomCentre, leftCentre;
        public int configuration;

        public Square(ControlNodes _topLeft, ControlNodes _topRight, ControlNodes _bottomRight, ControlNodes _bottomLeft)
        {
            leftTop = _topLeft;
            rightTop = _topRight;
            rightBottom = _bottomRight;
            leftBottom = _bottomLeft;

            centreTop = leftTop.right;
            rightCentre = rightBottom.above;
            bottomCentre = leftBottom.right;
            leftCentre = leftBottom.above;

            if (leftTop.active)
                configuration += 8;
            if (rightTop.active)
                configuration += 4;
            if (rightBottom.active)
                configuration += 2;
            if (leftBottom.active)
                configuration += 1;
        }

    }

    public class Nodes
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Nodes(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNodes : Nodes
    {

        public bool active;
        public Nodes above, right;

        public ControlNodes(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            above = new Nodes(position + Vector3.forward * squareSize / 2f);
            right = new Nodes(position + Vector3.right * squareSize / 2f);
        }

    }
}