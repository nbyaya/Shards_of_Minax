using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

public class SuperDragon : BaseCreature
{
    private DateTime _nextSpecialAbility;
    private int _phase = 1;

    [Constructable]
    public SuperDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
    {
        Name = "Super Dragon";
        Body = 12;
        BaseSoundID = 362;

        SetStr(1000);
        SetDex(200);
        SetInt(1000);

        SetHits(30000);
        SetMana(5000);

        SetDamage(50, 80);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Fire, 50);

        SetResistance(ResistanceType.Physical, 80);
        SetResistance(ResistanceType.Fire, 100);
        SetResistance(ResistanceType.Cold, 50);
        SetResistance(ResistanceType.Poison, 60);
        SetResistance(ResistanceType.Energy, 60);

        SetSkill(SkillName.EvalInt, 120.0);
        SetSkill(SkillName.Magery, 120.0);
        SetSkill(SkillName.Meditation, 120.0);
        SetSkill(SkillName.MagicResist, 120.0);
        SetSkill(SkillName.Tactics, 120.0);
        SetSkill(SkillName.Wrestling, 120.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 60;

        _nextSpecialAbility = DateTime.UtcNow;
    }

    public SuperDragon(Serial serial) : base(serial)
    {
    }

    public override void OnThink()
    {
        base.OnThink();

        if (Combatant == null)
            return;

        if (DateTime.UtcNow >= _nextSpecialAbility)
        {
            UseSpecialAbility();
            _nextSpecialAbility = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown
        }

        // Phase transition at 50% health
        if (Hits < HitsMax / 2 && _phase == 1)
        {
            _phase = 2;
            Say("You dare challenge me? Witness my true power!");
            DoPhaseTransition();
        }
    }

    private void UseSpecialAbility()
    {
        switch (Utility.Random(5))
        {
            case 0:
                FirestormBreath();
                break;
            case 1:
                FrostNovaBlast();
                break;
            case 2:
                VenomousRain();
                break;
            case 3:
                ThunderousRoar();
                break;
            case 4:
                SummonDragonkin();
                break;
        }
    }

    private void FirestormBreath()
    {
        Say("Feel the wrath of my firestorm!");
        foreach (Mobile m in GetMobilesInRange(10))
        {
            if (m != this && m is PlayerMobile)
            {
                int damage = Utility.RandomMinMax(80, 120);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                m.PlaySound(0x208);

                if (Utility.RandomDouble() < 0.3) // 30% chance
                    m.SendLocalizedMessage(1008112); // The intense heat has damaged your weapon!
            }
        }
    }

    private void FrostNovaBlast()
    {
        Say("Freeze in your tracks!");
        foreach (Mobile m in GetMobilesInRange(8))
        {
            if (m != this && m is PlayerMobile)
            {
                int damage = Utility.RandomMinMax(60, 100);
                AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                m.FixedParticles(0x374A, 10, 30, 5013, EffectLayer.Waist);
                m.PlaySound(0x5C3);

                if (Utility.RandomDouble() < 0.4) // 40% chance
                {
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.SendLocalizedMessage(1005384); // You cannot move!
                }
            }
        }
    }

    private void VenomousRain()
    {
        Say("My venom shall consume you!");
        foreach (Mobile m in GetMobilesInRange(12))
        {
            if (m != this && m is PlayerMobile)
            {
                int damage = Utility.RandomMinMax(40, 80);
                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                m.PlaySound(0x474);

                if (Utility.RandomDouble() < 0.5) // 50% chance
                {
                    Poison p = Poison.Greater;
                    m.ApplyPoison(this, p);
                    m.SendLocalizedMessage(1008116); // You have been poisoned!
                }
            }
        }
    }

    private void ThunderousRoar()
    {
        Say("Tremble before my mighty roar!");
        foreach (Mobile m in GetMobilesInRange(8))
        {
            if (m != this && m is PlayerMobile)
            {
                int damage = Utility.RandomMinMax(70, 110);
                AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                m.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Head);
                m.PlaySound(0x29);

                if (Utility.RandomDouble() < 0.3) // 30% chance
                {
                    m.Stam -= 50;
                    m.SendLocalizedMessage(1072829); // Your stamina has been drained!
                }
            }
        }
    }

    private void SummonDragonkin()
    {
        Say("Come forth, my children!");
        for (int i = 0; i < 3; i++)
        {
            BaseCreature minion = new Drake();
            minion.Team = this.Team;
            minion.MoveToWorld(GetSpawnPosition(2), this.Map);
            minion.Combatant = Combatant;
        }
    }

    private void DoPhaseTransition()
    {
        // Heal a bit
        Hits += 5000;

        // Increase damage
        SetDamage(70, 100);

        // Add energy damage
        SetDamageType(ResistanceType.Physical, 33);
        SetDamageType(ResistanceType.Fire, 33);
        SetDamageType(ResistanceType.Energy, 34);

        // Visual effect
        FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
        PlaySound(0x665);
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(_phase);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        _phase = reader.ReadInt();
    }
}