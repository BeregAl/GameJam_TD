using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float speed;
    public bool speedBoosted;
    public bool invulnerable;

    public GameObject speedBoostParticles;
    public Vector3 invulScaleBoost= new Vector3(3f,3f,3f);
    public Vector3 invulScaleDecrease = new Vector3(0.33f, 0.33f, 0.33f);


    private Coroutine movingCor;
    // Start is called before the first frame update
    void Start()
    {
        movingCor = StartCoroutine(HumanTravelling(GameController.instance.nodes));
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

    public IEnumerator HumanTravelling( List<GameObject> nodes)
    {
        GetComponent<Animator>().Play("Walk");
        int countdown = nodes.Count - 1;
        //Debug.Log("cd: " + countdown);
        while (countdown > 0)
        {
           // Debug.Log("prev: " + transform.position);
            transform.position = nodes[countdown].transform.position;
            transform.LookAt(nodes[countdown - 1].transform.position);
            //Debug.Log("prev2: " + transform.position);
            while (Vector3.Distance(transform.position, nodes[countdown - 1].transform.position) > 0.1f)
            {
                transform.LookAt(nodes[countdown - 1].transform.position);
                //Debug.Log("thru: " + human.transform.position);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                yield return null;
            }
            //Debug.Log("after: " + transform.position);

            countdown--;
        }

        GameController.instance.Points += 1;
        yield return null;
        DestroyIt();
    }

    public void Invul()
    {
        StartCoroutine(BoostSpeedCoroutine());
        //GetComponent<MeshRenderer>()
    }

    public void BoostSpeed()
    {
        StartCoroutine(InvulnerableCoroutine());
        //GetComponent<MeshRenderer>()
    }

    IEnumerator BoostSpeedCoroutine()
    {
        speed *= 1.5f;
        speedBoosted = true;
        speedBoostParticles.SetActive(true);
        yield return new WaitForSeconds(4f);
        speedBoostParticles.SetActive(false);
        speedBoosted = false;
        speed /= 1.5f;
    }

    IEnumerator InvulnerableCoroutine()
    {
        float tmp = hp;
        hp = 99999;
        invulnerable = true;
        transform.localScale=Vector3.Scale(transform.localScale, invulScaleBoost);
        yield return new WaitForSeconds(4f);
        transform.localScale=Vector3.Scale(transform.localScale, invulScaleDecrease);
        invulnerable = false;
        hp = tmp;
    }
}
