using UnityEngine;
 using UnityEditor;
 using System.Collections;
 
 public class ColliderToFit : MonoBehaviour {



    void Start()     {
         FitColliderToChildren(this);
    }

    public void FitColliderToChildren (MonoBehaviour parentObject)  {
        print("FitColliderToChildren ");

        print("FitColliderToChildren " + parentObject.name);
        MeshFilter cube = parentObject.GetComponent<MeshFilter>();
        BoxCollider bc = parentObject.GetComponent<BoxCollider>();
        if (bc == null)
        {
            print("BoxCollider notFound");
            // bc = parentObject.AddComponent<BoxCollider>();
            return;
        }
        print("BoxCollider " + bc);

       // Renderer wireCollide = this;
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)
        {
            //print("render "+ render.name);
            if (render.name == "wireCollide")
            {
                //wireCollide = render;
            }

            if (hasBounds)
            {
                bounds.Encapsulate(render.bounds);
            }
            else
            {
                bounds = render.bounds;
                hasBounds = true;
            }
        }
        if (hasBounds)
        {
            print("BoxCollider hasBounds :" + bounds.size);
            print("BoxCollider bc.center :" + bc.center);
            print("BoxCollider bc.size :" + bc.size);
            bc.center = bounds.center - parentObject.transform.position;
            bc.size = new Vector3(bounds.size.x / parentObject.transform.localScale.x, bounds.size.y / parentObject.transform.localScale.y, bounds.size.z / parentObject.transform.localScale.z);
            createCubeMesh(cube, bc.size, bc.center);

        }
        else
        {
            print("BoxCollider no bounds");
            bc.size = bc.center = Vector3.zero;
            bc.size = Vector3.zero;
            createCubeMesh(cube, bc.size, bc.center);
        }
    }

    private void createCubeMesh(MeshFilter cube, Vector3 size, Vector3 center)    {
        // You can change that line to provide another MeshFilter
        Mesh mesh = cube.mesh;
        mesh.Clear();

        float length = size.x;
        float width = size.y;
        float height = size.z;

        #region Vertices
        Vector3 p0 = new Vector3(center.x-length*.5f , center.y - width * .5f, center.z+ height * .5f);
        Vector3 p1 = new Vector3(center.x+length * .5f, center.y - width * .5f, center.z + height * .5f);
        Vector3 p2 = new Vector3(center.x + length * .5f, center.y - width * .5f, center.z -height * .5f);
        Vector3 p3 = new Vector3(center.x - length * .5f, center.y - width * .5f, center.z -height * .5f);

        Vector3 p4 = new Vector3(center.x - length * .5f, center.y+width * .5f, center.z + height * .5f);
        Vector3 p5 = new Vector3(center.x + length * .5f, center.y+width * .5f, center.z + height * .5f);
        Vector3 p6 = new Vector3(center.x + length * .5f, center.y+width * .5f, center.z -height * .5f);
        Vector3 p7 = new Vector3(center.x - length * .5f, center.y+width * .5f, center.z -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
	// Bottom
	p0, p1, p2, p3,

	// Left
	p7, p4, p0, p3,

	// Front
	p4, p5, p1, p0,

	// Back
	p6, p7, p3, p2,

	// Right
	p5, p6, p2, p1,

	// Top
	p7, p6, p5, p4
        };
        #endregion

        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	// Bottom
	down, down, down, down,

	// Left
	left, left, left, left,

	// Front
	front, front, front, front,

	// Back
	back, back, back, back,

	// Right
	right, right, right, right,

	// Top
	up, up, up, up
        };
        #endregion

        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	// Bottom
	_11, _01, _00, _10,

	// Left
	_11, _01, _00, _10,

	// Front
	_11, _01, _00, _10,

	// Back
	_11, _01, _00, _10,

	// Right
	_11, _01, _00, _10,

	// Top
	_11, _01, _00, _10,
        };
        #endregion

        #region Triangles
        int[] triangles = new int[]
        {
	// Bottom
	3, 1, 0,
    3, 2, 1,			

	// Left
	3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
    3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

	// Front
	3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
    3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

	// Back
	3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
    3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

	// Right
	3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
    3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

	// Top
	3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
    3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

        };
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }
 }