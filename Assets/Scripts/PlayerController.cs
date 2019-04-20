using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
    }
    
    public void SpawnNode()
    {
        if (GameController.instance.nodesCount < 5)
        {
            //сюда запишется инфо о пересечении луча, если оно будет
            RaycastHit hit;
            //сам луч, начинается от позиции этого объекта и направлен в сторону цели
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), target.transform.position - transform.position);
            //пускаем луч
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Cube")
                {
                    GameController.instance.CmdCreateNode(hit.point);

                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            SpawnNode();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SpawnTower();
        }

    }

    public void SpawnTower()
    {
        if (GameController.instance.towersCount < 3)
        {
            //сюда запишется инфо о пересечении луча, если оно будет
            RaycastHit hit;
            //сам луч, начинается от позиции этого объекта и направлен в сторону цели
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), target.transform.position - transform.position);
            //пускаем луч
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Cube")
                {
                    GameController.instance.CmdCreateTower(hit.point);

                }
            }
        }
    }
    

}
