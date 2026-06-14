using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Importante para controlar o componente Image
using TMPro;

public class BalaoDialogo : MonoBehaviour
{
    [Header("Componentes de UI")]
    public TextMeshProUGUI campoTexto; 
    public Image imagemFundoBalao; // MUDOU: Agora arrastamos o componente Image direto aqui

    [Header("Configurações do Texto")]
    [TextArea(2, 5)]
    public string fraseEfeito = "Seja bem-vindo à cabana de îanypaba...";
    public float velocidadeDigitacao = 0.05f;

    public void DispararDialogo()
    {
        if (imagemFundoBalao != null)
        {
            imagemFundoBalao.gameObject.SetActive(true);
            
            // Força a opacidade de ambos a voltar para 100% (Visível)
            Color corFundo = imagemFundoBalao.color;
            corFundo.a = 1f;
            imagemFundoBalao.color = corFundo;

            if (campoTexto != null)
            {
                Color corTexto = campoTexto.color;
                corTexto.a = 1f;
                campoTexto.color = corTexto;
            }

            StartCoroutine(EfeitoMaquinaEscrever());
        }
    }

    // NOVA LÓGICA DE FADE OUT: Altera o Alpha da imagem e do texto manualmente
    public IEnumerator FadeOutBalao(float duracao)
    {
        if (imagemFundoBalao == null || campoTexto == null)
        {
            if (imagemFundoBalao != null) imagemFundoBalao.gameObject.SetActive(false);
            yield break;
        }

        float tempo = 0;
        Color corFundoInicial = imagemFundoBalao.color;
        Color corTextoInicial = campoTexto.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / duracao;

            // Vai diminuindo o Alpha (a) de 1 para 0 gradualmente
            corFundoInicial.a = Mathf.Lerp(1f, 0f, progresso);
            corTextoInicial.a = Mathf.Lerp(1f, 0f, progresso);

            imagemFundoBalao.color = corFundoInicial;
            campoTexto.color = corTextoInicial;

            yield return null;
        }

        // Garante que zerou tudo e desativa o objeto no final do fade
        corFundoInicial.a = 0f;
        corTextoInicial.a = 0f;
        imagemFundoBalao.color = corFundoInicial;
        campoTexto.color = corTextoInicial;
        
        imagemFundoBalao.gameObject.SetActive(false); 
    }

    IEnumerator EfeitoMaquinaEscrever()
    {
        campoTexto.text = ""; 
        foreach (char letra in fraseEfeito.ToCharArray())
        {
            campoTexto.text += letra; 
            yield return new WaitForSeconds(velocidadeDigitacao); 
        }
    }
}