using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressionMatrix
{
    private static AggressionValue[] totalMatrix = new AggressionValue[] {
        new AggressionValue("Player", "Enemy", true),
        new AggressionValue("Player", "Undead", true),
        new AggressionValue("Enemy", "Player", true),
        new AggressionValue("Enemy", "Undead", true),
        new AggressionValue("Enemy", "Enemy", false),
        new AggressionValue("Undead", "Enemy", true),
        new AggressionValue("Undead", "Player", false),
        new AggressionValue("Undead", "Undead", false),
    };

    private Dictionary<string,bool> aggression = new Dictionary<string, bool>();

    public AggressionMatrix(string attacker)
    {
        foreach(AggressionValue agg in totalMatrix)
        {
            if(agg.attacker == attacker)
            {
                aggression.Add(agg.defender, agg.aggressive);
            }
        }
    }

    public bool CheckAggression(string defender)
    {
        bool result;
        try { result = aggression[defender]; }
        catch { result = false; }
        return result;
    }
}

public class AggressionValue
{
    public string attacker;
    public string defender;
    public bool aggressive;

    public AggressionValue(string attacker, string defender, bool aggressive)
    {
        this.attacker = attacker;
        this.defender = defender;
        this.aggressive = aggressive;
    }
}