using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for SpellHelper (if using spell-like effects)
using System.Collections.Generic;
using Server.Network; // Needed for Packet (if sending custom packets)
using Server.Targeting; // Needed for Target

namespace Server.Mobiles
{
    [CorpseName("a drakon boar corpse")] // Custom corpse name
    public class DrakonBoar : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextFieryCharge;
        private DateTime m_NextVolcanicStomp;
        private DateTime m_NextMoltenTusks;
        private DateTime m_NextTerrifyingRoar;

        // Unique Hue for the Drakon Boar (Fiery Red/Orange)
        private const int UniqueHue = 1644; // Example hue, adjust as needed

        [Constructable]
        public DrakonBoar()
            // AIType.AI_Melee gives melee focus. Adjust based on desired behavior.
            // FightMode.Aggressor targets anyone who attacks it.
            // 10, 1, 0.2, 0.4 are default AI parameters.
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Drakon Boar"; // Monster name
            Body = 0x122; // Using the Boar body
            BaseSoundID = 0xC4; // Using the Boar sounds
            Hue = UniqueHue; // Apply the unique hue

            // Significantly increased stats compared to a regular Boar
            SetStr(500, 600);
            SetDex(80, 120);
            SetInt(150, 200); // Increased intelligence for abilities

            SetHits(1500, 1800); // High health
            SetMana(100); // Mana for abilities (if needed, otherwise can be 0)
            SetStam(200, 250); // Stamina for physical abilities

            SetDamage(15, 25); // Increased base damage

