using UnityEngine;
using UnityEngine.InputSystem;

public class PinturaPorProjecao : MonoBehaviour
{
    [Header("Configurações do Alvo")]
    public LayerMask layerDaSuperficie;

    [Header("Configurações do Pincel")]
    [Range(2, 50)] public int raioPincel = 12;
    public Color corPintura = Color.black;

    private MeshRenderer pincelMeshRenderer;
    private Texture2D texturaDinamica;
    private int tamanhoTextura;

    void Start()
    {
        pincelMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // 1. O Pincel segue o mouse visualmente na tela
        Vector2 posicaoMouse = Mouse.current.position.ReadValue();
        Ray raio = Camera.main.ScreenPointToRay(posicaoMouse);
        RaycastHit hit;

        // 2. Projeta o raio continuamente
        if (Physics.Raycast(raio, out hit, 100f, layerDaSuperficie))
        {
            // Posiciona a esfera visual exatamente no ponto da casca do cilindro
            transform.position = hit.point;
            if (pincelMeshRenderer != null) pincelMeshRenderer.enabled = true;

            // 3. MÁGICA DA PINTURA: Se o jogador segurar o clique esquerdo do mouse, pinta!
            if (Mouse.current.leftButton.isPressed)
            {
                // Pega o componente Renderer do objeto que o raio ACERTOU (o Cilindro)
                Renderer cilindroRenderer = hit.transform.GetComponent<Renderer>();

                if (cilindroRenderer != null)
                {
                    // Pega a textura que está rodando no material do cilindro
                    texturaDinamica = (Texture2D)cilindroRenderer.material.mainTexture;

                    if (texturaDinamica != null)
                    {
                        tamanhoTextura = texturaDinamica.width;

                        // Converte a coordenada UV do ponto de impacto para pixels
                        Vector2 coordenadaUV = hit.textureCoord;
                        int pixelX = (int)(coordenadaUV.x * tamanhoTextura);
                        int pixelY = (int)(coordenadaUV.y * tamanhoTextura);

                        // Executa a pintura circular
                        PintarNaTextura(pixelX, pixelY);
                    }
                }
            }
        }
        else
        {
            if (pincelMeshRenderer != null) pincelMeshRenderer.enabled = false;
        }
    }

    void PintarNaTextura(int centroX, int centroY)
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
        texturaDinamica.Apply(); // Força a atualização na GPU
    }
}