using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private MeshRenderer rend;
    private Color originalColor;
    public Color hitColor = Color.red; // Couleur quand touché par un rayon

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        originalColor = rend.material.color; // Stocke la couleur d'origine
    }

    void Update()
    {
        // Vérifier si un rayon touche la sphère
        if (IsHitByRaycast())
        {
            rend.material.color = hitColor; // Change de couleur
        }
        else
        {
            rend.material.color = originalColor; // Revient à la couleur originale
        }
    }

    private bool IsHitByRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity))
        {
            return hit.collider.CompareTag("Laser"); // Vérifie si c'est un laser
        }
        return false;
    }
}
