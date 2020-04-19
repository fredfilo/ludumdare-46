using System.Collections.Generic;

public class Inventory
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private readonly List<InventoryItem> m_items = new List<InventoryItem>();

    private int m_selectedIndex;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void SelectNextItem()
    {
        m_selectedIndex++;

        if (m_selectedIndex >= m_items.Count) {
            m_selectedIndex = 0;
        }

        InventoryItem item = GetItem(m_selectedIndex);
        GameController.instance.notifier.Notify(new InventoryItemSelected(item, m_selectedIndex));
    }

    public int GetItemsCount()
    {
        return m_items.Count;
    }
    
    public InventoryItem GetItem(int index)
    {
        if (index < 0) {
            return null;
        }

        if (index >= m_items.Count) {
            return null;
        }

        return m_items[index];
    }
    
    public InventoryItem GetItem(InventoryItem.ItemType itemType)
    {
        foreach (InventoryItem item in m_items) {
            if (item.type == itemType) {
                return item;
            }
        }

        return null;
    }

    public int GetItemIndex(InventoryItem.ItemType itemType)
    {
        for (int i = 0; i < m_items.Count; i++) {
            if (m_items[i].type == itemType) {
                return i;
            }
        }

        return -1;
    }

    public int GetItemQuantity(InventoryItem.ItemType itemType)
    {
        InventoryItem item = GetItem(itemType);

        return item?.quantity ?? 0;
    }

    public void Add(InventoryItem.ItemType itemType, int quantity)
    {
        InventoryItem item = GetItem(itemType);
        if (item == null) {
            item = new InventoryItem(itemType);
            m_items.Add(item);
        }

        item.Add(quantity);
        // GameController.instance.notifier.Notify(new InventoryItemUpdated(item, GetItemIndex(itemType)));
    }
}
