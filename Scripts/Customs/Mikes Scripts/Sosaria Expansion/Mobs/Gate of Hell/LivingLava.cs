using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Fourth; // For potential FireField use or adaptation

namespace Server.Mobiles
{
    // --- Temporary Lava Puddle Item ---
    // This item is created by the Living Lava and deals damage over time.
    public class TemporaryLavaPuddle : Item
    {
        private Timer m_Timer;
        private DateTime m_DecayTime;
        private Mobile m_Owner; // The Living Lava that created it

        // Define how much damage the puddle does per tick
        private const int DamagePerTick = 5;
        // Define how often the damage ticks (e.g., every 2 seconds)
        private static readonly TimeSpan DamageInterval = TimeSpan.FromSeconds(2);
        // Define how long the puddle lasts (e.g., 10-15 seconds)
        private static readonly TimeSpan DecayDuration = TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

        [Constructable]
        public TemporaryLavaPuddle(Mobile owner) : base(Utility.RandomList(0x1AE4, 0x1AE1, 0x1AD7, 0x1AD8)) // Static Lava graphics
        {
            Name = "scorching lava";
            Movable = false;
            Hue = 2118; // Molten Red/Orange Hue for the puddle
            m_Owner = owner;
            m_DecayTime = DateTime.UtcNow + DecayDuration;

            m_Timer = Timer.DelayCall(TimeSpan.Zero, DamageInterval, OnTick);
            m_Timer.Start();
        }

        public TemporaryLavaPuddle(Serial serial) : base(serial)
        {
        }

        public override void OnDelete()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
            base.OnDelete();
        }

