using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // For SpellHelper, effects
using Server.Spells.Fourth; // Needed for FireField on death (example)

namespace Server.Mobiles
{
    [CorpseName("a searing dolphin husk")] // Thematic corpse name
    public class LavaDolphin : BaseCreature
    {
        // --- Private Fields for Ability Cooldowns ---
        private DateTime m_NextSteamBreath;
        private DateTime m_NextEruptingLeap;
        private DateTime m_NextBoilingAuraTick;

        // --- Constants ---
        private const int UniqueHue = 1260; // Fiery Orange/Red hue
        private const int AuraRange = 3;     // Range of the boiling aura in tiles
        private const int AuraDamage = 5;    // Damage per tick of the boiling aura

        [Constructable]
        public LavaDolphin() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Uses Mage AI for ability logic, slightly faster reaction/move speed
        {
            Name = "a Lava Dolphin";
            Body = 0x97; // Dolphin body shape
            BaseSoundID = 357; // Changed from dolphin sound to a more fiery/hissing sound
            Hue = UniqueHue;

            // --- Stats (Significantly boosted from base Dolphin) ---
            SetStr(250, 300);
            SetDex(150, 200); // Keep decent Dex for agility theme
            SetInt(300, 350); // High Int for ability power/mana pool

            SetHits(800, 1000); // Tougher health pool
            SetStam(150, 200); // Stamina for leaping/movement
            SetMana(300, 350); // Mana pool for abilities

            SetDamage(18, 24); // Increased base damage

            // Damage Type: Primarily Fire
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Fire, 85);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 75, 85); // High Fire resistance
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to Cold damage
            SetResistance(ResistanceType.Poison, 40, 50); // Moderate Poison resistance
            SetResistance(ResistanceType.Energy, 40, 50); // Moderate Energy resistance

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 95.1, 110.0);
            SetSkill(SkillName.Magery, 95.1, 110.0); // Useful for AI logic even without casting spells
            SetSkill(SkillName.MagicResist, 100.1, 115.0);
            SetSkill(SkillName.Tactics, 90.1, 105.0);
            SetSkill(SkillName.Wrestling, 90.1, 105.0);
            SetSkill(SkillName.Anatomy, 80.0, 95.0); // Bonus skill

            Fame = 15000; // High fame reward
            Karma = -15000; // Evil creature

            VirtualArmor = 60; // Good passive defense
            ControlSlots = 4; // Difficult to control/tame

            // --- Initialize Ability Cooldowns ---
            m_NextSteamBreath = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)); // Stagger initial use
            m_NextEruptingLeap = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Stagger initial use
            m_NextBoilingAuraTick = DateTime.UtcNow + TimeSpan.FromSeconds(2); // Aura starts ticking soon

            // --- Creature Properties ---
            CanSwim = true; // Can swim if it encounters water
            CantWalk = false; // IMPORTANT: MUST be able to move on land in Gate of Hell

            // --- Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(4, 8))); // Fire reagent
            PackItem(new DaemonBone(Utility.RandomMinMax(1, 3))); // Rare reagent
        }

        // --- Base Properties Overrides ---
        public override bool BleedImmune { get { return true; } } // Immune to bleed effects
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to all poisons
        public override int TreasureMapLevel { get { return 4; } } // Drops decent treasure maps


        // --- Ability Trigger Logic ---
        public override void OnThink()
        {
            base.OnThink();

            // Don't process abilities if dead, no target, or in internal map
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // --- Boiling Blood Aura Check ---
            // Periodically damages nearby enemies
            if (DateTime.UtcNow >= m_NextBoilingAuraTick)
            {
                ApplyBoilingAura();
                m_NextBoilingAuraTick = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)); // Re-apply aura damage every 2-4 seconds
            }

            // --- Special Ability Checks ---
            // Prioritize Erupting Leap if available and target is in range
            if (DateTime.UtcNow >= m_NextEruptingLeap && InRange(Combatant.Location, 10) && CanUseSpecialAbility(100)) // Check range and mana
            {
                DoEruptingLeap();
                m_NextEruptingLeap = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Set cooldown
            }
            // Otherwise, check for Scalding Steam Breath
            else if (DateTime.UtcNow >= m_NextSteamBreath && InRange(Combatant.Location, 6) && this.InLOS(Combatant) && CanUseSpecialAbility(50))
            {
                DoSteamBreath();
                m_NextSteamBreath = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14)); // Set cooldown
            }
        }

        // Helper function to check and consume mana for abilities
        private bool CanUseSpecialAbility(int manaCost)
        {
            if (this.Mana >= manaCost)
            {
                this.Mana -= manaCost;
                return true;
            }
            return false;
        }

        #region Unique Abilities

        // --- Ability 1: Boiling Blood (Passive Aura) ---
        public void ApplyBoilingAura()
        {
            if (Map == null || Map == Map.Internal) return; // Safety check

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, AuraRange);

            bool playedEffect = false;
            foreach (Mobile m in eable)
            {
                // Check if the mobile is a valid target (not self, hostile, alive)
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                    // Play sound/effect only once per tick if valid targets exist
                    if (!playedEffect)
                    {
                        PlaySound(0x108); // Sizzling sound effect
                        // Optional subtle visual:
                        // Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3789, 10, 5, UniqueHue, 0, 5029, 0);
                        playedEffect = true;
                    }
                }
            }
            eable.Free(); // Release mobile collection

            // Damage all valid targets found in range
            foreach (Mobile target in targets)
            {
                DoHarmful(target); // Mark as harmful action
                AOS.Damage(target, this, AuraDamage, 0, 100, 0, 0, 0); // Deal 100% Fire damage
                target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Small steam/heat effect on target
            }
        }

        // --- Ability 2: Scalding Steam Breath ---
        public void DoSteamBreath()
        {
            if (Combatant == null) return;

            // *** Check if Combatant is Mobile ***
            if (Combatant is Mobile target)
            {
                // Send a particle effect from dolphin towards the target
                this.MovingParticles(target, 0x36D4, 7, 0, false, true, UniqueHue, 0, 9502, 4019, 0, 0); // Steam/gas graphic
                this.PlaySound(0x230); // Hissing/steam sound

                // Use a timer to delay damage, syncing with particle travel time
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    // Re-check validity inside the timer callback
                    if (!Alive || target == null || !target.Alive || !InRange(target.Location, 8)) return;

                    int damage = Utility.RandomMinMax(35, 50);
                    DoHarmful(target);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0); // Deal 100% Fire damage

                    // Optional: Apply a short stamina debuff for flavor
                    target.SendMessage(0x25, "You are choked by scalding steam!"); // Red system message
                    target.Stam -= Utility.RandomMinMax(15, 25); // Reduce target's stamina

                    // Visual effect on impact
                    target.FixedParticles(0x374A, 10, 30, 5038, EffectLayer.Head); // Steam impact effect
                });
            }
            // else: Combatant is not a Mobile (e.g., an item or static object), do nothing.
        }

        // --- Ability 3: Erupting Leap ---
        public void DoEruptingLeap()
        {
            if (Combatant == null || Map == null) return;

            // *** Check if Combatant is Mobile ***
            if (Combatant is Mobile target)
            {
                Point3D targetLoc = target.Location;
                Map map = Map;

                // Check if target is valid and location is accessible
                if (!CanBeHarmful(target, false) || !map.CanFit(targetLoc.X, targetLoc.Y, targetLoc.Z, 16, false, false))
                {
                    // Abort if target is invalid or location blocked (can add logic later to find nearby spot)
                    return;
                }

                Say("*FWOOSH!*"); // Flavor text
                PlaySound(0x5C8); // Fiery woosh/leap sound

                // Visual effect at the starting position
                Point3D startLoc = Location;
                Effects.SendLocationParticles(EffectItem.Create(startLoc, map, EffectItem.DefaultDuration), 0x3728, 10, 20, UniqueHue, 0, 5044, 0); // Fire burst effect

                // Use one of the Dolphin's jump animations
                if (Utility.RandomBool())
                    this.Animate(3, 16, 1, true, false, 0);
                else
                    this.Animate(4, 20, 1, true, false, 0);

                // Teleport the creature to the target location (can adjust timing if needed)
                this.Location = targetLoc;
                this.ProcessDelta(); // Ensure position updates visually

                // Delay the impact effects slightly to simulate landing
                Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
                {
                    // Re-check validity inside timer
                    if (!Alive || Map == null || Map == Map.Internal) return;

                    // Impact sound and visual effect at landing spot
                    PlaySound(0x208); // Explosion sound
                    Effects.SendLocationParticles(EffectItem.Create(targetLoc, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Flamestrike-like effect

                    // --- Area of Effect Damage on Landing ---
                    List<Mobile> impactedTargets = new List<Mobile>();
                    IPooledEnumerable eable = map.GetMobilesInRange(targetLoc, 3); // 3 tile impact radius

                    foreach (Mobile m in eable)
                    {
                        if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            impactedTargets.Add(m);
                        }
                    }
                    eable.Free(); // Release mobile collection

                    // Damage all valid targets in the impact zone
                    foreach (Mobile victim in impactedTargets)
                    {
                        DoHarmful(victim);
                        int damage = Utility.RandomMinMax(50, 75); // High damage for the signature move
                        AOS.Damage(victim, this, damage, 0, 100, 0, 0, 0); // 100% Fire damage
                        victim.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head); // Fiery hit visual on victims
                    }
                });
            }
            // else: Combatant is not a Mobile, do nothing.
        }

        #endregion

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich); // Good gold/magic item chance
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls); // Chance for medium level scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6)); // Drop gems

            // Chance for unique crafting resource
            if (Utility.RandomDouble() < 0.10) // 10% chance
            {
                PackItem(new LavaDolphinHide()); // Pack the unique hide
            }
        }

        // --- Death Effects ---
        public override void OnDeath(Container c)
        {
            // Optional: Add a small visual/sound effect on death
            if (Map != null && Map != Map.Internal)
            {
                // Basic fiery explosion on death
                Effects.PlaySound(Location, Map, 0x208); // Explosion sound
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 50, UniqueHue, 0, 5016, 0); // Large fire visual

                // Leave a few temporary damaging fire fields (using FireField as example)
                for (int i = 0; i < Utility.RandomMinMax(2, 4); i++)
                {
                    Point3D spot = new Point3D(Location.X + Utility.RandomMinMax(-1, 1), Location.Y + Utility.RandomMinMax(-1, 1), Location.Z);
                    if (Map.CanFit(spot.X, spot.Y, spot.Z, 16, false, false)) // Check if the spot is valid
                    {
                        VortexTile field = new VortexTile(); //Duration 10-15s, Damage 3-6 per tick
                        field.Hue = UniqueHue; // Match the dolphin's hue
                    }
                }
            }
            base.OnDeath(c); // Perform standard death procedures
        }

        // --- Serialization ---
        public LavaDolphin(Serial serial) : base(serial)
        {
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

            // Re-initialize cooldowns on server load/restart
            m_NextSteamBreath = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextEruptingLeap = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoilingAuraTick = DateTime.UtcNow + TimeSpan.FromSeconds(2);

            // Ensure CantWalk is false after loading, in case it was saved true previously
            if (CantWalk)
                CantWalk = false;
        }
    }

    // --- Unique Loot Item Definition ---
    // Basic example of the unique hide resource. Can be expanded for crafting systems.
    public class LavaDolphinHide : Item
    {
        [Constructable]
        public LavaDolphinHide() : this(1)
        {
        }

        [Constructable]
        public LavaDolphinHide(int amount) : base(0x1081) // Using graphical ID for standard leather/hides
        {
            Name = "Lava Dolphin Hide";
            Stackable = true;
            Amount = amount;
            Weight = 1.0;
            Hue = 1260; // Match the dolphin's hue
            // Add Item Rarity or other properties if your shard uses them
        }

        public LavaDolphinHide(Serial serial) : base(serial)
        {
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
        }
    }
}