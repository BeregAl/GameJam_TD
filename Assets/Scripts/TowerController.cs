using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject currentTarget = null;
    public float range = 7.5f;
    public Transform projectileOrigin;


    public GameObject KepchukCloud;

    private GameObject projectileInstance;

    private float attackCountdown = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        projectileInstance = Instantiate(KepchukCloud);
        projectileInstance.transform.position = projectileOrigin.position;
        InvokeRepeating("CheckTarget",0f,0.5f);
        //InvokeRepeating("RotateToEnemy", 0f, 0.5f);
        StartCoroutine(AttackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateToEnemy()
    {
        if (currentTarget != null)
        {
            transform.LookAt(currentTarget.transform);
        }
    }

    void CheckTarget()
    {
        GameObject actualTarget = null;
        float closestEnemyDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject en in enemies)
        {
            float dist = Vector3.Distance(transform.position, en.transform.position);
            if (dist < closestEnemyDistance && dist <=range)
            {
                closestEnemyDistance = dist;
                actualTarget = en;
            }
        }
        
        currentTarget = actualTarget;
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(transform.position, range);
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            float curTime = Time.time;
            //delay перед атакой
            while (Time.time - curTime < attackCountdown)
            {
                yield return null;
            }

            if (currentTarget != null)
            {
                projectileInstance.SetActive(true);
                GameObject projectileTarget = currentTarget;
                

                while (currentTarget!=null )
                {
                    if (Vector3.Distance(projectileInstance.transform.position, projectileTarget.transform.position) > 1f)
                    {
                        projectileInstance.transform.LookAt(projectileTarget.transform.position);
                        projectileInstance.transform.Translate(Vector3.forward * 15f * Time.deltaTime);
                        yield return null;
                    }
                    else
                    {
                        currentTarget.GetComponent<Enemy>().TakeDamage(1);
                        break;
                    }
                }
                projectileInstance.SetActive(false);
                projectileInstance.transform.position = projectileOrigin.position;

            }
        }
    }
    
}
