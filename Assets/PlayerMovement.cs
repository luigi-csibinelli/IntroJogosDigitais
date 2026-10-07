using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float speed;
    public float speedCorrida = 8f;
    public float staminaMax = 2f; // segundos que consegue correr
    public float stamina;
    public AudioClip somColeta;
    public AudioClip somDano;
    AudioSource audio;

    private bool cansado = false;
    private float tempoInvencivel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        stamina = staminaMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.gameOver)
        {
            sprite.enabled = true;
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // corrida: Shift/Espaco no teclado, X/Y no controle
        bool querCorrer = Input.GetButton("Fire3") || Input.GetButton("Jump");
        bool correndo = querCorrer && !cansado && movement != Vector2.zero;

        if (correndo)
        {
            stamina -= Time.fixedDeltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                cansado = true;
            }
        }
        else
        {
            stamina += Time.fixedDeltaTime * 0.5f;
            if (stamina > staminaMax) stamina = staminaMax;
            if (stamina > staminaMax * 0.4f) cansado = false;
        }

        float velocidade = correndo ? speedCorrida : speed;
        rb.MovePosition(rb.position + movement.normalized * velocidade * Time.fixedDeltaTime);

        if (moveHorizontal != 0)
        {
            sprite.flipX = moveHorizontal < 0;
        }

        // pisca enquanto esta invencivel depois de tomar dano
        if (tempoInvencivel > 0)
        {
            tempoInvencivel -= Time.fixedDeltaTime;
            sprite.enabled = tempoInvencivel <= 0 || Mathf.FloorToInt(tempoInvencivel * 10) % 2 == 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coletavel")
        {
            audio.PlayOneShot(somColeta);
            GameController.Collect();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Inimigo" && tempoInvencivel <= 0 && !GameController.gameOver)
        {
            audio.PlayOneShot(somDano);
            GameController.PerdeVida();
            tempoInvencivel = 1.5f;
        }
    }
}
