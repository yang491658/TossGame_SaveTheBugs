using UnityEngine;

[ExecuteAlways]
public class AutoBackground : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sr;

    private int lastW, lastH;
    private float lastAspect, lastOrthoSize;

    [Header("Scroll")]
    [SerializeField] private float speed = 0.5f;
    private Transform img, clone;
    private float scroll;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (enabled) Fit();

        if (img == null) img = GameObject.Find("Image")?.transform;
    }
#endif

    private void Awake()
    {
        cam = Camera.main;
        sr = img.GetComponent<SpriteRenderer>();

        Fit();
    }

    private void Update()
    {
        if (cam == null) cam = Camera.main;

        if (Screen.width != lastW || Screen.height != lastH ||
            !Mathf.Approximately(cam.aspect, lastAspect) ||
            !Mathf.Approximately(cam.orthographicSize, lastOrthoSize))
            Fit();

        if (Application.isPlaying)
            Scroll();
    }

    private void OnEnable()
    {
        Fit();
    }

    private void Fit()
    {
        if (cam == null || !cam.orthographic || sr.sprite == null) return;

        lastW = Screen.width;
        lastH = Screen.height;
        lastAspect = cam.aspect;
        lastOrthoSize = cam.orthographicSize;

        var sp = sr.sprite;
        float ppu = sp.pixelsPerUnit;
        if (ppu <= 0f) return;

        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        float spriteW = sp.rect.width / ppu;
        float spriteH = sp.rect.height / ppu;
        if (spriteW <= 0f || spriteH <= 0f) return;

        var parent = transform.parent;
        Vector3 parentLossy = (parent != null) ? parent.lossyScale : Vector3.one;
        float invX = (parentLossy.x == 0f) ? 1f : parentLossy.x;
        float invY = (parentLossy.y == 0f) ? 1f : parentLossy.y;

        float localX = (worldW / spriteW) / invX;
        float localY = (worldH / spriteH) / invY;
        transform.localScale = new Vector3(localX, localY, transform.localScale.z);

        var b = sr.bounds;
        Vector3 camCenter = cam.transform.position;
        Vector3 delta = new Vector3(camCenter.x - b.center.x, camCenter.y - b.center.y, 0f);
        transform.position += delta;
    }

    private void Scroll()
    {
        speed = Mathf.Min(3f, speed + Time.deltaTime / 200f);

        if (clone == null)
        {
            clone = Instantiate(img.gameObject, transform).transform;
            scroll = sr.bounds.size.y;
            clone.position = img.position + Vector3.up * scroll;
        }

        img.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
        clone.Translate(Vector2.down * speed * Time.deltaTime, Space.World);

        float lower = cam.transform.position.y - scroll;
        if (img.position.y < lower)
            img.position = clone.position + Vector3.up * scroll;
        if (clone.position.y < lower)
            clone.position = img.position + Vector3.up * scroll;
    }
}
