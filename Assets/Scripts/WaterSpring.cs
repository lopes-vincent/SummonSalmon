using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class WaterSpring : MonoBehaviour
{
    public float height = 0f;
    public float velocity = 0f;
    
    private float _force = 0f;
    private float _targetHeight = 0f;
    
    [SerializeField]
    private SpriteShapeController _spriteShapeController = null;
    private int _waveIndex = 0;
    private float _resistance = 40f;
    
    [SerializeField]
    private List<string> _affectedByTags = new List<string>();
    
    public void Init(SpriteShapeController spriteShapeController) { 

        var index = transform.GetSiblingIndex();
        _waveIndex = index+1;
        
        _spriteShapeController = spriteShapeController;
        velocity = 0;
        height = transform.localPosition.y;
        _targetHeight = transform.localPosition.y;
    }
    
    
    public void WaveSpringUpdate(float springStiffness, float dampening)
    {
        Vector3 position = transform.localPosition;
        
        height = position.y;
        float x = height - _targetHeight;
        float loss = -dampening * velocity;
        
        _force = -springStiffness * x + loss;
        velocity += _force;
        
        float y = position.y;
        
        transform.localPosition = new Vector3(position.x, y + velocity, position.z);
    }
    
    public void WavePointUpdate() { 
        if (_spriteShapeController != null) {
            Spline waterSpline = _spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(_waveIndex);
            Vector3 newPosition = new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z);
            if (IsPositionValid(waterSpline, _waveIndex, _waveIndex + 1, newPosition))
            {
                waterSpline.SetPosition(_waveIndex, newPosition);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (_affectedByTags.Contains(other.gameObject.tag) ) {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            var speed = rb.velocity;

            velocity += speed.y / _resistance;
        }
    }
    
    private bool IsPositionValid(Spline waterSpline, int index, int next, Vector3 point)
    {
        int pointCount = waterSpline.GetPointCount();
        if (waterSpline.isOpenEnded && (index == 0 || index == pointCount))
            return true;
            
        int prev = (index == 0) ? (pointCount - 1) : (index - 1);
        if (prev >= 0)
        {
            Vector3 diff = waterSpline.GetPosition(prev) - point;
            if (diff.magnitude < 0.01f)
                return false;
        }
        next = (next >= pointCount) ? 0 : next;
        if (next < pointCount)
        {
            Vector3 diff =  waterSpline.GetPosition(next) - point;
            if (diff.magnitude < 0.01f)
                return false;
        }
        return true;
    }
}
