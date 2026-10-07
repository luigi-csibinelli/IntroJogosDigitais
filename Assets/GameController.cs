using UnityEngine;

public static class GameController
{
    public const float tempoInicial = 30f;
    public const float bonusTempo = 3f;
    public const int vidasIniciais = 3;

    private static int collectableCount = 4;
    private static int totalColetaveis = 4;

    public static int score;
    public static int vidas;
    public static float tempoRestante;
    public static float tempoJogado;

    public static bool venceu
    {
        get { return collectableCount <= 0; }
    }

    public static bool gameOver
    {
        get { return venceu || vidas <= 0 || tempoRestante <= 0; }
    }

    public static int coletados
    {
        get { return totalColetaveis - collectableCount; }
    }

    public static int total
    {
        get { return totalColetaveis; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Init()
    {
        collectableCount = 4;
        totalColetaveis = 4;
        score = 0;
        vidas = vidasIniciais;
        tempoRestante = tempoInicial;
        tempoJogado = 0;
    }

    // chamado no inicio da fase com a quantidade de cristais que existem na cena
    public static void SetColetaveis(int quantidade)
    {
        collectableCount = quantidade;
        totalColetaveis = quantidade;
    }

    public static void Collect()
    {
        collectableCount--;
        score += 100;
        tempoRestante += bonusTempo;

        if (venceu)
        {
            // bonus pelo oxigenio e pelas vidas que sobraram
            score += Mathf.FloorToInt(tempoRestante) * 10 + vidas * 50;
        }
    }

    public static void PerdeVida()
    {
        vidas--;
    }

    public static void PassaTempo(float delta)
    {
        if (gameOver) return;

        tempoJogado += delta;
        tempoRestante -= delta;
        if (tempoRestante < 0) tempoRestante = 0;
    }

    public static string FormataTempo(float tempo)
    {
        int minutos = Mathf.FloorToInt(tempo / 60);
        int segundos = Mathf.FloorToInt(tempo % 60);
        return minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}
