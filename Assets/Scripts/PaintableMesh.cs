using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableMesh : MonoBehaviour
{
    [SerializeField] private LayerMask _detectableObjects;

    private Mesh _mesh;
    private int _detectedTriIndex;
    private RaycastHit hits;

  
    [SerializeField] private float _brushSize;

    public bool DetectMesh()
    {
        
        Ray centerRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
        bool detecting = Physics.Raycast(centerRay,out hits, 2000,_detectableObjects);
        _detectedTriIndex = hits.triangleIndex;

        return detecting;
    }

    public void PaintTriangle(Color color)
    {
        _mesh = hits.collider.GetComponent<MeshFilter>().mesh;
        if(_mesh == null)
            return;
    
        
        Vector3[] vertices = _mesh.vertices; 
        Color[] colors;
        if (_mesh.colors.Length > 0) 
        {
            colors = _mesh.colors;
        } else 
        {
            colors = new Color[vertices.Length];
        }
        for (int i = 0; i < vertices.Length; i++) 
        {
            Vector3 vertPos = hits.transform.TransformPoint(vertices[i]);
            float sqrMag = (vertPos - hits.point).sqrMagnitude;
            if (sqrMag > _brushSize) 
            {
                colors[i] = Color.white;
                continue;
            } 
            else 
            {
                    
                colors[i] = color;
            }
        }
        _mesh.colors = colors;

       /* int v0 = triangles[i0];
        int v1 = triangles[i1];
        int v2 = triangles[i2];

        
        //Debug.Log("colors array length:" + colors.Length);
        colors[v0] = color;
        colors[v1] = color;
        colors[v2] = color;

        _mesh.colors = colors;*/

    }

}
