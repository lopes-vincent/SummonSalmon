using UnityEngine;

public class WaterZone : MonoBehaviour
{
    [SerializeField]
    private SalmonKing _salmonKing;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("SalmonKing")) {
            _salmonKing.setInWater();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag.Equals("SalmonKing")) {
            _salmonKing.setOutOfWater();
        }
    }
}
