using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class WaterShapeController : MonoBehaviour
{
    public float spread = 0.006f;
    
    [SerializeField]
    private GameObject wavePointPrefab;
    [SerializeField]
    private GameObject wavePoints;
    
    [SerializeField]
    private float _dampening = 0.03f;
    
    [SerializeField]
    private float _springStiffness = 0.1f;
    
    [SerializeField]
    private List<WaterSpring> _waterSprings = new ();

    private int CornersCount = 2;
    [SerializeField]
    private SpriteShapeController _spriteShapeController;

    [SerializeField]
    [Range(1, 100)]
    private int WaveCount;
    
    void Start() { 
        StartCoroutine(CreateWaves());
    }
    
    void OnValidate() {
        // StartCoroutine(CreateWaves());
    }
    
    IEnumerator CreateWaves() {
        foreach (Transform child in wavePoints.transform) {
            StartCoroutine(Destroy(child.gameObject));
        }
        yield return null;
        SetWaves();
        yield return null;
    }
    
    IEnumerator Destroy(GameObject go) {
        yield return null;
        DestroyImmediate(go);
    }
    
    private void SetWaves()
    {
        Spline waterSpline = _spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();
        
        for (int i = CornersCount; i < waterPointsCount - CornersCount; i++) {
            waterSpline.RemovePointAt(CornersCount);
        }
        
        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

        float spacingPerWave = waterWidth / (WaveCount + 1);

        for (int i = WaveCount; i > 0; i--)
        {
            int index = CornersCount;
            
            float xPosition = waterTopLeftCorner.x + (spacingPerWave * i);
            Vector3 wavePoint = new Vector3(xPosition, waterTopLeftCorner.y, waterTopLeftCorner.z);
            waterSpline.InsertPointAt(index, wavePoint);
            waterSpline.SetHeight(index, 0.1f);
            waterSpline.SetCorner(index, false);
        }
        
        _waterSprings = new();
        for (int i = 0; i <= WaveCount+1; i++) {
            int index = i + 1; 
            
            Smoothen(waterSpline, index);

            GameObject wavePoint = Instantiate(wavePointPrefab, wavePoints.transform, false);
            wavePoint.transform.localPosition = waterSpline.GetPosition(index);

            WaterSpring waterSpring = wavePoint.GetComponent<WaterSpring>();
            waterSpring.Init(_spriteShapeController);
            _waterSprings.Add(waterSpring);
        }
    }

    private void FixedUpdate()
    {
        foreach (WaterSpring waterSpring in _waterSprings)
        {
            waterSpring.WaveSpringUpdate(_springStiffness, _dampening);
            waterSpring.WavePointUpdate();
        }
        UpdateSprings();
    }
    
    private void Splash(int index, float speed)
    {
        if (index >= 0 && index < _waterSprings.Count)
        {
            _waterSprings[index].velocity = speed;
        }
    }
    
    private void UpdateSprings()
    {
        int count = _waterSprings.Count;
        float[] leftDeltas = new float[count];
        float[] rightDeltas = new float[count];
        for (int i = 0; i < count; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = spread * (_waterSprings[i].height - _waterSprings[i - 1].height);
                _waterSprings[i - 1].velocity += leftDeltas[i];
            }
            if (i < count - 1)
            {
                rightDeltas[i] = spread * (_waterSprings[i].height - _waterSprings[i + 1].height);
                _waterSprings[i + 1].velocity += rightDeltas[i];
            }
        }
    }
    
    private void Smoothen(Spline waterSpline, int index)
    {
        Vector3 position = waterSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;
        if (index > 1) {
            positionPrev = waterSpline.GetPosition(index-1);
        }
        if (index - 1 <= WaveCount) {
            positionNext = waterSpline.GetPosition(index+1);
        }

        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);
        
        waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
        waterSpline.SetLeftTangent(index, leftTangent);
        waterSpline.SetRightTangent(index, rightTangent);
    }
}
