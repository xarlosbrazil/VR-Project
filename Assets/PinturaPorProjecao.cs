using UnityEngine;
using UnityEngine.InputSystem;

public class PinturaPorProjecao : MonoBehaviour
{
    [Header("Modo de Jogo")]
    public bool modoVR = false; // Marque True no Inspector quando for testar no Oculus!

    [Header("Configurações do Alvo")]
    public LayerMask layerDaSuperficie;

    [Header("Configurações do Pincel")]
    [Range(2, 50)] public int raioPincel = 12;
    public Color corPintura = Color.black;

    [Header("Controles VR (Oculus/Quest)")]
    [Tooltip("Arraste aqui a ação de Gatilho do controle (ex: XR Controller / Optional Controls / Trigger)")]
    public InputActionReference gatilhoVRClick;

    private MeshRenderer pincelMeshRenderer;
    private Texture2D texturaDinamica;
    private int tamanhoTextura;

    void OnEnable()
    {
        if (modoVR && gatilhoVRClick != null) gatilhoVRClick.action.Enable();
    }

    void OnDisable()
    {
        if (modoVR && gatilhoVRClick != null) gatilhoVRClick.action.Disable();
    }

    void Start()
    {
        pincelMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Ray raio;
        bool estaClicando = false;

        if (modoVR)
        {
            // === LOGICA PARA VR (META QUEST 3) ===
            // O raio nasce na posição do controle e atira para a frente dele (Z positivo)
            raio = new Ray(transform.parent.position, transform.parent.forward);

            // Verifica se o gatilho do controle está pressionado
            if (gatilhoVRClick != null)
            {
                estaClicando = gatilhoVRClick.action.IsPressed();
            }
        }
        else
        {
            // === LOGICA PARA PC (MOUSE) ===
            if (Mouse.current == null) return;
            Vector2 posicaoMouse = Mouse.current.position.ReadValue();
            raio = Camera.main.ScreenPointToRay(posicaoMouse);
            estaClicando = Mouse.current.leftButton.isPressed;
        }

        RaycastHit hit;

        // Executa o disparo do raio
        if (Physics.Raycast(raio, out hit, 100f, layerDaSuperficie))
        {
            // Move a esferinha visual exatamente para onde o laser do VR está apontando no cilindro
            transform.position = hit.point;
            if (pincelMeshRenderer != null) pincelMeshRenderer.enabled = true;

            // Se estiver apertando o gatilho (VR) ou clique esquerdo (PC), pinta!
            if (estaClicando)
            {
                Renderer cilindroRenderer = hit.transform.GetComponent<Renderer>();

                if (cilindroRenderer != null)
                {
                    texturaDinamica = (Texture2D)cilindroRenderer.material.mainTexture;

                    if (texturaDinamica != null)
                    {
                        tamanhoTextura = texturaDinamica.width;
                        Vector2 coordenadaUV = hit.textureCoord;
                        int pixelX = (int)(coordenadaUV.x * tamanhoTextura);
                        int pixelY = (int)(coordenadaUV.y * tamanhoTextura);

                        PintarNaTextura(pixelX, pixelY);
                    }
                }
            }
        }
        else
        {
            // Se o controle apontar para o teto ou fora do cilindro, esconde a esferinha
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
        texturaDinamica.Apply();
    }
}