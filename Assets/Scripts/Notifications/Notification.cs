public abstract class Notification
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum Type
    {
        READ_MESSAGES,
        UI_INTERACTION_STARTED,
        UI_INTERACTION_ENDED,
        PLAYER_WANTS_TO_INTERACT,
        INVENTORY_ITEM_UPDATED,
        INVENTORY_ITEM_SELECTED
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    protected Type m_type;

    // ACCESSORS
    // -------------------------------------------------------------------------

    public Type type => m_type;
}
