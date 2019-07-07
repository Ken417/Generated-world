using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("新しい枝の太さのスケールの最小値"), SerializeField]
    float thickMin = 0.8f;
    [Header("新しい枝の太さのスケールの最大値"), SerializeField]
    float thickMax = 0.8f;

    [Header("新しい枝の長さのスケールの最小値"), SerializeField]
    float lengthMin = 0.85f;
    [Header("新しい枝の長さのスケールの最大値"), SerializeField]
    float lengthMax = 0.9f;

    [Header("新しい枝の付く角度の最小値"), SerializeField]
    float angleMin = 0;

    [Header("新しい枝の付く角度の最大値"), SerializeField]
    float angleMax = 45;

    [Header("成長させる回数"), SerializeField]
    int resultGrowNum = 100;

    [Header("一つのノードに対する枝の最大数"), SerializeField]
    int maxNodeCount = 2;

    [Header("木の色"), SerializeField]
    Color treeColor = Color.white;

    [Header("葉っぱオブジェクト")]
    public GameObject leaf = null;

    [Header("スペースキー入力で木を成長させるか"), SerializeField]
    bool growUpdate = false;

    [Header("成長を試みた回数確認"), SerializeField]
    int tryGrowNum = 0;
    [Header("成長した回数確認"), SerializeField]
    int growNum = 0;

    const int hierarchyMaxNum = 10;
    [Header("階層にある枝の数確認"), SerializeField]
    int[] hierarchyBranchNum = new int[hierarchyMaxNum];

    public List<GameObject>[] hierarchyBranchList = new List<GameObject>[hierarchyMaxNum];


    GameObject tree;

    private void Awake()
    {
        for(int i = 0; i<hierarchyMaxNum; ++i)
        {
            hierarchyBranchList[i] = new List<GameObject>();
        }
        tryGrowNum = 0;
        growNum = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateBaseStem();
        //GrowNewBranch(baseStem);
        //GrowNewBranch(baseStem);
        CreateTree(resultGrowNum);

    }

    void CreateBaseStem()
    {
        tree = new GameObject("Tree");

        tree.transform.parent = transform;

        GameObject baseStem = GameObject.CreatePrimitive(PrimitiveType.Cube);
        baseStem.GetComponent<Renderer>().material.color = treeColor;
        baseStem.name = "BaseStem";
        baseStem.transform.parent = tree.transform;

        Branch branch = baseStem.AddComponent<Branch>();
        branch.hierarchyNum = 0;
        hierarchyBranchList[branch.hierarchyNum].Add(baseStem);
        hierarchyBranchNum[branch.hierarchyNum]++;
        baseStem.transform.localScale = new Vector3(1, 5, 1);
    }

    void CreateTree(int rGrowNum)
    {
        CreateBaseStem();

        for (int hNum = 0; hNum < hierarchyMaxNum; hNum++)
        {
            //print("hNum = " + hNum + "  hierarchyBranchList[hNum].Count = " + hierarchyBranchList[hNum].Count);

            if (hierarchyBranchList[hNum].Count > 1 && hNum > 0)
            {
                hierarchyBranchList[hNum].Sort((a, b) => (int)a.GetComponent<Branch>().parentBranch.nodeCount - (int)b.GetComponent<Branch>().parentBranch.nodeCount);
            }
            for (int bNum = 0; bNum < hierarchyBranchList[hNum].Count; bNum++)
            {
                //print(hierarchyBranchList[hNum][bNum].GetComponent<Branch>().parentBranch.nodeCount);
                for (int n = 0; n < maxNodeCount; n++)
                {
                    tryGrowNum++;
                    //print("hierarchyBranchList[" + hNum + "][" +bNum + "]");
                    GameObject gnb = GrowNewBranch(hierarchyBranchList[hNum][bNum]);
                    if (gnb)
                    {
                        growNum++;
                        if (growNum > rGrowNum)
                        {
                            return;
                        }
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //CreateTree()より綺麗に育つ、育つ枝のセレクトはランダムのほうが自然な感じになる
        if (Input.GetKey(KeyCode.Space) && growUpdate)
        {
            tryGrowNum++;
            var b = tree.GetComponentsInChildren<Branch>();
            int n = 0;
            foreach (var b2 in b)
            {
                n++;
            }
            GameObject gnb = GrowNewBranch(b[Random.Range(0, n)].gameObject);
            if (gnb)
            {
                growNum++;
            }
        }
    }

    bool CheckFibonacci(int hierarchyNum)
    {
        if (hierarchyNum > 0)
        {
            int h1 = hierarchyBranchNum[hierarchyNum - 1];
            int h2 = 1;
            if (hierarchyNum > 1)
            {
                h2 = hierarchyBranchNum[hierarchyNum - 2];
            }
            if (hierarchyBranchNum[hierarchyNum] == h1 + h2)
            {
                return false;
            }
        }
        else
        {
            if (hierarchyBranchNum[hierarchyNum] == 1)
            {
                return false;
            }
        }
        return true;
    }

    //枝を作るかどうかの基本チェック関数
    bool CreateCheck(GameObject parentBranch)
    {
        Branch parentBranchComp = parentBranch.GetComponent<Branch>();
        int newBranchHierarchyNum = parentBranchComp.hierarchyNum + 1;

        //フィボナッチの階層が配列の最大数を超えていないか
        if (newBranchHierarchyNum == hierarchyMaxNum) { return false; }


        //ランダムで規則以外も許可
        if (Random.Range(0,10) == 0)
        {
            return true;
        }


        //一つのノードに対する枝の最大数のチェック
        if (parentBranchComp.nodeCount >= maxNodeCount)
        {
            print(parentBranchComp.nodeCount);
            return false;
        }

        //フィボナッチチェック
        return CheckFibonacci(newBranchHierarchyNum);
    }

    GameObject CreateCloneBranch(GameObject parentBranch, string name)
    {
        GameObject clone = GameObject.Instantiate(parentBranch);
        Branch newBranchComp = clone.GetComponent<Branch>();
        newBranchComp.parentBranch = parentBranch.GetComponent<Branch>();
        newBranchComp.nodeList = new List<Node>();
        newBranchComp.hierarchyNum++;//自分が何階層の枝なのか記録
        hierarchyBranchList[newBranchComp.hierarchyNum].Add(clone);
        hierarchyBranchNum[newBranchComp.hierarchyNum]++;//階層の枝の数を増やす。
        clone.name = name + newBranchComp.hierarchyNum + "-" + hierarchyBranchNum[newBranchComp.hierarchyNum];

        return clone;
    }

    GameObject CreateNode(GameObject parentBranch)
    {
        GameObject node = new GameObject("Node");
        parentBranch.GetComponent<Branch>().nodeList.Add(node.AddComponent<Node>());
        return node;
    }

    void CreateLeafs(GameObject branch)
    {
        if(leaf && branch.transform.lossyScale.y < 2f && branch.transform.lossyScale.y > 1.87f)
        {
            int leafNum = 2;
            for(int i = 0; i< leafNum; i++)
            {
                GameObject clone = GameObject.Instantiate(leaf);
                clone.transform.parent = branch.transform.parent;
                Vector3 pos = Vector3.zero;
                pos.y = (branch.transform.lossyScale.y / (leafNum + 1)) * i;
                clone.transform.localPosition = pos;
                pos = clone.transform.position;
                pos.y += branch.transform.lossyScale.y / 5;
                clone.transform.position = pos;
                float s = branch.transform.lossyScale.y*3;
                clone.transform.localScale = new Vector3(s,s,s);
                clone.transform.localRotation = Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));
            }
        }
    }

    GameObject GrowNewBranch(GameObject parentBranch)
    {
        return GrowNewBranch(parentBranch, true);
    }

    GameObject GrowNewBranch(GameObject parentBranch,bool check)
    {
        if(check)
        {
            if (!CreateCheck(parentBranch)) { return null; }
        }

        GameObject newBranch = CreateCloneBranch(parentBranch,"Branch");
        GameObject newNode = CreateNode(parentBranch);
        newBranch.transform.parent = newNode.transform;
        newNode.transform.parent = parentBranch.transform.parent;


        //新しい枝の縮小
        Vector3 newBranchScale = newBranch.transform.localScale;
        newBranchScale.x *= Random.Range(thickMin,thickMax);
        newBranchScale.y *= Random.Range(lengthMin,lengthMax);
        newBranchScale.z = newBranchScale.x;
        newBranch.transform.localScale = newBranchScale;

        //新しい枝の位置調整
        newBranch.transform.localPosition = new Vector3(0, newBranch.transform.lossyScale.y / 2, 0);

        //新しいノードの位置調整
        newNode.transform.localPosition = new Vector3(0, parentBranch.transform.lossyScale.y / 2 + parentBranch.transform.localPosition.y, 0);

        Branch parentBranchComp = parentBranch.GetComponent<Branch>();
        parentBranchComp.SortNodeList();

        float angle = 0;      //スペース
        int spaceNode = 0;    //スペースのあるノードの隣
        int nodeNum = parentBranchComp.nodeCount;
        if (nodeNum > 1)
        {
            for (int i = 0; i < nodeNum - 1; ++i)
            {
                float f = parentBranchComp.nodeList[i + 1].yRot - parentBranchComp.nodeList[i].yRot;
                if (angle < f)
                {
                    angle = f;
                    spaceNode = i;
                }
            }
            //一番回転している枝を一番回転してない枝と比べる
            float f2 = (360 - parentBranchComp.nodeList[nodeNum - 1].yRot) + parentBranchComp.nodeList[0].yRot;
            if (angle < f2)
            {
                angle = f2;
                spaceNode = nodeNum - 1;
            }
        }

        Node newNodeComp = newNode.GetComponent<Node>();


        if (nodeNum == 0)
        {
            newNodeComp.yRot = Random.Range(0, 360);//最初の枝
        }
        else
        {
            if (angle == 0) { angle = 360; }//二番目の枝
            newNodeComp.yRot = parentBranchComp.nodeList[spaceNode].yRot + (angle / 2) + Random.Range(0, angle / 10);
        }

        if (newNodeComp.yRot > 360)
        {
            newNodeComp.yRot -= 360;
        }

        newNode.transform.localRotation = /*newNode.transform.rotation * Quaternion.identity * */Quaternion.Euler(Random.Range(angleMin,angleMax), newNodeComp.yRot, 0);

        CreateLeafs(newBranch);

        return newBranch;
    }

}
