
using Graphics;

#if UNITY_EDITOR
using UnityEngine;
#endif

#if DebugFromWyb

using Color255 = UnityEngine.Color;

#endif


public class MapHightData
{

    #region instance fields

    private int m_width = -1;

    public int width 
    {
        set { this.m_width = value; }
        get { return this.m_width; }
    }

    private int m_height = -1;

    public int height 
    {
        set { this.m_height = value; }
        get { return this.m_height; }
    }


    private float m_realWidth = -1f;

    public float realWidth
    {
        set { this.m_realWidth = value; }
        get { return this.m_realWidth; }
    }

    private float m_realHeight = -1f;

    public float realHeight 
    {
        set { this.m_realHeight = value; }
        get { return this.m_realHeight; }
    }

    private float m_cellSizeX;//

    public float CellSizeX
    {
        set { this.m_cellSizeX = value; }
        get { return this.m_cellSizeX; }
    }


    private float m_cellSizeZ;

    public float CellSizeZ
    {
        set { this.m_cellSizeZ = value; }
        get { return this.m_cellSizeZ; }
    }


    private int m_cellNumX = -1;

    public int CellNumX 
    {
        set { this.m_cellNumX = value; }
        get { return this.m_cellNumX; }
      
    }

    private int m_cellNumZ = -1;

    public int CellNumZ 
    {
        set { this.m_cellNumZ = value; }
        get { return this.m_cellNumZ; }
    }



    private float m_maxHeightDiff = -1;

    public float maxHeightDiff 
    {
        set { this.m_maxHeightDiff = value; }
        get { return this.m_maxHeightDiff; }
    }


    private float m_minHeight = -1f;

    public float minHeight 
    {
        set { this.m_minHeight = value; }
        get { return this.m_minHeight; }
    }


    private float m_originX;

    public float originX 
    {
        set { this.m_originX = value; }

        get { return this.m_originX; }
    }

    private float m_originZ;

    public float originZ 
    {
        set { this.m_originZ = value; }
        get { return this.m_originZ; }
    }

    private byte[,] m_datas;

    public byte[,] datas 
    {
        set 
        {
            this.m_datas = value;
        }
        get 
        {
            return this.m_datas;
        }
    }

    #if DebugFromWyb

    GameObject m_mapHighMesh;

    public GameObject mapHighMesh 
    {
        get { return this.m_mapHighMesh; }
    }

    public Vector3 _A;

    public Vector3 _B;

    public Vector3 _C;

    public Vector3 _D;

    public Vector3 _E;

    private Vector2D mapData2DOrigin;


    public class HightMapDataDebug : MonoBehaviour
    {


        void Update() 
        {
            Debug.DrawLine(MapHightDataHolder.CurMapData._A, MapHightDataHolder.CurMapData._B, Color255.green);

            Debug.DrawLine(MapHightDataHolder.CurMapData._B, MapHightDataHolder.CurMapData._C, Color255.green);

            Debug.DrawLine(MapHightDataHolder.CurMapData._C, MapHightDataHolder.CurMapData._A, Color255.green);

            Debug.DrawLine(MapHightDataHolder.CurMapData._A, MapHightDataHolder.CurMapData._D, Color255.yellow);

            Debug.DrawLine(MapHightDataHolder.CurMapData._D, MapHightDataHolder.CurMapData._E, Color255.blue);


        }

        void OnDrawGizmos()
        {
            //Gizmos.DrawSphere(pos, 0.3f);

            Vector3[] vs = MapHightDataHolder.GetPoints();

            if (vs != null)
            {
                Gizmos.color = Color255.red;
                Gizmos.DrawSphere(MapHightDataHolder.CurMapData._A, 0.1f);//A

                Gizmos.color = Color255.green;
                Gizmos.DrawSphere(MapHightDataHolder.CurMapData._B, 0.1f);//B

                Gizmos.color = Color255.blue;
                Gizmos.DrawSphere(MapHightDataHolder.CurMapData._C, 0.1f);//C

                Gizmos.color = Color255.yellow;
                Gizmos.DrawSphere(MapHightDataHolder.CurMapData._D, 0.1f);//D

                Gizmos.color = Color255.white;
                Gizmos.DrawSphere(MapHightDataHolder.CurMapData._E, 0.1f);//E
            }
        }

