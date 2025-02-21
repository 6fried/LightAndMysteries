using UnityEngine;

public class Box : MonoBehaviour
{
    private Transform snapPoint;

    private void OnMouseDrag()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cursorPosition.x, transform.position.y, cursorPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Decal"))
        {
            snapPoint = other.transform;
        }
    }

    private void OnMouseUp()
    {
        transform.position = new Vector3(snapPoint.position.x, transform.position.y, snapPoint.position.z);
    }
}
