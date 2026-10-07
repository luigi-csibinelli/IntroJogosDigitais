using UnityEngine;

public class Musica : MonoBehaviour
{
    private static Musica instancia;

    void Awake()
    {
        // a musica continua tocando entre as cenas, entao so pode existir uma
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
    }
}
