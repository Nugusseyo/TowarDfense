namespace _Scripts.Agent
{
    public interface IHealable
    {
        void TakeHeal(int healAmount);
        void TakeDamage(int damageAmount);
    }
}
