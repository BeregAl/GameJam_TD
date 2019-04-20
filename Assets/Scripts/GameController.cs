using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{

    public WaveInfo wave;
    public static GameController instance;

    public TextMeshProUGUI nodesText;
    public TextMeshProUGUI towersText;

    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject roadPrefab;

    public List<GameObject> nodes = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    [SyncVar]public int towersCount = 0;
    [SyncVar] public int nodesCount = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nodes.Add(Instantiate(nodePrefab));
        nodes[0].transform.position = new Vector3(0, 0.5f, 0);
        nodesCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(WaveInfo _wave)
    {
        StartCoroutine(SpawningCoroutine());
    }

    [Command]
    public void CmdCreateNode(Vector3 pos)
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
        nodesCount++;
        nodesText.text = "Дорог: " + (nodesCount-1).ToString();
        Debug.Log("Netw serv1");
        NetworkServer.Spawn(go);
        Debug.Log("Netw serv2");
        NetworkServer.Spawn(roadTmp);
    }

    public void CreateTower(Vector3 pos)
    {
        GameObject go = Instantiate(towerPrefab);
        go.transform.position = pos;
        towersCount++;
        towersText.text = "Башен: "+towersCount.ToString();
        /*
        nodes.Add(go);
        GameObject roadTmp = Instantiate(roadPrefab);
        roadTmp.transform.position = nodes[nodes.Count - 1].transform.position;
        roadTmp.transform.LookAt(nodes[nodes.Count - 2].transform);
        //Debug.Log("Distance between nodes: " + Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roadTmp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1.33f, Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position));
        roads.Add(roadTmp);
        */
    }

    IEnumerator SpawningCoroutine()
    {
        for (int j = 0; j < wave.waves.Length; j++)
        {
            for (int i = 0; i < wave.waves[j].amount; i++)
            {
                GameObject goTmp = Instantiate(wave.waves[j].hi.humanPrefab);
                goTmp.GetComponent<Enemy>().hp = wave.waves[j].hi.health;
                goTmp.GetComponent<Enemy>().speed = wave.waves[j].hi.speed;
                //goTmp.transform.position = nodes[nodes.Count-1].transform.position;
                //StartCoroutine(goTmp.GetComponent<Enemy>().HumanTravelling(wave.waves[0].hi.speed, nodes));
                yield return new WaitForSeconds(1.5f);
            }
        }

    }
    
    


}
