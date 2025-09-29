using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform orignalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        orignalParent = transform.parent; //Save OG parent
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //Follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null) { 
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null) { 
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSLot = orignalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                // SLot has an item - swap Items
                dropSlot.currentItem.transform.SetParent(originalSLot.transform);
                originalSLot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSLot.currentItem = null;
            }

            // Move item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        } else
        {
            // If where we're dropping is not within the inventory
            if(!IsWithinInventory(eventData.position))
            {
                // Drop our item
                DropItem(originalSLot);
            }
            else
            {
                // Snap back to og slot
                transform.SetParent(orignalParent);
            }
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = orignalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition); 
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        // Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null) 
        {
            Debug.LogError("Missing 'Player' Tag");
            return;
        }

        // Random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        // Instantiate drop item and bounce
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().StartBouce();

        // Destroy the UI one
        Destroy(gameObject);
    }
}
