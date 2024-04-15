using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private Transform _obstacleTarget;

    [SerializeField] private float _waveMakerAmplitude = 0f;
    
    [SerializeField]
    private float _speed = 1.5f;

    private Rigidbody2D _rigidBody2D;
    
    [CanBeNull] public GameManager GameManager;
    
    public void SetObstacleTarget(Transform target)
    {
        _obstacleTarget = target;
    }
    
    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());
    }

    void FixedUpdate()
    {
        Vector2 direction = (_obstacleTarget.position - transform.position).normalized;
        Vector2 velocity = direction * _speed;

        if (null != GameManager)
        {
            velocity *= GameManager.speedFactor;
        }

        if (_waveMakerAmplitude != 0f)
        {
            velocity = new Vector2(velocity.x, velocity.y + _waveMakerAmplitude);
        }
        _rigidBody2D.velocity = velocity;
    }
    
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1.2f));
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _waveMakerAmplitude *= -1f;
        }
    }
}
