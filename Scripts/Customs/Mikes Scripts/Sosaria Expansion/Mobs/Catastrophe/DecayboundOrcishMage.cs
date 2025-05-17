using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network; // Required for SendMessage
using Server.Targeting; // Required for Target

namespace Server.Mobiles
{
    [CorpseName("a decaying orcish corpse")]
    public class DecayboundOrcishMage : BaseCreature
    {
        private DateTime m_NextBlightBoltTime;
        private DateTime m_NextDecayNovaTime;

        [Constructable]
        public DecayboundOrcishMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction times
        {
            this.Name = "a Decaybound Orcish Mage";
            this.Body = 140; // Orcish Mage Body
            this.BaseSoundID = 0x45A; // Orcish Mage Sound
            this.Hue = 2952; // A sickly greenish-grey hue



            // Significantly increased stats
            this.SetStr(150, 180);
            this.SetDex(120, 150);
            this.SetInt(250, 300); // High Int for potent magic

            this.SetHits(400, 500); // Much tougher

            this.SetDamage(10, 20); // Decent melee damage as a backup

            // Damage types remain physical for melee
            this.SetDamageType(ResistanceType.Physical, 100);

            // Adjusted resistances - stronger against poison and physical
            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 60, 70); // High poison resist
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Advanced magic skills and decent combat skills
            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 95.1, 100.0);
            this.SetSkill(SkillName.Tactics, 70.1, 80.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 80.0);
            this.SetSkill(SkillName.Meditation, 80.0, 90.0); // To regen mana faster

            this.Fame = 8000; // Higher fame
            this.Karma = -8000; // Higher negative karma

            this.VirtualArmor = 50; // Increased VA

            // Pack some reagents fitting the theme
            this.PackReg(10); // More regs
            this.PackItem(new NoxCrystal(Utility.RandomMinMax(2, 5)));
            this.PackItem(new GraveDust(Utility.RandomMinMax(3, 6)));

            // Keep OrcishKinMask drop chance
            if (0.05 > Utility.RandomDouble())
                this.PackItem(new OrcishKinMask());

            // Reduced chance for Yeast, maybe something else?
            if (0.1 > Utility.RandomDouble())
                PackItem(new Yeast());

