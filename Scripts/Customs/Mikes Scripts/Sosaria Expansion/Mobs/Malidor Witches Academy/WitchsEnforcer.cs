using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Fifth; // For Mind Blast sound/effect potentially
using Server.Spells.Seventh; // For Mana Vampire effect potentially
using Server.Spells.Eighth; // For Energy Vortex visuals/summon

namespace Server.Mobiles
{
    // Using the Betrayer CorpseName as a base, modifying for theme
    [CorpseName("a warped arcane core")]
    public class WitchsEnforcer : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextArcanePulseTime;
        private DateTime m_NextRiftShiftTime;
        private DateTime m_NextManaSiphonTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation; // For passive movement ability

        // Unique Hue - Example: 1159 is a vibrant, unstable blue/purple.
        private const int UniqueHue = 1159;

        // List to keep track of summoned echoes
        private List<Mobile> m_SummonedEchoes = new List<Mobile>();

        [Constructable]
        public WitchsEnforcer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Fast reactions
        {
            Name = "a Witch's Enforcer";
            Body = 767; // Betrayer Body
            Hue = UniqueHue;

            // Base Sounds from Betrayer
            // Death: 0x423, Attack: 0x23B, Hurt: 0x140 (Set below in overrides)

            // --- Significantly Boosted Stats (Magic Focused) ---
            SetStr(400, 500); // Still physically imposing
            SetDex(150, 200); // Decent agility/casting speed
            SetInt(450, 550); // High magical power

            SetHits(1300, 1600); // High health pool
            SetStam(150, 200);
            SetMana(600, 800); // Large mana pool for abilities

            SetDamage(20, 26); // Base damage

            // Primarily Energy damage, some Physical
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // --- Adjusted Resistances (Strong vs Magic) ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 30, 40); // Slight weakness
            SetResistance(ResistanceType.Energy, 70, 80); // Strong Energy resist

            // --- Enhanced Skills (Magic Focused) ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0); // High casting ability
            SetSkill(SkillName.Meditation, 100.1, 115.0); // Good mana regen
            SetSkill(SkillName.Focus, 100.0, 115.0); // Synergy with Meditation
            SetSkill(SkillName.MagicResist, 120.2, 135.0); // Very high resistance
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0); // Strong melee defense/offense

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80; // High passive defense
            ControlSlots = 5; // Difficult boss creature

            // Initialize ability cooldowns (staggered)
            m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextManaSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Thematic pack items
            PackItem(new ArcaneGem(Utility.RandomMinMax(1, 3)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));

