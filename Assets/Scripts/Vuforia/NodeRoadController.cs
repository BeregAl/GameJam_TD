using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoadController : MonoBehaviour {

    public static NodeRoadController instance;
    
    private GameObject towerPrefab;    
    private GameObject nodePrefab;  
    private GameObject roadPrefab;

    [SerializeField]
    private float ghostRoadWitdth;

    GameObject ghostRoad=null;
    Transform ghostNode=null;
    //List<GameObject> GameController.instance.nodes = new List<GameObject>();
    //List<GameObject> GameController.instance.roads = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Добавляем первую ноду дороги- спавн поинт в список нод
        roadPrefab = GameController.instance.roadPrefab;
        GameController.instance.nodes.Add(GameController.instance.mainRoadNode);       
        //GameController.instance.nodes.Add(Instantiate(nodePrefab));
        // GameController.instance.nodes[0].transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount>0 )
        {
            if (ghostNode != null)
            {
                Touch touch = Input.GetTouch(0);               
                FinalizeNode();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {                        
            if (ghostNode != null)
            {                
                Debug.Log("FinalizingNode");
                FinalizeNode();
            }
        }

    }   

    public void FinalizeNode()
    {
        GameController.instance.nodesCount++;
        //Destroy(ghostRoad);
        GameObject finalizedNode = Instantiate(GameController.instance.nodePrefab, GameController.instance.mainTargetContainer);
        finalizedNode.transform.position = ghostNode.position;        
        //ghostNode.transform.SetParent(GameController.instance.mainTargetImage);       
        GameController.instance.nodes.Add(finalizedNode);
        //Debug.Log("Distance between GameController.instance.nodes: " + Vector3.Distance(GameController.instance.nodes[GameController.instance.nodes.Count - 1].transform.position, GameController.instance.nodes[GameController.instance.nodes.Count - 2].transform.position));
        ghostRoad.transform.SetParent(finalizedNode.transform);
        GameController.instance.roads.Add(ghostRoad);
        ghostNode = null;
        ghostRoad = null;
        StopAllCoroutines();
    }

    public void DisplayGhostNodeRoad(Transform m_ghostNode)
    {
        if (ghostRoad == null || !GameController.instance.mainRoadNode.activeInHierarchy)
        {
            ghostNode = m_ghostNode;
            // Начать показ дороги к не добавленный на основной таргет ноде
            ghostNode = m_ghostNode;
            ghostRoad = Instantiate(roadPrefab,m_ghostNode);
            ghostRoad.transform.position = m_ghostNode.position;
            ghostRoad.transform.LookAt(GameController.instance.nodes[GameController.instance.nodes.Count - 1].transform.position);
            ghostRoad.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ghostRoadWitdth, Vector3.Distance(m_ghostNode.position, GameController.instance.nodes[GameController.instance.nodes.Count - 1].transform.position));
            // Как только инициализировали ноду запускаем коротину с обновлением позиции дороги  
            IEnumerator cor = GhostRoadCoroutine(m_ghostNode);
            StartCoroutine(cor);
        }
    }

    IEnumerator GhostRoadCoroutine(Transform ghostNode)
    {
        yield return new WaitForEndOfFrame();
        
        // Дорогу показываем корутиной чтобы не гонять условия в апдейте
        ghostRoad.transform.position = ghostNode.position;
        ghostRoad.transform.LookAt(GameController.instance.nodes[GameController.instance.nodes.Count - 1].transform.position);
        ghostRoad.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ghostRoadWitdth, Vector3.Distance(ghostNode.position, GameController.instance.nodes[GameController.instance.nodes.Count - 1].transform.position));
        StartCoroutine(GhostRoadCoroutine(ghostNode));
    }

    public void ClearGhostRoad()
    {
        StopAllCoroutines();
        if (ghostRoad != null)
        {
            Destroy(ghostRoad);           
            ghostRoad = null;
        }
        ghostNode = null;
    }
}
