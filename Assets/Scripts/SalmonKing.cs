using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SalmonKing : MonoBehaviour
{
    public Vector2 _movementDirection;
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private float _forceAmount;
    
    public bool _inWater = true;
    public bool _canMove = true;
    public float _outWaterTimeElapsed = 0f;
    
    [SerializeField]
    private float _life = 3f; 
    
    [SerializeField]
    private float _airGravity = 5f;    
    [SerializeField]
    private float _waterGravity = -5f; 
    [SerializeField]
    private float _waterFlowForce = 0.5f;

    [SerializeField] private Transform _salmonContainer;
    [SerializeField] private GameObject _salmonBeaverPrefab;
    [SerializeField] private GameObject _salmonKnightPrefab;
    [SerializeField] private Transform _salmonTarget;
    
    private bool _isInvincible = false;
    
    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private List<Image> _lifeImages = new List<Image>();
    private Queue<Image> _lifeImageQueue = new Queue<Image>();
    
    [SerializeField]
    private Animation _cooldownAnimation;
    private float _cooldownTime = 1f;
    private bool _isOnCooldown = false;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector3.down * _forceAmount;
        _lifeImages.ForEach(image => _lifeImageQueue.Enqueue(image));
    }
    
    public void LostLife()
    {
        if (!_isInvincible)
        {
            _life--;
            StartCoroutine(InvincibilityFrames());
            Image lifeImage = _lifeImageQueue.Dequeue();
            Destroy(lifeImage.gameObject);
            if (_life <= 0)
            {
                SceneManager.LoadScene("End");
            }
        }
    }
    
    IEnumerator InvincibilityFrames()
    {
        _isInvincible = true;
        _animator.SetTrigger("Invincible");
        yield return new WaitForSeconds(1f);
        _isInvincible = false;
        _animator.SetTrigger("Vincible");
    }

    private void SpawnSalmon(GameObject salmonPrefab)
    {
        if (_isOnCooldown)
        {
            return;
        }
        
        GameObject salmon = Instantiate(salmonPrefab, transform.position, Quaternion.identity, _salmonContainer);
        salmon.GetComponent<MovingObject>().SetObstacleTarget(_salmonTarget);
        StartCoroutine(Cooldown());
    }
    
    IEnumerator Cooldown()
    {
        _cooldownAnimation.Play();
        _isOnCooldown = true;
        yield return new WaitForSeconds(_cooldownTime);
        _isOnCooldown = false;
    }

    public void Update()
    {        
        if (Input.GetKeyUp(KeyCode.B))
        {
             SpawnSalmon(_salmonBeaverPrefab);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnSalmon(_salmonKnightPrefab);
        }
        
        float xMovement = Input.GetAxis("Horizontal");

        if (_inWater)
        {
            xMovement -= _waterFlowForce;
        }
        _movementDirection = new Vector2(xMovement, Input.GetAxis("Vertical"));
    }


    void FixedUpdate()
    {
        if (!_inWater)
        {
            _outWaterTimeElapsed += Time.deltaTime;
        }

        if (_outWaterTimeElapsed > 0.7f)
        {
            _canMove = false;
        }

        if (_canMove)
        {
            _rigidbody2D.velocity = _movementDirection * _forceAmount;
        }

    }
    
    public void setOutOfWater() {
        _inWater = false;
        _rigidbody2D.gravityScale = _airGravity;
    }
    
    public void setInWater() {
        _inWater = true;
        _outWaterTimeElapsed = 0f;
        _canMove = true;
        _rigidbody2D.gravityScale = _waterGravity;
    }
}
