using UnityEngine;

public class SwitchColorBulette : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Tableau des couleurs possibles
    private static readonly Color[] colors = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    // Compteur statique partagé entre toutes les balles
    private static int colorIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // On applique une couleur différente à chaque nouvelle bulette
            spriteRenderer.color = colors[colorIndex];

            // On avance l’index pour la prochaine bulette
            colorIndex = (colorIndex + 1) % colors.Length;
        }
        else
        {
            Debug.LogError("Pas de SpriteRenderer trouvé sur " + gameObject.name);
        }
    }
}

