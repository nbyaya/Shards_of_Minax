using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects
using Server.Spells.Fifth; // Needed for Polymorph effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting

namespace Server.Mobiles
{
    // Define the hazard tile(s) we'll use
    public class ArcaneRiftTile : ManaDrainTile // Inherit from ManaDrainTile or make custom
    {
        // Could add custom effects or visuals here if needed
        public ArcaneRiftTile() : base()
        {
            // Optional: Change Hue or ItemID if desired
            // Hue = 1173; // Example: Match Antlion's Hue
            // ItemID = 0x37CC; // Example: Sparkle effect graphic
            Name = "an unstable arcane rift";
        }

        public ArcaneRiftTile(Serial serial) : base(serial) { }

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


    [CorpseName("a shifting arcane husk")] // More thematic corpse
    public class ArcaneAntlion : BaseCreature
    {
        private DateTime m_NextShiftTime;
        private DateTime m_NextSiphonTime;
        private DateTime m_NextPolymorphTime;
        private DateTime m_NextAnomalyTime;

        private Map m_StartShiftMap;
        private Point3D m_StartShiftLoc;
        private bool m_Shifting; // Renamed from _Tunneling

        // Unique Hue - Example: 1173 is a vibrant arcane purple/pink.
        public const int UniqueHue = 1173;

        [Constructable]
        public ArcaneAntlion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster, Mage AI
        {
            Name = "an Arcane Antlion";
            Body = 787; // Antlion body
            BaseSoundID = 1006; // Antlion sound (can adjust if needed)
            Hue = UniqueHue;

            // --- Significantly Boosted Stats (Magic Focus) ---
            SetStr(350, 420);
            SetDex(150, 200);
            SetInt(450, 550); // High Intelligence for magic

            SetHits(1100, 1400); // High health pool
            SetStam(150, 200);
            SetMana(450, 550); // Large mana pool

            SetDamage(18, 25); // Moderate base damage

            // Primarily Energy/Magic damage
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            // --- Adjusted Resistances (Strong vs Energy) ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 85); // High Energy resist

            // --- Enhanced Skills (Magic Focus) ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0); // Very high resist
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 70; // High passive defense
            ControlSlots = 5; // Boss-level creature

            // Initialize ability cooldowns
            m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Initial delay
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextAnomalyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));

