using UnityEngine;

public class Lab04PressurePlate : MonoBehaviour
{
    [Header("Who can press the plate")]
    [SerializeField] private LayerMask activatorLayers;

    [Header("Gate")]
    [SerializeField] private Rigidbody gate;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float openSpeed = 3f;

    [Header("Feedback")]
    [SerializeField] private Renderer plateRenderer;
    [SerializeField] private Material plateOffMaterial;
    [SerializeField] private Material plateOnMaterial;
    [SerializeField] private Transform plateVisual;

    private Vector3 closedPosition;
    private Vector3 plateVisualStart;
    private int occupants;

    private void Start()
    {
        closedPosition = gate.position;
        plateVisualStart = plateVisual.localPosition;
        SetPressed(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((activatorLayers.value &
             (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        bool wasEmpty = occupants == 0;
        occupants++;

        if (wasEmpty)
        {
            SetPressed(true);
        }

        Debug.Log(
            $"{other.name} pressed plate. " +
            $"Occupants: {occupants}");
    }

    private void OnTriggerExit(Collider other)
    {
        if ((activatorLayers.value &
             (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        occupants = Mathf.Max(0, occupants - 1);

        if (occupants == 0)
        {
            SetPressed(false);
        }

        Debug.Log(
            $"{other.name} left plate. " +
            $"Occupants: {occupants}");
    }

    private void FixedUpdate()
    {
        Vector3 target = occupants > 0
            ? closedPosition + openOffset
            : closedPosition;

        gate.MovePosition(
            Vector3.MoveTowards(
                gate.position,
                target,
                openSpeed * Time.fixedDeltaTime));
    }

    private void SetPressed(bool pressed)
    {
        if (plateRenderer != null)
        {
            plateRenderer.sharedMaterial = pressed
                ? plateOnMaterial
                : plateOffMaterial;
        }

        if (plateVisual != null)
        {
            plateVisual.localPosition = plateVisualStart +
                (pressed ? Vector3.down * 0.05f : Vector3.zero);
        }
    }
}