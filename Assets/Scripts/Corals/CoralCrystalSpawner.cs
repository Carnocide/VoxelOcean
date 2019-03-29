﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Modular Asset Coral Mesh
/// Created by Cameron Garchow to mimic Crystalline Structures
/// Based on scientific and a mix of parasitic crystal structures
/// The idea behind it is to create crystalline structures that 'spread' like a disease.
/// This can be changed to any value we want, we could take this into a game controller and randomize it
/// For this we can randomize its 
/// </summary>
public class CoralCrystalSpawner : MonoBehaviour
{
    /// <summary>
    /// Iterations : how many times the object is repeated
    /// </summary>
    [Range(2, 8)] public int iterations = 3;// Iterations of the object
    /// <summary>
    /// Angle1 : what angle an object is pointed to rotated by
    /// </summary>
    [Range(0, 45)] public int angle1 = 25; // Tendril 1's Angle of origin
    /// <summary>
    /// Angle2 : What angle2 an object is pointed to rotated by (alternate)
    /// </summary>
    [Range(0, 45)] public int angle2 = 25; // Tendril 2's angle of origin
    /// <summary>
    /// Angle3 : what angle3 an object is pointed to rotated by
    /// </summary>
    [Range(0, 45)] public int angle3 = 25; //tendril 3's angel of origin
    /// <summary>
    /// Scalar : Scale of the object, that decreases over time based on per iteration
    /// </summary>
    [Range(.1f, .85f)] public float scalar = 0.5f; //scale
    /// <summary>
    /// OBJPOS : is the object position of the spawner
    /// </summary>
    [Range(.25f, .8f)] public float objpos = 0.5f;
    /// <summary>
    /// branchScale : is the scale of each branch object / transform scale
    /// </summary>
    public Vector3 branchScale = new Vector3(.25f, 2, .25f);
    
/// <summary>
/// Start / Build Function
/// Creates the object and coral mesh
/// </summary>
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    /// <summary>
    /// Update
    /// Called once a frame
    /// Once per a frame there is an update increasing and decreasing the Scalar or scale of the object.
    /// </summary>
    void Update()
    {
        if (scalar <= .25f)
        {
            scalar += .1f;
        }
        else if (scalar >= .85f) {
            scalar -= .1f;
        }
    }

    /// <summary>
    /// Build
    /// Builds the object from the meshes.
    /// Gets a reference from the square function and uses it to build an object.
    /// Combines meshes to reduce object count
    /// Meshfilters it from reference
    /// </summary>
    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Grow(meshes, iterations, Vector3.zero, Quaternion.identity, 1);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }

    /// <summary>
    /// Grow
    /// Grows objects from a single branch that is then randomized aned give iterations that the player can control.
    /// Objects are controlled entirely in the grow function
    /// </summary>
    /// <param name="meshes"> mesh that it takes</param>
    /// <param name="num"> number of meshes</param>
    /// <param name="pos"> postion of the object</param>
    /// <param name="rot"> rotation of the obj</param>
    /// <param name="scale"> objects scale randomized</param>
    private void Grow(List<CombineInstance> meshes, int num, Vector3 pos, Quaternion rot, float scale)
    { 

        if (num <= 0) return; // stop recursive function
            RandomizeRanges();
            CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        //inst.transform =
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(Random.Range(0, 1), 1, Random.Range(0, 1)));

        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(objpos, objpos, objpos));

        Vector3 sidePos2 = inst.transform.MultiplyPoint(new Vector3(-objpos, objpos, -objpos));

        Quaternion rot1 = rot * Quaternion.Euler(angle3, angle1, angle2);
        Quaternion rot2 = rot * Quaternion.Euler(0, angle2, 0);
        Quaternion rot3 = rot * Quaternion.Euler(-angle3, 0, -angle2); //to avoid stretching

        scale *= scalar;
        //BUG - Objects will sometimes 'stretch'
        Grow(meshes, num, pos, rot1, scale); //doing one for tendril like
        Grow(meshes, num, sidePos, rot2, scale); // doing for tendril 2
        Grow(meshes, num, sidePos2, rot3, scale); // tendril 3

    }
    /// <summary>
    /// Randomize Range
    /// Randomize the range of the objects inside of the grow function.
    /// 
    /// </summary>
    private void RandomizeRanges()
    {
        angle1 = Random.Range(0, 45);
        angle2 = Random.Range(0, 45);
        angle3 = Random.Range(0, 45);
        scalar = Random.Range(.5f, .8f);
        objpos = Random.Range(.4f, .6f);
        branchScale = new Vector3(Random.Range(.25f, .35f), (float)2, Random.Range(.25f, .35f));
    }

    //Cube Data
    // Makes a cube
    /// <summary>
    /// Cube 
    /// Makes a cube object
    /// </summary>
    /// <param name="num"> Number of iterations </param>
    /// <returns> A Mesh / Cube object</returns>
    private Mesh MakeCube(int num)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();




        //FRONT
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        //Back
        vertices.Add(new Vector3(-0.5f, 0, +0.5f));
        vertices.Add(new Vector3(+0.5f, 0, +0.5f));
        vertices.Add(new Vector3(+0.5f, 1, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);


        //Left
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);

        //Right
        vertices.Add(new Vector3(+0.5f, 0, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, 0.5f));
        vertices.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);


        //top
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        vertices.Add(new Vector3(-0.5f, 1, 0.5f));
        vertices.Add(new Vector3(0.5f, 1, 0.5f));
        vertices.Add(new Vector3(0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(16);
        tris.Add(17);
        tris.Add(18);
        tris.Add(18);
        tris.Add(19);
        tris.Add(16);

        //bottom
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0, 0.5f));
        vertices.Add(new Vector3(0.5f, 0, 0.5f));
        vertices.Add(new Vector3(0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(20);
        tris.Add(21);
        tris.Add(22);
        tris.Add(22);
        tris.Add(23);
        tris.Add(20);


        //Set hue min and Max
        float hueMin = .4f;
        float hueMax = Random.Range(.6f, 1);

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float) iterations));
        /*
         * for each vertices located in the array color them in a hue 
        */
        foreach (Vector3 pos in vertices)
        {
            float tempHue = hue;

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color);
        }

        Mesh mesh = new Mesh();

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        mesh.SetColors(colors);
        return mesh;
    }


}
/// <summary>
/// Editor for the CoralCrystal Spawner, allows building in editor for testing
/// </summary>
[CustomEditor(typeof(CoralCrystalSpawner))]
public class CoralCrystalSpawnerEditor : Editor
{

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralCrystalSpawner c = (target as CoralCrystalSpawner);
            c.Build();
        }

    }
}