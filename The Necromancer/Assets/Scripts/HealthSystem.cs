using System;

public class HealthSystem {

    public event EventHandler OnHealthChanged;

    private float health;
    private readonly float healthMax;

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
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health > healthMax) health = 0;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Resurrect()
    {
        if (health == 0)
        {
            health = healthMax;
        }

    }
}