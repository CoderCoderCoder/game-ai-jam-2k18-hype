using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour {

    public DSPixelPerfectCamera cam;
    private Vector2 camThreshold;

    public float scrollFactor = 0.0f;
    public Vector2 spriteSize;

    private Vector3 lastCameraPosition;

	void Start ()
    {
        lastCameraPosition = cam.transform.position;
        camThreshold = new Vector2(0.5f * cam.GetComponent<Camera>().aspect * cam.VertUnitsOnScreen + 1, 0.5f * cam.VertUnitsOnScreen + 1);
    }
	
	void Update ()
    {
        Vector3 scrollAmount = scrollFactor * (cam.transform.position - lastCameraPosition);
        transform.position += scrollAmount;
        lastCameraPosition = cam.transform.position;

        Vector2 relativePosition = cam.transform.position - transform.position;
        Vector3 offset = Vector3.zero;

        if (relativePosition.x <= -(0.5f * spriteSize.x - camThreshold.x))
            offset.x = -spriteSize.x;
        else if (relativePosition.x >= (0.5f * spriteSize.x + camThreshold.x))
            offset.x = spriteSize.x;
        if (relativePosition.y <= -(0.5f * spriteSize.y - camThreshold.y))
            offset.y = -spriteSize.y;
        else if (relativePosition.y >= (0.5f * spriteSize.y + camThreshold.y))
            offset.y = spriteSize.y;

        transform.position += offset;
    }
}
