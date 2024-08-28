using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Creature
{
    const int CORE_SLOTS = 3;
    const int EMPOWERED_SLOTS = 3;
    const int MASTERY_SLOTS = 1;

    public CreatureBase Species { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int Energy { get; set; }
    public string Nickname { get; set; }

    public List<CoreAction> KnownCoreActions { get; set; }
    public List<EmpoweredAction> KnownEmpoweredActions { get; set; }
    public List<MasteryAction> KnownMasteryActions { get; set; }

    public CoreAction[] EquippedCoreActions { get; set; }
    public EmpoweredAction[] EquippedEmpoweredActions { get; set; }
    public MasteryAction[] EquippedMasteryActions { get; set; }

    public Creature(CreatureBase creatureBase, int creatureLevel, string nickname = "")
    {
        Species = creatureBase;
        Level = creatureLevel;
        HP = MaxHP;
        Energy = 0;
        if (nickname != "")
        {
            Nickname = nickname;
        } 
        else
        {
            Nickname = creatureBase.CreatureName;
        }

        KnownCoreActions = new List<CoreAction>();
        KnownEmpoweredActions = new List<EmpoweredAction>();
        KnownMasteryActions = new List<MasteryAction>();
        EquippedCoreActions = new CoreAction[CORE_SLOTS];
        EquippedEmpoweredActions = new EmpoweredAction[EMPOWERED_SLOTS];
        EquippedMasteryActions = new MasteryAction[MASTERY_SLOTS];

        initTalents();
        equipTalents();

    }

    public int MaxHP { get { return calc_hp(Species.HP); } }
    public int MaxEnergy { get { return calc_energy(Species.Energy); } }
    public int Strength { get { return calc_stat(Species.Strength); } }
    public int Magic { get { return calc_stat(Species.Magic); } }
    public int Skill { get { return calc_stat(Species.Skill); } }
    public int Speed { get { return calc_stat(Species.Speed); } }
    public int Defense { get { return calc_stat(Species.Defense); } }
    public int Resistance { get { return calc_stat(Species.Resistance); } }


    private int calc_stat(int baseStat)
    {
        return Mathf.FloorToInt((baseStat * Level) / 100f) + 5;
    }

    private int calc_hp(int baseHp)
    {
        return Mathf.FloorToInt((baseHp * Level) / 100f) + 10 + Level;
    }

    private int calc_energy(int baseEnergy)
    {
        return Mathf.FloorToInt((baseEnergy * Level) / 100f) + 30 + Level;
    }

    private void initTalents()
    {
        foreach (var talent in Species.LearnableTalents)
        {
            if (talent.Level <= Level)
            {
                if (talent.TalentBase is CoreActionBase coreActionBase)
                {
                    KnownCoreActions.Add(new CoreAction(coreActionBase));
                }
                else if (talent.TalentBase is EmpoweredActionBase empoweredActionBase)
                {
                    KnownEmpoweredActions.Add(new EmpoweredAction(empoweredActionBase));
                }
                else if (talent.TalentBase is MasteryActionBase masteryActionBase)
                {
                    KnownMasteryActions.Add(new MasteryAction(masteryActionBase));
                }
            }
        }
    }

    private void equipTalents()
    {
        if (KnownCoreActions.Count > 0)
        {
            int i = KnownCoreActions.Count - 1;

            while (i >= 0 && (EquippedCoreActions[0] == null || EquippedCoreActions[1] == null || EquippedCoreActions[2] == null))
            {
                if (KnownCoreActions[i].Action.Category == ActionCategory.Physical && EquippedCoreActions[0] == null)
                {
                    EquippedCoreActions[0] = KnownCoreActions[i]; // Equip last learned Physical Core Action
                } 
                else if (KnownCoreActions[i].Action.Category == ActionCategory.Magical && EquippedCoreActions[1] == null)
                {
                    EquippedCoreActions[1] = KnownCoreActions[i]; // Equip last learned Magical Core Action
                } 
                else if (KnownCoreActions[i].Action.Category == ActionCategory.Defensive && EquippedCoreActions[2] == null)
                {
                    EquippedCoreActions[2] = KnownCoreActions[i]; // Equip last learned Defensive Core Action
                }
                i--;
            }
        }

        if (KnownEmpoweredActions.Count > 0)
        {
            int i = KnownEmpoweredActions.Count - 1;
            int slot = 0;

            while (i >= 0 && slot < EMPOWERED_SLOTS)
            {
                EquippedEmpoweredActions[slot] = KnownEmpoweredActions[i];
                slot++;
                i--;
            }
        }

        if (KnownMasteryActions.Count > 0) 
        {
            int i = KnownMasteryActions.Count - 1;
            int slot = 0;

            while (i >= 0 && slot < MASTERY_SLOTS)
            {
                EquippedMasteryActions[slot] = KnownMasteryActions[i];
                slot++;
                i--;
            }
        }
    }
}

