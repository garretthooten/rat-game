using System.Collections;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 0.75f;
    //public IEnumerator moveTrailCoroutine;

    // public void OnDisable()
    // {
    //     if (moveTrailCoroutine != null)
    //         StopCoroutine(moveTrailCoroutine);
    // }
    
    public void MoveTrail(Vector3 spawnPoint, Vector3 hitPoint)
    {
        StartCoroutine(MoveTrailCoroutine(spawnPoint, hitPoint));
    }

    IEnumerator MoveTrailCoroutine(Vector3 spawnPoint, Vector3 hitPoint)
    {
        transform.position = spawnPoint;
        Vector3 direction =  (hitPoint - spawnPoint).normalized;
        while (Vector3.Distance(transform.position, hitPoint) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hitPoint, Time.deltaTime * speed);
            yield return null;
        }

        float time = Time.time;
        float bulletLifeTime = GetComponent<TrailRenderer>().time;
        while (Time.time - time < bulletLifeTime)
        {
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
