using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // Needed for StatMod
using Server.Misc; // For TribeType if needed

namespace Server.Mobiles
{
    [CorpseName("an orcish alchemist's corpse")]
    public class OrcishAlchemist : BaseCreature
    {
        // --- Timers for Special Abilities ---
        private DateTime m_NextThrowTime;
        private DateTime m_NextGasCloudTime;
        private DateTime m_NextUnstableFlaskTime;
        private Point3D m_LastLocation;
        private int m_ThrownCounter; // To potentially vary throws

        // --- Unique Hue ---
        // Example: 1110 is a murky, dark amethyst/purple - fitting for corrupted alchemy
        private const int UniqueHue = 1110;
        // Example: 68 is a sickly, unnatural green
        // private const int UniqueHue = 68;

        [Constructable]
        public OrcishAlchemist() : base(AIType.AI_Archer, FightMode.Closest, 10, 7, 0.15, 0.3) // Archer AI for ranged preference, 7 tile range
        {
            Name = "an Orcish Alchemist";
            Body = 182; // OrcBomber body
            BaseSoundID = 0x45A; // OrcBomber sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats - Alchemy Focused ---
            SetStr(350, 450);    // Strong Orc base
            SetDex(150, 200);    // Good agility for throwing
            SetInt(300, 400);    // Unusually high for an Orc, key for alchemy

            SetHits(850, 1100); // Much tougher than a bomber
            SetStam(200, 250);   // Good stamina pool for actions
            SetMana(300, 400);   // Represents "alchemical energy" / special concoctions

            SetDamage(12, 18);   // Moderate physical damage, potions are the main threat

            // Mixed damage reflecting chemical burns, poison, and explosions
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 30); // Explosions/Reactions
            SetDamageType(ResistanceType.Poison, 30); // Toxins

            // --- Adjusted Resistances - Tough but with weaknesses ---
            SetResistance(ResistanceType.Physical, 55, 65); // Tough Orc hide
            SetResistance(ResistanceType.Fire, 45, 55);     // Used to heat
            SetResistance(ResistanceType.Cold, 25, 35);     // Potential weakness
            SetResistance(ResistanceType.Poison, 70, 80);     // Handles toxins well
            SetResistance(ResistanceType.Energy, 35, 45);     // Magical energy is less familiar

            // --- Enhanced Skills - Focus on Alchemy & Combat ---
            SetSkill(SkillName.Alchemy, 100.0, 120.0); // Master Alchemist
            SetSkill(SkillName.Anatomy, 80.0, 90.0);   // Knows weak spots
            SetSkill(SkillName.MagicResist, 85.0, 95.0); // Decent magical defense
            SetSkill(SkillName.Tactics, 95.0, 105.0);  // Good combat sense
            SetSkill(SkillName.Wrestling, 95.0, 105.0); // Strong grappler
            SetSkill(SkillName.Throwing, 95.0, 105.0); // Expert thrower (if using Throwing skill)

            Fame = 15000; // High fame threat
            Karma = -15000; // Very evil

            VirtualArmor = 65; // Good natural protection
            ControlSlots = 4; // Challenging creature

