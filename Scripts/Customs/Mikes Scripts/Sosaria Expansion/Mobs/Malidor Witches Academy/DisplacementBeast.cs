using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a warped panther corpse")] // Evocative name
    public class DisplacementBeast : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextPhaseShiftTime;
        private DateTime m_NextMirrorImageTime;
        private DateTime m_NextWarpRealityTime;
        private DateTime m_NextPhaseStrikeTime;
        private Point3D m_LastLocation;

        // Unique Hue - Example: 1157 is a shimmering light blue/purple
        private const int UniqueHue = 1157;

        [Constructable]
        public DisplacementBeast() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.15, 0.3) // Slightly faster reaction
        {
            Name = "a Displacer Beast";
            Body = 0xD6; // Panther body
            BaseSoundID = 0x462; // Panther sound
            Hue = UniqueHue;

            // --- Enhanced Stats - Dexterity & Intelligence Focused ---
            SetStr(350, 450);    // Good strength
            SetDex(400, 500);    // Very High Dexterity for evasion/speed
            SetInt(450, 550);    // High Intelligence for magic

            SetHits(1200, 1500); // High survivability
            SetStam(300, 400);   // Large stamina pool
            SetMana(500, 650);   // Large mana pool

            SetDamage(18, 24);   // Good physical damage

            // Mix of Physical and Energy damage
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30); // Displacement energy

            // --- Adjusted Resistances - Strong vs Physical/Energy ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 35, 45); // Slightly weaker vs Fire
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 35, 45); // Slightly weaker vs Poison
            SetResistance(ResistanceType.Energy, 60, 70); // Strong energy resist

            // --- Enhanced Skills - Focus on Evasion & Magic ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.1, 125.0); // Very high resist skill
            SetSkill(SkillName.Meditation, 90.0, 100.0); // Good mana regen
            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.Wrestling, 100.1, 115.0);
            SetSkill(SkillName.Anatomy, 90.1, 100.0); // For damage output

            Fame = 18000; // High fame boss
            Karma = -18000; // Very evil

            VirtualArmor = 75; // High passive defense due to displacement
            ControlSlots = 4; // Challenging creature, not quite top-tier boss

            // Initialize ability cooldowns
            m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMirrorImageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextWarpRealityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextPhaseStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));

            // Loot: Reagents and potential unique drop
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15))); // Illusion component

            m_LastLocation = this.Location;
        }

        // --- Movement Effect: Unstable Trail ---
        // Leaves short-lived hazardous tiles behind
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
             // We don't want the beast itself triggering its own trail while pathing
             if (m == this)
             {
                 if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.20) // 20% chance per move tick
                 {
                    Point3D trailLoc = m_LastLocation; // Leave tile where it WAS
                    m_LastLocation = this.Location; // Update current location

                    // Check if the tile can be placed
                    if (Map.CanFit(trailLoc.X, trailLoc.Y, trailLoc.Z, 16, false, false))
                    {
                        // Use ManaDrainTile or VortexTile for a damaging/draining effect
                        ManaDrainTile hazard = new ManaDrainTile();
                        hazard.Hue = UniqueHue; // Match beast's hue
                        hazard.MoveToWorld(trailLoc, this.Map);
                        Effects.PlaySound(trailLoc, this.Map, 0x1F0); // Fizzle sound
                    }
                    else
                    {
                        int validZ = Map.GetAverageZ(trailLoc.X, trailLoc.Y);
                         if (Map.CanFit(trailLoc.X, trailLoc.Y, validZ, 16, false, false))
                         {
                            ManaDrainTile hazard = new ManaDrainTile();
                            hazard.Hue = UniqueHue;
                            hazard.MoveToWorld(new Point3D(trailLoc.X, trailLoc.Y, validZ), this.Map);
                            Effects.PlaySound(trailLoc, this.Map, 0x1F0); // Fizzle sound
                         }
                    }
                }
                else
                {
                    m_LastLocation = this.Location; // Update location even if no tile dropped
                }
            }

            base.OnMovement(m, oldLocation);
        }


        // --- Thinking Process for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and situation
            if (DateTime.UtcNow >= m_NextMirrorImageTime)
            {
                MirrorImages();
                m_NextMirrorImageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextWarpRealityTime && this.GetDistanceToSqrt(Combatant) < 8) // Use when target is close
            {
                 WarpReality();
                 m_NextWarpRealityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextPhaseShiftTime)
            {
                PhaseShift();
                m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 16));
            }

            // Phase Strike check (modifies next attack)
            if (DateTime.UtcNow >= m_NextPhaseStrikeTime && this.GetDistanceToSqrt(Combatant) <= 2) // Only prepare when close
            {
                 // Just set a flag or modify next hit directly if possible in your system
                 // For simplicity here, we'll just trigger the effect/sound
                 this.FixedParticles(0x375A, 10, 15, 5037, UniqueHue, 0, EffectLayer.Waist); // Shimmer effect
                 this.PlaySound(0x51D); // Phasing sound
                 m_NextPhaseStrikeReady = true; // Flag that next hit is special
                 m_NextPhaseStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(7, 12)); // Reset cooldown
            }
        }

        private bool m_NextPhaseStrikeReady = false;

        // Enhance melee hit if Phase Strike is ready
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (m_NextPhaseStrikeReady)
            {
                m_NextPhaseStrikeReady = false; // Consume the charge

                // Check if defender is a Mobile before accessing properties
                if (defender is Mobile targetMobile)
                {
                    this.Say("*Phases through guard*"); // Flavor
                    targetMobile.FixedParticles(0x374A, 10, 15, 5039, UniqueHue, 0, EffectLayer.Head); // Hit effect
                    targetMobile.PlaySound(0x1FB); // Energy hit sound

                    // Add bonus energy damage (simulates bypassing some defense)
                    int bonusDamage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(targetMobile, this, bonusDamage, 0, 0, 0, 0, 100); // Pure Energy bonus
                }
                // Else: If defender is not Mobile, we can't apply bonus effects/damage easily here.
                // Depending on IDamageable implementation, a direct damage call might still work if needed.
                // AOS.Damage(defender, this, bonusDamage, 0, 0, 0, 0, 100); // Try direct damage if possible
            }
        }

		public static int GetDistance(Point3D p1, Point3D p2)
		{
			int dx = p1.X - p2.X;
			int dy = p1.Y - p2.Y;
			return (int)Math.Sqrt(dx * dx + dy * dy);
		}

        // --- Unique Ability: Phase Shift (Short Teleport) ---
        public void PhaseShift()
        {
            if (Map == null) return;

            this.Say("*Which is real?*"); // Flavor text
            this.PlaySound(0x1F3); // Summon sound

            int count = Utility.RandomMinMax(2, 3); // Summon 2 or 3 images

            for (int i = 0; i < count; ++i)
            {
                Point3D spawnLoc = this.Location;
                bool success = false;
                for(int attempt = 0; attempt < 10; ++attempt) // Try to find nearby spot
                {
                    int x = this.X + Utility.RandomMinMax(-2, 2);
                    int y = this.Y + Utility.RandomMinMax(-2, 2);
                    int z = this.Z;
					int avgZ = 0, top = 0, bottom = 0;
					this.Map.GetAverageZ(x, y, ref avgZ, ref top, ref bottom);
					z = avgZ; // ← no `int` keyword here
                    Point3D tryLoc = new Point3D(x, y, z);
                    if(Map.CanSpawnMobile(tryLoc))
                    {
                        spawnLoc = tryLoc;
                        success = true;
                        break;
                    }
                }

                if(success)
                {
                    DisplacerIllusion illusion = new DisplacerIllusion(this); // Pass self as master
                    illusion.MoveToWorld(spawnLoc, this.Map);
                    // Small puff effect on spawn
                    Effects.SendLocationParticles(EffectItem.Create(spawnLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 5, UniqueHue, 0, 5023, 0);
                }
            }
        }

        // --- Unique Ability: Mirror Images (Summon Illusions) ---
        public void MirrorImages()
        {
            if (Map == null) return;

            this.Say("*Which is real?*"); // Flavor text
            this.PlaySound(0x1F3); // Summon sound

            int count = Utility.RandomMinMax(2, 3); // Summon 2 or 3 images

            for (int i = 0; i < count; ++i)
            {
                Point3D spawnLoc = this.Location;
                bool success = false;
                for(int attempt = 0; attempt < 10; ++attempt) // Try to find nearby spot
                {
                    int x = this.X + Utility.RandomMinMax(-2, 2);
                    int y = this.Y + Utility.RandomMinMax(-2, 2);
                    int z = this.Z;
					int avgZ = 0, top = 0, bottom = 0;
					this.Map.GetAverageZ(x, y, ref avgZ, ref top, ref bottom);
					z = avgZ; // ← no `int` keyword here
                    Point3D tryLoc = new Point3D(x, y, z);
                    if(Map.CanSpawnMobile(tryLoc))
                    {
                        spawnLoc = tryLoc;
                        success = true;
                        break;
                    }
                }

                if(success)
                {
                    DisplacerIllusion illusion = new DisplacerIllusion(this); // Pass self as master
                    illusion.MoveToWorld(spawnLoc, this.Map);
                    // Small puff effect on spawn
                    Effects.SendLocationParticles(EffectItem.Create(spawnLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 5, UniqueHue, 0, 5023, 0);
                }
            }
        }


        // --- Unique Ability: Warp Reality (AoE Debuff - HCI/DCI/Stam Drain) ---
        public void WarpReality()
        {
            if (Map == null) return;

            this.Say("*Reality bends!*");
            this.PlaySound(0x2F4); // Distortion sound
            this.FixedParticles(0x37B9, 10, 25, 5032, UniqueHue, 0, EffectLayer.Waist); // Rippling field effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

            foreach (Mobile m in eable)
            {
                // Check if target is Mobile before accessing Mobile properties
                if (m is Mobile targetMobile && m != this && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    targets.Add(targetMobile);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    // Visual effect on target
                    target.FixedParticles(0x376A, 9, 32, 5032, UniqueHue, 0, EffectLayer.Head); // Warping effect on target

                    // Apply debuff (example: temporary Hit Chance / Defense Chance reduction)
                    // You might need a custom timed stat mod system for this
                    int reduction = Utility.RandomMinMax(10, 20); // Reduce by 10-20%
                    TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));

                    // --- Placeholder for Stat Mod ---
                    // StatMod modHCI = new StatMod(StatType.HitChance, "WarpHCI", -reduction, duration);
                    // StatMod modDCI = new StatMod(StatType.DefendChance, "WarpDCI", -reduction, duration);
                    // target.AddStatMod(modHCI);
                    // target.AddStatMod(modDCI);
                    // --- End Placeholder ---
                    target.SendMessage(0x22, "Reality warps around you, hindering your movements!");

                    // Stamina Drain
                    int stamDrained = Utility.RandomMinMax(20, 40);
                    if (target.Stam >= stamDrained)
                    {
                        target.Stam -= stamDrained;
                        target.SendMessage(0x22, "The distortion saps your energy!");
                        target.PlaySound(0x1DF); // Stam drain sound?
                    }
                }
            }
        }


        // --- Death Effect: Final Distortion ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The illusion... breaks!*");
            // Large central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x211); // Magic explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion

            // AoE Damage and Confusion effect
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius AoE

            foreach (Mobile m in eable)
            {
                 // Check if target is Mobile before accessing Mobile properties
                if (m is Mobile targetMobile && m != this && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    targets.Add(targetMobile);
                }
            }
            eable.Free();

             if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                     DoHarmful(target);
                     int damage = Utility.RandomMinMax(40, 60); // Moderate death explosion damage
                     // Deal 50% Energy, 50% Physical damage
                     AOS.Damage(target, this, damage, 50, 0, 0, 0, 50);

                     // Confusion effect (e.g., short screen shake or temporary stat reduction)
                     target.SendMessage(0x35, "You are disoriented by the breaking illusion!");
                     target.FixedParticles(0x376A, 9, 32, 5032, UniqueHue, 0, EffectLayer.Head); // Warp effect
                     target.PlaySound(0x2F4); // Distortion sound

                     // --- Placeholder for Confusion/Stat Debuff ---
                     // Could apply a short duration Str/Int debuff here
                     // TimeSpan confusionDuration = TimeSpan.FromSeconds(5);
                     // target.AddStatMod(new StatMod(StatType.Str, "ConfusionStr", -15, confusionDuration));
                     // target.AddStatMod(new StatMod(StatType.Int, "ConfusionInt", -15, confusionDuration));
                     // --- End Placeholder ---
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return false; } } // Can bleed
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } } // From Panther
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } } // From Panther
        public override int Hides { get { return 10; } } // From Panther, maybe reduce?
        public override int Meat { get { return 1; } } // From Panther

        // Increased Dispel difficulty due to magical nature
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1); // Good base loot
            AddLoot(LootPack.Rich, 1);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2)); // Mid-level scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // Good gems

            // Chance for Shadow Panther Cloak (slightly better than base panther)
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                PackItem(new ShadowPantherCloak());
            }

            // Chance for a unique thematic item
            if (Utility.RandomDouble() < 0.015) // 1.5% chance
            {
                // Example: Replace with an actual item defined elsewhere
                 // PackItem(new PhaseShiftBoots());
                 PackItem( new MaxxiaScroll(1) ); // Placeholder unique drop
            }
             if (Utility.RandomDouble() < 0.05) // 5% chance for Shifting Essence (rare reagent/resource)
            {
                 PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3))); // Placeholder rare resource
            }
        }

        // --- Serialization ---
        public DisplacementBeast(Serial serial) : base(serial)
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

            // Re-initialize timers on load/restart
            m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMirrorImageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextWarpRealityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextPhaseStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
        }
    }

    // --- Helper Mobile: Displacer Illusion ---
    public class DisplacerIllusion : BaseCreature
    {
        private DisplacementBeast m_Owner;
        private DateTime m_ExpireTime;

        // Use the same hue as the master
        private const int IllusionHue = 1157;

        public DisplacerIllusion(DisplacementBeast owner) : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Owner = owner;

            Name = "a displacer illusion";
            Body = 0xD6; // Panther body
            BaseSoundID = 0x462; // Panther sound (maybe silence it? Or use a poof sound?)
            Hue = IllusionHue; // Match owner's hue

            // Very low stats, mainly visual distraction
            SetStr(10);
            SetDex(50);
            SetInt(10);

            SetHits(30); // Very fragile
            SetStam(50);
            SetMana(0);

            SetDamage(1, 2); // Almost no damage

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10);
            SetResistance(ResistanceType.Energy, 10); // Minimal resistances

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 10;
            ControlSlots = 0; // Not controllable
            Tamable = false;

            m_ExpireTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12)); // Lasts 8-12 seconds
        }

        public override bool IsEnemy(Mobile m)
        {
            // Illusions should try to attack the same target as their master
            if (m_Owner != null && m_Owner.Combatant == m)
                return true;

            // Illusions are generally hostile to players/non-allies of the owner
            if (m_Owner != null && m is PlayerMobile && m_Owner.CanBeHarmful(m, false))
                 return true;

            return base.IsEnemy(m); // Default hostility checks
        }

         public override void OnThink()
        {
            base.OnThink();

            // Expire check
            if (DateTime.UtcNow >= m_ExpireTime)
            {
                 Expire();
                 return;
            }

             // If owner is dead or gone, expire
            if (m_Owner == null || m_Owner.Deleted || !m_Owner.Alive)
            {
                Expire();
                return;
            }

            // Try to mimic owner's target
            if (m_Owner.Combatant != null && this.Combatant != m_Owner.Combatant)
            {
                 this.Combatant = m_Owner.Combatant;
            }
        }

         public override void OnGotMeleeAttack(Mobile attacker)
        {
            // Poof when hit
            Expire();
            base.OnGotMeleeAttack(attacker);
        }

         public override void OnDamagedBySpell(Mobile caster)
        {
             // Poof when hit by spell
            Expire();
            base.OnDamagedBySpell(caster);
        }


        public void Expire()
        {
             if (this.Map != null)
            {
                // Small explosion effect on death/expire
                Effects.PlaySound(this.Location, this.Map, 0x1F0); // Fizzle sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 5, IllusionHue, 0, 5023, 0); // Small puff

                 // Optional: Minor debuff to nearby players on expiration?
                 /*
                 IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 1);
                 foreach (Mobile m in eable)
                 {
                     if (m is PlayerMobile && m_Owner != null && m_Owner.CanBeHarmful(m, false))
                     {
                        // Apply a very short duration minor debuff (e.g., -5 Int for 3 seconds)
                        m.SendMessage(0x22, "The dissipating illusion leaves you momentarily confused.");
                        // StatMod placeholder:
                        // m.AddStatMod(new StatMod(StatType.Int, "IllusionFade", -5, TimeSpan.FromSeconds(3)));
                     }
                 }
                 eable.Free();
                 */
            }
            this.Delete(); // Remove the illusion
        }

        public override void GenerateLoot() { } // No loot
        public override bool DeleteCorpseOnDeath { get { return true; } } // No corpse

        public DisplacerIllusion(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Owner);
            writer.WriteDeltaTime(m_ExpireTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile() as DisplacementBeast;
            m_ExpireTime = reader.ReadDeltaTime();

            // If owner is gone or timer expired on load, delete immediately
            if(m_Owner == null || DateTime.UtcNow >= m_ExpireTime)
                 Expire(); // Use Expire to handle effects/deletion properly
                 //this.Delete();
        }
    }
}