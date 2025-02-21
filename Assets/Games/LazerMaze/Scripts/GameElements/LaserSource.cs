using System.Collections.Generic;
using UnityEngine;

public class LaserSource : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private LineRenderer laser;
    private List<Vector3> laserIndices = new();

    [SerializeField]
    private LayerMask mirrorMask;
    [SerializeField]
    private LayerMask goalMask;
    [SerializeField]
    private LayerMask refractorMask;

    private List<GameObject> goalsTouched;

    Dictionary<string, float> refractiveMaterials = new Dictionary<string, float>()
    {
        {"Air", 1.0f},
        {"Glass", 1.5f}
    };

    private void Start()
    {
        goalsTouched = new List<GameObject>();

        laser.startWidth = .1f;
        laser.endWidth = .1f;
        laser.material = material;
        laser.startColor = Color.green;
    }

    private void Update()
    {
        laserIndices.Clear();
        goalsTouched.Clear();
        CastRay(transform.position, transform.forward, laser);
    }

    private void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            CheckHit(hit, dir, laser);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(100));
            UpdateLaser();
        }
    }

    private void UpdateLaser()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach (var i in laserIndices)
        {
            laser.SetPosition(count, i);
            count++;
        }
    }

    private void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser)
    {
        int hitLayer = (int) Mathf.Pow(2, hitInfo.transform.gameObject.layer);
        if (hitLayer == mirrorMask)
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

            CastRay(pos, dir, laser);
        }
        else if(hitLayer == refractorMask)
        {
            Vector3 pos = hitInfo.point;
            laserIndices.Add(pos);

            Vector3 newPos1 = new Vector3(
                Mathf.Abs(direction.x) / (direction.x + 0.0001f) * 0.001f + pos.x, 
                Mathf.Abs(direction.y) / (direction.y + 0.0001f) * 0.001f + pos.y, 
                Mathf.Abs(direction.z) / (direction.z + 0.0001f) * 0.001f + pos.z);

            float n1 = refractiveMaterials["Air"];
            float n2 = refractiveMaterials["Glass"];

            Vector3 norm = hitInfo.normal;
            Vector3 incident = direction;

            Vector3 refractedVector = Refract(n1, n2, norm, incident);

            CastRay(newPos1, refractedVector, laser);
        }
        else if(hitLayer == goalMask)
        {
            if (!Input.GetMouseButton(0))
            {
                if (!goalsTouched.Contains(hitInfo.collider.gameObject))
                {
                    goalsTouched.Add(hitInfo.collider.gameObject);
                    GameController.instance.CheckSuccess(goalsTouched.Count);
                }
            }
            Vector3 pos = hitInfo.point + direction.normalized * 0.2f;
            
            laserIndices.Add(pos);

            CastRay(pos, direction, laser);
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaser();
        }
    }


    Vector3 Refract(float n1, float n2, Vector3 norm, Vector3 incident)
    {
        incident.Normalize();

        Vector3 refractedVector = (n1 / n2 * Vector3.Cross(norm, Vector3.Cross(-norm, incident)) - norm * Mathf.Sqrt(1- Vector3.Dot(Vector3.Cross(norm, incident) * (n1/n2 * n1/n2), Vector3.Cross(norm, incident)))).normalized;
        return refractedVector;
    }
}
