public abstract class Notification
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum Type
    {
        READ_MESSAGE
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    protected Type m_type;

    // ACCESSORS
    // -------------------------------------------------------------------------

    public Type type => m_type;
}
