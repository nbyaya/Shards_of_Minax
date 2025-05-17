using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells; // Needed for checking dismounting
using Server.Mobiles; // Needed for Mobile type check

namespace Server.Mobiles
{
    [CorpseName("a glazeborn corpse")] // Unique corpse name
    public class Glazeborn : BaseCreature
    {
        // --- Configurable Constants ---
        private const double ShatteringGlazeCooldown = 20.0; // Cooldown in seconds for AOE
        private const double ShatteringGlazeWindup = 2.5;    // Windup time in seconds for AOE
        private const int ShatteringGlazeRange = 6;          // Radius of the AOE attack
        private const int ShatteringGlazeMinDamage = 40;     // Min damage for AOE
        private const int ShatteringGlazeMaxDamage = 60;     // Max damage for AOE
        private const double FrozenTouchChance = 0.25;      // Chance to apply Frozen Touch on melee hit (25%)
        private const double FrozenTouchDuration = 7.0;     // Duration of the Frozen Touch debuff in seconds

        // --- Private Fields ---
        private DateTime _NextShatteringGlaze;
        private DateTime _NextFrozenTouchCheck; // Optional: if you want a cooldown on the debuff application chance itself

        [Constructable]
        public Glazeborn()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a glazeborn";
            Body = 43; // Uses the Ice Fiend body
            BaseSoundID = 357; // Uses the Ice Fiend sounds

            // --- Enhanced Stats (Significantly higher than Ice Fiend) ---
            SetStr(500, 600); // Higher Strength
            SetDex(100, 150); // Moderate Dexterity
            SetInt(300, 400); // High Intelligence

            SetHits(800, 1000); // Much higher Hit Points

            SetDamage(15, 25); // Higher base melee damage

            // --- Skills (Higher than Ice Fiend) ---
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 95.1, 105.0); // Can exceed 100 for advanced monsters
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0); // Can exceed 100 for advanced monsters

            // --- Resistances (Enhanced, especially Cold/Energy) ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 95, 100); // Near or full immunity to Cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 80); // High Energy Resistance

            Fame = 20000; // Higher Fame
            Karma = -20000; // Higher negative Karma

            VirtualArmor = 70; // Higher Virtual Armor

            // --- Unique Hue ---
            Hue = 0x481; // A unique icy blue/white hue

            // --- Initial Cooldowns ---
            _NextShatteringGlaze = DateTime.UtcNow;
            _NextFrozenTouchCheck = DateTime.UtcNow;
        }

        public Glazeborn(Serial serial)
            : base(serial)
        {
        }

        // --- Override Properties ---
        public override int TreasureMapLevel => 5; // Advanced map level
        public override int Meat => 1;
        public override bool CanFly => true;      // Keep the ability to fly
        public override bool BardImmune => true;  // Immune to Bard control
        public override bool Unprovokable => true; // Cannot be provoked

        // --- Custom Abilities ---

        // AOE Ability: Shattering Glaze
        public void PerformShatteringGlaze()
        {
            if (DateTime.UtcNow < _NextShatteringGlaze)
                return;

            bool hasNearbyTargets = false;
            IPooledEnumerable eable = GetMobilesInRange(ShatteringGlazeRange);
            foreach (Mobile m in eable)
            {
                if (m != this && m.Alive && !m.Blessed && CanBeHarmful(m))
                {
                    hasNearbyTargets = true;
                    break;
                }
            }
            eable.Free();

            if (!hasNearbyTargets)
                return;

            _NextShatteringGlaze = DateTime.UtcNow + TimeSpan.FromSeconds(ShatteringGlazeCooldown);

            // Wind‐up effect
            FixedParticles(0x3779, 1, 30, 9964, 0x481, 0, EffectLayer.Waist);
            PlaySound(0x1F5);

            Timer.DelayCall(TimeSpan.FromSeconds(ShatteringGlazeWindup), () =>
            {
                PlaySound(0x5C7);
                FixedParticles(0x3779, 10, 30, 9502, 0x481, 0, EffectLayer.Head);

                var targets = GetMobilesInRange(ShatteringGlazeRange);
                foreach (Mobile m in targets)
                {
                    if (m != this && m.Alive && !m.Blessed && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        m.FixedParticles(0x3779, 1, 20, 9964, 0x481, 0, EffectLayer.Waist);
                        m.PlaySound(0x307);

                        int damage = Utility.RandomMinMax(ShatteringGlazeMinDamage, ShatteringGlazeMaxDamage);
                        AOS.Damage(m, this, damage, 0, 0, 75, 0, 25);
                    }
                }
                targets.Free();
            });
        }

        // Melee Ability: Frozen Touch (Debuff on melee hit)
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is Mobile target && Utility.RandomDouble() < FrozenTouchChance)
            {
                DoHarmful(target);

                // Apply a 20% Dex reduction for the duration
                int dexLoss = (int)(target.Dex * 0.20);
                var mod = new StatMod(StatType.Dex, "GlazebornDex", -dexLoss, TimeSpan.FromSeconds(FrozenTouchDuration));
                target.AddStatMod(mod);

                target.SendLocalizedMessage(1008111, false, ": The glacial touch chills you to the bone!");
                target.FixedParticles(0x374A, 1, 15, 9904, 0x481, 0, EffectLayer.Waist);
                target.PlaySound(0x254);
            }
        }

        // Combat tick override to attempt AOE
        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var target = Combatant as Mobile;
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 15) || !CanBeHarmful(target) || !InLOS(target))
                return;

            PerformShatteringGlaze();
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);   // Very high tier loot
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));

            if (Utility.RandomDouble() < 0.01) // 1% for unique
            {
                // this.PackItem(new GlazedShardArtifact());
            }
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(_NextShatteringGlaze);
            writer.Write(_NextFrozenTouchCheck);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                _NextShatteringGlaze = reader.ReadDateTime();
                _NextFrozenTouchCheck = reader.ReadDateTime();
            }
            else
            {
                _NextShatteringGlaze = DateTime.UtcNow;
                _NextFrozenTouchCheck = DateTime.UtcNow;
            }
        }
    }
}
