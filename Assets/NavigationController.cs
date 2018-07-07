using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour {
    List<Tile> allTiles = new List<Tile>();
    public List<Node> allNodes = new List<Node>();

    public float edgeDist, obstructionCheckRadius;

    public LayerMask obstructionMask;

    public bool drawGizmos;
	// Use this for initialization
	void Start () {
        allTiles = new List<Tile>(Tile.allTiles);
        StartCoroutine(Generation());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Generation ()
    {
        yield return StartCoroutine(GenerateNodes());
        yield return StartCoroutine(GenerateEdges());
        yield return StartCoroutine(DetectObstructions());
        yield break;
    }
    IEnumerator GenerateNodes ()
    {
        foreach (Tile t in allTiles)
        {
            Node newNode = new Node()
            {
                position = t.transform.position,
                obstructed = false,
                edges = new List<Edge>()
            };

            allNodes.Add(newNode);
        }
        yield break;
    }

    IEnumerator GenerateEdges ()
    {
        int nCounter = 0;
        int nAverage = 0;
        foreach (Node n in allNodes)
        {
            foreach (Node nn in allNodes)
            {
                float dist = Vector3.Distance(n.position, nn.position);
                if (n != nn && dist < edgeDist)
                {
                    nCounter++;
                    n.edges.Add(new Edge()
                    {
                        endNode = nn
                    });
                }
            }
        }
        print(nCounter);
        yield break;
    }

    IEnumerator DetectObstructions ()
    {
        foreach (Node n in allNodes)
        {
            bool obstructed = Physics.CheckSphere(n.position, obstructionCheckRadius, obstructionMask);
            n.obstructed = obstructed;
        }

        yield break;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            foreach (Node n in allNodes)
            {
                Gizmos.color = Color.blue;
                Vector3 heightOffset = Vector3.up * 0.75f;
                Gizmos.DrawWireSphere(n.position + heightOffset, 0.2f);

                foreach (Edge e in n.edges)
                {
                    Gizmos.DrawLine(n.position + heightOffset, e.endNode.position + heightOffset);
                }
            }
        }
    }
}
//[System.Serializable]
public class Node
{
    public Vector3 position;
    public bool obstructed;
    public List<Edge> edges = new List<Edge>();
}

//[System.Serializable]
public class Edge
{
    public Node endNode;
}