            // Add a light source if desired (optional, Betrayer didn't have one)
            // AddItem( new Server.Items.Torch() ); // Example
        }

        // --- Base Sound Overrides (from Betrayer) ---
        public override int GetDeathSound() { return 0x423; }
        public override int GetAttackSound() { return 0x23B; }
        public override int GetHurtSound() { return 0x140; }

        // --- Standard Properties ---
        public override bool AlwaysMurderer { get { return true; } }
        public override bool BardImmune { get { return !Core.AOS; } } // Same as Betrayer
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Same as Betrayer
        public override bool BleedImmune { get { return true; } } // Magic construct/entity likely doesn't bleed
        public override int Meat { get { return 1; } } // Same as Betrayer
        public override int TreasureMapLevel { get { return 5; } } // High-level map

        // Increased Dispel difficulty (harder to dispel summons/effects)
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        // --- Passive Ability: Unstable Trail ---
        // Leaves behind hazardous vortexes when moving
        public override void OnThink()
        {
            base.OnThink();

            // Check if the enforcer has moved significantly
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && !this.Deleted)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Only leave a trail if moved a certain distance (optional, prevents spam on small steps)
				if (GetDistance(oldLocation, this.Location) > 0)
                {
                    // Leave a Vortex Tile at the old location
                    VortexTile trail = new VortexTile(); // Use the provided VortexTile
                    trail.Hue = UniqueHue; // Match the Enforcer's hue
                    trail.MoveToWorld(oldLocation, this.Map);
                    // Optional: Adjust VortexTile duration/damage if needed via its own class definition
                }
            }

            // Clean up dead/deleted summons from the list
            m_SummonedEchoes.RemoveAll(m => m == null || !m.Alive || m.Deleted);


            // --- Active Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || Deleted || !Alive)
                return;

            // Use abilities based on cooldowns and range
            if (DateTime.UtcNow >= m_NextArcanePulseTime && this.InRange(Combatant.Location, 8)) // Medium range AoE
            {
                ArcanePulseAttack();
                m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (DateTime.UtcNow >= m_NextRiftShiftTime && this.InRange(Combatant.Location, 10)) // Long range disruption
            {
                RiftShiftAttack();
                m_NextRiftShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
            }
            else if (DateTime.UtcNow >= m_NextManaSiphonTime && this.InRange(Combatant.Location, 6)) // Shorter range drain
            {
                 ManaSiphonAttack();
                 m_NextManaSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            }
             else if (DateTime.UtcNow >= m_NextSummonTime && m_SummonedEchoes.Count < 3) // Limit number of summons
            {
                 SummonArcaneEcho();
                 m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

		private int GetDistance(Point3D p1, Point3D p2)
		{
			int dx = p1.X - p2.X;
			int dy = p1.Y - p2.Y;
			return (int)Math.Sqrt(dx * dx + dy * dy);
		}

        // --- Unique Ability: Arcane Pulse ---
        // Area of effect energy damage and potential mana disruption
        public void ArcanePulseAttack()
        {
            if (Map == null || Deleted || !Alive) return;

            this.PlaySound(0x211); // Energy Bolt impact sound
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet); // Large energy explosion effect on self

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

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
                 this.Say("*Feel the unstable energies!*"); // Flavor text

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55); // Significant AoE damage
                    // Deal 100% energy damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Chance to also drain some mana
                    if (target is Mobile && 0.3 > Utility.RandomDouble()) // 30% chance
                    {
                        Mobile mobTarget = (Mobile)target; // Already checked CanBeHarmful, so cast should be safe
                        int manaDrained = Utility.RandomMinMax(20, 40);
                        if (mobTarget.Mana >= manaDrained)
                        {
                            mobTarget.Mana -= manaDrained;
                            mobTarget.SendMessage("Arcane energy disrupts your concentration!");
                            // Optional: Visual effect for mana drain on target
                            mobTarget.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                            mobTarget.PlaySound(0x1F8); // Mana Drain sound
                        }
                    }

                    // Add a visual effect on the target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head); // Energy hit effect
                }
            }
        }


        // --- Unique Ability: Rift Shift ---
        // Teleports the target randomly and leaves a hazard
        public void RiftShiftAttack()
        {
            Mobile target = Combatant as Mobile; // Use the check
            if (target == null || Map == null || Deleted || !Alive || !CanBeHarmful(target))
                return;

            Point3D originalLocation = target.Location;
            Map targetMap = target.Map;

            // Try to find a random valid location nearby
            Point3D newLocation = Point3D.Zero;
            bool foundLocation = false;
            for (int i = 0; i < 10; ++i) // Try 10 times to find a spot
            {
                Point3D checkLoc = new Point3D(
                    target.X + Utility.RandomMinMax(-8, 8),
                    target.Y + Utility.RandomMinMax(-8, 8),
                    target.Z);

                 // Use Map.CanSpawnMobile for a more robust check
                 if (targetMap.CanSpawnMobile(checkLoc))
                 {
                     // Check Z-level, adjust if needed
                     checkLoc.Z = targetMap.GetAverageZ(checkLoc.X, checkLoc.Y);
                      if (targetMap.CanSpawnMobile(checkLoc))
                      {
                            newLocation = checkLoc;
                            foundLocation = true;
                            break;
                      }
                 }
            }


            if (foundLocation)
            {
                this.Say("*Reality bends!*");
                // Effects at original location
                Effects.SendLocationParticles(EffectItem.Create(originalLocation, targetMap, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0); // Gate travel appearance
                Effects.PlaySound(originalLocation, targetMap, 0x1FE); // Teleport sound

                // Move the target
                target.Location = newLocation;
                target.ProcessDelta(); // Update client

                // Effects at new location
                Effects.SendLocationParticles(EffectItem.Create(newLocation, targetMap, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0); // Gate travel appearance
                Effects.PlaySound(newLocation, targetMap, 0x1FE); // Teleport sound

                target.SendMessage("You are violently shifted through a rift!");

                 // Leave a Chaotic Teleport Tile at the *original* location
                 ChaoticTeleportTile hazard = new ChaoticTeleportTile(); // Use the provided tile
                 hazard.Hue = UniqueHue;
                 hazard.MoveToWorld(originalLocation, targetMap);
                 // Optional: Adjust ChaoticTeleportTile parameters if needed
            }
            else
            {
                 // If teleport fails, maybe do a smaller direct damage effect instead?
                 DoHarmful(target);
                 AOS.Damage(target, this, Utility.RandomMinMax(20,30), 0, 0, 0, 0, 100); // Minor energy damage
                 target.SendMessage("The rift fails to fully engulf you!");
            }
        }


        // --- Unique Ability: Mana Siphon ---
        // Drains mana from the target, potentially healing the Enforcer
        public void ManaSiphonAttack()
        {
            Mobile target = Combatant as Mobile; // Use the check
            if (target == null || Map == null || Deleted || !Alive || !CanBeHarmful(target))
                return;

            this.PlaySound(0x1F8); // Mana Drain sound

            int manaToDrain = Utility.RandomMinMax(50, 80);
            int actualDrain = 0;

            if (target.Mana >= manaToDrain)
            {
                target.Mana -= manaToDrain;
                actualDrain = manaToDrain;
            }
            else
            {
                actualDrain = target.Mana;
                target.Mana = 0;
            }

            if (actualDrain > 0)
            {
                // Visual effect from target to caster
                Effects.SendMovingParticles(new Entity(Serial.Zero, target.Location, target.Map), this, 0x376A, 15, 0, false, false, UniqueHue, 0, 9501, 1, 0, EffectLayer.Head, 0x100);

                target.SendMessage("Your magical energy is siphoned away!");
                this.Say("*Your power sustains me!*");

                // Heal the Enforcer for a portion of the mana drained
                int healAmount = actualDrain / 2; // Heal for 50% of mana drained
                this.Heal(healAmount);
            }
             else
             {
                 target.SendMessage("The enforcer attempts to siphon your mana, but you have none left!");
             }
        }

        // --- Unique Ability: Summon Arcane Echo ---
        // Summons unstable, short-lived magical entities
        public void SummonArcaneEcho()
        {
             if (Map == null || Deleted || !Alive) return;

             int summons = Utility.RandomMinMax(1, 2); // Summon 1 or 2 echoes
             this.Say("*Fragments of failure, arise!*");

             for (int i = 0; i < summons; ++i)
             {
                 // Attempt to find a spawn location near the Enforcer
                 Point3D spawnLoc = Point3D.Zero;
                 bool foundLoc = false;
                 for (int j = 0; j < 10; ++j)
                 {
                     Point3D checkLoc = new Point3D(
                         this.X + Utility.RandomMinMax(-3, 3),
                         this.Y + Utility.RandomMinMax(-3, 3),
                         this.Z);

                     if (Map.CanSpawnMobile(checkLoc))
                     {
                        checkLoc.Z = Map.GetAverageZ(checkLoc.X, checkLoc.Y);
                        if(Map.CanSpawnMobile(checkLoc))
                        {
                             spawnLoc = checkLoc;
                             foundLoc = true;
                             break;
                        }
                     }
                 }

                 if (!foundLoc) continue; // Skip if no location found

                 // Summoning effect
                 Effects.SendLocationParticles(EffectItem.Create(spawnLoc, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0);
                 Effects.PlaySound(spawnLoc, this.Map, 0x212); // Summon creature sound

                 // Create the echo (customize ArcaneEcho class as needed)
                 ArcaneEcho echo = new ArcaneEcho();
                 echo.Hue = this.Hue; // Match hue
                 echo.MoveToWorld(spawnLoc, this.Map);
                 echo.Combatant = this.Combatant; // Set initial target
                 echo.ControlMaster = this; // Mark the summoner
                 echo.ControlOrder = OrderType.Attack;

                 m_SummonedEchoes.Add(echo); // Track the summon
             }
        }

        // --- Death Effect: Arcane Overload ---
        // Large explosion, hazardous ground effects, kills summons
        public override void OnDeath(Container c)
        {
            // Kill off any remaining echoes
            List<Mobile> echoesToKill = new List<Mobile>(m_SummonedEchoes); // Copy list to avoid modification issues
            foreach (Mobile echo in echoesToKill)
            {
                if (echo != null && echo.Alive)
                {
                    Effects.SendLocationParticles(EffectItem.Create(echo.Location, echo.Map, TimeSpan.FromSeconds(0.5)), 0x3728, 10, 10, Hue, 0, 5023, 0);
                    echo.Kill();
                }
            }
            m_SummonedEchoes.Clear();


            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            // Big central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x211); // Energy Explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Large central energy effect


            // Scatter hazardous tiles around the death location
            int hazardsToDrop = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                if (xOffset == 0 && yOffset == 0) xOffset = 1; // Avoid exact center too often

                Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                       continue; // Skip if we can't find a valid spot
                }

                // Choose a random magic-themed hazard tile
                Item hazardTile = null;
                switch (Utility.Random(3))
                {
                    case 0: hazardTile = new VortexTile(); break;
                    case 1: hazardTile = new ManaDrainTile(); break;
                    case 2: hazardTile = new ChaoticTeleportTile(); break;
                     // Add more types if desired (e.g., Energy field tile if you create one)
                }

                if(hazardTile != null)
                {
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(hazardLocation, this.Map);
                    // Small effect at each hazard location
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, TimeSpan.FromSeconds(1.0)), 0x37C4, 10, 10, UniqueHue, 0, 5023, 0); // Sparkle effect
                }
            }

            base.OnDeath(c);
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Good base gold/items
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1,2)); // Chance for 6th/7th scrolls
            AddLoot(LootPack.HighScrolls); // Chance for 8th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10)); // More gems
            AddLoot(LootPack.Potions, Utility.RandomMinMax(1,3)); // Potion chance


            // Rare Thematic Drops
            // Example: A unique staff or piece of armor
            double randomValue = Utility.RandomDouble();

            if (randomValue < 0.01) // 1% chance
            {
                PackItem(new MalidorsFracturedStaff()); // Placeholder name for a unique rare item
            }
            else if (randomValue < 0.03) // Additional 2% chance (total 3%)
            {
                PackItem(new EnforcersBindingBracers()); // Placeholder name for another unique rare item
            }
             else if (randomValue < 0.10) // Additional 7% chance (total 10%)
             {
                  // Pack a less rare thematic item, like a special reagent or resource
                  PackItem(new VoidCore(Utility.RandomMinMax(1, 2))); // Placeholder item
             }

             // Keep the PowerCrystal and Book drops from Betrayer if desired
             if (0.1 > Utility.RandomDouble()) // 10% chance for Power Crystal
                 PackItem(new PowerCrystal());
             if (0.02 > Utility.RandomDouble()) // 2% chance for Book
                 PackItem(new BlackthornWelcomeBook());
        }

        // --- Serialization ---
        public WitchsEnforcer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Serialize the list of summoned echoes
            writer.WriteMobileList<Mobile>(m_SummonedEchoes);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize cooldowns on load/restart
            m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextManaSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

             // Deserialize the list of summoned echoes
            m_SummonedEchoes = reader.ReadStrongMobileList<Mobile>();

            // Add a timer to clean up any potentially broken summons on server load
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                 if(this != null && !this.Deleted)
                     m_SummonedEchoes.RemoveAll(m => m == null || m.Deleted || !m.Alive);
            });
        }


        // --- Nested Class for Summoned Echoes ---
        // A simple, short-lived aggressive summon
        public class ArcaneEcho : BaseCreature
        {
            private DateTime m_ExpireTime;

            public ArcaneEcho() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
            {
                Name = "an arcane echo";
                // Using Energy Vortex body, or could use Wisp, etc.
                Body = (Utility.RandomBool() ? 163 : 164); // Energy vortex body variants
                BaseSoundID = 0x3E9; // Energy vortex sound

                // Hue should be set by the summoner

                SetStr(100, 150);
                SetDex(100, 150);
                SetInt(50, 80);

                SetHits(150, 200);
                SetStam(100, 150);
                SetMana(0); // No mana needed for basic melee

                SetDamage(12, 18);
                SetDamageType(ResistanceType.Physical, 20);
                SetDamageType(ResistanceType.Energy, 80);

                SetResistance(ResistanceType.Physical, 30, 40);
                SetResistance(ResistanceType.Energy, 50, 60);
                // Other resists lower maybe

                SetSkill(SkillName.MagicResist, 70.0, 90.0);
                SetSkill(SkillName.Tactics, 80.0, 100.0);
                SetSkill(SkillName.Wrestling, 80.0, 100.0);

                Fame = 0;
                Karma = -2000; // Echoes are still harmful

                ControlSlots = 1; // Takes up a slot while active

                // Set expiration time (e.g., 30-45 seconds)
                m_ExpireTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));

                // Timer to check for expiration
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire);
            }

            private void CheckExpire()
            {
                if (DateTime.UtcNow >= m_ExpireTime)
                {
                    Expire();
                }
                 else if (ControlMaster == null || !ControlMaster.Alive || ControlMaster.Deleted) // Despawn if master dies
                 {
                      Expire();
                 }
            }

             private void Expire()
             {
                  if (Map != null && !Deleted)
                  {
                      // Optional: Small disintegration effect
                      Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(0.5)), 0x3728, 10, 10, this.Hue, 0, 5023, 0);
                      this.PlaySound(0x1FD); // Fizzle sound
                      this.Delete();
                  }
             }

             // Echoes drop no loot
             public override bool DeleteCorpseOnDeath { get { return true; } }
             public override void GenerateLoot() {}


             public ArcaneEcho(Serial serial) : base(serial)
             {
             }

             public override void Serialize(GenericWriter writer)
             {
                 base.Serialize(writer);
                 writer.Write((int)0); // version
                 writer.WriteDeltaTime(m_ExpireTime);
             }

             public override void Deserialize(GenericReader reader)
             {
                 base.Deserialize(reader);
                 int version = reader.ReadInt();
                 m_ExpireTime = reader.ReadDeltaTime();

                  // Restart expiration timer on load
                  TimeSpan remaining = m_ExpireTime - DateTime.UtcNow;
                  if (remaining <= TimeSpan.Zero)
                        Expire(); // Expire immediately if time is up
                  else
                        Timer.DelayCall(remaining, Expire);

                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), CheckExpire); // Also restart regular check

             }
        }

         // --- Placeholder Classes for Unique Loot ---
         // Define these items properly in your Items directory
         public class MalidorsFracturedStaff : BaseStaff // Or BaseWeapon
         {

             [Constructable]
             public MalidorsFracturedStaff() : base(0xDF0) // Base Staff ItemID
             {
                 Name = "Malidor's Fractured Staff"; // Set name directly if no LabelNumber
                 Hue = WitchsEnforcer.UniqueHue; // Match Enforcer Hue
                 // Add Attributes/SkillBonuses here
                 Attributes.SpellDamage = 15;
                 Attributes.LowerManaCost = 10;
                 Attributes.CastRecovery = 1;
                 WeaponAttributes.HitManaDrain = 30;
             }
             public MalidorsFracturedStaff(Serial serial) : base(serial) { }
             public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
             public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
         }

         public class EnforcersBindingBracers : BaseArmor // Assuming it's armor
         {

             public override int BasePhysicalResistance { get { return 5; } }
             public override int BaseEnergyResistance { get { return 10; } } // Higher energy resist
             public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } } // Example
             public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

             [Constructable]
             public EnforcersBindingBracers() : base(0x1086) // Base Plate Arms ItemID (change if needed)
             {
                 Name = "Enforcer's Binding Bracers";
                 Hue = WitchsEnforcer.UniqueHue;
                 // Add Attributes/SkillBonuses here
                 Attributes.BonusInt = 5;
                 Attributes.RegenMana = 2;
                 Attributes.LowerRegCost = 15;
             }
             public EnforcersBindingBracers(Serial serial) : base(serial) { }
             public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
             public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
         }

          public class VoidCore : Item // Example simple item
         {
             [Constructable]
             public VoidCore() : this(1) { }
             [Constructable]
             public VoidCore(int amount) : base(0x1F13) // Example ItemID (Crystal Ball)
             {
                 Name = "Void Core";
                 Stackable = true;
                 Amount = amount;
                 Weight = 0.1;
                 Hue = 1109; // Dark purple/black
             }
             public VoidCore(Serial serial) : base(serial) { }
             public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
             public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
         }
    }
}