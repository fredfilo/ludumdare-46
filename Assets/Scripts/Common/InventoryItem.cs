public class InventoryItem
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum ItemType
    {
        AXE,
        WOOD
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    private ItemType m_type;
    private int m_quantity;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public ItemType type => m_type;

    public int quantity => m_quantity;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------

    public InventoryItem(ItemType itemType)
    {
        m_type = itemType;
    }
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void Add(int count)
    {
        m_quantity += count;

        if (m_quantity < 0) {
            m_quantity = 0;
        }
    }

    public void Remove(int count)
    {
        m_quantity -= count;

        if (m_quantity < 0) {
            m_quantity = 0;
        }
    }
}
