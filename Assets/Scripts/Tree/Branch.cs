using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{

    public List<Node> nodeList = new List<Node>();
    public GameObject masterNode = null;
    public Branch parentBranch = null;
    public int hierarchyNum = 0;

    public int nodeCount
    {
        get
        {
            return nodeList.Count;
        }
    }

    //回転が少ない順に並び替える
    public void SortNodeList()
    {
        nodeList.Sort((a, b) => (int)a.yRot - (int)b.yRot);
    }

}
