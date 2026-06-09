using UnityEngine;
using UnityEngine.XR; // Necessário para checar o VR

public class SeletorDePlataforma : MonoBehaviour
{
    public GameObject pincelPC;
    public GameObject pincelVR;

    void Awake()
    {
        // Verifica se há um Headset de VR ativo e conectado no momento em que o jogo inicia
        if (XRLoaderAtivo())
        {
            // Modo VR: Ativa a mão do Quest e desativa o controle do mouse
            pincelPC.SetActive(false);
            pincelVR.SetActive(true);
            Debug.Log("SISTEMA: Modo Realidade Virtual Ativado.");
        }
        else
        {
            // Modo PC: Ativa o mouse/scroll e desativa os nós de VR
            pincelPC.SetActive(true);
            pincelVR.SetActive(false);
            Debug.Log("SISTEMA: Modo Simulador de PC Ativado.");
        }
    }

    bool XRLoaderAtivo()
    {
        // Uma checagem simples para ver se o subsistema de XR (VR) inicializou
        var xrSettings = XRSettings.enabled;
        return xrSettings;
    }
}