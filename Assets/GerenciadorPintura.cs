using UnityEngine;
using UnityEngine.InputSystem;

public class GerenciadorPinturaContato : MonoBehaviour
{
    [Header("Configurações da Textura")]
    public int tamanhoTextura = 512;
    public Texture2D texturaBaseFundo;
    public Color corPintura = Color.black;

    [Header("Configurações do Pincel")]
    [Range(2, 50)]
    public int raioPincel = 8;
    public int passoMudancaGrossura = 2;

    private Texture2D texturaDinamica;
    private Renderer objetoRenderer;

    void Start()
    {
        objetoRenderer = GetComponent<Renderer>();

        if (texturaBaseFundo != null)
        {
            tamanhoTextura = texturaBaseFundo.width;
        }

        texturaDinamica = new Texture2D(tamanhoTextura, tamanhoTextura, TextureFormat.RGBA32, false);
        ResetarPintura();
        objetoRenderer.material.mainTexture = texturaDinamica;
    }

    void Update()
    {
        // Atalhos de teclado continuam funcionando de forma global
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) ResetarPintura();
        ControlarGrossuraPincel();
    }

    // ESTA É A MÁGICA DO CONTATO: Executa continuamente enquanto o Pincel estiver encostando no Cilindro
    private void OnTriggerStay(Collider outroColisor)
    {
        // Verifica se quem encostou foi o objeto com o nome "Pincel"
        if (outroColisor.name == "Pincel")
        {
            // Para descobrir onde o contato aconteceu na malha sem usar clique do mouse, 
            // disparamos um raio minúsculo da posição do pincel em direção ao centro do cilindro.
            Vector3 direcaoAoCentro = (transform.position - outroColisor.transform.position).normalized;

            RaycastHit hit;
            // O raio parte de um pouco atrás do pincel e viaja na direção do contato
            if (Physics.Raycast(outroColisor.transform.position - direcaoAoCentro * 0.1f, direcaoAoCentro, out hit, 0.5f))
            {
                // Se o raio interno encontrar a malha, pega a coordenada UV e pinta
                if (hit.transform == transform)
                {
                    Vector2 coordenadaUV = hit.textureCoord;
                    int pixelX = (int)(coordenadaUV.x * tamanhoTextura);
                    int pixelY = (int)(coordenadaUV.y * tamanhoTextura);

                    PintarNaTexturaRedondo(pixelX, pixelY);
                }
            }
        }
    }

    void PintarNaTexturaRedondo(int centroX, int centroY)
    {
        int startX = Mathf.Clamp(centroX - raioPincel, 0, tamanhoTextura);
        int startY = Mathf.Clamp(centroY - raioPincel, 0, tamanhoTextura);
        int endX = Mathf.Clamp(centroX + raioPincel, 0, tamanhoTextura);
        int endY = Mathf.Clamp(centroY + raioPincel, 0, tamanhoTextura);

        float raioAoQuadrado = raioPincel * raioPincel;

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                float diferencaX = x - centroX;
                float diferencaY = y - centroY;
                float distanciaAoQuadrado = (diferencaX * diferencaX) + (diferencaY * diferencaY);

                if (distanciaAoQuadrado <= raioAoQuadrado)
                {
                    texturaDinamica.SetPixel(x, y, corPintura);
                }
            }
        }
        texturaDinamica.Apply();
    }

    void ControlarGrossuraPincel()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.equalsKey.wasPressedThisFrame || Keyboard.current.numpadPlusKey.wasPressedThisFrame)
            raioPincel = Mathf.Clamp(raioPincel + passoMudancaGrossura, 2, 50);
        if (Keyboard.current.minusKey.wasPressedThisFrame || Keyboard.current.numpadMinusKey.wasPressedThisFrame)
            raioPincel = Mathf.Clamp(raioPincel - passoMudancaGrossura, 2, 50);
    }

    public void ResetarPintura()
    {
        if (texturaBaseFundo != null)
        {
            texturaDinamica.SetPixels(texturaBaseFundo.GetPixels());
        }
        else
        {
            Color[] pixelsFundo = new Color[tamanhoTextura * tamanhoTextura];
            for (int i = 0; i < pixelsFundo.Length; i++) pixelsFundo[i] = Color.white;
            texturaDinamica.SetPixels(pixelsFundo);
        }
        texturaDinamica.Apply();
    }
}