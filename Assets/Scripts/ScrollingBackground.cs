using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 1f;
    
    [SerializeField]
    private Renderer _renderer;

    public GameManager GameManager;

    void Update()
    {
        _renderer.material.mainTextureOffset += new Vector2(scrollSpeed * GameManager.speedFactor / 100 * Time.deltaTime, 0f);
    }
}