        Vector3 SetPos(Graphics.Vector2D vs)
        {
            return new Vector3(vs.x, 2, vs.y);
        }
    
    }

    #endif

    #endregion

    #region Instance Methods


    public void Init(int width , int height , float realWidth,float realHeight , float maxHeightDiff ,float minHeight ,byte[] bytes) 
    {
        this.m_width = width;
        this.m_height = height;
        this.m_realWidth = realWidth;
        this.m_realHeight = realHeight;

        this.m_maxHeightDiff = maxHeightDiff;
        this.minHeight = minHeight;

        //Debug.Log("this minHeight : "+this.minHeight);

        this.m_cellSizeX = this.m_realWidth / (float)this.m_width;
        this.m_cellSizeZ = this.m_realHeight / (float)this.m_height;

        this.m_cellNumX = this.m_width - 1;
        this.m_cellNumZ = this.m_height - 1;

        this.m_originX = -this.m_realWidth / 2f;
        this.m_originZ = -this.m_realHeight / 2f;

#if DebugFromWyb
        this.mapData2DOrigin = new Vector2D(this.m_originX,this.m_originZ);
#endif
        this.m_datas = new byte[this.m_height,this.m_width];

        for (int i = 0; i < this.m_height; i++)
        {
            for (int j = 0; j < this.m_width; j++)
            {
                this.m_datas[i, j] = bytes[i * this.m_width + j];
            }
        }

        #region Debug
#if DebugFromWyb
        GenerateHightMesh();
#endif
        #endregion

    }

#if DebugFromWyb && UNITY_EDITOR

    public GameObject GenerateHightMesh() 
    {
        /*
        0.8196143 minHeight :  -3.84196 height cha:  4.661575
        */
        if (m_mapHighMesh == null)
        {
            m_mapHighMesh = new GameObject("MapMesh");
            m_mapHighMesh.AddComponent<HightMapDataDebug>();
        }
        // Create the game object containing the renderer
        m_mapHighMesh.AddComponent<MeshFilter>();
        Renderer render = m_mapHighMesh.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Transparent/Diffuse"));

        render.sharedMaterial = mat;
        render.sharedMaterial.color = new UnityEngine.Color32(20, 26, 6, 190);


        // Retrieve a mesh instance
        Mesh mesh = m_mapHighMesh.GetComponent<MeshFilter>().sharedMesh;

        if(mesh == null)
        {
            mesh = new Mesh();

            this.m_mapHighMesh.GetComponent<MeshFilter>().sharedMesh = mesh ;
        }

        var y = 0;
        var x = 0;

        // Build vertices and UVs
        Vector3[] vertices = new Vector3[height * width];
        Vector2[] uv = new Vector2[height * width];
        Vector4[] tangents = new Vector4[height * width];

        Vector3 size = new Vector3(realWidth, this.m_maxHeightDiff, realHeight);

        var uvScale = new Vector2(1.0f / (width - 1), 1.0f / (height - 1));
        var sizeScale = new Vector3(size.x / (width - 1), size.y, size.z / (height - 1));

        float originX = -realWidth / 2f;
        float originZ = -realHeight / 2f;

        for (y = 0; y < height; y++)
        {
            for (x = 0; x < width; x++)
            {
                var pixelHeight = ((float)this.m_datas[y, x]) / byte.MaxValue;
                var vertex = new Vector3(x, pixelHeight, y);
                vertices[y * width + x] = Vector3.Scale(sizeScale, vertex);// Convert to the real point
                vertices[y * width + x].y = vertices[y * width + x].y + this.m_minHeight;

                vertices[y * width + x].x = vertices[y * width + x].x + originX;
                vertices[y * width + x].z = vertices[y * width + x].z + originZ;

                //Debug.Log(" pos : "+vertices[y * width + x]);
                uv[y * width + x] = Vector2.Scale(new Vector2(x, y), uvScale);

                // Calculate tangent vector: a vector that goes from previous vertex
                // to next along X direction. We need tangents if we intend to
                // use bumpmap shaders on the mesh.

                int _xL = Mathf.Clamp(x - 1, 0, width - 1);
                int _xR = Mathf.Clamp(x + 1, 0, width - 1);

                var vertexL = new Vector3(_xL, ((float)this.m_datas[y, _xL]) / byte.MaxValue, y);
                var vertexR = new Vector3(_xR, ((float)this.m_datas[y, _xR]) / byte.MaxValue, y);
                var tan = Vector3.Scale(sizeScale, vertexR - vertexL).normalized;
                tangents[y * width + x] = new Vector4(tan.x, tan.y, tan.z, -1.0f);
            }
        }

        // Assign them to the mesh
        mesh.vertices = vertices;
        mesh.uv = uv;

        // Build triangle indices: 3 indices into vertex array for each triangle
        var triangles = new int[(height - 1) * (width - 1) * 6];
        var index = 0;
        for (y = 0; y < height - 1; y++)
        {
            for (x = 0; x < width - 1; x++)
            {
                // For each grid cell output two triangles
                triangles[index++] = (y * width) + x;//0
                triangles[index++] = ((y + 1) * width) + x;//1
                triangles[index++] = (y * width) + x + 1;//2

                triangles[index++] = ((y + 1) * width) + x;//1
                triangles[index++] = ((y + 1) * width) + x + 1;//4
                triangles[index++] = (y * width) + x + 1;//2
            }
        }
        // And assign them to the mesh
        mesh.triangles = triangles;

        // Auto-calculate vertex normals from the mesh
        mesh.RecalculateNormals();

        // Assign tangents after recalculating normals
        mesh.tangents = tangents;

        return m_mapHighMesh;
    }

