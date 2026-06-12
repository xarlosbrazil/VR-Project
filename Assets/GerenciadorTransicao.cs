using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorTransicao : MonoBehaviour
{
    public Image telaPreta;
    public float duracaoFade = 3.0f;

    void Start()
    {
        // Se você esqueceu de arrastar no Inspector, o script tenta achar o componente no próprio Canvas
        if (telaPreta == null)
        {
            telaPreta = GetComponentInChildren<Image>();
        }

        // Agora checamos se ele realmente achou antes de tentar mudar o Alpha
        if (telaPreta != null)
        {
            Color c = telaPreta.color;
            c.a = 1f;
            telaPreta.color = c;

            // Só começa a transição se a imagem existir
            StartCoroutine(AguardarEClarear());
        }
        else
        {
            Debug.LogError("ERRO: O script GerenciadorTransicao não encontrou nenhuma 'Image' dentro do Canvas! Crie uma imagem preta sob o Canvas.");
        }
    }

    IEnumerator AguardarEClarear()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Fade(1f, 0f));
    }

    public IEnumerator Fade(float valorInicial, float valorAlvo)
    {
        float tempoPassado = 0f;
        Color corAtual = telaPreta.color;

        // Garante que o loop rode estritamente durante o tempo estipulado
        while (tempoPassado < duracaoFade)
        {
            tempoPassado += Time.deltaTime;

            // Calcula a proporção do progresso (entre 0 e 1)
            float progresso = tempoPassado / duracaoFade;

            // Altera o Alpha gradualmente
            corAtual.a = Mathf.Lerp(valorInicial, valorAlvo, progresso);
            telaPreta.color = corAtual;

            // CRÍTICO: Espera o final do frame para renderizar a mudança na tela antes de continuar o loop
            yield return new WaitForEndOfFrame();
        }

        // Garante o valor exato no final da transição para evitar teto quebrado
        corAtual.a = valorAlvo;
        telaPreta.color = corAtual;
    }
}