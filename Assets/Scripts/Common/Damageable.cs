public interface Damageable
{
    bool IsAlive();
    
    int GetHealthPoints();
    
    void ApplyDamage(int damage);
}
