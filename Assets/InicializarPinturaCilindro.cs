using UnityEngine;
using UnityEngine.InputSystem;

public class InicializarTexturaCilindro : MonoBehaviour
{
    public int tamanhoTextura = 512;
    public Texture2D fotoFundoGabarito; // Se quiser usar uma foto de fundo

    private Texture2D texturaDinamica;

    void Start()
    {
        Renderer r = GetComponent<Renderer>();

        if (fotoFundoGabarito != null)
        {
            tamanhoTextura = fotoFundoGabarito.width;
            texturaDinamica = new Texture2D(tamanhoTextura, fotoFundoGabarito.height, TextureFormat.RGBA32, false);
            texturaDinamica.SetPixels(fotoFundoGabarito.GetPixels());
        }
        else
        {
            texturaDinamica = new Texture2D(tamanhoTextura, tamanhoTextura, TextureFormat.RGBA32, false);
            Color[] pixelsFundo = new Color[tamanhoTextura * tamanhoTextura];
            for (int i = 0; i < pixelsFundo.Length; i++) pixelsFundo[i] = Color.white;
            texturaDinamica.SetPixels(pixelsFundo);
        }

        texturaDinamica.Apply();
        r.material.mainTexture = texturaDinamica;
    }

    void Update()
    {
        // O botão de reset (R) pode ficar aqui de forma global para o cilindro
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Resetar();
        }
    }

    public void Resetar()
    {
        if (fotoFundoGabarito != null)
        {
            texturaDinamica.SetPixels(fotoFundoGabarito.GetPixels());
        }
        else
        {
            Color[] pixelsFundo = new Color[tamanhoTextura * texturaDinamica.height];
            for (int i = 0; i < pixelsFundo.Length; i++) pixelsFundo[i] = Color.white;
            texturaDinamica.SetPixels(pixelsFundo);
        }
        texturaDinamica.Apply();
    }
}