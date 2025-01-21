using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMovement : MonoBehaviour
{
    [Header("Map Movement")]
    [SerializeField] private float mapSpeed;
    private Vector3 startPosition;

    [Header("Map Light")]
    [SerializeField] private Tilemap[] tilemapLights;
    [SerializeField] private SpriteRenderer mapOpacity;
    [SerializeField] private float fadeTime;
    public int fadingMapLight = 2;
    public bool mapLightKickOff = false;
    public bool mapLightsFinished = false;
    public bool mapLightMovingForward = true;
    private Tilemap currentTileMap;
    private Tilemap previousTileMap;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (PlayerController.gameOn)
        {
            MapMovementUpdate();

            if (!mapLightKickOff)
            {
                mapLightKickOff = true;
                StartCoroutine(MapLightFadeIn(fadingMapLight));
            }
        }

    }

    private void MapMovementUpdate()
    {
        transform.Translate(translation: mapSpeed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -74.49f)
        {
            transform.position = startPosition;

            if (fadingMapLight == tilemapLights.Length && mapLightsFinished)
            {
                mapLightMovingForward = false;
            }

            SetMapLightProgress();
        }
    }

    private void SetMapLightProgress()
    {
        if (mapLightMovingForward && fadingMapLight <= tilemapLights.Length)
        {
            StartCoroutine(MapLightFadeIn(fadingMapLight));
            
        }
        else if (!mapLightMovingForward && fadingMapLight >= 2)
        {
            StartCoroutine(MapLightFadeOut(fadingMapLight));
        }
    }

    private IEnumerator MapLightFadeIn(int fadingMapLightNumber)
    {
        int fadingMapLightIndex = fadingMapLightNumber - 1;
        currentTileMap = tilemapLights[fadingMapLightIndex];
        previousTileMap = tilemapLights[fadingMapLightIndex - 1];
        currentTileMap.gameObject.SetActive(true);
        currentTileMap.color = new Color(1, 1, 1, 0);
        float startTime = Time.time;
        float elapsedTime = 0;
        
        StartCoroutine(MapOpacityChangeForward(fadingMapLight));

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            currentTileMap.color = new Color(1, 1, 1, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        currentTileMap.color = new Color(1, 1, 1, 1);

        if (fadingMapLightNumber < 5)
        {
            previousTileMap.gameObject.SetActive(false);
        }

        if (fadingMapLight == 2 && mapLightsFinished)
        {
            mapLightsFinished = false;
        }

        if (fadingMapLight == tilemapLights.Length && !mapLightsFinished)
        {
            mapLightsFinished = true;
        }

        if (fadingMapLight < tilemapLights.Length && mapLightMovingForward)
        {
            fadingMapLight++;
        }
    }

    private IEnumerator MapLightFadeOut(int fadingMapLightNumber)
    {
        int fadingMapLightIndex = fadingMapLightNumber - 1;
        currentTileMap = tilemapLights[fadingMapLightIndex];
        previousTileMap = tilemapLights[fadingMapLightIndex - 1];
        previousTileMap.gameObject.SetActive(true);
        previousTileMap.color = new Color(1, 1, 1, 1);
        currentTileMap.color = new Color(1, 1, 1, 1);
        float startTime = Time.time;
        float elapsedTime = 0;

        StartCoroutine(MapOpacityChangeBackward(fadingMapLight));

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            currentTileMap.color = new Color(1, 1, 1, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        currentTileMap.color = new Color(1, 1, 1, 0);

        currentTileMap.gameObject.SetActive(false);

        if (fadingMapLight == 2 && !mapLightMovingForward)
        {
            mapLightMovingForward = true;
        }

        if (fadingMapLight > 2 && !mapLightMovingForward)
        {
            fadingMapLight--;
        }
    }

    private IEnumerator MapOpacityChangeForward(int fadingMapLightNumber)
    {
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            if (fadingMapLightNumber == 2)
            {
                float alpha = Mathf.Lerp(0.4f, 0.3f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 3)
            {
                float alpha = Mathf.Lerp(0.3f, 0.2f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 4)
            {
                float alpha = Mathf.Lerp(0.2f, 0.1f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 5)
            {
                float alpha = Mathf.Lerp(0.1f, 0, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        if (fadingMapLightNumber == 2)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.3f);
        }
        else if (fadingMapLightNumber == 3)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.2f);
        }
        else if (fadingMapLightNumber == 4)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.1f);
        }
        else if (fadingMapLightNumber == 5)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0);
        }
    }

    private IEnumerator MapOpacityChangeBackward(int fadingMapLightNumber)
    {
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            if (fadingMapLightNumber == 2)
            {
                float alpha = Mathf.Lerp(0.3f, 0.4f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 3)
            {
                float alpha = Mathf.Lerp(0.2f, 0.3f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 4)
            {
                float alpha = Mathf.Lerp(0.1f, 0.2f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }
            else if (fadingMapLightNumber == 5)
            {
                float alpha = Mathf.Lerp(0, 0.1f, elapsedTime / fadeTime);
                mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, alpha);
            }

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        if (fadingMapLightNumber == 2)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.4f);
        }
        else if (fadingMapLightNumber == 3)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.3f);
        }
        else if (fadingMapLightNumber == 4)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.2f);
        }
        else if (fadingMapLightNumber == 5)
        {
            mapOpacity.color = new Color(0.75f, 0.75f, 0.75f, 0.1f);
        }
    }
}
