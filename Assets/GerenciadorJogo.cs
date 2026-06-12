using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GerenciadorJogo : MonoBehaviour
{
    [Header("Componentes do Fluxo")]
    public GerenciadorTransicao scriptTransicao;
    public InicializarTexturaCilindro cilindroComponente;
    public GameObject personagemCadeira;

    [Header("Configurações de Áudio (Clips)")]
    public AudioClip somPassosSaindo;
    public AudioClip somPassosChegando;

    private bool emTransicao = false;

    void Start()
    {
        // Garante que o jogo comece limpando a tela para o jogador ver o cenário de largada
        if (scriptTransicao != null)
        {
            // Força a tela a começar 100% preta
            if (scriptTransicao.telaPreta != null)
            {
                Color c = scriptTransicao.telaPreta.color;
                c.a = 1f;
                scriptTransicao.telaPreta.color = c;
            }

            // Faz o Fade-In inicial automático (de 1 para 0)
            StartCoroutine(scriptTransicao.Fade(1f, 0f));
        }

        // Toca a primeira interjeição/saudação do dia assim que o jogo inicia
        if (GerenciadorAudio.Instancia != null && personagemCadeira != null)
        {
            GerenciadorAudio.Instancia.TocarSaudacaoAleatoria(personagemCadeira.transform.position);
        }
    }

    void Update()
    {
        // TESTE DE VIDA: Isso TEM que aparecer no console a cada frame!
        // Se não aparecer, o script não está ativo na cena.
        // Debug.Log("O Gerenciador de Jogo está vivo e rodando!");

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("BOTÃO DETECTADO: Você apertou o Espaço!");

            if (!emTransicao)
            {
                StartCoroutine(FluxoProximoPersonagem());
            }
            else
            {
                Debug.Log("Aviso: O jogo já está no meio de uma transição!");
            }
        }
    }

    IEnumerator FluxoProximoPersonagem()
    {
        emTransicao = true;
        Debug.Log("1. Começou o Fade Out");

        if (scriptTransicao != null)
        {
            yield return StartCoroutine(scriptTransicao.Fade(0f, 1f));
        }

        MeshRenderer personagemRenderer = null;
        if (personagemCadeira != null)
        {
            personagemRenderer = personagemCadeira.GetComponent<MeshRenderer>();
            if (personagemRenderer != null) personagemRenderer.enabled = false;
        }

        if (cilindroComponente != null) cilindroComponente.Resetar();

        Debug.Log("2. Tocando passos de saída... Esperando 2.5s");
        if (somPassosSaindo != null && GerenciadorAudio.Instancia != null)
        {
            Vector3 direcaoSaida = transform.position + new Vector3(-10f, 0f, 2f);
            GerenciadorAudio.Instancia.TocarSom3D(somPassosSaindo, direcaoSaida);
        }
        yield return new WaitForSeconds(2.5f);

        Debug.Log("3. Silêncio na cabana... Esperando 1.5s");
        yield return new WaitForSeconds(1.5f);

        Debug.Log("4. Tocando passos de chegada... Esperando 2.5s");
        if (somPassosChegando != null && GerenciadorAudio.Instancia != null)
        {
            Vector3 direcaoChegada = transform.position + new Vector3(3f, 0f, 10f);
            GerenciadorAudio.Instancia.TocarSom3D(somPassosChegando, direcaoChegada);
        }
        yield return new WaitForSeconds(2.5f);

        if (personagemRenderer != null) personagemRenderer.enabled = true;

        Debug.Log("5. Começou o Fade In");
        if (scriptTransicao != null)
        {
            yield return StartCoroutine(scriptTransicao.Fade(1f, 0f));
        }

        if (GerenciadorAudio.Instancia != null && personagemCadeira != null)
        {
            GerenciadorAudio.Instancia.TocarSaudacaoAleatoria(personagemCadeira.transform.position);
        }

        emTransicao = false;
        Debug.Log("6. Fluxo concluído!");
    }
}