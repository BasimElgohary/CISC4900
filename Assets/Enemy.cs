using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
   [SerializeField] private float redColorDuration = 1;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Debug.Log(Time.deltaTime); // Logs the time in seconds it took to complete the last frame (higher fps = lower deltaTime | lower fps = higher deltaTime)
    }
    public void takeDamage() {
        Debug.Log(gameObject.name + " Enemy took damage");
        sr.color = Color.red;
        Invoke(nameof(TurnWhite), redColorDuration);
    }

    private void TurnWhite() {
        sr.color = Color.white;
    }

    
}
