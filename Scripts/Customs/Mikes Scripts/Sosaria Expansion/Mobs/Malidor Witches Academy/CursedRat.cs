using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects, Curse
using Server.Spells.Third; // For Poison spell effect basis
using Server.Spells.First; // For Magic Arrow effect basis
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a cursed rat corpse")] // Themed corpse name
    public class CursedRat : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextMiasmaTime;
        private DateTime m_NextScrabbleTime;
        private DateTime m_NextBoltTime;

        // Unique Hue - Example: 1266 is a dark, corrupted purple.
        public const int UniqueHue = 1266;

        [Constructable]
        public CursedRat() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Mage AI, quick reaction
        {
            Name = "a Cursed Rat"; // Name as requested
            Body = 0xD7;           // Rat body
            BaseSoundID = 0x188;   // Rat sound
            Hue = UniqueHue;       // Apply unique hue

            // --- Significantly Boosted Stats - Magic Focused ---
            SetStr(150, 200);    // Modest strength
            SetDex(200, 250);    // Agile for casting/movement
            SetInt(400, 500);    // High Intelligence for magic

            SetHits(800, 1100);  // Durable but not a pure tank
            SetStam(200, 250);   // Good stamina pool
            SetMana(500, 650);   // Large mana pool for abilities

            SetDamage(10, 15);   // Low physical damage, abilities are the main threat

            // Mix of Poison and Energy damage
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 40); // Corrupted nature
            SetDamageType(ResistanceType.Energy, 50); // Unstable magic

            // --- Adjusted Resistances - Resists magic/poison well ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40); // Slightly weaker to cleansing fire?
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80); // High resistance to its own element
            SetResistance(ResistanceType.Energy, 60, 70); // Good energy resistance

            // --- Enhanced Skills - Focus on Magic ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0); // Capable mage
            SetSkill(SkillName.MagicResist, 105.2, 120.0); // High resist skill
            SetSkill(SkillName.Meditation, 90.0, 100.0); // Good mana regen
            SetSkill(SkillName.Tactics, 80.1, 95.0);
            SetSkill(SkillName.Wrestling, 85.1, 100.0); // Decent melee defense

            Fame = 15000; // High fame creature
            Karma = -15000; // Very evil

            VirtualArmor = 65; // Good passive defense
            ControlSlots = 4; // Difficult to control/tame

            // Initialize ability cooldowns
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextScrabbleTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextBoltTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));

            // Loot: Magic reagents, high gold, chance for unique drop
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(8, 12)));
            PackItem(new NoxCrystal(Utility.RandomMinMax(3, 5))); // Less common reagent
        }

        // --- Passive Ability: Corrupting Aura ---
        // Affects those nearby with minor poison and curse effect
        public override void OnThink() // Check aura periodically during think cycle
        {
            base.OnThink();

            if (Alive && !Deleted && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.1) // Check periodically
            {
                ApplyCorruptingAura();
            }

            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and situation
            if (DateTime.UtcNow >= m_NextBoltTime && this.InRange(Combatant.Location, 10))
            {
                UnstableBoltAttack();
                m_NextBoltTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28)); // Longer cooldown after use
            }
            else if (DateTime.UtcNow >= m_NextMiasmaTime && this.InRange(Combatant.Location, 8))
            {
                MiasmaCloudAttack();
                m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30)); // Longer cooldown
            }
            else if (DateTime.UtcNow >= m_NextScrabbleTime && this.InRange(Combatant.Location, 1)) // Only use in melee range
            {
                ArcaneScrabbleAttack();
                m_NextScrabbleTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        public void ApplyCorruptingAura()
        {
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 2); // Small radius aura

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
                // Apply effects only if the target is a Mobile
                Mobile targetMobile = target;
                DoHarmful(targetMobile);

                // Apply weak, short poison
                targetMobile.ApplyPoison(this, Poison.Lesser); // Apply a mild poison
                targetMobile.FixedParticles(0x374A, 1, 15, 9905, 3, 3, EffectLayer.Head); // Poison effect visuals
                targetMobile.PlaySound(0x230); // Poison sound

                // Apply a very short Curse effect
                targetMobile.SendLocalizedMessage(1072064); // You are enveloped in a curse.
                int effect = -(targetMobile.Skills.Total / (10 * Utility.RandomMinMax(1, 3))); // Minor stat loss, very short
                ResistanceMod[] mods = null;
                if (effect != 0)
                {
                    mods = new ResistanceMod[]
                    {
                        new ResistanceMod(ResistanceType.Physical, effect),
                        new ResistanceMod(ResistanceType.Fire, effect),
                        new ResistanceMod(ResistanceType.Cold, effect),
                        new ResistanceMod(ResistanceType.Poison, effect),
                        new ResistanceMod(ResistanceType.Energy, effect)
                    };
                }
                var timer = new CurseTimer(targetMobile, TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)), effect, mods);
                timer.Start();

                targetMobile.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                targetMobile.PlaySound(0x1E6); // Curse sound
            }
        }

        // --- Unique Ability: Miasma Cloud (Targeted AoE Hazard) ---
        public void MiasmaCloudAttack()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            // Determine target location
            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                targetLocation = targetMobile.Location;
            }
            else
            {
                targetLocation = targetDamageable.Location;
            }

            this.Say("*Hsssss... suffocate!*"); // Flavor text
            PlaySound(0x230); // Poison Field sound

            // Effect: Initial burst at target location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x11A8, 10, 20, UniqueHue, 0, 5039, 0); // Greenish cloud burst

            // Create the hazardous tile after a short delay
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return; // Re-check map validity

                Point3D spawnLoc = targetLocation;
                // Ensure the tile can be placed on the ground
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return; // Give up if no valid spot
                }

                // Use ToxicGasTile for lingering poison damage
                ToxicGasTile gasTile = new ToxicGasTile();
                gasTile.Hue = UniqueHue; // Match rat's hue
                gasTile.MoveToWorld(spawnLoc, this.Map);

                // Play sound at the tile location
                Effects.PlaySound(spawnLoc, this.Map, 0x230); // Persistent hiss sound
            });
        }

        // --- Unique Ability: Arcane Scrabble (Melee Flurry + Mana Drain) ---
        public void ArcaneScrabbleAttack()
        {
            if (Combatant == null || Map == null || !this.InRange(Combatant.Location, 1))
                return; // Must be in melee range

            this.Say("*Squeak-Skreeee!*"); // Frantic sound
            this.Animate(9, 5, 1, true, false, 0); // Basic attack animation, sped up?

            // Visual effect for speed/magic infusion
            this.FixedParticles(0x376A, 9, 32, 5030, UniqueHue, 0, EffectLayer.Waist);
            this.PlaySound(0x133); // Quick swing sound?

            int attackCount = Utility.RandomMinMax(2, 3); // 2 or 3 quick hits

            for (int i = 0; i < attackCount; ++i)
            {
                // Small delay between hits
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    if (Combatant == null || !Alive || !Combatant.Alive || !this.InRange(Combatant.Location, 1))
                        return;

                    DoHarmful(Combatant);
                    // Use base melee damage calculation
                    this.Combatant = Combatant; // Ensure combatant is set for base attack logic
                    this.Attack(Combatant);

                    // Chance to drain mana on each hit
                    if (Utility.RandomDouble() < 0.50) // 50% chance per hit
                    {
                        if (Combatant is Mobile targetMobile)
                        {
                            int manaDrained = Utility.RandomMinMax(5, 15); // Small drain per hit
                            if (targetMobile.Mana >= manaDrained)
                            {
                                targetMobile.Mana -= manaDrained;
                                targetMobile.SendMessage(0x35, "The rat's claws siphon your magical energy!");
                                targetMobile.FixedParticles(0x374A, 1, 15, 5032, UniqueHue, 0, EffectLayer.Waist); // Mana drain particle
                                targetMobile.PlaySound(0x1F8); // Mana drain sound
                            }
                        }
                    }
                });
            }
        }

        // --- Unique Ability: Unstable Bolt (Single Target + Short Paralyze) ---
        public void UnstableBoltAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Curse you!*");
            PlaySound(0x1DE); // Magic Arrow sound? Or maybe 0x5C4 (Wither sound?)

            IDamageable target = Combatant;

            // Visual effect: Bolt from rat to target
            Effects.SendMovingParticles(
                this, // From self
                target, // To target
                0x3818, // Energy Bolt graphic ID
                7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            // Apply damage and effect after a delay for travel time
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (target == null || !target.Alive || !Alive || Map == null)
                    return;

                DoHarmful(target);
                int damage = Utility.RandomMinMax(40, 60); // Good single target damage
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // Pure energy damage
                target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist); // Energy impact effect

                // Chance to briefly paralyze
                if (Utility.RandomDouble() < 0.30) // 30% chance
                {
                    if (target is Mobile targetMobile)
                    {
                        targetMobile.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4))); // Short paralyze
                        targetMobile.SendMessage(0x22, "The bolt's energy locks your muscles!");
                        targetMobile.FixedParticles(0x376A, 9, 32, 5005, UniqueHue, 0, EffectLayer.Waist); // Paralyze particle effect
                        targetMobile.PlaySound(0x204); // Paralyze sound
                    }
                }
            });
        }

        // --- Death Effect: Plague Burst ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Screee... curse... spreads...*");
            // Central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x231); // Poison explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36B0, 10, 30, UniqueHue, 0, 5039, 0); // Poison field explosion style

            // AoE Damage burst
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius

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
                int damage = Utility.RandomMinMax(30, 50); // Moderate AoE damage
                // Deal 50% Poison, 50% Energy damage
                AOS.Damage(target, this, damage, 0, 0, 0, 50, 50);
                target.FixedParticles(0x374A, 10, 15, 5021, UniqueHue, 0, EffectLayer.Waist); // Hit effect
                // Chance to apply stronger poison on death burst
                if (target is Mobile targetMobile && Utility.RandomDouble() < 0.6)
                {
                    targetMobile.ApplyPoison(this, Poison.Greater); // Apply Greater poison
                }
            }

            // Optional: Spawn a few short-lived PoisonTiles
            int hazardsToDrop = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                Point3D hazardLocation = Location;
                for (int j = 0; j < 5; ++j) // Try a few times to find a spot nearby
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    Point3D tryLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                    if (Map.CanFit(tryLoc.X, tryLoc.Y, tryLoc.Z, 16, false, false))
                    {
                        hazardLocation = tryLoc;
                        break;
                    }
                    else
                    {
                        tryLoc.Z = Map.GetAverageZ(tryLoc.X, tryLoc.Y);
                        if (Map.CanFit(tryLoc.X, tryLoc.Y, tryLoc.Z, 16, false, false))
                        {
                            hazardLocation = tryLoc;
                            break;
                        }
                    }
                }

                // Only place if we found a valid spot (or default to original loc if needed)
                if (Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    PoisonTile pTile = new PoisonTile(); // Simple poison hazard
                    pTile.Hue = UniqueHue;
                    pTile.MoveToWorld(hazardLocation, this.Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Often magical creatures don't bleed
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override int TreasureMapLevel { get { return 5; } } // High level map drop

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1); // Good gold/gems
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2)); // Chance for 4-6th scrolls
            if (Utility.RandomDouble() < 0.1) AddLoot(LootPack.HighScrolls); // Small chance for 7-8th

            // Chance for a unique thematic drop
            if (Utility.RandomDouble() < 0.03) // 3% chance
            {
                // Placeholder for a thematic item drop - Define this item elsewhere!
                // Example: PackItem(new RingOfCorruptedVigor());
                // Example: PackItem(new MalidorsFailedExperimentNotes());
                PackItem(new MalidorsCorruptedEssence()); // Using a placeholder item
            }
            if (Utility.RandomDouble() < 0.15) // 15% chance for extra NoxCrystal
            {
                PackItem(new NoxCrystal(Utility.RandomMinMax(1, 2)));
            }
        }

        // --- Serialization ---
        public CursedRat(Serial serial) : base(serial)
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

            // Re-initialize timers on load/restart to prevent issues
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextScrabbleTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextBoltTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
        }
    }

    // --- Placeholder Item Definition ---
    // You would need to define this item properly elsewhere in your scripts.
    public class MalidorsCorruptedEssence : Item
    {
        [Constructable]
        public MalidorsCorruptedEssence() : base(0x186A) // Using a gem graphic as placeholder
        {
            Name = "Malidor's Corrupted Essence";
            Hue = CursedRat.UniqueHue; // Match the rat's hue
            Weight = 0.1;
            Stackable = true; // Make it stackable if it's a crafting component
            Amount = 1;
        }

        public MalidorsCorruptedEssence(Serial serial) : base(serial) { }

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

    // --- New CurseTimer Class ---
    // This timer applies the negative resistance mods and then removes them after a short duration.
	public class CurseTimer : Timer
	{
		private Mobile m_Target;
		private List<ResistanceMod> m_Mods;

		public CurseTimer(Mobile target, TimeSpan delay, int effect, ResistanceMod[] mods)
			: base(delay)
		{
			m_Target = target;
			m_Mods = new List<ResistanceMod>();
			Priority = TimerPriority.OneSecond;

			if (mods != null)
			{
				foreach (ResistanceMod mod in mods)
				{
					m_Target.AddResistanceMod(mod);
					m_Mods.Add(mod); // Store the reference so we can remove it later
				}
			}
		}

		protected override void OnTick()
		{
			if (m_Mods != null)
			{
				foreach (ResistanceMod mod in m_Mods)
				{
					m_Target.RemoveResistanceMod(mod);
				}
			}
		}
	}

}
