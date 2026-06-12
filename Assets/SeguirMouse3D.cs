using UnityEngine;
using UnityEngine.InputSystem;

public class SeguirMouse3D : MonoBehaviour
{
    [Header("Configurações do Alvo")]
    public LayerMask layerDaSuperficie; // Opcional: para o pincel focar apenas no cilindro

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 posicaoMouse = Mouse.current.position.ReadValue();
        Ray raio = Camera.main.ScreenPointToRay(posicaoMouse);
        RaycastHit hit;

        // ADICIONADO: layerDaSuperficie no final do Raycast. 
        // Isso faz o laser ignorar TUDO que não pertença à camada que vamos escolher.
        if (Physics.Raycast(raio, out hit, 100f, layerDaSuperficie))
        {
            // Empurra a esfera um pouquinho para dentro para garantir o OnTriggerStay
            Vector3 posicaoAdentro = hit.point + (raio.direction * 0.05f);
            transform.position = posicaoAdentro;

            if (meshRenderer != null) meshRenderer.enabled = true;
        }
        else
        {
            if (meshRenderer != null) meshRenderer.enabled = false;
        }
    }
}