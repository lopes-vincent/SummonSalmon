using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private List<string> _weakTo = new List<string>();

    [SerializeField] 
    private GameObject _hitPrefab;
    
    [SerializeField]
    private int _life = 2;

    public GameObject smokePrefab;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_weakTo.Contains(other.gameObject.tag))
        {
            StartCoroutine(takeDamage(other.gameObject));
        }

        if (other.gameObject.tag.Equals("SalmonKing"))
        {
            other.gameObject.GetComponent<SalmonKing>().LostLife();
            Instantiate(_hitPrefab, other.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    
    IEnumerator takeDamage(GameObject other)
    {
        _life--;
        Instantiate(smokePrefab, transform.position, Quaternion.identity, transform);
        Destroy(other);
        yield return new WaitForSeconds(0.2f);
        if (_life <= 0) {
            Destroy(gameObject);
        }
    } 
    
}