        private void OnTick()
        {
            if (Deleted || Map == null || m_Owner == null || m_Owner.Deleted)
            {
                Delete();
                return;
            }

            if (DateTime.UtcNow >= m_DecayTime)
            {
                // Optional: Add a fade-out effect here if desired
                // Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x372A, 10, 60, 5029); // Smoke effect
                Delete();
                return;
            }

            // Find mobiles standing on the puddle
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 0); // Range 0 means only those on the same tile
            foreach (Mobile m in eable)
            {
                // Check if the mobile can be harmed by the owner of the puddle
                if (m != null && m.Alive && m_Owner.CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(m_Owner, m))
                {
                    // Apply fire damage
                    m_Owner.DoHarmful(m); // Ensure attacker/defender relationship is set
                    AOS.Damage(m, m_Owner, DamagePerTick, 0, 100, 0, 0, 0); // 100% Fire Damage
                    m.PlaySound(0x208); // Fire hit sound
                    m.FixedParticles(0x3709, 1, 10, 5016, EffectLayer.Waist); // Small fire burst effect
                }
            }
            eable.Free();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Owner);
            writer.WriteDeltaTime(m_DecayTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_DecayTime = reader.ReadDeltaTime();

            m_Timer = Timer.DelayCall(TimeSpan.Zero, DamageInterval, OnTick);
            m_Timer.Start();
        }
    }


    // --- The Living Lava Monster ---
    [CorpseName("a cooling magma husk")] // Themed corpse name
    public class LivingLava : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSpitTime;
        private DateTime m_NextEruptionTime;
        private DateTime m_NextAuraCheckTime; // Less frequent check for aura effect
        private Point3D m_LastLocation;

        // Unique Hue - Molten Red/Orange
        private const int UniqueHue = 2118;

        [Constructable]
        public LivingLava() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly slower reaction than elemental, more ponderous
        {
            Name = "a Living Lava";
            Body = 51;          // Slime body
            BaseSoundID = 456;  // Slime sound (as requested)
            Hue = UniqueHue;    // Apply the unique molten hue

            // --- Powerful Stats ---
            SetStr(500, 600);
            SetDex(80, 110); // Slow and ponderous
            SetInt(400, 480);

            SetHits(1800, 2200); // Very high health
            SetStam(150, 200); // Moderate stamina
            SetMana(400, 480); // Large mana pool for abilities

            SetDamage(25, 32); // High base damage

            // Primarily Fire damage
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 90, 100); // Extremely high fire resist
            SetResistance(ResistanceType.Cold, -20, -5); // Vulnerable to Cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 105.1, 120.0);
            SetSkill(SkillName.Magery, 105.1, 120.0); // For abilities and potential spell use
            SetSkill(SkillName.MagicResist, 115.2, 130.0); // High magic resist
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);
            SetSkill(SkillName.Anatomy, 60.0, 80.0); // Lower anatomy

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80; // High natural armor
            ControlSlots = 5; // Very difficult to control (likely untamable by design)
            MinTameSkill = 120.0; // Set high if tamable needed, otherwise remove Tamable=true below

            // Tamable = false; // Uncomment if it should NOT be tamable

            // Initialize ability cooldowns with stagger
            m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextAuraCheckTime = DateTime.UtcNow + TimeSpan.FromSeconds(5); // Check aura relatively often
            m_LastLocation = this.Location;

            // Pack appropriate loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(8, 15)));
            PackItem(new ObsidianShard(Utility.RandomMinMax(1, 3))); // Custom item or placeholder
            PackItem(new HotRock(Utility.RandomMinMax(5,10))); // Just flavor item maybe
        }

        // --- Movement -> Leave Lava Puddles ---
        public override void OnThink()
        {
            base.OnThink();

            // Check if moved and leave a puddle
            if (this.Alive && this.Map != null && this.Map != Map.Internal && this.Location != m_LastLocation)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Small chance to leave a puddle on movement
                if (Utility.RandomDouble() < 0.25) // 25% chance per move step
                {
                    LeaveLavaPuddle(oldLocation);
                }
            }

            // Ability Checks
            if (Combatant == null || Map == null || Map == Map.Internal || !this.Alive)
                return;

            // Check Heat Aura
            if (DateTime.UtcNow >= m_NextAuraCheckTime)
            {
                ApplyHeatAura();
                m_NextAuraCheckTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(4, 6));
            }

            // Check Molten Spit (ranged)
            if (DateTime.UtcNow >= m_NextSpitTime && this.InRange(Combatant.Location, 10))
            {
                 // Check Line of Sight before attempting ranged attack
                 if (this.InLOS(Combatant))
                 {
                    MoltenSpitAttack();
                    m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
                 }
                 else
                 {
                     // Target not in LOS, maybe try again sooner
                     m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(2);
                 }
            }
            // Check Lava Eruption (close range AoE)
            else if (DateTime.UtcNow >= m_NextEruptionTime && this.InRange(Combatant.Location, 5)) // Shorter range for eruption
            {
                LavaEruptionAttack();
                m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // Helper to create a lava puddle
        public void LeaveLavaPuddle(Point3D location)
        {
             if (!this.Alive || this.Map == null || this.Map == Map.Internal) return;

             // Ensure the location is valid on the map
             Map map = this.Map;
             if (!map.CanFit(location.X, location.Y, location.Z, 16, false, false))
             {
                location.Z = map.GetAverageZ(location.X, location.Y);
                if (!map.CanFit(location.X, location.Y, location.Z, 16, false, false))
                    return; // Still can't fit, give up
             }

             TemporaryLavaPuddle puddle = new TemporaryLavaPuddle(this);
             puddle.MoveToWorld(location, map);

             // Optional: Small visual effect when puddle appears
             Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x3709, 5, 10, UniqueHue, 0, 5016, 0); // Small burst
        }


        // --- Unique Ability: Molten Spit ---
        public void MoltenSpitAttack()
        {
            if (Combatant == null || Map == null || !this.Alive) return;

            // *** IMPORTANT: Check if Combatant is a Mobile ***
            if (Combatant is Mobile target)
            {
                if (!CanBeHarmful(target) || !InLOS(target)) return; // Check harm/LOS again

                this.Say("*Hsssss-splort!*"); // Lava spit sound/flavor
                this.MovingEffect(target, 0x36E4, 10, 0, false, false, UniqueHue, 0); // Molten rock projectile effect
                this.PlaySound(0x160); // Maybe meteor impact sound or a custom sound

                // Delay damage to match projectile travel time (adjust as needed)
                Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
                {
                    if (this.Alive && target.Alive && CanBeHarmful(target, false)) // Re-check validity
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(35, 50); // Good single target damage
                        // Deal 20% physical, 80% fire damage
                        AOS.Damage(target, this, damage, 20, 80, 0, 0, 0);

                        // Apply Stamina Drain effect
                        target.SendMessage("The molten rock saps your strength!");
                        target.Stam -= Utility.RandomMinMax(20, 40); // Drain stamina
                        target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, (int)EffectLayer.Head);

                    }
                });
            }
        }

        // --- Unique Ability: Lava Eruption ---
        public void LavaEruptionAttack()
        {
            if (Map == null || !this.Alive) return;

            this.PlaySound(BaseSoundID); // Use its base sound, maybe pitched?
            this.PlaySound(0x218); // Explosion sound
            this.Say("*GROOOWL-BOOM!*");

            // Large fiery/lava explosion effect on self
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4); // 4 tile radius AoE

            foreach (Mobile m in eable)
            {
                // Check if the mobile can be harmed and is not the caster
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(55, 75); // High AoE damage
                    // Deal 10% physical, 90% fire damage
                    AOS.Damage(target, this, damage, 10, 90, 0, 0, 0);

                    // Add a visual effect on the target
                    target.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

                    target.PlaySound(0x208); // Fire impact sound

                    // Optional: Short stun/daze effect (if desired)
                    // target.Paralyze(TimeSpan.FromSeconds(0.5)); // Very short interrupt
                    // target.SendMessage("You are momentarily stunned by the eruption!");
                }
            }
        }

        // --- Unique Ability: Heat Aura ---
        public void ApplyHeatAura()
        {
            if (Map == null || !this.Alive) return;

            // Check for nearby targets for the aura effect
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3); // 3 tile radius aura
            bool appliedEffect = false;

            foreach (Mobile m in eable)
            {
                 // *** IMPORTANT: Check if m is a Mobile ***
                if (m != this && m is Mobile targetMobile && targetMobile.Alive && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    // Apply small fire damage and maybe Stamina/Mana drain
                    DoHarmful(targetMobile);
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0); // Small fire damage
                    targetMobile.Stam -= Utility.RandomMinMax(5, 10); // Drain small amount of Stamina
                    // targetMobile.Mana -= Utility.RandomMinMax(3, 6); // Optional Mana drain

                    // Visual effect for the aura hitting someone
                    targetMobile.FixedParticles(0x374A, 1, 15, 5013, 1153, 0, EffectLayer.Waist); // Heat distortion/fire effect
                    appliedEffect = true;
                }
            }
            eable.Free();

            if (appliedEffect)
            {
                this.PlaySound(0x22F); // Sizzle sound for aura
            }
        }

        // --- Death Effect: Spawn Lava Puddles ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.PlaySound(0x218); // Death explosion sound
            // Large central death explosion visual
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 80, UniqueHue, 0, 5052, 0);

            int puddlesToSpawn = Utility.RandomMinMax(5, 8); // Number of puddles on death
            for (int i = 0; i < puddlesToSpawn; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D spawnLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                // Use the helper to spawn puddles, it handles Z checks
                LeaveLavaPuddle(spawnLocation);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override int TreasureMapLevel { get { return 5; } } // High-level map drop

        public override double DispelDifficulty { get { return 140.0; } } // Very hard to dispel
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2); // Generous loot
            AddLoot(LootPack.HighScrolls, 1); // Chance for high scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            // Chance for unique items
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                PackItem(new MagmaTouchedRing()); // Example Unique Item 1
            }
             if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                 PackItem(new ObsidianScalemailFragment()); // Example Unique Item 2
            }
        }

        // --- Serialization ---
        public LivingLava(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // No need to save timers if we re-initialize on load
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize cooldowns on load/restart
            m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextAuraCheckTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5));
            m_LastLocation = this.Location; // Initialize last location
        }
    }

    // --- Placeholder/Example Unique Items ---
    // Define these properly elsewhere if needed
    public class ObsidianShard : Item { public ObsidianShard() : this(1) {} [Constructable] public ObsidianShard(int amount) : base(0xF8F) { Stackable = true; Amount = amount; Name = "Obsidian Shard"; Hue = 1109;} public ObsidianShard(Serial serial) : base(serial) { } public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); } }
    public class HotRock : Item { public HotRock() : this(1) {} [Constructable] public HotRock(int amount) : base(0x1363) { Stackable = true; Amount = amount; Name = "Porous Lava Rock"; Hue = 1175;} public HotRock(Serial serial) : base(serial) { } public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); } }
    public class MagmaTouchedRing : GoldRing { [Constructable] public MagmaTouchedRing() { Name = "Magma-Touched Ring"; Hue = 2118; Attributes.RegenHits = 2; Attributes.BonusStr = 5; Resistances.Fire = 10; } public MagmaTouchedRing(Serial serial) : base(serial) { } public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); } }
    public class ObsidianScalemailFragment : Item { [Constructable] public ObsidianScalemailFragment() : base(0x1B7B) { Name = "Obsidian Scalemail Fragment"; Hue = 1109; Weight = 5.0;} public ObsidianScalemailFragment(Serial serial) : base(serial) { } public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); } } // Just a deco/craft item?
}