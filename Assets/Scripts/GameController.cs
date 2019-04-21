using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public WaveInfo wave;
    public static GameController instance;
    private int _points=0;

    [Header("Ссылки на основные игровые объекты")]
    // Ссылки на основные игровые объекты   
    public GameObject mainBase;   
    public GameObject mainRoadNode;
    public Transform cameraTransform;
    public GameObject pointsCanvas;
    public TextMeshProUGUI pointsText;
    public GameObject AbilityPoint;

    [Header("Ссылки на основные AR объекты")]
    // Ссылки на основные AR объекты
    public Transform mainTargetContainer;

    [Header("UI элементы")]
    public GameObject startGameButton;
    public GameObject startWaveButton;

    [Header("Все остальное")]

    public TextMeshProUGUI nodesText;
    public TextMeshProUGUI towersText;
    
    [SerializeField]
    public GameObject towerPrefab;  
    [SerializeField]
    public GameObject nodePrefab;
    [SerializeField]
    public GameObject roadPrefab;

    public List<GameObject> nodes = new List<GameObject>();
    public List<GameObject> roads = new List<GameObject>();

    public int towersCount = 0;
    public int nodesCount = 0;

    public TextMeshProUGUI AbilityTimer;
    public int abilityCountdown = 3;
    public Coroutine abilityCoroutine;

    // Переменные для логики игры
    public Transform ghostTower;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startWaveButton.SetActive(false);
        startGameButton.SetActive(true);
        //nodes.Add(Instantiate(nodePrefab));
        //nodes[0].transform.position = new Vector3(0, 0.5f, 0);
        //nodesCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostTower != null)
        {
            if (Input.touchCount > 0)
            {
                    Touch touch = Input.GetTouch(0);
                    CmdCreateTower(ghostTower.position);
                
            }

            if (Input.GetMouseButtonDown(0))
            {   
                
                    Debug.Log("PlacingTower");
                    CmdCreateTower(ghostTower.position);
            }
        }
    }

    public void StartWave(WaveInfo _wave)
    {
        Debug.Log("Понеслась");
        StartCoroutine(SpawningCoroutine());
    }

    public void StartGame()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 rndPosition = mainBase.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), -0.5f, Random.Range(-1.5f, 1.5f));
            CmdCreateTower(rndPosition);
        }
        startGameButton.SetActive(false);
        startWaveButton.SetActive(true);
    }

    public void CmdCreateNode(Vector3 pos)
    {
        Debug.Log("Cmd has executed!");
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
    }
    
    public void CmdCreateTower(Vector3 towerTransformPosition)
    {
        GameObject tower = Instantiate(towerPrefab, mainTargetContainer) as GameObject;
        tower.transform.position = towerTransformPosition;        
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
                GameObject goTmp = Instantiate(wave.waves[j].hi.humanPrefab,mainTargetContainer);
                goTmp.GetComponent<Enemy>().hp = wave.waves[j].hi.health;
                goTmp.GetComponent<Enemy>().speed = wave.waves[j].hi.speed;
                //goTmp.transform.position = nodes[nodes.Count-1].transform.position;
                //StartCoroutine(goTmp.GetComponent<Enemy>().HumanTravelling(wave.waves[0].hi.speed, nodes));
                yield return new WaitForSeconds(1.5f);
            }
        }

    }

    public void AbilityRun()
    {
        abilityCountdown = 3;
        abilityCoroutine = StartCoroutine(AbilityCoroutine());
    }

    IEnumerator AbilityCoroutine()
    {
        for (int i = abilityCountdown; i > 0; i--)
        {
            AbilityTimer.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        AbilityTimer.text = "";
        GameObject[] enemiesObjects = GameObject.FindGameObjectsWithTag("enemy");
        foreach (var en in enemiesObjects)
        {
            if (Vector3.Distance(AbilityPoint.transform.position, en.transform.position) < 0.8f)
            {
                Debug.Log("Dist: " + Vector3.Distance(AbilityPoint.transform.position, en.transform.position));
                en.GetComponent<Enemy>().BoostSpeed();
            }
        }
        yield return null;

    }

    public void AlignPoints()
    {
       // Debug.Log("Align points");
        StartCoroutine(AlignPointsToCameraCoroutine());
    }

    IEnumerator AlignPointsToCameraCoroutine()
    {
        while (true)
        {
            pointsCanvas.transform.LookAt(cameraTransform.position);
            yield return new WaitForSeconds(0.3f);
        }
    }


    public int Points
    {
        get { return _points; }
        set
        {
            _points = value;
            pointsText.text = _points.ToString();
        }
    }
}