            m_NextBlightBoltTime = DateTime.UtcNow;
            m_NextDecayNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); // Initial delay for nova
        }

        public DecayboundOrcishMage(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 3; // Higher level map
        public override int Meat => 1; // Standard Orc meat
        public override TribeType Tribe => TribeType.Orc;
        public override OppositionGroup OppositionGroup => OppositionGroup.SavagesAndOrcs;

        // --- Unique Abilities ---

        public override void OnActionCombat()
        {
            IDamageable combatant = Combatant;

            if (combatant == null || combatant.Deleted || !combatant.Alive || combatant.Map != Map || !InRange(combatant.Location, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            DateTime now = DateTime.UtcNow;

            // Prioritize Decay Nova if ready
            if (now >= m_NextDecayNovaTime && Utility.RandomDouble() < 0.3) // 30% chance to use Nova if ready
            {
                CastDecayNova(combatant);
                m_NextDecayNovaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Cooldown
                m_NextBlightBoltTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)); // Short delay after nova
            }
            // Otherwise, consider Blight Bolt
            else if (now >= m_NextBlightBoltTime)
            {
                CastBlightBolt(combatant);
                m_NextBlightBoltTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)); // Cooldown
            }
            else // If special abilities aren't ready, fall back to standard AI mage behavior
            {
                base.OnActionCombat();
            }
        }

        public void CastBlightBolt(IDamageable target)
        {
            if (target == null || !CanBeHarmful(target)) return;

            this.MovingParticles(target, 0x36E4, 5, 0, false, true, 3006, 4006, 0); // Greenish bolt effect
            this.PlaySound(0x1E5); // Zap sound

            DoHarmful(target);

            // Calculate damage based on Int/EvalInt
            int damage = (int)(this.Int * 0.3 + this.Skills[SkillName.EvalInt].Value * 0.4);
            damage = Utility.RandomMinMax(damage - 5, damage + 5);

            // Type check before accessing Mobile properties/methods
            if (target is Mobile mobileTarget)
            {
                // Delay damage slightly for travel time
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    if (mobileTarget.Alive)
                    {
                        // Deal damage (mix of physical and poison)
                        AOS.Damage(mobileTarget, this, damage, 50, 0, 0, 50, 0);

                        // Chance to apply potent poison
                        if (Utility.RandomDouble() < 0.4) // 40% chance
                        {
                            mobileTarget.ApplyPoison(this, Poison.Deadly); // Apply Deadly poison
                            mobileTarget.SendLocalizedMessage(1070749, false, "The blight seeps into your veins!"); // You have been poisoned! with custom text
                        }
                    }
                });
            }
            else // Handle non-Mobile IDamageable if necessary (basic damage)
            {
                 Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                 {
                     if (target.Alive) {
                        target.Damage(damage, this);
                     }
                 });
            }
        }

        public void CastDecayNova(IDamageable centerTarget)
        {
            if (centerTarget == null) return;

            Map map = this.Map;
            if (map == null) return;

            DoHarmful(centerTarget);

            // Visual effect on caster
            this.FixedParticles(0x3709, 10, 30, 5052, 1153, 2, EffectLayer.Head); // Greenish poison explosion effect
            this.PlaySound(0x22F); // Poison field sound

            // Find targets in range
            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(6)) // Affect targets within 6 tiles
            {
                if (m != this && CanBeHarmful(m) && m.Alive && InLOS(m))
                {
                    targets.Add(m);
                }
            }

            // Damage and effect application
            foreach (Mobile m in targets)
            {
                DoHarmful(m);

                // Lower AoE damage (Poison type)
                int damage = Utility.RandomMinMax(25, 40);
                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0); // 100% Poison damage

                // Apply a short 'Withering' debuff (reduces healing received)
                if (Utility.RandomDouble() < 0.6) // 60% chance
                {
                    ApplyWitheringDebuff(m, TimeSpan.FromSeconds(10));
                    m.SendMessage(61, "A decaying aura weakens your ability to heal!"); // Custom message color
                }
            }

            // Check if centerTarget is a Mobile for specific messages/effects
            if (centerTarget is Mobile mobileCenterTarget)
            {
                 // Apply stronger poison to the main target
                if (Utility.RandomDouble() < 0.8) // 80% chance on main target
                {
                   mobileCenterTarget.ApplyPoison(this, Poison.Lethal);
                   mobileCenterTarget.SendLocalizedMessage(1070749, false, "You are engulfed in potent decay!");
                }
            }
        }

        // Helper method for the healing debuff
        private static Dictionary<Mobile, Timer> m_WitheringDebuffTimers = new Dictionary<Mobile, Timer>();

        public static bool IsWithering(Mobile m)
        {
            return m_WitheringDebuffTimers.ContainsKey(m);
        }

        public static void ApplyWitheringDebuff(Mobile m, TimeSpan duration)
        {
            if (m_WitheringDebuffTimers.ContainsKey(m))
            {
                m_WitheringDebuffTimers[m].Stop(); // Reset timer if already applied
            }

            Timer t = new InternalWitheringTimer(m, duration);
            m_WitheringDebuffTimers[m] = t;
            t.Start();
            // Optionally: BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Clumsy, 1017384, 1075835, duration, m)); // Using Clumsy icon as placeholder for debuff
        }

        public static void RemoveWitheringDebuff(Mobile m)
        {
            if (m_WitheringDebuffTimers.ContainsKey(m))
            {
                m_WitheringDebuffTimers[m].Stop();
                m_WitheringDebuffTimers.Remove(m);
                m.SendMessage("The decaying weakness fades.");
                // Optionally: BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
            }
        }

        private class InternalWitheringTimer : Timer
        {
            private Mobile m_Target;
            public InternalWitheringTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                RemoveWitheringDebuff(m_Target);
            }
        }

        // Modify healing checks elsewhere in your code (e.g., Bandage.cs, Potion.cs, healing spells)
        // to check: if (DecayboundOrcishMage.IsWithering(patient)) { heal_amount /= 2; } // Example: Halve healing

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Chance for Withering Touch on melee hit
            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                defender.PlaySound(0x1E6); // Drain sound
                defender.FixedParticles(0x374A, 10, 15, 5013, 1153, 2, EffectLayer.Waist); // Greenish drain effect

                int strDrain = Utility.RandomMinMax(5, 10);
                int dexDrain = Utility.RandomMinMax(5, 10);

                // Apply temporary StatMod drain
                StatMod strMod = new StatMod(StatType.Str, "WitheringTouchStr", -strDrain, TimeSpan.FromSeconds(15));
                StatMod dexMod = new StatMod(StatType.Dex, "WitheringTouchDex", -dexDrain, TimeSpan.FromSeconds(15));

                defender.AddStatMod(strMod);
                defender.AddStatMod(dexMod);

                defender.SendMessage(38, "The mage's touch drains your vitality!"); // Red feedback message
            }
        }


        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich, 2); // Better standard loot
            this.AddLoot(LootPack.MedScrolls, 1);
            this.AddLoot(LootPack.HighScrolls, 1); // Chance for high scrolls
            this.AddLoot(LootPack.Potions, 1); // Add potions

            // Chance for a unique themed item
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                switch (Utility.Random(3))
                {
                    case 0: PackItem(new StaffOfDecay()); break; // Example custom item
                    case 1: PackItem(new RobeOfBlight()); break;  // Example custom item
                    case 2: PackItem(new OrcishRitualComponent()); break; // Example custom item
                }
            }

            // Keep Orcish Mage Robe chance, maybe slightly higher?
            if (Utility.RandomDouble() < 0.015) // 1.5% chance
            {
                this.PackItem(new OrcishMageRobe());
            }
        }

        // --- Orcish Kin Mask Interaction (Preserved) ---
        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            // Type check before accessing Mobile properties/methods
            if (aggressor is Mobile mobileAggressor)
            {
                Item item = mobileAggressor.FindItemOnLayer(Layer.Helm);

                if (item is OrcishKinMask)
                {
                    AOS.Damage(mobileAggressor, 50, 0, 100, 0, 0, 0); // Pure energy damage maybe?
                    item.Delete();
                    mobileAggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    mobileAggressor.PlaySound(0x307);
                    mobileAggressor.SendMessage("Your mask shatters under the mage's scrutiny!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize timers on load
            m_NextBlightBoltTime = DateTime.UtcNow;
            m_NextDecayNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
        }
    }

    // --- Placeholder Custom Item Examples ---
    // You would need to create these item files separately

    public class StaffOfDecay : QuarterStaff
    {
        public override int LabelNumber => 1070848; // Staff of Decay (example label)
        [Constructable]
        public StaffOfDecay()
        {
            Hue = 2952; // Match mage hue
            Attributes.SpellDamage = 10;
            Attributes.CastRecovery = 1;
            WeaponAttributes.HitPoisonArea = 30; // Poison effect
        }
        public StaffOfDecay(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class RobeOfBlight : Robe
    {
         public override int LabelNumber => 1070849; // Robe of Blight (example label)
        [Constructable]
        public RobeOfBlight()
        {
            Hue = 1107; // Dark sickly green/brown
            Attributes.RegenMana = 1;
            Attributes.LowerRegCost = 10;
            Resistances.Poison = 10;
        }
        public RobeOfBlight(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

     public class OrcishRitualComponent : Item
    {
         public override int LabelNumber => 1070850; // Orcish Ritual Component (example label)
        [Constructable]
        public OrcishRitualComponent() : base(0x1F1C) // Skull candle appearance
        {
            Name = "Orcish Ritual Component";
            Hue = 2952;
            Weight = 1.0;
        }
        public OrcishRitualComponent(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

}