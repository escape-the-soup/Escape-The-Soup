using UnityEngine;
using UnityEngine.UI;

public class MenuShrimpBounceUI : MonoBehaviour
{
    public float speed = 200f;
    Image shrimpImage;
    RectTransform rect;
    Vector2 dir;

    /*
    public float speed = 5f;
    Vector2 direction;
    */

    void Start()
    {
        rect = GetComponent<RectTransform>();

        // Place shrimp randomly within canvas bounds
        RectTransform canvas = rect.parent.GetComponent<RectTransform>();
        shrimpImage = GetComponent<Image>();
        float halfW = canvas.rect.width / 2f - rect.rect.width / 2f;
        float halfH = canvas.rect.height / 2f - rect.rect.height / 2f;
        float margin = 50f; // keeps shrimp away from edges
        rect.anchoredPosition = new Vector2(
            Random.Range(-halfW + margin, halfW - margin),
            Random.Range(-halfH + margin, halfH - margin)
        );

        dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        /*
        // Pick a random initial direction
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        */
    }

    void Update()
    {
        rect.anchoredPosition += dir * speed * Time.deltaTime;

        Vector2 size = rect.rect.size;
        Vector2 canvasSize = rect.parent.GetComponent<RectTransform>().rect.size;
        Vector2 pos = rect.anchoredPosition;

        // After horizontal bounce
        if (pos.x < -canvasSize.x / 2 + size.x / 2 || pos.x > canvasSize.x / 2 - size.x / 2)
        {
            dir.x = -dir.x;
            rect.localScale = new Vector3(-Mathf.Sign(dir.x) * Mathf.Abs(rect.localScale.x), rect.localScale.y, rect.localScale.z);
        }
        if (pos.y < -canvasSize.y / 2 + size.y / 2 || pos.y > canvasSize.y / 2 - size.y / 2)
            dir.y = -dir.y;

        rect.anchoredPosition = pos;

        /*
        transform.Translate(direction * speed * Time.deltaTime);

        // Screen boundaries (using camera)
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPos.x <= 0f || screenPos.x >= 1f)
        {
            direction.x = -direction.x; // bounce horizontally
        }
        if (screenPos.y <= 0f || screenPos.y >= 1f)
        {
            direction.y = -direction.y; // bounce vertically
        }
        */
    }
}