public class InventoryItemSelected : Notification
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private int m_index;
    private InventoryItem m_item;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public int index => m_index;
    
    public InventoryItem inventoryItem => m_item;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------

    public InventoryItemSelected(InventoryItem item, int index)
    {
        m_type = Type.INVENTORY_ITEM_SELECTED;
        m_item = item;
        m_index = index;
    }
}
