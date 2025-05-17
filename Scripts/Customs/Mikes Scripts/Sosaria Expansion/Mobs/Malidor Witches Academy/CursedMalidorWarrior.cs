using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Second; // For Curse spell effect
using Server.Spells.First; // For Weaken/Feeblemind potentially

namespace Server.Mobiles
{
    [CorpseName("a cursed warrior's remains")] // More thematic corpse name
    public class CursedMalidorWarrior : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextEruptionTime;
        private DateTime m_NextMaliceTime;
        private DateTime m_NextAuraCheck; // Timer for aura application/refresh

        // Unique Hue - Example: 1157 is a deep, sickly purple/grey.
        public const int UniqueHue = 1157;

        [Constructable]
        public CursedMalidorWarrior() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction/aggression
        {
            Name = "a Cursed Malidor Warrior";
            Body = 771; // Base body from MeerWarrior
            Hue = UniqueHue;
            BaseSoundID = -1; // Disable default idle sound if desired, or leave default Meer sound

            // --- Significantly Boosted Stats - Melee Focus with Magical Corruption ---
            SetStr(450, 550);    // High strength for melee
            SetDex(200, 250);    // Good agility
            SetInt(350, 450);    // High Int for mana/ability scaling/resist

            SetHits(1500, 1800); // Very tough
            SetStam(200, 250);   // Good stamina for melee
            SetMana(400, 500);   // Large mana pool for abilities

            SetDamage(20, 28);   // Strong physical damage

            // Mixed damage reflecting physical prowess and magical corruption
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30); // Corrupted magic damage

            // --- Adjusted Resistances - Tough but with a weak point ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 35, 45); // Slightly weaker vs Cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 65, 75); // Strong vs Energy

            // --- Enhanced Skills - Focus on Combat & Magic Resist ---
            SetSkill(SkillName.Wrestling, 110.1, 120.0); // Elite Wrestling
            SetSkill(SkillName.Tactics, 110.1, 120.0); // Elite Tactics
            SetSkill(SkillName.Anatomy, 100.1, 110.0); // Good Anatomy
            SetSkill(SkillName.MagicResist, 115.2, 125.0); // Very high resist skill
            SetSkill(SkillName.Meditation, 90.0, 100.0); // Decent mana regen

            Fame = 22000; // High fame boss
            Karma = -22000; // Very evil

            VirtualArmor = 75; // High passive defense
            ControlSlots = 5; // Boss-level creature

