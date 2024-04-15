using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private List<string> _weakTo = new List<string>();
    
    [SerializeField]
    private int _life = 2;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_weakTo.Contains(other.gameObject.tag)) {
            _life--;
            Destroy(other.gameObject);
            if (_life <= 0) {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag.Equals("SalmonKing"))
        {
            other.gameObject.GetComponent<SalmonKing>().LostLife();
            Destroy(gameObject);
        }
    }
}
