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
    GameObject ghostNode=null;
    List<GameObject> nodes = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Добавляем первую ноду дороги- спавн поинт в список нод
        roadPrefab = ARGameController.instance.roadPref;
        nodes.Add(ARGameController.instance.mainRoadNode);       
        //nodes.Add(Instantiate(nodePrefab));
        // nodes[0].transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount>0 )
        {
            if (ghostNode != null)
            {
                Touch touch = Input.GetTouch(0);
                //сюда запишется инфо о пересечении луча, если оно будет
                RaycastHit hit;
                //сам луч, начинается от позиции этого объекта и направлен в сторону цели
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), target.transform.position - transform.position);
                //пускаем луч
                Physics.Raycast(ray, out hit);
                FinalizeNode();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Touch touch = Input.GetTouch(0);
            //сюда запишется инфо о пересечении луча, если оно будет
            RaycastHit hit;
            //сам луч, начинается от позиции этого объекта и направлен в сторону цели
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), target.transform.position - transform.position);
            //пускаем луч
            Physics.Raycast(ray, out hit);
            FinalizeNode();
        }

    }   

    public void FinalizeNode()
    {
        //Destroy(ghostRoad);
        GameObject finalizedNode = Instantiate(ARGameController.instance.nodePref, ARGameController.instance.mainTargetImage);
        finalizedNode.transform.position = ghostNode.transform.position;        
        //ghostNode.transform.SetParent(ARGameController.instance.mainTargetImage);       
        nodes.Add(finalizedNode);        
        //Debug.Log("Distance between nodes: " + Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roads.Add(ghostRoad);
        ghostNode = null;
        ghostRoad = null;
        StopAllCoroutines();
    }

    public void DisplayGhostNodeRoad(Transform ghostNode)
    {
        if (ghostRoad == null || !ARGameController.instance.mainRoadNode.activeInHierarchy)
        {
            // Начать показ дороги к не добавленный на основной таргет ноде
            ghostRoad = Instantiate(roadPrefab);
            ghostRoad.transform.position = ghostNode.position;
            ghostRoad.transform.LookAt(nodes[nodes.Count - 1].transform.position);
            ghostRoad.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ghostRoadWitdth, Vector3.Distance(ghostNode.position, nodes[nodes.Count - 1].transform.position));
            // Как только инициализировали ноду запускаем коротину с обновлением позиции дороги  
            IEnumerator cor = GhostRoadCoroutine(ghostNode);
            StartCoroutine(cor);
        }
    }

    IEnumerator GhostRoadCoroutine(Transform ghostNode)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("BuildingSHIT");
        // Дорогу показываем корутиной чтобы не гонять условия в апдейте
        ghostRoad.transform.position = ghostNode.position;
        ghostRoad.transform.LookAt(nodes[nodes.Count - 1].transform.position);
        ghostRoad.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ghostRoadWitdth, Vector3.Distance(ghostNode.position, nodes[nodes.Count - 1].transform.position));
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
