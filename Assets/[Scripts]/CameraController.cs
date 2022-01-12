using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    float zoomSpeed;
    float zoom;

    public float minZoom;
    public float maxZoom;
    public float moveSpeedMinZoom;
    public float moveSpeedMaxZoom;

    Camera cameraRef;
    // Start is called before the first frame update
    void Start()
    {
        cameraRef = GetComponent<Camera>();
        zoom = cameraRef.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
            AdjustZoom(zoomDelta);

        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || yDelta != 0f)
            AdjustPosition(xDelta, yDelta);
    }

    void AdjustZoom(float delta)
    {
        if(zoom >= minZoom && zoom <= maxZoom)
        {
            zoom += -delta;
            if (zoom > maxZoom)
                zoom = maxZoom;
            if (zoom < minZoom)
                zoom = minZoom;

            cameraRef.orthographicSize = zoom;
        }
    }

    //void AdjustPosition(float xDelta, float yDelta)
    //{
    //    Vector3 newPosition = new Vector3(transform.position.x + (xDelta * moveSpeed), transform.position.y + (yDelta * moveSpeed), transform.position.z);
    //
    //    transform.position = newPosition;
    //}

    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector3 direction = new Vector3(xDelta, yDelta, 0.0f).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
        float zoomy = zoom / maxZoom;

        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoomy) * damping * Time.deltaTime;
        Debug.Log(distance);

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }
    Vector3 ClampPosition(Vector3 position)
    {
        float xMax = 4;
        position.x = Mathf.Clamp(position.x, -xMax, xMax);

        float yMax = 4;
        position.y = Mathf.Clamp(position.y, -yMax, yMax);
    
        return position;
    }
}
