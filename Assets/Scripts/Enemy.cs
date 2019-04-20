using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp=5;

    private Coroutine movingCor;
    // Start is called before the first frame update
    void Start()
    {
        movingCor = StartCoroutine(HumanTravelling(5f, GameController.instance.nodes));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int _d)
    {
        hp -= _d;
        if (hp <= 0)
        {
            StopCoroutine(movingCor);
            Dying();
            Invoke("DestroyIt",1f);
        }
    }

    private void Dying()
    {
        GetComponent<Animator>().Play("Die");
    }

    private void DestroyIt()
    {
        Destroy(gameObject);
    }

    public IEnumerator HumanTravelling(float speed, List<GameObject> nodes)
    {
        GetComponent<Animator>().Play("Walk");
        int countdown = nodes.Count - 1;
        Debug.Log("cd: " + countdown);
        while (countdown > 0)
        {
            Debug.Log("prev: " + transform.position);
            transform.position = nodes[countdown].transform.position;
            transform.LookAt(nodes[countdown - 1].transform.position);
            Debug.Log("prev2: " + transform.position);
            while (Vector3.Distance(transform.position, nodes[countdown - 1].transform.position) > 0.1f)
            {
                transform.LookAt(nodes[countdown - 1].transform.position);
                //Debug.Log("thru: " + human.transform.position);
                transform.Translate(Vector3.forward * 3f * Time.deltaTime);
                yield return null;
            }
            Debug.Log("after: " + transform.position);

            countdown--;
        }
        yield return null;
    }


}
