using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    Vector3 pos;
    Vector3 dir;

    GameObject laserObj;
    LineRenderer laser;
    List<Vector3> laserIndices = new();

    private LayerMask mirrorMask;
    private LayerMask goalMask;
    private LayerMask glassMask;

    Dictionary<string, float> refractiveMaterials = new Dictionary<string, float>()
    {
        {"Air", 1.0f},
        {"Glass", 1.5f}
    };

    public LaserBeam(Vector3 pos, Vector3 dir, Material material, LayerMask mirror, LayerMask goal, LayerMask glass)
    {
        mirrorMask = mirror;
        goalMask = goal;
        glassMask = glass;

        laser = new LineRenderer();
        laserObj = new GameObject();
        laserObj.name = "Laser Beam";
        this.pos = pos;
        this.dir = dir;

        laser = laserObj.AddComponent<LineRenderer>();
        laser.startWidth = .1f;
        laser.endWidth = .1f;
        laser.material = material;
        laser.startColor = Color.green;

        CastRay(pos, dir, laser);
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
            laserIndices.Add(ray.GetPoint(30));
            UpdateLaser();
        }
        UpdateLaser();
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
        int hitLayer = (int)Mathf.Pow(2, hitInfo.transform.gameObject.layer);

        if (hitLayer == mirrorMask)
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

            CastRay(pos, dir, laser);
        }
        else if (hitLayer == glassMask)
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
        else if (hitLayer == goalMask)
        {
            //if (!Input.GetMouseButton(0))
            //{
            //    Debug.Log("End Game");
            //}
            Debug.Log(laserIndices.Count);
            laserIndices.Add(hitInfo.point);
            Debug.Log(laserIndices.Count);
            UpdateLaser();
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

        Vector3 refractedVector = (n1 / n2 * Vector3.Cross(norm, Vector3.Cross(-norm, incident)) - norm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(norm, incident) * (n1 / n2 * n1 / n2), Vector3.Cross(norm, incident)))).normalized;
        return refractedVector;
    }

}