            // Adjusted resistances, higher fire resistance
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 70, 80); // High fire resistance
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Increased skills
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.Anatomy, 70.0, 90.0); // Added anatomy for combat
            SetSkill(SkillName.Magery, 50.0, 70.0); // Basic magery for effects/utility

            Fame = 15000; // Higher fame
            Karma = -15000; // Negative karma

            VirtualArmor = 45; // Higher armor

            // Drakon Boars are not tamable
            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0.0;

            // Initialize cooldowns
            m_NextFieryCharge = DateTime.UtcNow;
            m_NextVolcanicStomp = DateTime.UtcNow;
            m_NextMoltenTusks = DateTime.UtcNow;
            m_NextTerrifyingRoar = DateTime.UtcNow;
        }

        public DrakonBoar(Serial serial)
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
            get { return Utility.RandomBool(); } // Can sometimes auto-dispel
        }

        public override HideType HideType
        {
            get { return HideType.Barbed; } // Drops barbed hides
        }

        public override int Hides
        {
            get { return 20; } // More hides
        }

        public override int Meat
        {
            get { return 10; } // More meat
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } // Same as base Boar
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Greater; } // Immune to greater poison
        }

        public override Poison HitPoison
        {
            get { return Utility.RandomBool() ? Poison.Lesser : Poison.Regular; } // Can apply weaker poison on hit (optional)
        }

        public override int TreasureMapLevel
        {
            get { return 3; } // Can drop level 3 treasure maps
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2); // Average loot pack
            AddLoot(LootPack.Gems, 3); // Some gems

            // Rare chance for a unique Drakon Boar item
            if (Utility.RandomDouble() < 0.02) // 2 in 100 chance
            {
                 // Assuming a unique item like 'DrakonBoarTusk' or 'MoltenHide' exists
                 // PackItem(new DrakonBoarTusk());
            }
            // Add a rare chance for a treasure map
            if (Utility.RandomDouble() < 0.05) // 1 in 20 chance
            {
                 PackItem(new TreasureMap(TreasureMapLevel, Map));
            }
        }

        // --- Sounds ---
        public override int GetAttackSound()
        {
            return 0xC3; // Boar Attack Sound
        }

        public override int GetHurtSound()
        {
            return 0xC4; // Boar Hurt Sound
        }

        public override int GetDeathSound()
        {
            return 0xC5; // Boar Death Sound
        }

        public override int GetAngerSound()
        {
            return 0xC2; // Boar Anger Sound
        }

        public override int GetIdleSound()
        {
             return 0xC6; // Boar Idle Sound
        }


        // --- Special Abilities ---

        // Fiery Charge: A line-based fire attack towards the target
        public void FieryChargeAttack()
        {
            Map map = this.Map;
            if (map == null || Combatant == null)
                return;

            Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
            if (target == null || target.Deleted || !target.Alive || target.Map != map)
                return; // Only target valid, alive Mobiles on the same map

             // Check if the target is within a reasonable range for a charge attack
            int range = 10;
            if (!Utility.InRange(this.Location, target.Location, range))
                 return;

            // Stop movement and face the target
            this.Combatant = target; // Ensure the target is set as combatant
            Direction = GetDirectionTo(target);
            Say("Hoooog Charge!"); // Example roar/snort sound effect or text

            // Calculate the direction towards the target
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int chargeLength = range; // Length of the charge path
            int damage = 20; // Damage per tile in the charge path

            // List to store locations for the charge effect and damage
            List<Point3D> chargeLocations = new List<Point3D>();

            // Calculate charge locations
            for (int i = 1; i <= chargeLength; i++)
            {
                int targetX = this.X + i * dx;
                int targetY = this.Y + i * dy;
                Point3D p = new Point3D(targetX, targetY, this.Z);

                // Check if the location is valid and add to the list
                if (map.CanFit(p, 16, false, false))
                {
                    chargeLocations.Add(p);
                }
                else
                {
                    // Try to find a valid Z if the initial Z is blocked
                    int targetZ = map.GetAverageZ(targetX, targetY);
                    Point3D p2 = new Point3D(targetX, targetY, targetZ);
                    if (map.CanFit(p2, 16, false, false))
                    {
                         chargeLocations.Add(p2);
                    }
                    else
                    {
                         // Stop the charge if it hits an impassable object
                         break;
                    }
                }
            }

            // Play sound and effect at the start of the charge
            Effects.PlaySound(this.Location, map, GetAngerSound()); // Use boar anger sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Example fiery particle effect

            // Apply effects and damage along the charge path
            foreach (Point3D p in chargeLocations)
            {
                 // Send fiery effect at each point
                 Effects.SendLocationEffect(p, map, 0x3039, 16, UniqueHue, 0); // Example fire field effect

                 // Damage mobiles in the area
                 IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
                 foreach (Mobile m in eable)
                 {
                     // Check if the mobile is a valid target (not self, not friendly)
                     if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                     {
                          DoHarmful(m); // Mark as harmful action
                          // Apply damage
                          AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Fire damage
                          // Optional: Add a knockback or stun effect on hit
                          // SpellHelper.StamDrain(m, damage); // Example stamina drain
                     }
                 }
                 eable.Free(); // Always free the enumerable
            }

            // Move the boar to the end of the charge path (or the location before hitting an obstacle)
            if (chargeLocations.Count > 0)
            {
                 Point3D finalPosition = chargeLocations[chargeLocations.Count - 1];
				if ( map.CanFit( finalPosition, 16, false, false ) )
				{
				   MoveToWorld( finalPosition, map );
				}
				else if ( chargeLocations.Count > 1 )
				{
				   var fallback = chargeLocations[ chargeLocations.Count - 2 ];
				   if ( map.CanFit( fallback, 16, false, false ) )
					   MoveToWorld( fallback, map );
				}
            }


            // Set cooldown
            m_NextFieryCharge = DateTime.UtcNow + TimeSpan.FromSeconds(8); // 8-second cooldown
        }

        // Volcanic Stomp: A radial physical and fire attack around the boar
        public void VolcanicStompAttack()
        {
            Map map = this.Map;
            if (map == null)
                return;

            int radius = 6; // Radius of the stomp effect
            int physicalDamage = 25; // Physical damage at center
            int fireDamage = 15; // Fire damage in the radius

            // Play sound and effect at the center
            Effects.PlaySound(this.Location, map, 0x11C); // Example stomp/ground pound sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Example ground eruption effect

            // Get all mobiles in the radius
            IPooledEnumerable eable = map.GetMobilesInRange(this.Location, radius);
            foreach (Mobile m in eable)
            {
                // Check if the mobile is a valid target (not self, not friendly)
                if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                {
                    DoHarmful(m); // Mark as harmful action

                    // Calculate distance for damage reduction
                    double distance = this.GetDistanceToSqrt(m);
                    double damageMultiplier = 1.0 - (distance / (radius + 1.0)); // Damage decreases with distance

                    // Apply damage
                    AOS.Damage(m, this, (int)(physicalDamage * damageMultiplier), 100, 0, 0, 0, 0); // Physical damage
                    AOS.Damage(m, this, (int)(fireDamage * damageMultiplier), 0, 100, 0, 0, 0); // Fire damage

                    // Optional: Add a stun or slow effect to those closest to the stomp
                    if (distance <= 2)
                    {
                        // Apply a short duration stun or slow
                        // m.AddAosSkillDelay(TimeSpan.FromSeconds(2)); // Example skill delay
                    }
                }
            }
            eable.Free(); // Always free the enumerable

             // Send a radial visual effect from the center
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Repeat effect for visibility


            // Set cooldown
            m_NextVolcanicStomp = DateTime.UtcNow + TimeSpan.FromSeconds(12); // 12-second cooldown
        }


        // Molten Tusks: A cone attack with fire damage and a burn debuff
        public void MoltenTusksAttack()
        {
             Map map = this.Map;
             if (map == null || Combatant == null)
                 return;

             Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
             if (target == null || target.Deleted || !target.Alive || target.Map != map)
                 return; // Only target valid, alive Mobiles on the same map


             int coneRange = 7; // Range of the cone
             int coneWidth = 4; // Width of the cone at its widest point
             int fireDamage = 30; // Initial fire damage
             // int burnDuration = 5; // Duration of the burn debuff (not implemented here)

             // Play sound and effect
             Effects.PlaySound(this.Location, map, GetAttackSound()); // Use boar attack sound
             Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3039, 10, 10, UniqueHue, 0, 2023, 0); // Example fiery particle effect

             // Calculate the direction towards the target
             Direction d = GetDirectionTo(target);
             int dx = 0, dy = 0;
             Movement.Movement.Offset(d, ref dx, ref dy);

             // Get all mobiles in a wider range to check for cone targets
             IPooledEnumerable eable = map.GetMobilesInRange(this.Location, coneRange + 2); // Check slightly beyond the cone range
             foreach (Mobile m in eable)
             {
                  // Check if the mobile is a valid target (not self, not friendly) and within the cone shape
                 if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)) && IsInCone(m, this, d, coneRange, coneWidth))
                 {
                      DoHarmful(m); // Mark as harmful action

                      // Apply initial fire damage
                      AOS.Damage(m, this, fireDamage, 0, 100, 0, 0, 0); // Fire damage

                      // Optional: Apply a burn debuff (requires a separate debuff system)
                      // ApplyBurnDebuff(m, burnDuration);
                 }
             }
             eable.Free(); // Always free the enumerable

             // Optional: Visual effect for the cone (more complex, might require client-side packets)
             // As a simpler alternative, send fire effects along the path or a burst at the target area
              if (target != null)
              {
                  Effects.SendLocationParticles(EffectItem.Create(target.Location, map, EffectItem.DefaultDuration), 0x3039, 10, 10, UniqueHue, 0, 2023, 0); // Fire burst at target
              }


             // Set cooldown
             m_NextMoltenTusks = DateTime.UtcNow + TimeSpan.FromSeconds(10); // 10-second cooldown
        }

        // Helper method to check if a mobile is within a cone shape
        private bool IsInCone(Mobile m, Mobile source, Direction dir, int range, int width)
        {
            if (!Utility.InRange(source.Location, m.Location, range))
                return false;

             // Vector from source to target
             int dx = m.X - source.X;
             int dy = m.Y - source.Y;
             double dist2 = dx*dx + dy*dy;
             if ( dist2 > range*range )
                 return false;
             double dist = Math.Sqrt( dist2 );
             if ( dist == 0.0 )
                 return true;
 
             // Facing vector for 'dir'
             int dirX = 0, dirY = 0;
             Movement.Movement.Offset( dir, ref dirX, ref dirY );
             double dirMag = Math.Sqrt( dirX*dirX + dirY*dirY );
 
             // Cosine of the angle between (dx,dy) & (dirX,dirY)
             double dot = dx*dirX + dy*dirY;
             double cosAngle = dot / ( dist * dirMag );
             cosAngle = Math.Max( -1.0, Math.Min( 1.0, cosAngle ) );  // clamp
 
             double angleDiff = Math.Acos( cosAngle );
             double maxAngle  = Math.Atan2( width / 2.0, dist );
 
             return angleDiff <= maxAngle;
        }


        // Terrifying Roar: An area debuff that reduces enemy damage and resistance
        public void TerrifyingRoarAbility()
        {
            Map map = this.Map;
            if (map == null)
                return;

            int radius = 8; // Radius of the roar effect
            int duration = 10; // Duration of the debuff in seconds
            double damageReduction = 0.15; // 15% damage reduction
            double resistanceReduction = 0.10; // 10% resistance reduction

            // Play sound and effect
            Effects.PlaySound(this.Location, map, GetAngerSound()); // Use boar anger sound for roar
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 1, 30, UniqueHue, 0, 9909, 0); // Example fear/energy particle effect

            // Get all mobiles in the radius
            IPooledEnumerable eable = map.GetMobilesInRange(this.Location, radius);
            foreach (Mobile m in eable)
            {
                // Check if the mobile is a valid target (not self, not friendly, and is a PlayerMobile or friendly BaseCreature)
                if (m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != this.Team)))
                {
                    DoHarmful(m); // Mark as harmful action

                    // Apply debuffs (requires a separate debuff system)
                    // Example:
                    // ApplyDamageReductionDebuff(m, damageReduction, TimeSpan.FromSeconds(duration));
                    // ApplyResistanceReductionDebuff(m, resistanceReduction, TimeSpan.FromSeconds(duration));

                     // As a simpler alternative, apply temporary stat/skill loss or a debuff that can be visually represented
                     m.SendLocalizedMessage(1008112); // Example: "You feel a wave of fear!"
                }
            }
            eable.Free(); // Always free the enumerable

             // Send a radial visual effect from the center
            Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3709, 1, 30, UniqueHue, 0, 9909, 0); // Repeat effect for visibility


            // Set cooldown
            m_NextTerrifyingRoar = DateTime.UtcNow + TimeSpan.FromSeconds(20); // 20-second cooldown
        }


        // --- OnThink (AI Logic) ---
        public override void OnThink()
        {
            base.OnThink();

            // Trigger special abilities based on cooldowns and combat state
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
                if (target != null && target.Alive && target.Map == this.Map) // Ensure target is a valid Mobile
                {
                    // Example logic: Use abilities when off cooldown and target is within range

                    // Volcanic Stomp: Use when enemies are close
                    if (DateTime.UtcNow >= m_NextVolcanicStomp && this.InRange(target, 6))
                    {
                        VolcanicStompAttack();
                    }
                    // Fiery Charge: Use when target is a bit further but within charge range
                    else if (DateTime.UtcNow >= m_NextFieryCharge && this.InRange(target, 4) && this.InRange(target, 10))
                    {
                         FieryChargeAttack();
                    }
                    // Molten Tusks: Use when target is within close/medium range
                    else if (DateTime.UtcNow >= m_NextMoltenTusks && this.InRange(target, 7))
                    {
                         MoltenTusksAttack();
                    }
                     // Terrifying Roar: Use less frequently, maybe when surrounded or at lower health (add health check if desired)
                    else if (DateTime.UtcNow >= m_NextTerrifyingRoar && this.InRange(target, 8))
                    {
                         TerrifyingRoarAbility();
                    }
                }
            }
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Version
            writer.Write(m_NextFieryCharge);
            writer.Write(m_NextVolcanicStomp);
            writer.Write(m_NextMoltenTusks);
            writer.Write(m_NextTerrifyingRoar);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_NextFieryCharge = reader.ReadDateTime();
            m_NextVolcanicStomp = reader.ReadDateTime();
            m_NextMoltenTusks = reader.ReadDateTime();
            m_NextTerrifyingRoar = reader.ReadDateTime();
        }
    }
}