            // Initialize ability cooldowns
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMaliceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAuraCheck = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5)); // Frequent aura checks

            // Loot: Academy themed, reagents, potential unique drop
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
        }

        // --- Unique Ability: Aura of Decay (Passive Debuff Nearby) ---
        public void CheckAura()
        {
            if (Map == null || Map == Map.Internal || !Alive)
                return;

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3); // 3 tile radius aura

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    // Check if the target is a Mobile before accessing Mobile properties
                    if (m is Mobile targetMobile)
                    {
                         // Apply a temporary Strength and Intelligence debuff
                        // Use unique names for the StatMods to prevent interference
                        string strModName = "CurseAuraStr" + this.Serial.Value.ToString();
                        string intModName = "CurseAuraInt" + this.Serial.Value.ToString();

                        // Only apply if not already affected by *this specific* warrior's aura
                        if (targetMobile.GetStatMod(strModName) == null)
                        {
                            DoHarmful(targetMobile, false); // No crime report for passive aura damage/debuff

                            targetMobile.AddStatMod(new StatMod(StatType.Str, strModName, -Utility.RandomMinMax(10, 20), TimeSpan.FromSeconds(8)));
                            targetMobile.AddStatMod(new StatMod(StatType.Int, intModName, -Utility.RandomMinMax(10, 20), TimeSpan.FromSeconds(8)));

                            targetMobile.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist); // Negative effect particle
                            targetMobile.PlaySound(0x1FB); // Curse-like sound
                            targetMobile.SendMessage(0x35, "You feel weakened by the warrior's cursed presence!");
                        }
                    }
                }
            }
            eable.Free();

            // Reset the aura check timer
            m_NextAuraCheck = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(4, 6));
        }

        // --- Unique Ability: Withering Touch (On Melee Hit Effect) ---
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 30% chance to apply effect
            if (Utility.RandomDouble() < 0.30)
            {
                // Ensure the defender is a Mobile before accessing Mobile properties
                if (defender is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                {
                    DoHarmful(targetMobile);

                    // Apply Weaken spell effect (lowers STR)
                    targetMobile.ApplyPoison( this, Poison.Deadly ); // Simple way to apply weaken effect visually + stat loss
                    // Or apply directly: SpellHelper.AddStatCurse(this, targetMobile, StatType.Str);

                    // Drain Stamina and Mana
                    int drainAmount = Utility.RandomMinMax(15, 25);
                    if (targetMobile.Stam >= drainAmount)
                    {
                        targetMobile.Stam -= drainAmount;
                    }
                    if (targetMobile.Mana >= drainAmount)
                    {
                        targetMobile.Mana -= drainAmount;
                    }

                    targetMobile.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist); // Energy drain effect
                    targetMobile.PlaySound(0x1F8); // Mana drain sound
                    targetMobile.SendMessage(0x22, "The cursed strike drains your vitality!");
                }
            }
        }


        // --- Thinking Process and Ability Checks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || Map == Map.Internal || !Alive)
                return;

             // Check Aura periodically
            if (DateTime.UtcNow >= m_NextAuraCheck)
            {
                 CheckAura();
            }

            // Ability Checks (only if fighting)
            if (Combatant == null || !Alive)
                return;

            // Prioritize abilities based on cooldown and range/situation
            if (DateTime.UtcNow >= m_NextEruptionTime && this.InRange(Combatant.Location, 10)) // Use Eruption when target is within range
            {
                UnstableEruption();
                m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25)); // Reset cooldown
            }
            else if (DateTime.UtcNow >= m_NextMaliceTime) // Can drop malice more frequently, especially when moving or being hit
            {
                 LingeringMalice();
                 m_NextMaliceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18)); // Reset cooldown
            }
        }

        // --- Unique Ability: Unstable Eruption (AoE Energy Burst + Curse Chance) ---
        public void UnstableEruption()
        {
            if (Map == null) return;

            this.Say("*Feel Malidor's Folly!*"); // Flavor text
            this.PlaySound(0x211); // Magic explosion sound (like SpellElemental)
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet); // Large arcane explosion effect on self

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8); // 8 tile radius AoE

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Visual effect outward
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0); // Magic arrow explosion style

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60); // Significant AoE damage
                    // Deal 100% energy damage reflecting the magical nature
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Add a visual effect on the target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head); // Arcane hit effect

                    // Chance to apply Curse (lowers all stats/resists)
                    if (Utility.RandomDouble() < 0.35) // 35% chance
                    {
                        // Ensure the target is a Mobile before applying spell effects
                        if (target is Mobile targetMobile)
                        {
                            targetMobile.PlaySound(0x17E); // Curse sound
                            targetMobile.SendMessage(0x22, "You feel the warrior's curse settle upon you!");
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Lingering Malice (Drops Hazard Tile) ---
        public void LingeringMalice()
        {
            if (Map == null) return;

            Point3D location = this.Location; // Drop hazard at own feet
            int validZ = Map.GetAverageZ(location.X, location.Y);

            // Check if the tile can be placed at current Z or average Z
            if (Map.CanFit(location.X, location.Y, location.Z, 16, false, false) ||
                Map.CanFit(location.X, location.Y, validZ, 16, false, false))
            {
                 Point3D spawnLoc = Map.CanFit(location.X, location.Y, location.Z, 16, false, false)
                                    ? location
                                    : new Point3D(location.X, location.Y, validZ);

                 ManaDrainTile drainTile = new ManaDrainTile(); // Use provided ManaDrainTile
                 drainTile.Hue = UniqueHue; // Match warrior's hue
                 drainTile.MoveToWorld(spawnLoc, this.Map);

                 this.Say("*Malice lingers...*");
                 Effects.PlaySound(spawnLoc, this.Map, 0x1F6); // Magic trap sound
                 Effects.SendLocationParticles(EffectItem.Create(spawnLoc, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0); // Small visual effect
            }
             // else: Cannot place tile, ability fails silently this time
        }

        // --- Death Effect: Final Curse Burst ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The curse... breaks free!*");
            // Trigger a final, slightly weaker Unstable Eruption
            this.FixedParticles(0x3709, 10, 40, 5052, UniqueHue, 0, EffectLayer.CenterFeet); // Final burst effect
            this.PlaySound(GetDeathSound()); // Use custom death sound
            this.PlaySound(0x211); // Additional explosion sound

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // Smaller radius for death burst

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                 DoHarmful(target);
                 int damage = Utility.RandomMinMax(30, 50); // Reduced damage for death burst
                 AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // Energy damage

                 // Higher chance to Curse on death
                 if (Utility.RandomDouble() < 0.50)
                 {
                     if (target is Mobile targetMobile)
                     {
                         targetMobile.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                         targetMobile.PlaySound(0x17E);
                     }
                 }
            }

            // Optionally drop a few more Malice tiles
            for(int i=0; i< Utility.RandomMinMax(1,3); ++i)
            {
                 LingeringMalice(); // Reuse the logic to drop tiles nearby
            }


            base.OnDeath(c); // Important: Call base AFTER custom death effects
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Cursed beings often don't bleed normally
        // Keep Poison resistance as defined, but could make immune:
        // public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } } // Appropriate for a tough dungeon mob

        // Increased Dispel difficulty (if it were summonable)
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus { get { return 60.0; } }

        // Use Meer Warrior Sounds
        public override int GetHurtSound() { return 0x156; }
        public override int GetDeathSound() { return 0x15C; }


        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich); // Good base loot
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions); // Might carry potions
            AddLoot(LootPack.HighScrolls); // Chance for 6th/7th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8)); // Good amount of gems

            // Chance for a unique Malidor-themed artifact/reagent
            if (Utility.RandomDouble() < 0.025) // 2.5% chance
            {
                // Example unique drop - replace with an actual item defined elsewhere
                PackItem(new MalidorAcademyRelic()); // Placeholder - Create this item!
            }
             if (Utility.RandomDouble() < 0.05) // 5% chance for another rare resource
            {
                 PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2))); // Placeholder: Use your existing rare resource
            }
             if (Utility.RandomDouble() < 0.10) // 10% chance for Void Core
            {
                 PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 1))); // Placeholder: Use your existing rare resource
            }
        }

        // --- Serialization ---
        public CursedMalidorWarrior(Serial serial) : base(serial)
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
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMaliceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAuraCheck = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
        }
    }

    // Placeholder for the unique drop item - You need to define this properly elsewhere!
    public class MalidorAcademyRelic : Item
    {
        public MalidorAcademyRelic() : base(0x1F1C) // Example itemID (Spellbook graphic)
        {
            Name = "Faded Academy Signet";
            Weight = 1.0;
            Hue = CursedMalidorWarrior.UniqueHue; // Match the warrior's hue
            LootType = LootType.Blessed; // Or Regular
        }

        public MalidorAcademyRelic(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}