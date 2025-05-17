using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network; // For particle effects and sound
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a stone gorilla corpse")]
    public class StoneGorilla : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextQuakeTime;
        private DateTime m_NextBoulderTime;
        private Point3D m_LastLocation;

        // Unique Hue: A stony gray with a supernatural glint.
        private const int UniqueHue = 1175;

        [Constructable]
        public StoneGorilla() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Stone Gorilla";
            Body = 0x1D;         // Same body as a standard gorilla
            BaseSoundID = 0x9E;    // Same base sound as a standard gorilla
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(450, 550);
            SetDex(150, 200);
            SetInt(100, 120);

            SetHits(1800, 2200);
            SetStam(300, 350);
            SetMana(150, 200);     // Some mana for ability costs/effects

            SetDamage(25, 35);     // Significantly higher damage output

            // Damage is 100% Physical for this earthbound brute
            SetDamageType(ResistanceType.Physical, 100);

            // --- Enhanced Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Improved Skills ---
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 3; // Not tamable, but advanced creatures take more slots if they were to be controlled

            // --- Initialize ability cooldown timers ---
            m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation = this.Location;

            // Loot: You can customize the loot to better match your shard.
            PackGold(1500, 2500);
            PackItem(new IronIngot(Utility.RandomMinMax(3, 8)));
        }

        // --- Passive OnMovement effect: The ground trembles beneath intruders ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && InRange(this.Location, 2))
            {
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    // Minor physical tremor damage and stamina drain
                    int damage = Utility.RandomMinMax(5, 10);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // 100% Physical damage

                    // Drain a small amount of stamina
                    int stamDrain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x22, "The ground trembles, sapping your strength!");
                        target.FixedParticles(0x3728, 10, 10, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x20D); // A stone-cracking sound
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Main AI Thinking Process ---
        public override void OnThink()
        {
            base.OnThink();

            // --- Leave a Trail of Cracked Earth as it moves ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    // Using EarthquakeTile to represent cracked, shifting stone
                    EarthquakeTile quakeTile = new EarthquakeTile();
                    quakeTile.Hue = UniqueHue;
                    quakeTile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile quakeTile = new EarthquakeTile();
                        quakeTile.Hue = UniqueHue;
                        quakeTile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // --- Ability Execution: Check for Combatant and cooldowns ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Use the Earthen Quake if the combatant is nearby (within 6 tiles)
            if (DateTime.UtcNow >= m_NextQuakeTime && this.InRange(Combatant.Location, 6))
            {
                EarthenQuakeAttack();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Otherwise, if the combatant is farther (within 10 tiles), use Boulder Toss
            else if (DateTime.UtcNow >= m_NextBoulderTime && this.InRange(Combatant.Location, 10))
            {
                BoulderTossAttack();
                m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Unique Ability: Earthen Quake ---
        // An area-of-effect physical attack that cracks the ground, dealing damage and draining stamina.
        public void EarthenQuakeAttack()
        {
            if (Map == null)
                return;

            this.Say("*The ground shudders!*");
            PlaySound(0x20C); // Earthquake or stone-crack sound effect
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 45);
                    // Deal 100% Physical damage
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    
                    // Chance to drain additional stamina
                    if (Utility.RandomDouble() < 0.30) // 30% chance
                    {
                        if (target is Mobile targetMobile)
                        {
                            int stamDrain = Utility.RandomMinMax(10, 20);
                            if (targetMobile.Stam >= stamDrain)
                            {
                                targetMobile.Stam -= stamDrain;
                                targetMobile.SendMessage(0x22, "The crushing quake leaves you weakened!");
                                targetMobile.FixedParticles(0x3728, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                                targetMobile.PlaySound(0x20D);
                            }
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Boulder Toss Attack (Chain Boulder) ---
        // The Stone Gorilla hurls a massive boulder that bounces between foes, dealing physical damage and stunning occasionally.
        public void BoulderTossAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the crushing force!*");
            PlaySound(0x20A); // A heavy rock throw sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;

            targets.Add(initialTarget);
            int maxTargets = 4; // Maximum number of bounces
            int bounceRange = 5; // Range for each bounce

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            // Iterate through each target to deliver the bouncing boulder
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: Boulder projectile moving from source to target.
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x36D, // Graphic effect representing a boulder (adjust if needed)
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (target != null && CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(25, 35);
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // 100% Physical damage
                        target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);

                        // 30% chance to stun the target for 1 second.
                        if (Utility.RandomDouble() < 0.30)
                        {
                            if (target is Mobile targetMobile)
                            {
                                targetMobile.SendMessage(0x22, "The impact leaves you dazed!");
                                targetMobile.Paralyze(TimeSpan.FromSeconds(1));
                            }
                        }
                    }
                });
            }
        }

        // --- Death Effect: Stonefall Cataclysm ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The weight of the mountain crashes down!*");
                PlaySound(0x20C);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn several hazard tiles (using EarthquakeTile to simulate falling rocks)
                int hazardsToDrop = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    EarthquakeTile hazardTile = new EarthquakeTile();
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        // --- Standard Properties & Loot Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            // Advanced loot appropriate for a Mountain Stronghold boss
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance for a unique stone or earthbound artifact
            if (Utility.RandomDouble() < 0.02)
            {
                // Example unique drop; replace with an actual defined item if desired
                PackItem(new MaxxiaScroll());
            }
        }

        // --- Serialization ---
        public StoneGorilla(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_NextQuakeTime);
            writer.Write(m_NextBoulderTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextQuakeTime = reader.ReadDateTime();
            m_NextBoulderTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if necessary
            if (m_NextQuakeTime < DateTime.UtcNow)
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            if (m_NextBoulderTime < DateTime.UtcNow)
                m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
