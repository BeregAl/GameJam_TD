using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public WaveInfo wave;
    public static GameController instance;

    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject roadPrefab;

    List<GameObject> nodes = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nodes.Add(Instantiate(nodePrefab));
        nodes[0].transform.position = new Vector3(0, 0.5f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(WaveInfo _wave)
    {
        StartCoroutine(SpawningCoroutine());
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

    IEnumerator SpawningCoroutine()
    {
        for (int i = 0; i < wave.waves[0].amount; i++)
        {
            GameObject goTmp = Instantiate(wave.waves[0].hi.humanPrefab);
            StartCoroutine(HumanTravelling(goTmp, wave.waves[0].hi.speed));
            yield return new WaitForSeconds(0.5f);
        }

    }
    
    IEnumerator HumanTravelling(GameObject human, float speed)
    {
        int countdown = nodes.Count-1;
        Debug.Log("cd: "+ countdown);
        while (countdown > 0)
        {
            human.transform.position = nodes[countdown].transform.position;
            human.transform.LookAt(nodes[countdown - 1].transform.position);
            while (Vector3.Distance(human.transform.position, nodes[countdown - 1].transform.position) >0.1f)
            {
                human.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                yield return null;
            }

            countdown--;
        }
        yield return null;
    }


}
