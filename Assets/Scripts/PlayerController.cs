using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //сюда запишется инфо о пересечении луча, если оно будет
        RaycastHit hit;
        //сам луч, начинается от позиции этого объекта и направлен в сторону цели
        Ray ray = new Ray(transform.position, target.position - transform.position);
        //пускаем луч
        Physics.Raycast(ray, out hit);

        //если луч с чем-то пересёкся, то..
        if (hit.collider != null)
        {
            //если луч не попал в цель
            if (hit.collider.gameObject != target.gameObject)
            {
                Debug.Log("Путь к врагу преграждает объект: " + hit.collider.name);
            }
            //если луч попал в цель
            else
            {
                Debug.Log("Попадаю во врага!!!");
            }
            //просто для наглядности рисуем луч в окне Scene
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(towerPrefab);
            go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }*/
    }
}
