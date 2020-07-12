public class HealthSystem {

    private float health;
    private float healthMax;

    public HealthSystem(float Max) {
        this.healthMax = Max;
        health = healthMax;
    }

    public float Current()
    {
        return health;
    }

    public float CurrentPercent()
    {
        return health / healthMax;
    }

    public void Damage(float dmg)
    {
        health -= dmg;
        if (health < 0) health = 0;
    }

    public void Heal(float dmg)
    {
        health += dmg;
        if (health > healthMax) health = 0;
    }
}