            // Pack magic-themed loot + base antlion drops
            PackItem(new Bone(Utility.RandomMinMax(3, 5))); // Keep bones
            PackItem(new FertileDirt(Utility.RandomMinMax(1, 3))); // Less dirt
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(5, 10)));

            if (Core.ML && Utility.RandomDouble() < .15) // Slightly higher chance for peculiar seeds
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(2));

            // Keep the ore/skeleton drops from original
            Item orepile = null;
            switch (Utility.Random(4))
            {
                case 0: orepile = new DullCopperOre(); break;
                case 1: orepile = new ShadowIronOre(); break;
                case 2: orepile = new CopperOre(); break;
                default: orepile = new BronzeOre(); break;
            }
            if (orepile != null)
            {
                orepile.Amount = Utility.RandomMinMax(1, 5); // Reduced amount maybe
                orepile.ItemID = 0x19B9;
                PackItem(orepile);
            }
            PackBones(); // Keep PackBones logic

            if (0.07 >= Utility.RandomDouble())
            {
                switch (Utility.Random(3))
                {
                    case 0: PackItem(new UnknownBardSkeleton()); break;
                    case 1: PackItem(new UnknownMageSkeleton()); break;
                    case 2: PackItem(new UnknownRogueSkeleton()); break;
                }
            }

            // Removed DragonBreath SpecialAbility
            // SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        // --- Ability Checks in OnThink ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Check if target is Mobile for abilities requiring it
            if (Combatant is Mobile target)
            {
                // Arcane Shift (Modified Tunnel)
                if (m_NextShiftTime < DateTime.UtcNow && target.InRange(Location, 10) && !m_Shifting)
                {
                    m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
                    DoArcaneShift(target);
                }

                // Mana Siphon Aura
                if (m_NextSiphonTime < DateTime.UtcNow && this.InRange(target.Location, 5) && !m_Shifting) // Shorter range
                {
                    ManaSiphonAura();
                    m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
                }

                // Unstable Polymorph (Reactionary - moved to OnGotHit)

                // Conjure Anomaly
                if (m_NextAnomalyTime < DateTime.UtcNow && this.InRange(target.Location, 12) && !m_Shifting)
                {
                    ConjureAnomaly(target);
                    m_NextAnomalyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 30));
                }
            }
            else // Combatant is not Mobile (e.g., a destructible item), handle gracefully
            {
                 // Arcane Shift can still happen based on proximity, but might target location instead?
                 // For simplicity, let's require a Mobile target for shifting too in this version.
                 // if (m_NextShiftTime < DateTime.UtcNow && Combatant.InRange(Location, 10) && !m_Shifting) { ... }
            }
        }

        // --- Ability 1: Arcane Shift (Modified Tunnel) ---
        private void DoArcaneShift(Mobile combatant)
        {
            if (m_Shifting) return; // Prevent double activation

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* The antlion destabilizes reality and vanishes! *");
            // Arcane/Teleport Vanish Effect
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, UniqueHue, 0, 5022, 0); // Energy Field?
            PlaySound(0x1F3); // Teleport sound

            Frozen = true;
            m_Shifting = true;
            m_StartShiftLoc = Location;
            m_StartShiftMap = Map;

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => // Shorter vanish time
                {
                    if (m_Shifting)
                    {
                        Hidden = true;
                        Blessed = true; // Prevent targeting while hidden

                        // No dirt piles, maybe a temporary shimmer effect?
                        // Example: Temporary static field item
                        Item shimmer = new Static(0x376A); // Energy Field graphic
                        shimmer.Name = "destabilized space";
                        shimmer.Hue = UniqueHue;
                        shimmer.MoveToWorld(Location, Map);
                        Timer.DelayCall(TimeSpan.FromSeconds(2), () => { if (shimmer != null) shimmer.Delete(); });


                        Timer.DelayCall(TimeSpan.FromSeconds(3), () => // Time before reappearing
                            {
                                Hidden = false;
                                Blessed = false;

                                Point3D reappearLoc = Point3D.Zero;
                                Map reappearMap = Map.Internal;

                                // Use the original AntLion's logic for choosing reappear location
                                if (!combatant.Alive || !combatant.InRange(m_StartShiftLoc, 20) || combatant.Map != m_StartShiftMap)
                                {
                                    reappearLoc = m_StartShiftLoc;
                                    reappearMap = m_StartShiftMap;
                                }
                                else
                                {
                                    reappearLoc = combatant.Location;
                                    reappearMap = combatant.Map;
                                }

                                // Move before effect/damage
                                MoveToWorld(reappearLoc, reappearMap);

                                // Arcane/Teleport Reappear Effect
                                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x374A, 10, 60, UniqueHue, 0, 5022, 0); // Gate travel effect
                                PlaySound(0x1F3); // Teleport sound

                                // Damage + Mana Drain effect on reappear IF near the original combatant
                                if (combatant.Alive && combatant.Location == reappearLoc && combatant.Map == reappearMap && CanBeHarmful(combatant))
                                {
                                    PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* It reappears with disruptive energy! *");
                                    DoHarmful(combatant);
                                    int damage = Utility.RandomMinMax(25, 40); // Reappear damage
                                    int manaDrain = Utility.RandomMinMax(30, 50);
                                    AOS.Damage(combatant, this, damage, 30, 0, 0, 0, 70); // 30% Phys, 70% Energy

                                    if (combatant.Mana >= manaDrain)
                                    {
                                        combatant.Mana -= manaDrain;
                                        combatant.SendMessage(0x22, "The chaotic energy drains your mana!");
                                        // Visual feedback for mana drain
                                        combatant.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                                    }
                                    else if (combatant.Mana > 0)
                                    {
                                        combatant.Mana = 0;
                                        combatant.SendMessage(0x22, "The chaotic energy drains your mana!");
                                        combatant.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                                    }
                                }

                                // Reset state
                                m_StartShiftLoc = Point3D.Zero;
                                m_StartShiftMap = null;
                                m_Shifting = false;
                                Frozen = false;
                            });
                    }
                    else // Shifting was interrupted
                    {
                         Frozen = false; // Ensure not stuck if interrupted before hide
                    }
                });
        }

        // Interrupt Logic (Modified from original AntLion)
        public override int Damage(int amount, Mobile from, bool informMount, bool checkDisrupt)
        {
            if (m_Shifting && !Hidden && 0.25 > Utility.RandomDouble()) // Can interrupt before it hides
            {
                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* You disrupt its arcane shift! *");
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37B9, 10, 20, UniqueHue, 0, 5016, 0); // Fizzle effect
                PlaySound(0x5C); // Fizzle sound

                Frozen = false;
                Hidden = false;
                Blessed = false;
                m_Shifting = false;
                m_StartShiftLoc = Point3D.Zero;
                m_StartShiftMap = null;
                m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Shorter cooldown after interrupt
            }

            return base.Damage(amount, from, informMount, checkDisrupt);
        }


        // --- Ability 2: Mana Siphon Aura ---
        public void ManaSiphonAura()
        {
            if (Map == null || m_Shifting) return;

            PlaySound(0x1F8); // Mana Drain sound
            // Visual Pulse Effect
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37C4, 10, 40, UniqueHue, 0, 5022, 0); // Energy pulse

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m.Player && m.Mana > 0) // Target players with mana
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* Arcane energy pulses outwards, draining mana! *");

                foreach (Mobile target in targets)
                {
                    if (!target.Alive) continue;

                    int manaDrain = Utility.RandomMinMax(15, 30);
                    if (target.Mana >= manaDrain)
                    {
                        target.Mana -= manaDrain;
                        target.SendMessage(0x22, "The antlion's aura siphons your mana!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head); // Mana drain effect on target
                        DoHarmful(target); // Register as harmful action
                    }
                    else if (target.Mana > 0)
                    {
                        target.Mana = 0;
                        target.SendMessage(0x22, "The antlion's aura siphons your mana!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        DoHarmful(target);
                    }
                }
            }
        }


        // --- Ability 3: Unstable Polymorph (Reactionary on Hit) ---
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
            TryUnstablePolymorph(attacker);
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);
            TryUnstablePolymorph(attacker);
        }

        public void TryUnstablePolymorph(Mobile attacker)
        {
            if (attacker == null || !attacker.Player || !attacker.Alive || m_Shifting || Map == null || Map == Map.Internal)
                 return;

            if (DateTime.UtcNow >= m_NextPolymorphTime && 0.15 > Utility.RandomDouble()) // 15% chance on hit when cooldown ready
            {
                 if (CanBeHarmful(attacker) && SpellHelper.ValidIndirectTarget(this, attacker))
                 {
                     m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35)); // Cooldown after attempt
                     DoHarmful(attacker);
                     PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* Unstable energies lash out! *");

                     // Basic polymorph effect - doesn't use actual spell to avoid reflection etc.
                     // Need custom logic if Polymorph spell isn't suitable or requires targeting.
                     // This is a simplified direct application:
                     if (attacker.Body.IsHuman) // Only polymorph humans/elves
                     {
                         int newBody = Utility.RandomList(26, 291, 292); // Chicken, Rabbit, Cat
                         attacker.BodyValue = newBody;
                         attacker.HueMod = -1; // Reset hue mod
                         attacker.SendMessage(0x22, "You are transformed by chaotic magic!");
                         attacker.PlaySound(0x20F); // Polymorph sound
                         attacker.FixedParticles(0x3779, 1, 15, 9905, EffectLayer.Head); // Polymorph sparks

                         // Timer to revert
                         Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)), () =>
                         {
                             if (attacker != null && attacker.Alive && attacker.BodyValue == newBody)
                             {
                                attacker.BodyValue = attacker.Race.Body(attacker); // Revert to original race body
                                attacker.SendMessage("The unstable magic wears off.");
                                attacker.PlaySound(0x20F); // Polymorph sound again
                             }
                         });
                     }
                 }
            }
        }


        // --- Ability 4: Conjure Anomaly ---
        public void ConjureAnomaly(Mobile target)
        {
            if (target == null || Map == null || m_Shifting) return;

            Point3D location = target.Location; // Target location
            this.Say("*A rift tears open!*");
            PlaySound(0x208); // Generic magic/explosion sound

            int anomaliesToSpawn = Utility.RandomMinMax(1, 3);
            for(int i=0; i < anomaliesToSpawn; i++)
            {
                Point3D spawnPoint = new Point3D(
                    location.X + Utility.RandomMinMax(-2, 2),
                    location.Y + Utility.RandomMinMax(-2, 2),
                    location.Z);

                // Find valid Z
                if (!Map.CanFit(spawnPoint.X, spawnPoint.Y, spawnPoint.Z, 16, false, false))
                {
                    spawnPoint.Z = Map.GetAverageZ(spawnPoint.X, spawnPoint.Y);
                    if (!Map.CanFit(spawnPoint.X, spawnPoint.Y, spawnPoint.Z, 16, false, false))
                        continue; // Skip if still invalid
                }

                // Spawn Effect
                Effects.SendLocationParticles(EffectItem.Create(spawnPoint, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5016, 0); // Sparkle/explosion effect

                // Spawn the Hazard Tile
                ArcaneRiftTile rift = new ArcaneRiftTile(); // Using our custom/mana drain tile
                rift.Hue = UniqueHue;
                rift.MoveToWorld(spawnPoint, Map);
            }
        }

        // --- Death Effect: Arcane Collapse ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Point3D deathLoc = this.Location;
            Map deathMap = this.Map;

            // Big Visual Collapse Effect
            Effects.PlaySound(deathLoc, deathMap, 0x1F3); // Teleport/Collapse sound
			TimeSpan doubleDuration = TimeSpan.FromTicks(EffectItem.DefaultDuration.Ticks * 2);
			Effects.SendLocationParticles(EffectItem.Create(deathLoc, deathMap, doubleDuration), 0x374A, 10, 80, UniqueHue, 0, 5032, 0);

            // Spawn several hazardous rifts around the death location
            int riftCount = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < riftCount; i++)
            {
                Point3D riftLoc = new Point3D(
                    deathLoc.X + Utility.RandomMinMax(-4, 4),
                    deathLoc.Y + Utility.RandomMinMax(-4, 4),
                    deathLoc.Z);

                if (!deathMap.CanFit(riftLoc.X, riftLoc.Y, riftLoc.Z, 16, false, false))
                {
                    riftLoc.Z = deathMap.GetAverageZ(riftLoc.X, riftLoc.Y);
                    if (!deathMap.CanFit(riftLoc.X, riftLoc.Y, riftLoc.Z, 16, false, false))
                        continue;
                }

                // Small effect at each rift spawn
                Effects.SendLocationParticles(EffectItem.Create(riftLoc, deathMap, EffectItem.DefaultDuration), 0x376A, 9, 20, UniqueHue, 0, 5016, 0);

                ArcaneRiftTile deathRift = new ArcaneRiftTile();
                deathRift.Hue = UniqueHue;
                deathRift.MoveToWorld(riftLoc, deathMap);
            }

            // Final damaging pulse
			// Final damaging pulse
			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = deathMap.GetMobilesInRange(deathLoc, 6);
			foreach (Mobile m in eable)
			{
				// Exclude the creature itself from taking the final pulse damage.
				if (m == this)
					continue;

				if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m)) // Use SpellHelper for LOS etc.
					targets.Add(m);
			}
			eable.Free();

			foreach (Mobile target in targets)
			{
				int damage = Utility.RandomMinMax(50, 75); // Final burst damage
				int manaDrain = Utility.RandomMinMax(50, 100);
				AOS.Damage(target, this, damage, 20, 0, 0, 0, 80); // 20% Phys, 80% Energy

				if (target.Mana >= manaDrain)
					target.Mana -= manaDrain;
				else
					target.Mana = 0;

				target.SendMessage(0x22, "The collapsing arcane energy blasts you!");
				target.FixedParticles(0x374A, 10, 30, 5032, EffectLayer.CenterFeet); // Big energy hit
			}



            base.OnDeath(c);
        }


        // --- Standard Properties & Loot Generation ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } } // Appropriate TMap level

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus { get { return 55.0; } }

        // Use Antlion sounds, maybe customize later if desired
        public override int GetAngerSound() { return 0x5A; } // Default Antlion sounds
        public override int GetIdleSound() { return 0x5A; }
        public override int GetAttackSound() { return 0x164; }
        public override int GetHurtSound() { return 0x187; }
        public override int GetDeathSound() { return 0x1BA; } // Use this for the sound just before the visual collapse

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Good base loot
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1,2)); // High level scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // Lots of gems

            // Chance for a unique Malidor-themed item
             if (Utility.RandomDouble() < 0.01) // 1 in 100 chance
             {
                 PackItem(new MalidorianFocusCrystal()); // Example unique drop - Define this item elsewhere
             }
             if (Utility.RandomDouble() < 0.05) // 5% chance for rare resource
             {
                 PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
             }
             // Keep original Antlion artifact drop chance? Or replace it? Let's replace it.
             // if (Utility.RandomDouble() < 0.001) { this.PackItem(new AntLionsEmbraceLeggings()); }
        }


        // --- Serialization ---
        public ArcaneAntlion(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // Note: Timers are not typically serialized; they re-initialize in Deserialize or constructor.
            // State variables like m_Shifting should be serialized if needed across server restarts.
            writer.Write(m_Shifting);
            writer.Write(m_StartShiftLoc);
            writer.Write(m_StartShiftMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Shifting = reader.ReadBool();
            m_StartShiftLoc = reader.ReadPoint3D();
            m_StartShiftMap = reader.ReadMap();

            // Re-initialize cooldowns on load/restart to prevent instant ability use
            m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextAnomalyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));

            // Ensure state consistency
            if (m_Shifting)
            {
                // If saved while shifting but before hiding, need to reset state cleanly
                // Or, better yet, ensure the state is resolved before saving (tricky with timers)
                // Safest bet is often to reset shift state on load:
                 if(!Hidden) // If somehow saved mid-shift before hiding
                 {
                    m_Shifting = false;
                    Frozen = false;
                    m_StartShiftLoc = Point3D.Zero;
                    m_StartShiftMap = null;
                 }
                 // If it was saved while Hidden/Blessed during shift, need timer recovery logic or reset.
                 // For simplicity, let's assume server restarts cancel the shift cleanly:
                 Hidden = false;
                 Blessed = false;
                 Frozen = false;
                 m_Shifting = false; // Reset shift state on load
            }
            else
            {
                 Hidden = false; // Ensure not hidden if not shifting
                 Blessed = false;
                 Frozen = false;
            }
        }
    }

    // Define the example unique item (replace with actual properties)
    public class MalidorianFocusCrystal : Item
    {
        public override string DefaultName { get { return "Malidorian Focus Crystal"; } }

        [Constructable]
        public MalidorianFocusCrystal() : base(0x1F1C) // Example item ID (crystal ball shard)
        {
            Hue = ArcaneAntlion.UniqueHue; // Match hue
            Weight = 1.0;
            // Add unique properties here, e.g., spell damage increase, mana cost reduction, etc.
            // Attributes.SpellDamage = 5;
            // Attributes.LowerManaCost = 3;
        }

        public MalidorianFocusCrystal(Serial serial) : base(serial) { }

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