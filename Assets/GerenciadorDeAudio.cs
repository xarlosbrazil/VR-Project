using System.Collections;
using UnityEngine;

public class GerenciadorAudio : MonoBehaviour
{
    public static GerenciadorAudio Instancia { get; private set; }

    [Header("Canais de Áudio")]
    [Tooltip("AudioSource configurado em 2D para música de fundo")]
    public AudioSource canalMusicaAmbiente;
    [Tooltip("AudioSource configurado em 3D para efeitos espaciais (passos, cadeiras, etc)")]
    public AudioSource canalEfeitos3D;

    [Header("Playlist de Interjeições (Tupi-Guarani)")]
    public AudioClip[] saudacoesTupi;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            Debug.Log("SISTEMA DE ÁUDIO: Gerenciador inicializado com sucesso e pronto para tocar!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Toca a música ambiente de fundo
    public void TocarMusicaAmbiente(AudioClip musica, float volume = 0.5f)
    {
        if (canalMusicaAmbiente == null || musica == null) return;
        canalMusicaAmbiente.clip = musica;
        canalMusicaAmbiente.loop = true;
        canalMusicaAmbiente.volume = volume;
        canalMusicaAmbiente.Play();
    }

    public void TocarSom3D(AudioClip som, Vector3 posicaoMundo, float volume = 1f)
    {
        if (som == null)
        {
            Debug.LogError("SISTEMA DE ÁUDIO: Você tentou tocar um som, mas o arquivo de AudioClip está VAZIO (Null)!");
            return;
        }

        if (canalEfeitos3D == null)
        {
            Debug.LogError("SISTEMA DE ÁUDIO: O campo 'canalEfeitos3D' está VAZIO no Inspector! Você esqueceu de arrastar o AudioSource filho.");
            return;
        }

        Debug.Log($"SISTEMA DE ÁUDIO: Tocando o som '{som.name}' agora via código!");
        canalEfeitos3D.transform.position = posicaoMundo;
        canalEfeitos3D.spatialBlend = 1.0f;
        canalEfeitos3D.volume = volume;
        canalEfeitos3D.PlayOneShot(som);
    }

    // 3. Dispara uma interjeição aleatória tupi-guarani
    public void TocarSaudacaoAleatoria(Vector3 posicaoPersonagem)
    {
        if (saudacoesTupi == null || saudacoesTupi.Length == 0) return;

        int indiceAleatorio = Random.Range(0, saudacoesTupi.Length);
        TocarSom3D(saudacoesTupi[indiceAleatorio], posicaoPersonagem, 0.9f);
    }
}