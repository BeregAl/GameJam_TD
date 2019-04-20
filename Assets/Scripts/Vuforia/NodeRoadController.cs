using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoadController : MonoBehaviour {

    
    private GameObject towerPrefab;    
    private GameObject nodePrefab;  
    private GameObject roadPrefab;

    GameObject ghostNode=null;
    List<GameObject> nodes = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    


    // Start is called before the first frame update
    void Start()
    {
        // Добавляем первую ноду дороги- спавн поинт в список нод
        nodes.Add(ARGameController.instance.mainRoadNode);

        //nodes.Add(Instantiate(nodePrefab));
        // nodes[0].transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            //сюда запишется инфо о пересечении луча, если оно будет
            RaycastHit hit;
            //сам луч, начинается от позиции этого объекта и направлен в сторону цели
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), target.transform.position - transform.position);
            //пускаем луч
            Physics.Raycast(ray, out hit);
            CreateNode(hit.point);
        }

    }   

    public void CreateNode(Vector3 pos)
    {
        GameObject go = Instantiate(nodePrefab);
        go.transform.position = pos;
        nodes.Add(go);
        GameObject roadTmp = Instantiate(roadPrefab);
        roadTmp.transform.position = nodes[nodes.Count - 1].transform.position;
        roadTmp.transform.LookAt(nodes[nodes.Count - 2].transform);
        //Debug.Log("Distance between nodes: " + Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roadTmp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1.33f, Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roads.Add(roadTmp);
    }

    private void DisplayGhostRoad(GameObject ghostNode)
    {
        GameObject go = Instantiate(nodePrefab);
        //go.transform.position = pos;
        //nodes.Add(go);
        GameObject roadTmp = Instantiate(roadPrefab);
        roadTmp.transform.position = nodes[nodes.Count - 1].transform.position;
        roadTmp.transform.LookAt(nodes[nodes.Count - 2].transform);
        //Debug.Log("Distance between nodes: " + Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roadTmp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1.33f, Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roads.Add(roadTmp);

    }

    public IEnumerator GhostRoadCoroutine()
    {

        yield return new WaitForFixedUpdate();
    }
}
