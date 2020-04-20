public class InventoryItemUpdated : Notification
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private InventoryItem m_item;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public InventoryItem inventoryItem => m_item;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------

    public InventoryItemUpdated(InventoryItem item)
    {
        m_type = Type.INVENTORY_ITEM_UPDATED;
        m_item = item;
    }
}
