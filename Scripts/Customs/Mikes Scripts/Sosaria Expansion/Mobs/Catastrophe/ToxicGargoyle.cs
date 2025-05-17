using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a toxic gargoyle corpse")]
    public class ToxicGargoyle : BaseCreature
    {
        private DateTime _NextToxicBreath;

        [Constructable]
        public ToxicGargoyle()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Toxic Gargoyle";
            Body = 722;              // Same body as the Undead Gargoyle
            BaseSoundID = 372;       // Same base sound
            Hue = 0x497;             // Unique toxic-green hue

            // Enhanced Stats
            SetStr(350, 450);
            SetDex(150, 170);
            SetInt(350, 450);
            SetHits(300, 400);

            SetDamage(20, 35);

            // Damage Types: 40% Physical and 60% Poison
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // Resistances: Especially high on Poison
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);
            SetSkill(SkillName.Necromancy, 80.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 110.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            // Optionally pack any unique loot here:
            // PackItem(new ToxicShard());
        }

        public ToxicGargoyle(Serial serial) : base(serial)
        {
        }

        // This override periodically uses the Toxic Breath ability on its combatant.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map ||
                !InRange(combatant, 10) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= _NextToxicBreath)
            {
                DoToxicBreath(combatant);
                // Set the cooldown for the next toxic breath between 8 and 12 seconds.
                _NextToxicBreath = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + (Utility.RandomDouble() * 4.0));
            }
        }

        // This method executes the toxic ranged attack.
        public void DoToxicBreath(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            // Ensure target is a Mobile before accessing mobile-specific members
            if (target is Mobile m)
            {
                // Trigger a toxic breath animation (adjust parameters as needed)
                Animate(12, 5, 1, true, false, 0);

                DoHarmful(target);

                // Show toxic breath visual effects using moving particles

                // Simulate the travel delay of the toxic breath
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    // 75% chance for the toxic breath to hit the target
                    if (Utility.RandomDouble() <= 0.75)
                    {
                        // Apply poison-type damage (100% poison)
                        int damage = Utility.RandomMinMax(25, 35);
                        AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);

                        // Apply a deadly poison effect on the target
                        target.ApplyPoison(this, Poison.Deadly);

                        // Drain some mana from the target as part of the toxic effect
                        int manaDrain = Utility.RandomMinMax(10, 20);
                        if (m.Mana >= manaDrain)
                            m.Mana -= manaDrain;
                        else
                            m.Mana = 0;

                        m.SendAsciiMessage("You are seared by toxic fumes!");
                    }
                    else
                    {
                        m.SendAsciiMessage("The toxic breath narrowly misses you.");
                    }
                });
            }
        }

        // OnGaveMeleeAttack override: on melee hits, the Gargoyle has a chance to apply further toxic effects.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender == null || defender.Deleted)
                return;

            // 50% chance to trigger additional toxic damage and effects during melee attacks
            if (Utility.RandomDouble() <= 0.50)
            {
                if (defender is Mobile target)
                {
                    // Deal extra poison damage on melee hit
                    int extraDamage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(defender, this, extraDamage, 0, 0, 0, 100, 0);

                    // 30% chance to apply a Greater Poison debuff on the target
                    if (Utility.RandomDouble() <= 0.30)
                    {
                        defender.ApplyPoison(this, Poison.Greater);
                    }

                    // Drain a small amount of mana from the defender
                    int manaDrain = Utility.RandomMinMax(5, 10);
                    if (target.Mana >= manaDrain)
                        target.Mana -= manaDrain;
                    else
                        target.Mana = 0;

                    defender.SendAsciiMessage("The Toxic Gargoyle's toxic touch burns and drains you!");
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 2; } }
        public override int Meat { get { return 1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
