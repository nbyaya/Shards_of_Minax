using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for SpellHelper
using System.Collections.Generic;
using Server.Network; // Needed for Packet

namespace Server.Mobiles
{
    [CorpseName("a drakkon heir corpse")] // Custom corpse name
    public class DrakkonHeirJr : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextDraconicBeam;
        private DateTime m_NextEarthenRupture;
        private DateTime m_NextCausticBreathBarrage;
        private DateTime m_NextAncientWard;
        private DateTime m_NextVolcanicBurst;

        // Unique Hue for the Drakkon Heir (Dark Gold/Bronze)
        private const int UniqueHue = 2213; // Example hue, adjust as needed

        [Constructable]
        public DrakkonHeirJr()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Drakkon Heir"; // Monster name
            Body = 46; // Using the AncientWyrm body
            BaseSoundID = 362; // Using the AncientWyrm sounds
            Hue = UniqueHue; // Apply the unique hue

            // Significantly increased stats compared to AncientWyrm
            SetStr(1200, 1300);
            SetDex(100, 200);
            SetInt(800, 900);

            SetHits(800, 900);

            SetDamage(35, 45); // Increased damage

            // Adjusted resistances, higher fire resistance
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 90, 100); // Very high fire resistance
            SetResistance(ResistanceType.Cold, 75, 85);
            SetResistance(ResistanceType.Poison, 65, 75);
            SetResistance(ResistanceType.Energy, 65, 75);

            // Increased skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher karma

            VirtualArmor = 80; // Higher armor

            // Initialize cooldowns
            m_NextDraconicBeam = DateTime.UtcNow;
            m_NextEarthenRupture = DateTime.UtcNow;
            m_NextCausticBreathBarrage = DateTime.UtcNow;
            m_NextAncientWard = DateTime.UtcNow;
            m_NextVolcanicBurst = DateTime.UtcNow;
        }

        public DrakkonHeirJr(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override bool ReacquireOnMovement
        {
            get { return true; }
        }

        public override bool AutoDispel
        {
            get { return true; }
        }

        public override HideType HideType
        {
            get { return HideType.Barbed; } // Same as AncientWyrm
        }

        public override int Hides
        {
            get { return 50; } // More hides
        }

        public override int Meat
        {
            get { return 25; } // More meat
        }

        public override int Scales
        {
            get { return 15; } // More scales
        }

        public override ScaleType ScaleType
        {
            get { return (ScaleType)Utility.Random(4); } // Same as AncientWyrm
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; } // Immune to lethal poison
        }

        public override Poison HitPoison
        {
            get { return Utility.RandomBool() ? Poison.Deadly : Poison.Lethal; } // Can apply Deadly or Lethal poison on hit
        }

        public override int TreasureMapLevel
        {
            get { return 6; } // Higher treasure map level
        }

        public override bool CanFly
        {
            get { return true; } // Can fly
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4); // Higher quality and quantity of loot
            AddLoot(LootPack.Gems, 8); // More gems

            // Rare chance for a unique Drakkon Heir item
            if (Utility.RandomDouble() < 0.01) // 1 in 100 chance
            {
                // Assuming a unique item like 'DrakkonScaleChestplate' exists
                // PackItem(new DrakkonScaleChestplate());
            }
            // Add a very rare chance for a high-level treasure map
            if (Utility.RandomDouble() < 0.05) // 1 in 20 chance
            {
                PackItem(new TreasureMap(6, Map));
            }
        }

        // --- Sounds ---
        public override int GetIdleSound()
        {
            return 0x2D3; // Same as AncientWyrm
        }

        public override int GetHurtSound()
        {
            return 0x2D1; // Same as AncientWyrm
        }

        // --- Special Abilities ---

        // Draconic Beam: A line-based lightning/energy attack
        public void DraconicBeamAttack()
        {
            Map map = this.Map;
            if (map == null || Combatant == null)
                return;

            Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
            if (target == null)
                return; // Only target Mobiles

            // Ensure the target is a valid combatant (player or base creature)
            if (!(target is PlayerMobile) && !(target is BaseCreature))
                return;

            // Check if the target is within a reasonable range for a beam attack
            int range = 15;
            // Corrected: Using Utility.InRange for location-based range check
            if (!Utility.InRange(this.Location, target.Location, range))
                return;

            // Calculate the direction towards the target
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int beamLength = range; // Length of the beam
            int damage = 40; // Damage per tile in the beam

            // List to store locations for the beam effect and damage
            List<Point3D> beamLocations = new List<Point3D>();

            // Calculate beam locations
            for (int i = 1; i <= beamLength; i++)
            {
                int targetX = this.X + i * dx;
                int targetY = this.Y + i * dy;
                Point3D p = new Point3D(targetX, targetY, this.Z);

                // Check if the location is valid and add to the list
                if (map.CanFit(p, 16, false, false))
                {
                    beamLocations.Add(p);
                }
                else
                {
                    // Try to find a valid Z if the initial Z is blocked
                    int targetZ = map.GetAverageZ(targetX, targetY);
                    Point3D p2 = new Point3D(targetX, targetY, targetZ);
                    if (map.CanFit(p2, 16, false, false))
                    {
                        beamLocations.Add(p2);
                    }
                    else
                    {
                        // Stop the beam if it hits an impassable object
                        break;
                    }
                }
            }

            // Play sound and effect at the start of the beam
            Effects.PlaySound(this.Location, map, 0x65A); // Example lightning sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 2023, 0); // Example energy particle effect

            // Apply effects and damage along the beam
            foreach (Point3D p in beamLocations)
            {
                // Send lightning effect at each point
                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0); // Example lightning effect

                // Damage mobiles in the area
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
                foreach (Mobile m in eable)
                {
                    // Check if the mobile is a valid target (not self, not friendly)
                    if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                    {
                        // Apply damage
                        DoHarmful(m); // Mark as harmful action
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100); // Energy damage
                    }
                }
                eable.Free(); // Always free the enumerable
            }

            // Set cooldown
            m_NextDraconicBeam = DateTime.UtcNow + TimeSpan.FromSeconds(10); // 10-second cooldown
        }

        // Earthen Rupture: A radial physical attack
        public void EarthenRuptureAttack()
        {
            Map map = this.Map;
            if (map == null)
                return;

            int radius = 5; // Radius of the rupture
            int damage = 30; // Damage per affected tile

            // Play sound and effect at the center
            Effects.PlaySound(this.Location, map, 0x2F3); // Example earthquake sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5029, 0); // Example ground effect

            // Get all tiles in the radius
            List<Point3D> affectedLocations = new List<Point3D>();
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Point3D p = new Point3D(this.X + x, this.Y + y, this.Z);

                    // Check if the location is valid and within the radius (circular check)
                    // Corrected: Using Utility.InRange for location-based range check
                    if (map.CanFit(p, 16, false, false) && Utility.InRange(this.Location, p, radius))
                    {
                        affectedLocations.Add(p);
                    }
                    else
                    {
                        // Try to find a valid Z if the initial Z is blocked
                        int targetZ = map.GetAverageZ(this.X + x, this.Y + y);
                        Point3D p2 = new Point3D(this.X + x, this.Y + y, targetZ);
                        // Corrected: Using Utility.InRange for location-based range check
                        if (map.CanFit(p2, 16, false, false) && Utility.InRange(this.Location, p2, radius))
                        {
                            affectedLocations.Add(p2);
                        }
                    }
                }
            }

            // Apply effects and damage to affected locations
            foreach (Point3D p in affectedLocations)
            {
                // Send earthquake/ground effect at each point
                Effects.SendLocationEffect(p, map, 0x3709, 16, UniqueHue, 0); // Example ground effect

                // Damage mobiles in the area
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
                foreach (Mobile m in eable)
                {
                    // Check if the mobile is a valid target (not self, not friendly)
                    if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                    {
                        // Apply damage
                        DoHarmful(m); // Mark as harmful action
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0); // Physical damage
                        // Optional: Add a stun or slow effect
                        // m.ApplyAosDamage(damage, this, true, true, false); // Example for physical + stun/slow
                    }
                }
                eable.Free(); // Always free the enumerable

                // Optional: Spawn EarthquakeTile at some locations
                if (Utility.RandomDouble() < 0.2) // 20% chance to spawn a tile
                {
                    // Ensure EarthquakeTile is defined elsewhere in your scripts
                    // EarthquakeTile tile = new EarthquakeTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            // Set cooldown
            m_NextEarthenRupture = DateTime.UtcNow + TimeSpan.FromSeconds(15); // 15-second cooldown
        }

        // Caustic Breath Barrage: Cone attack leaving damaging tiles
        public void CausticBreathBarrageAttack()
        {
            Map map = this.Map;
            if (map == null || Combatant == null)
                return;

            Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
            if (target == null)
                return; // Only target Mobiles

            // Ensure the target is a valid combatant
            if (!(target is PlayerMobile) && !(target is BaseCreature))
                return;

            int coneRange = 8; // Range of the cone
            int coneWidth = 3; // Width of the cone at its widest point
            int damage = 20; // Initial damage per projectile
            // int tileDuration = 10; // Duration of the damaging tiles in seconds (not used in this method)

            // Play sound and effect
            Effects.PlaySound(this.Location, map, BaseSoundID); // Use the dragon's breath sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Example particle effect

            // Calculate the direction towards the target
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            // List to store locations within the cone
            List<Point3D> coneLocations = new List<Point3D>();

            // Calculate cone locations
            for (int i = 1; i <= coneRange; i++)
            {
                int perpendicularOffset = (int)(i * (coneWidth / (double)coneRange)); // Calculate width at this distance
                for (int j = -perpendicularOffset; j <= perpendicularOffset; j++)
                {
                    int targetX = this.X + i * dx;
                    int targetY = this.Y + i * dy;

                    // Adjust based on the direction to create a cone shape
                    if (dx == 0) // North/South
                    {
                        targetX += j;
                    }
                    else if (dy == 0) // East/West
                    {
                        targetY += j;
                    }
                    else // Intercardinal
                    {
                        // This part is a bit more complex to get a perfect cone
                        // A simpler approach for intercardinal is to use a diamond shape
                        if (Math.Abs(j) + i <= coneRange)
                        {
                            if (dx * dy > 0) // SE or NW
                            {
                                targetX += j;
                                targetY -= j;
                            }
                            else // NE or SW
                            {
                                targetX += j;
                                targetY += j;
                            }
                        }
                        else continue; // Skip if outside the diamond shape
                    }

                    Point3D p = new Point3D(targetX, targetY, this.Z);

                    // Check if the location is valid and add to the list
                    if (map.CanFit(p, 16, false, false))
                    {
                        coneLocations.Add(p);
                    }
                    else
                    {
                        // Try to find a valid Z
                        int targetZ = map.GetAverageZ(targetX, targetY);
                        Point3D p2 = new Point3D(targetX, targetY, targetZ);
                        if (map.CanFit(p2, 16, false, false))
                        {
                            coneLocations.Add(p2);
                        }
                    }
                }
            }

            // Apply effects, damage, and spawn tiles in the cone
            foreach (Point3D p in coneLocations)
            {
                // Send particle effect at each point
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x36B0, 10, 10, UniqueHue, 0, 2023, 0); // Example toxic particle effect

                // Damage mobiles in the area
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
                foreach (Mobile m in eable)
                {
                    // Check if the mobile is a valid target (not self, not friendly)
                    if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                    {
                         DoHarmful(m); // Mark as harmful action
                         // Apply initial damage
                         AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Poison damage
                         // Optional: Apply poison debuff
                         // m.ApplyPoison(this, Poison.Deadly);
                    }
                }
                eable.Free(); // Always free the enumerable

                // Spawn ToxicGasTile or FlamestrikeHazardTile
                if (Utility.RandomDouble() < 0.5) // 50% chance to spawn a tile at each location
                {
                    if (Utility.RandomBool())
                    {
                        // Ensure ToxicGasTile is defined elsewhere in your scripts
                        // ToxicGasTile gasTile = new ToxicGasTile();
                        // gasTile.MoveToWorld(p, map);
                    }
                    else
                    {
                         // Ensure FlamestrikeHazardTile is defined elsewhere in your scripts
                         // FlamestrikeHazardTile flameTile = new FlamestrikeHazardTile();
                         // flameTile.MoveToWorld(p, map);
                    }
                }
            }

            // Set cooldown
            m_NextCausticBreathBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(12); // 12-second cooldown
        }

        // Ancient Ward: Creates a protective/detrimental zone around the Heir
        public void AncientWardAbility()
        {
            Map map = this.Map;
            if (map == null)
                return;

            int radius = 4; // Radius of the ward effect
            // int tileDuration = 15; // Duration of spawned tiles (not used in this method)

            // Play sound and effect at the center
            Effects.PlaySound(this.Location, map, 0x1F2); // Example magical sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x37C4, 1, 10, UniqueHue, 0, 9909, 0); // Example magical ward effect

            // Get all tiles in the radius
            List<Point3D> affectedLocations = new List<Point3D>();
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Point3D p = new Point3D(this.X + x, this.Y + y, this.Z);

                    // Check if the location is valid and within the radius (circular check)
                    // Corrected: Using Utility.InRange for location-based range check
                    if (map.CanFit(p, 16, false, false) && Utility.InRange(this.Location, p, radius))
                    {
                        affectedLocations.Add(p);
                    }
                    else
                    {
                        // Try to find a valid Z if the initial Z is blocked
                        int targetZ = map.GetAverageZ(this.X + x, this.Y + y);
                        Point3D p2 = new Point3D(this.X + x, this.Y + y, targetZ);
                        // Corrected: Using Utility.InRange for location-based range check
                        if (map.CanFit(p2, 16, false, false) && Utility.InRange(this.Location, p2, radius))
                        {
                            affectedLocations.Add(p2);
                        }
                    }
                }
            }

            // Apply effects and spawn tiles in the ward area
            foreach (Point3D p in affectedLocations)
            {
                // Send ward effect at each point
                 Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x37CB, 1, 10, UniqueHue, 0, 9909, 0); // Example smaller ward effect

                // Randomly spawn HealingPulseTile or ManaDrainTile
                if (Utility.RandomDouble() < 0.3) // 30% chance to spawn a tile at each location
                {
                    if (Utility.RandomBool())
                    {
                        // Spawn HealingPulseTile near the center or on the Heir's location
                        // Ensure HealingPulseTile is defined elsewhere in your scripts
                        // if (Utility.InRange(this.Location, p, 1))
                        // {
                        //     HealingPulseTile healTile = new HealingPulseTile();
                        //     healTile.MoveToWorld(p, map);
                        // }
                    }
                    else
                    {
                        // Spawn ManaDrainTile further from the center
                        // Ensure ManaDrainTile is defined elsewhere in your scripts
                        //  if (!Utility.InRange(this.Location, p, 1))
                        //  {
                        //     ManaDrainTile manaTile = new ManaDrainTile();
                        //     manaTile.MoveToWorld(p, map);
                        //  }
                    }
                }
            }

            // Set cooldown
            m_NextAncientWard = DateTime.UtcNow + TimeSpan.FromSeconds(20); // 20-second cooldown
        }

        // Volcanic Burst: Targeted fire AOE leaving a HotLavaTile
        public void VolcanicBurstAttack()
        {
            Map map = this.Map;
            if (map == null || Combatant == null)
                return;

            Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
            if (target == null)
                return; // Only target Mobiles

             // Ensure the target is a valid combatant
            if (!(target is PlayerMobile) && !(target is BaseCreature))
                return;

            int burstRadius = 3; // Radius of the burst effect
            int initialDamage = 60; // High initial damage at the center
            // int tileDuration = 15; // Duration of the HotLavaTile (not used in this method)

            // Play sound and effect at the target location
            Effects.PlaySound(target.Location, map, 0x227); // Example explosion sound
            Effects.SendLocationParticles(EffectItem.Create(target.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Example fire explosion effect

            // Get all tiles in the burst radius around the target
            List<Point3D> affectedLocations = new List<Point3D>();
            for (int x = -burstRadius; x <= burstRadius; x++)
            {
                for (int y = -burstRadius; y <= burstRadius; y++)
                {
                    Point3D p = new Point3D(target.X + x, target.Y + y, target.Z);

                    // Check if the location is valid and within the radius (circular check)
                    // Corrected: Using Utility.InRange for location-based range check
                    if (map.CanFit(p, 16, false, false) && Utility.InRange(target.Location, p, burstRadius))
                    {
                        affectedLocations.Add(p);
                    }
                    else
                    {
                        // Try to find a valid Z
                        int targetZ = map.GetAverageZ(target.X + x, target.Y + y);
                        Point3D p2 = new Point3D(target.X + x, target.Y + y, targetZ);
                        // Corrected: Using Utility.InRange for location-based range check
                        if (map.CanFit(p2, 16, false, false) && Utility.InRange(target.Location, p2, burstRadius))
                        {
                            affectedLocations.Add(p2);
                        }
                    }
                }
            }

            // Apply effects and damage to affected locations
            foreach (Point3D p in affectedLocations)
            {
                 // Send fire particle effect at each point
                 Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x3039, 10, 10, UniqueHue, 0, 2023, 0); // Example fire particle effect

                // Damage mobiles in the area
                IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
                foreach (Mobile m in eable)
                {
                    // Check if the mobile is a valid target (not self, not friendly)
                    if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                    {
                         DoHarmful(m); // Mark as harmful action
                         // Apply initial damage, higher at the center, less towards the edges
                         // Corrected: Manual distance calculation using Math.Sqrt
                         double dx = target.X - p.X;
                         double dy = target.Y - p.Y;
                         double dz = target.Z - p.Z; // Include Z for 3D distance
                         double distance = Math.Sqrt(dx*dx + dy*dy + dz*dz);
                         double damageMultiplier = 1.0 - (distance / (burstRadius + 1.0)); // Damage decreases with distance
                         AOS.Damage(m, this, (int)(initialDamage * damageMultiplier), 0, 100, 0, 0, 0); // Fire damage
                    }
                }
                eable.Free(); // Always free the enumerable
            }

            // Spawn HotLavaTile at the target location
            // Ensure HotLavaTile is defined elsewhere in your scripts
            // HotLavaTile lavaTile = new HotLavaTile();
            // lavaTile.MoveToWorld(target.Location, map);


            // Set cooldown
            m_NextVolcanicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(8); // 8-second cooldown
        }


        // --- OnThink (AI Logic) ---
        public override void OnThink()
        {
            base.OnThink();

            // Trigger special abilities based on cooldowns and combat state
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
                if (target != null) // Ensure target is a Mobile
                {
                    // Example logic: Use abilities when off cooldown and target is within range
                    // Corrected: Using this.InRange for mobile-to-mobile range check
                    if (DateTime.UtcNow >= m_NextDraconicBeam && this.InRange(target, 15))
                    {
                        DraconicBeamAttack();
                    }
                    // Corrected: Using Utility.InRange for location-to-location range check (radial around self)
                    else if (DateTime.UtcNow >= m_NextEarthenRupture && Utility.InRange(this.Location, target.Location, 5))
                    {
                        EarthenRuptureAttack();
                    }
                    // Corrected: Using this.InRange for mobile-to-mobile range check
                    else if (DateTime.UtcNow >= m_NextCausticBreathBarrage && this.InRange(target, 8))
                    {
                        CausticBreathBarrageAttack();
                    }
                    // Corrected: Using this.InRange for mobile-to-mobile range check
                     else if (DateTime.UtcNow >= m_NextVolcanicBurst && this.InRange(target, 10)) // Targeted burst
                    {
                        VolcanicBurstAttack();
                    }
                    // Corrected: Removed !IsMoving check as the property does not exist
                    else if (DateTime.UtcNow >= m_NextAncientWard) // Use ward when off cooldown
                    {
                        AncientWardAbility();
                    }
                }
            }
        }


        // --- OnDeath Effect ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int tilesToDrop = 20; // More tiles on death
            int effectRadius = 5; // Wider explosion radius
            // int tileDuration = 20; // Tiles last longer (not used in this method)

            // Play a large explosion sound
            Effects.PlaySound(this.Location, this.Map, 0x218); // Large explosion sound

            // Create a large central explosion effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion effect

            // Scatter damaging tiles in a wider radius
            for (int i = 0; i < tilesToDrop; i++)
            {
                Point3D p = GetRandomValidLocation(this.Location, effectRadius, this.Map);

                if (p != Point3D.Zero) // Check if a valid location was found
                {
                    // Randomly choose between HotLavaTile and FlamestrikeHazardTile
                    if (Utility.RandomBool())
                    {
                        // Ensure HotLavaTile is defined elsewhere in your scripts
                        // HotLavaTile droppedLava = new HotLavaTile();
                        // droppedLava.MoveToWorld(p, this.Map);
                    }
                    else
                    {
                        // Ensure FlamestrikeHazardTile is defined elsewhere in your scripts
                        // FlamestrikeHazardTile droppedFlame = new FlamestrikeHazardTile();
                        // droppedFlame.MoveToWorld(p, this.Map);
                    }

                    // Create a smaller effect at each tile's location
                    Effects.SendLocationParticles(EffectItem.Create(p, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0); // Smaller/faster effect
                }
            }

            base.OnDeath(c);
        }

        // Helper method to find a random valid location within a radius
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++) // Try up to 10 times to find a valid spot
            {
                int xOffset = Utility.RandomMinMax(-radius, radius);
                int yOffset = Utility.RandomMinMax(-radius, radius);
                Point3D p = new Point3D(center.X + xOffset, center.Y + yOffset, center.Z);

                // Corrected: Using Utility.InRange for location-based range check
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                {
                    return p;
                }
                else
                {
                    // Try to find a valid Z
                    int targetZ = map.GetAverageZ(p.X, p.Y);
                    Point3D p2 = new Point3D(p.X, p.Y, targetZ);
                    // Corrected: Using Utility.InRange for location-based range check
                    if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    {
                        return p2;
                    }
                }
            }
            return Point3D.Zero; // Return Zero if no valid location is found after several attempts
        }


        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            // Add any custom properties here in future versions
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Read any custom properties here in future versions
        }
    }
}
