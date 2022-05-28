using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraMesh : MonoBehaviour
{
    public float maxDistance = 1;

    void Update()
    {
        RaycastHit hit;
        RaycastHit con;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            bool inlay = Input.GetMouseButtonDown(1);
            if (Physics.Raycast(ray, out hit))
            {
                Mesh m = hit.collider.GetComponent<MeshFilter>().mesh;
                if (m != null)
                {
                    // This makes it grow randomly
                    // int ind = Mathf.RoundToInt(Random.Range(0, m.triangles.Length) / 3) * 3;
                    int ind = hit.triangleIndex * 3;
                    int i1 = m.triangles[ind];
                    int i2 = m.triangles[ind + 1];
                    int i3 = m.triangles[ind + 2];

                    Vector3 v1 = m.vertices[i1];
                    Vector3 v2 = m.vertices[i2];
                    Vector3 v3 = m.vertices[i3];
                    Vector3 v4 = (v1 + v2 + v3) / 3;

                    Vector3 normal = Vector3.Cross(v1 - v2, v1 - v3).normalized;
                    // Could definitely optimize distance function
                    float distance = Mathf.Sqrt(Mathf.Pow((Vector3.Distance(v1, v2) + Vector3.Distance(v2, v3) + Vector3.Distance(v1, v3)) / 3, 2) - Mathf.Pow((Vector3.Distance(v1, v4) + Vector3.Distance(v2, v4) + Vector3.Distance(v3, v4)) / 3, 2));
                    v4 += normal * distance * (inlay?-1:1);

                    Vector3 w1 = hit.transform.TransformPoint(v1);
                    Vector3 w2 = hit.transform.TransformPoint(v2);
                    Vector3 w3 = hit.transform.TransformPoint(v3);
                    Vector3 w4 = hit.transform.TransformPoint(v4);

                    List<int> tris = new List<int>(m.triangles);
                    List<Vector3> verts = new List<Vector3>(m.vertices);

                    verts.Add(v4);
                    int i4 = verts.Count - 1;

                    tris.RemoveAt(ind);
                    tris.RemoveAt(ind);
                    tris.RemoveAt(ind);

                    //1
                    Vector3 start = (w1 + w2 + w4) / 3;
                    Vector3 end = Vector3.Cross(w1 - w2, w1 - w4).normalized;
                    Ray check = new Ray(start, end);
                    if (!Physics.Raycast(check, out con, Vector3.Distance(start, w4)))
                    {
                        Debug.DrawRay(start, end * Vector3.Distance(start, w4), Color.green, 5);
                        tris.Add(i1);
                        tris.Add(i2);
                        tris.Add(i4);
                    }
                    else if (CheckSideMatches(i1, i2, i4, con) > -1)
                    {

                    }

                    //2
                    start = (w3 + w2 + w4) / 3;
                    end = Vector3.Cross(w2 - w3, w2 - w4).normalized;
                    check = new Ray(start, end);
                    if (!Physics.Raycast(check, out con, Vector3.Distance(start, w4)))
                    {
                        Debug.DrawRay(start, end * Vector3.Distance(start, w4), Color.green, 5);
                        tris.Add(i2);
                        tris.Add(i3);
                        tris.Add(i4);
                    }
                    else
                    {
                        
                    }

                    //3
                    start = (w1 + w3 + w4) / 3;
                    end = Vector3.Cross(w3 - w1, w3 - w4).normalized;
                    check = new Ray(start, end);
                    if (!Physics.Raycast(check, out con, Vector3.Distance(start, w4)))
                    {
                        Debug.DrawRay(start, end * Vector3.Distance(start, w4), Color.green, 5);
                        tris.Add(i1);
                        tris.Add(i4);
                        tris.Add(i3);
                    }
                    else
                    {
                        
                    }

                    m.vertices = verts.ToArray();
                    m.triangles = tris.ToArray(); 
                    hit.collider.GetComponent<MeshCollider>().sharedMesh = m;
                }
            }
        }
    }

    public int CheckSideMatches(int a1, int a2, int a3, RaycastHit hit)
    {
        Mesh temp = hit.collider.GetComponent<MeshFilter>().mesh;
        int b1 = temp.triangles[hit.triangleIndex * 3];
        int b2 = temp.triangles[hit.triangleIndex * 3 + 1];
        int b3 = temp.triangles[hit.triangleIndex * 3 + 2];
        int count = 0;
        count += a1 == b1 ? 1 : 0;
        count += a1 == b2 ? 1 : 0;
        count += a1 == b3 ? 1 : 0;
        count += a2 == b1 ? 1 : 0;
        count += a2 == b2 ? 1 : 0;
        count += a2 == b3 ? 1 : 0;
        count += a3 == b1 ? 1 : 0;
        count += a3 == b2 ? 1 : 0;
        count += a3 == b3 ? 1 : 0;
        return count;
    }

/*    public void ConnectTris(Mesh o, Vector3 a, Vector3 b, Vector3 c,  RaycastHit hit, bool inlay)
    {
        Mesh m = hit.collider.GetComponent<MeshFilter>().mesh;
        if (m != null)
        {
            int ind = hit.triangleIndex * 3;
            Vector3 v1 = m.vertices[m.triangles[ind]];
            Vector3 v2 = m.vertices[m.triangles[ind + 1]];
            Vector3 v3 = m.vertices[m.triangles[ind + 2]];

            List<int> tris = new List<int>(m.triangles);
            List<Vector3> verts = new List<Vector3>(m.vertices);

            tris.RemoveAt(ind);
            tris.RemoveAt(ind);
            tris.RemoveAt(ind);

            m.vertices = verts.ToArray();
            m.triangles = tris.ToArray();
            hit.collider.GetComponent<MeshCollider>().sharedMesh = m;
        }
    }*/
}
