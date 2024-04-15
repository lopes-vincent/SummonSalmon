using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _obstacleTarget;

    public List<ObstaclePattern> _obstaclePatterns = new List<ObstaclePattern>();
    
    private ObstaclePattern _currentPattern;
    
    public GameManager GameManager;

    private void Start()
    {
        StartCoroutine(SpawnCycle());
    }
    
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            _currentPattern = _obstaclePatterns[UnityEngine.Random.Range(0, _obstaclePatterns.Count)];
            foreach (var obstacle in _currentPattern.Obstacles)
            {
                GameObject obstacleInstance = Instantiate(obstacle, transform.position, Quaternion.identity);
                MovingObject movingObject = obstacleInstance.GetComponent<MovingObject>();
                movingObject.SetObstacleTarget(_obstacleTarget);
                movingObject.GameManager = GameManager;
                
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(5f);
        }
    }
}

[Serializable]
public struct ObstaclePattern
{
    public List<GameObject> Obstacles;
}