    public void OnDestroy() 
    {
        Object.DestroyImmediate(this.m_mapHighMesh);
    }

#endif




    public float GetMapHeight(float x , float z) 
    {
        x = x - this.m_originX;
        z = z - this.m_originZ;
        if(x > this.m_realWidth || z > this.m_realHeight)
        {
            return 0f;
        }

        int column = (int)((x / this.m_realWidth) * this.m_cellNumX);

        if (column >= this.m_width)
        {
            column = this.m_width - 1;
        }

        int row = (int)((z / this.m_realHeight) * this.m_cellNumZ);

        //Debug.LogWarning("z : " + z + "  m_realHeight : " + this.m_realHeight + " ratio z/h : " + (z / this.m_realHeight) + " row : " + row);

        if (row >= this.m_height)
        {
            row = this.m_height - 1;
        }

        //float x0 = this.CellSizeX * column;//
        //float z0 = this.CellSizeZ * row;//

        float x0 = (this.m_realWidth * column)/(float)(this.m_width - 1);//
        float z0 = (this.m_realHeight * row) / (float)(this.m_height - 1);//


        float heightA = 0;

        if (System.Math.Abs(x0 - x) < float.Epsilon && System.Math.Abs(z0 - x) < float.Epsilon)//如果给出的点的x,y坐标与第一个定点的xy相等
        {
            heightA = GetVetexHeight(row, column);
            return heightA;
        }

        float height = 0;

        //Debug.LogWarning("row : " + row + " column : " + column + " CellSizeX : " + this.CellSizeX);

        //Debug.LogWarning("x : "+x+" z:  "+z+" x0 : "+x0+" z0 : "+z0);

        if ((x - x0 + z - z0) <= this.CellSizeX)
        {
            
            Vector2D A = new Vector2D(x0,z0);
            Vector2D B = new Vector2D(x0, z0 + this.m_cellSizeZ);
            Vector2D C = new Vector2D(x0 + this.m_cellSizeX,z0);
            Vector2D D = new Vector2D(x,z);

            float dist = 0;
            Vector2D E = Vector2D.Zero;

            if (GraphUtils.LineIntersection2D(A, D, B, C, ref dist, ref E))
            {
                heightA = GetVetexHeight(row, column);

                float r0 = Vector2D.Distance(E, B) / Vector2D.Distance(C, B);

                float heightB = GetVetexHeight(row + 1, column);

                float heightC = GetVetexHeight(row, column + 1);

                float heightE = heightB * (1 - r0) + r0 * heightC;

                float r1 = Vector2D.Distance(A, D) / Vector2D.Distance(A, E);

                height = heightA * (1 - r1) + r1 * heightE;

#if DebugFromWyb
                Vector2D v = A + this.mapData2DOrigin;
                _A = new Vector3(v.x,heightA,v.y);

                v = B + this.mapData2DOrigin;
                _B = new Vector3(v.x, heightB, v.y);

                v = C + this.mapData2DOrigin;
                _C = new Vector3(v.x, heightC, v.y);

                v = D + this.mapData2DOrigin;
                _D = new Vector3(v.x, height, v.y);

                v = E + this.mapData2DOrigin;
                _E = new Vector3(v.x, heightE, v.y);
#endif
            }
            else
            {

#if UNITY_EDITOR
                Debug.LogWarning("lineIntersection false 1!");
#endif
            }

            //Debug.LogWarning("left triangle : dist : "+dist);

        }
        else 
        {


            Vector2D A = new Vector2D(x0 + this.m_cellSizeX, z0 + this.m_cellSizeZ);
            Vector2D B = new Vector2D(x0, z0 + this.m_cellSizeZ);
            Vector2D C = new Vector2D(x0 + this.m_cellSizeX, z0);
            Vector2D D = new Vector2D(x, z);

            float dist = 0;
            Vector2D E = Vector2D.Zero;

            
            if (GraphUtils.LineIntersection2D(A, D, B, C, ref dist, ref E))
            {

                heightA = GetVetexHeight(row + 1, column + 1);

                float r0 = Vector2D.Distance(E, B) / Vector2D.Distance(C, B);

                r0 = GraphUtils.Clamp01(r0);

                float heightB = GetVetexHeight(row + 1, column);

                float heightC = GetVetexHeight(row, column + 1);

                float heightE = heightB * (1 - r0) + r0 * heightC;

                float r1 = Vector2D.Distance(A, D) / Vector2D.Distance(A, E);

                r1 = GraphUtils.Clamp01(r1);

                height = heightA * (1 - r1) + r1 * heightE;

#if DebugFromWyb
                Vector2D v = A + this.mapData2DOrigin;
                _A = new Vector3(v.x, heightA, v.y);

                v = B + this.mapData2DOrigin;
                _B = new Vector3(v.x, heightB, v.y);

                v = C + this.mapData2DOrigin;
                _C = new Vector3(v.x, heightC, v.y);

                v = D + this.mapData2DOrigin;
                _D = new Vector3(v.x, height, v.y);

                v = E + this.mapData2DOrigin;
                _E = new Vector3(v.x, heightE, v.y);
#endif
            }
            else 
            {
#if UNITY_EDITOR
                Debug.LogWarning("lineIntersection false 2!");
#endif
            }

            //Debug.LogWarning("right triangle : dist : " + dist);
        }

        return height;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public float GetVetexHeight(int row , int column) 
    {
        //Debug.Log("row : "+row+" column : "+column);

        float height = (this.m_datas[row, column] * this.m_maxHeightDiff) / byte.MaxValue + this.minHeight;

        return height;
    }

    //public static bool LineIntersection2D(Vector2D A,
    //                           Vector2D B,
    //                           Vector2D C,
    //                           Vector2D D,
    //                           ref float dist,
    //                           ref Vector2D point)
    //{ 
    
    //}

    #if DebugFromWyb

    Vector3 CalculateIntersectionPoint(Vector3 A, Vector3 D, Vector3 B, Vector3 C)
    {
        Vector3 v0 = B - A;

        Vector3 d2 = C - B;

        Vector3 d1 = D - A;

        Vector3 dot = crossDot(d1, d2);

        float dotValue = Vector3.Dot(dot, dot);

        Vector3 crossValue = crossDot(v0, d2);

        float value = Vector3.Dot(crossValue, dot) / dotValue;

        return A + d1 * value;
    }


    Vector3 crossDot(Vector3 v0, Vector3 v1)
    {
        return new Vector3(v0.y * v1.z - v0.z * v1.y,
                           v0.z * v1.x - v0.x * v1.z,
                           v0.x * v1.y - v0.y * v1.x);
    }

#endif

    #endregion

}


