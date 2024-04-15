using System.Collections;
using UnityEngine;

public class WaveMakerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _waveMakerPrefab;
    
    [SerializeField]
    private Transform _obstacleTarget;

    public GameManager GameManager;

    void Start()
    {
        StartCoroutine(SpawnWaveMaker());
    }
    
    IEnumerator SpawnWaveMaker()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            GameObject waveMaker = Instantiate(_waveMakerPrefab, transform.position, Quaternion.identity);
            MovingObject movingObject = waveMaker.GetComponent<MovingObject>();
            movingObject.SetObstacleTarget(_obstacleTarget);
            movingObject.GameManager = GameManager;
        }
    }


}
