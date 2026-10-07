using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float speed = 3f;
    public Vector2 pontoB;          // patrulha entre a posicao inicial e esse ponto
    public bool persegue = false;   // se true, vai atras do player quando ele chega perto
    public float raioVisao = 3.5f;
    public Color corPerseguindo = Color.red;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Transform player;
    private Vector2 pontoA;
    private bool indoParaB = true;
    private Color corNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        pontoA = transform.position;
        corNormal = sprite.color;

        PlayerMovement p = FindFirstObjectByType<PlayerMovement>();
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (GameController.gameOver) return;

        Vector2 alvo = indoParaB ? pontoB : pontoA;

        bool perseguindo = persegue && player != null &&
                           Vector2.Distance(rb.position, player.position) < raioVisao;

        if (perseguindo)
        {
            alvo = player.position;
        }
        else if (Vector2.Distance(rb.position, alvo) < 0.1f)
        {
            indoParaB = !indoParaB;
        }

        sprite.color = perseguindo ? corPerseguindo : corNormal;
        sprite.flipX = alvo.x < rb.position.x;

        rb.MovePosition(Vector2.MoveTowards(rb.position, alvo, speed * Time.fixedDeltaTime));
    }
}
