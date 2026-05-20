using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public sealed class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Color touchedColor = Color.yellow;

    [SerializeField]
    private bool restoreWhenHandLeaves = true;

    [SerializeField]
    private Renderer targetRenderer;

    [SerializeField] private string sceneToLoad; // El nombre de la escena para cargar
    public GameObject confirmPanel; // Panel de confirmacion para mostrar al detectar el color amarillo

    private XRSimpleInteractable interactable;
    private MaterialPropertyBlock propertyBlock;
    private Color initialColor = Color.white;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        propertyBlock = new MaterialPropertyBlock();
        initialColor = ReadInitialColor();
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        SetColor(touchedColor);
        confirmPanel.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (restoreWhenHandLeaves)
        {
            SetColor(initialColor);
            confirmPanel.SetActive(false);
        }
    }

    private Color ReadInitialColor()
    {
        if (targetRenderer == null || targetRenderer.sharedMaterial == null)
            return Color.white;

        Material material = targetRenderer.sharedMaterial;

        if (material.HasProperty(BaseColorId))
            return material.GetColor(BaseColorId);

        if (material.HasProperty(ColorId))
            return material.GetColor(ColorId);

        return Color.white;
    }

    private void SetColor(Color color)
    {
        if (targetRenderer == null)
            return;

        targetRenderer.GetPropertyBlock(propertyBlock);

        // URP/Lit suele usar _BaseColor.
        propertyBlock.SetColor(BaseColorId, color);

        // Shaders legacy suelen usar _Color.
        propertyBlock.SetColor(ColorId, color);

        targetRenderer.SetPropertyBlock(propertyBlock);
    }

    private bool CheckColorYellow()
    {
        if (targetRenderer.sharedMaterial.color == Color.yellow)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadScene()
    {
        if (confirmPanel.activeSelf)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            Debug.Log("Cargando escena: " + sceneToLoad);
        }
        else
        {
            Debug.Log("El color actual no es amarillo, no se cargará la escena.");
        }
    }
}