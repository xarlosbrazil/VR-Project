using UnityEngine;
using UnityEngine.InputSystem; // Importante para o Novo Input System

public class ControleCabecaTeclado : MonoBehaviour
{
    [Header("Velocidade da Rotação")]
    public float velocidadeRotacao = 50f; // Graus por segundo

    [Header("Limites de Rotação (Olhar para Cima/Baixo)")]
    public float anguloMinimoX = -60f;
    public float anguloMaximoX = 60f;

    private float rotacaoX = 0f;
    private float rotacaoY = 0f;

    void Start()
    {
        // Como o mouse vai ser usado SÓ para pintar, precisamos garantir
        // que o cursor esteja visível e livre para andar pela tela
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Inicializa com os ângulos atuais da câmera na cena
        Vector3 angulosAtuais = transform.localRotation.eulerAngles;
        rotacaoX = angulosAtuais.x;
        rotacaoY = angulosAtuais.y;
    }

    void Update()
    {
        // Garante que o Novo Input System está ativo
        if (Keyboard.current == null) return;

        float inputHorizontal = 0f;
        float inputVertical = 0f;

        // Leitura do WASD ou Setas do Teclado
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputVertical = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputVertical = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputHorizontal = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputHorizontal = 1f;

        // Calcula a rotação baseada no tempo (Time.deltaTime) para não travar com o framerate
        rotacaoY += inputHorizontal * velocidadeRotacao * Time.deltaTime;
        rotacaoX -= inputVertical * velocidadeRotacao * Time.deltaTime; // Invertido para o W olhar para cima

        // Limita o eixo vertical (olhar para cima/baixo) para evitar rotação infinita
        rotacaoX = Mathf.Clamp(rotacaoX, anguloMinimoX, anguloMaximoX);

        // Aplica a rotação na câmera
        transform.localRotation = Quaternion.Euler(rotacaoX, rotacaoY, 0f);
    }
}