            // Initialize ability cooldowns
            m_NextThrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(1, 3));
            m_NextGasCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextUnstableFlaskTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));

            m_LastLocation = this.Location;

            // --- Loot: Alchemy themed ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 25)));
            PackItem(new Garlic(Utility.RandomMinMax(10, 20)));
            PackItem(new Ginseng(Utility.RandomMinMax(10, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(10, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
            PackItem(new MortarPestle());
            PackItem(new Bottle(Utility.RandomMinMax(5, 10)));

            // Pack some random potions
            PackItem( Loot.RandomPotion() );
             if (0.2 > Utility.RandomDouble())
                 PackItem( Loot.RandomPotion() );
             if (0.1 > Utility.RandomDouble())
                PackItem( new PotionKeg() ); // Rare chance for a keg

            // Unique drop chance
            if (0.02 > Utility.RandomDouble()) // 2% chance
            {
                 // Example: PackItem(new MalidorsDiscardedRecipe());
                 // PackItem( new OrcishAlchemistsGoggles() ); // Placeholder
            }
        }

        // --- Standard Orc Properties ---
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override TribeType Tribe { get { return TribeType.Orc; } } // Assuming standard Orc tribe interactions
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.SavagesAndOrcs; } }

        public override bool IsEnemy(Mobile m)
        {
            // Retain Orc Mask behavior if desired
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            // Retain Orc Mask break behavior if desired
            Item item = aggressor.FindItemOnLayer(Layer.Helm);
            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, this, 50, 0, 100, 0, 0, 0); // Direct damage ignoring resist
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                this.Say("*Mask fool Orc!? Smash!*"); // Flavor
            }
        }

        // --- Movement Effect: Corrosive Slime Trail ---
        public void LeaveCorrosiveSlime()
        {
            if (this.Map == null || this.Map == Map.Internal) return;

             // Check if the tile can be placed at the old location
            Point3D slimeLoc = m_LastLocation;
            if (!Map.CanFit(slimeLoc.X, slimeLoc.Y, slimeLoc.Z, 16, false, false))
            {
                slimeLoc.Z = Map.GetAverageZ(slimeLoc.X, slimeLoc.Y); // Try average ground height
            }

            // Final check if placement is valid
            if (Map.CanFit(slimeLoc.X, slimeLoc.Y, slimeLoc.Z, 16, false, false))
            {
                // Use PoisonTile as the base for the acid effect
                PoisonTile slime = new PoisonTile();
                slime.Hue = 68; // Sickly green hue for acid/slime
                slime.MoveToWorld(slimeLoc, this.Map);
                // Optional: Add a bubbling sound effect at the location
                Effects.PlaySound(slimeLoc, this.Map, 0x231); // Slime sound
            }
        }

        // --- Core Combat Logic: Throwing Potions and Using Abilities ---
        public override void OnThink()
        {
            base.OnThink();

            // --- Movement Effect Check ---
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.30) // 30% chance on movement
            {
                LeaveCorrosiveSlime();
            }
            m_LastLocation = this.Location; // Always update last location

            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive || !CanBeHarmful(Combatant))
                return;

            // Check abilities based on cooldowns and range
            if (DateTime.UtcNow >= m_NextGasCloudTime && this.InRange(Combatant.Location, 8))
            {
                ReleaseDebilitatingGas();
                m_NextGasCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextUnstableFlaskTime && this.InRange(Combatant.Location, 10))
            {
                 ThrowUnstableFlask(Combatant);
                 m_NextUnstableFlaskTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Continue regular throwing if other abilities aren't ready/in range
            else if (DateTime.UtcNow >= m_NextThrowTime && this.InRange(Combatant.Location, 7)) // Use AI_Archer range
            {
                ThrowRandomPotion(Combatant);
                // Slightly variable throw speed
                 m_NextThrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6));
            }
        }

        // Override OnActionCombat to prioritize abilities over basic attacks if possible
        public override void OnActionCombat()
        {
             // Check abilities first in combat tick as well
             if (Combatant == null || Map == null || Map == Map.Internal || !Alive || !CanBeHarmful(Combatant))
             {
                 base.OnActionCombat(); // Fallback to default AI combat
                 return;
             }

            // Prioritize abilities if ready and in range
             if (DateTime.UtcNow >= m_NextGasCloudTime && this.InRange(Combatant.Location, 8))
             {
                 ReleaseDebilitatingGas();
                 m_NextGasCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
             }
             else if (DateTime.UtcNow >= m_NextUnstableFlaskTime && this.InRange(Combatant.Location, 10))
             {
                  ThrowUnstableFlask(Combatant);
                  m_NextUnstableFlaskTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
             }
             // Standard potion throw if other abilities are on cooldown/out of range
             else if (DateTime.UtcNow >= m_NextThrowTime && this.InRange(Combatant.Location, 7))
             {
                 ThrowRandomPotion(Combatant);
                 m_NextThrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6));
             }
             else // If nothing else to do, fallback to base combat (likely wrestling)
             {
                 base.OnActionCombat();
             }
        }


        // --- Ability: Throw Random Potion (Primary Attack) ---
        public void ThrowRandomPotion(IDamageable target)
        {
            if (!CanBeHarmful(target) || Map == null) return;

            this.MovingParticles(target, 0x0F0D, 7, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0); // Standard potion throw effect
            this.PlaySound(0x059); // Throw sound

            m_ThrownCounter++;
            PotionType type;

            // Select potion type - Maybe make explosion more common?
            double rand = Utility.RandomDouble();
            if (rand < 0.40)
                type = PotionType.Explosion; // 40%
            else if (rand < 0.70)
                type = PotionType.Poison;    // 30%
            else if (rand < 0.90)
                type = PotionType.Acid;      // 20%
            else
                type = PotionType.Debuff;    // 10%

            DoHarmful(target);
            new InternalPotionTimer(target, this, type).Start();
        }

        // --- Ability: Throw Unstable Flask (Targeted AoE Hazard) ---
        public void ThrowUnstableFlask(IDamageable target)
        {
            if (!CanBeHarmful(target) || Map == null) return;

            Point3D targetLocation = target.Location;

            this.Say("*Catch this... heheh!*");
            this.MovingParticles(target, 0x0F0D, 7, 0, false, true, 38, 0, 9502, 1, 0, EffectLayer.Waist, 0); // Red hue for danger
            this.PlaySound(0x059); // Throw sound

            // Delay the effect slightly
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (this.Map == null) return; // Map check

                 // Effect at target location before hazard appears
                 Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 38, 0, 5039, 0); // Explosion precursor effect
                 Effects.PlaySound(targetLocation, this.Map, 0x11D); // Explosion sound precursor

                 // Further delay for the hazardous tile
                 Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                 {
                    if (this.Map == null) return;

                    Point3D spawnLoc = targetLocation;
                     if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                     {
                         spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                     }

                     if (Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                     {
                        // Use ToxicGasTile for lingering poison/acid cloud effect
                        ToxicGasTile gas = new ToxicGasTile();
                        gas.Hue = 68; // Sickly green
                        gas.MoveToWorld(spawnLoc, this.Map);
                        Effects.PlaySound(spawnLoc, this.Map, 0x230); // Gas sound
                     }
                 });
            });
        }

        // --- Ability: Release Debilitating Gas Cloud (AoE Debuff) ---
        public void ReleaseDebilitatingGas()
        {
            if (Map == null) return;

            this.Say("*Breathe deep! Hurhur!*");
            this.PlaySound(0x230); // Gas release sound
            // Particle effect radiating outwards from the alchemist
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 60, 55, 0, 5044, 0); // Grayish cloud effect outward

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius

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
                foreach (Mobile target in targets)
                {
                    if (target is Mobile mobileTarget) // Ensure it's a mobile
                    {
                        DoHarmful(mobileTarget);

                        // Apply debuff effect - temporary stat reduction
                        int reduction = Utility.RandomMinMax(15, 25); // Reduce stats by 15-25 points
                        string statModName = "[OrcDebuff]";

                        // Apply Strength debuff
                        mobileTarget.AddStatMod(new StatMod(StatType.Str, statModName + "Str", -reduction, TimeSpan.FromSeconds(15.0)));
                        // Apply Dexterity debuff
                        mobileTarget.AddStatMod(new StatMod(StatType.Dex, statModName + "Dex", -reduction, TimeSpan.FromSeconds(15.0)));
                        // Apply Intelligence debuff
                        mobileTarget.AddStatMod(new StatMod(StatType.Int, statModName + "Int", -reduction, TimeSpan.FromSeconds(15.0)));

                        // Visual and sound effect on target
                        mobileTarget.FixedParticles(0x374A, 10, 15, 5032, 55, 0, EffectLayer.Waist); // Gray cloud effect on target
                        mobileTarget.PlaySound(0x10B); // Cough sound?
                        mobileTarget.SendMessage(0x3B2, "You feel weakened and sluggish from the noxious fumes!");

                        // Chance to apply poison
                        if (Utility.RandomDouble() < 0.50) // 50% chance
                        {
                             mobileTarget.ApplyPoison(this, Poison.Regular); // Apply regular poison
                        }
                    }
                }
            }
        }


        // --- Death Effect: Volatile Demise ---
        public override void OnDeath(Container c)
        {
            if (Map == null || Map == Map.Internal)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*It... BUUUURNS!*");
            // Big visual explosion
            Effects.PlaySound(this.Location, this.Map, 0x207); // Large explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36B0, 10, 40, UniqueHue, 0, 9904, 0); // Fire column type effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 40, 68, 0, 9904, 0); // Poison cloud type effect mixed in

            // AoE Damage
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius for death explosion
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
                int damage = Utility.RandomMinMax(50, 75);
                // Deal mixed Fire/Poison damage
                AOS.Damage(target, this, damage, 0, 50, 0, 50, 0); // 50% Fire, 50% Poison
                target.FixedParticles(0x374A, 10, 15, 5032, 38, 0, EffectLayer.Waist); // Hit effect
            }

             // Optional: Spawn a few lingering hazards
             int hazardsToDrop = Utility.RandomMinMax(2, 4);
             for (int i = 0; i < hazardsToDrop; i++)
             {
                 Point3D hazardLocation = GetRandomNearbyLocation(3); // Helper function needed
                 if (Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                 {
                     if (Utility.RandomBool())
                     {
                         FlamestrikeHazardTile fireTile = new FlamestrikeHazardTile();
                         fireTile.Hue = 38; // Fiery red
                         fireTile.MoveToWorld(hazardLocation, this.Map);
                     }
                     else
                     {
                         PoisonTile poisonTile = new PoisonTile();
                         poisonTile.Hue = 68; // Sickly green
                         poisonTile.MoveToWorld(hazardLocation, this.Map);
                     }
                 }
             }

            base.OnDeath(c);
        }

         // Helper to get a random nearby valid location
         private Point3D GetRandomNearbyLocation(int range)
         {
             if (Map == null) return Location;

             for (int i = 0; i < 10; i++) // Try 10 times
             {
                 int x = Location.X + Utility.RandomMinMax(-range, range);
                 int y = Location.Y + Utility.RandomMinMax(-range, range);
                 int z = Location.Z;

                 if (!Map.CanFit(x, y, z, 16, false, false))
                 {
                     z = Map.GetAverageZ(x, y);
                 }

                 if (Map.CanFit(x, y, z, 16, false, false))
                 {
                     return new Point3D(x, y, z);
                 }
             }
             return Location; // Fallback to current location
         }


        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return false; } } // Orcs can bleed
        public override Poison PoisonImmune { get { return Poison.Deadly; } } // Highly resistant, not immune
        public override int TreasureMapLevel { get { return 4; } } // Decent map level

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1); // Good gold/gems
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1,2)); // Chance for mid-level scrolls
            AddLoot(LootPack.HighScrolls); // Chance for 6/7th scrolls
            AddLoot(LootPack.Potions, Utility.RandomMinMax(2, 4)); // Extra potions
        }

        // --- Serialization ---
        public OrcishAlchemist(Serial serial) : base(serial)
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
            m_NextThrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(1, 3));
            m_NextGasCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextUnstableFlaskTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
        }

        // --- Internal Timer for Potion Effects ---
        private enum PotionType { Explosion, Poison, Acid, Debuff }

        private class InternalPotionTimer : Timer
        {
            private readonly IDamageable m_Target;
            private readonly Mobile m_From;
            private readonly PotionType m_Type;

            public InternalPotionTimer(IDamageable target, Mobile from, PotionType type) : base(TimeSpan.FromSeconds(1.0)) // 1 second travel time
            {
                m_Target = target;
                m_From = from;
                m_Type = type;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || m_From == null || m_From.Deleted || m_From.Map == null)
                {
                    Stop();
                    return;
                }

                // Ensure target is still valid and in range-ish (prevent sniping across map loads)
                if (m_Target is Mobile mobTarget && !m_From.InRange(mobTarget.Location, 15))
                {
                     Stop();
                     return;
                }


                 // Apply effect based on type
                 Mobile targetMobile = m_Target as Mobile; // Check if target is Mobile for certain effects

                 switch (m_Type)
                 {
                     case PotionType.Explosion:
                         m_Target.PlaySound(0x11D); // Explosion sound
                         // Deal Fire damage primarily
                         AOS.Damage(m_Target, m_From, Utility.RandomMinMax(25, 40), 10, 80, 0, 10, 0); // Phys 10, Fire 80, Poison 10
                          if (targetMobile != null)
                             targetMobile.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Explosion effect
                         break;

                     case PotionType.Poison:
                         m_Target.PlaySound(0x230); // Gas sound
                         // Deal Poison damage and attempt to poison
                         AOS.Damage(m_Target, m_From, Utility.RandomMinMax(15, 25), 10, 0, 0, 90, 0); // Phys 10, Poison 90
                         if (targetMobile != null)
                         {
                             targetMobile.FixedParticles(0x374A, 10, 15, 5032, 62, 0, EffectLayer.Waist); // Poison cloud effect
                             // Apply Greater poison
                              if (Utility.RandomDouble() < 0.6) // 60% chance to poison
                                 targetMobile.ApplyPoison(m_From, Poison.Greater);
                         }
                         break;

                     case PotionType.Acid:
                         m_Target.PlaySound(0x231); // Slime sound
                         // Deal Physical/Energy (acid) damage and potentially lower armor
                         AOS.Damage(m_Target, m_From, Utility.RandomMinMax(20, 30), 50, 0, 0, 0, 50); // Phys 50, Energy 50
                         if (targetMobile != null)
                         {
                              targetMobile.FixedParticles(0x3779, 10, 25, 5032, 68, 0, EffectLayer.Head); // Acid hit effect
                              // Add a temporary armor debuff? (Could use StatMod on AR, but simpler to just damage)
                              targetMobile.SendMessage(0x3B2, "Corrosive acid splashes onto you!");
                         }
                         break;

                     case PotionType.Debuff:
                          m_Target.PlaySound(0x1E6); // Weaken sound?
                          // Lower resists or stats temporarily
                           if (targetMobile != null)
                           {
                                targetMobile.FixedParticles(0x376A, 9, 62, 5032, EffectLayer.Waist); // Debuff effect
                                int reduction = Utility.RandomMinMax(10, 15);
                                TimeSpan duration = TimeSpan.FromSeconds(10.0);
                                targetMobile.AddStatMod(new StatMod(StatType.Str, "[OrcDebuffPotion]Str", -reduction, duration));
                                targetMobile.AddStatMod(new StatMod(StatType.Dex, "[OrcDebuffPotion]Dex", -reduction, duration));
                                targetMobile.SendMessage(0x3B2, "A foul concoction weakens your body!");
                           }
                           // Minimal damage
                            AOS.Damage(m_Target, m_From, Utility.RandomMinMax(5, 10), 100, 0, 0, 0, 0); // Pure Phys
                         break;
                 }
                 Stop();
            }
        }
    }
}