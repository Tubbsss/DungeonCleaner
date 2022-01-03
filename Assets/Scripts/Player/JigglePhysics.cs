using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===============================================================================================================
// Script by Unity City - https://www.youtube.com/watch?v=Kwh4TkQqqf8
//===============================================================================================================


public class JigglePhysics : MonoBehaviour
{
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer meshRenderer;
    private JiggleVertex[] jv;
    private Vector3[] vertexArray;

    // Start is called before the first frame update
    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh; //Set original mesh.
        MeshClone = Instantiate(OriginalMesh); //Clone original mesh.
        GetComponent<MeshFilter>().sharedMesh = MeshClone; //Set mesh to clone.
        meshRenderer = GetComponent<MeshRenderer>(); //Get Mesh Renderer.
        jv = new JiggleVertex[MeshClone.vertices.Length]; //Create array of mesh clone vertices.
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JiggleVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices; //Set array to original mesh vertices.
        for (int i = 0; i < jv.Length; i++)
        {
            //Make vertex jiggle.
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (meshRenderer.bounds.max.y - target.y) / meshRenderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }
        MeshClone.vertices = vertexArray; //Apply to mesh clone.
    }

    public class JiggleVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;
        
        public JiggleVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < 0.001f)
            {
                Position = target;
            }
        }
    }
}
