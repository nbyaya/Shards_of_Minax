using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For potential spell effects
using Server.Network;        // For effect functions
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a stonefang cougar corpse")]
    public class StonefangCougar : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextRoarTime;
        private DateTime m_NextPounceTime;
        private DateTime m_NextShardBarrageTime;
        private Point3D m_LastLocation;

        // Unique hue for a stone-like appearance (adjust as desired)
        private const int UniqueHue = 1175;

        [Constructable]
        public StonefangCougar() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Stonefang Cougar";
            Body = 63;               // Based on the normal cougar body
            BaseSoundID = 0x73;        // And its sound
            Hue = UniqueHue;           // Unique stone-like hue

            // Significantly enhanced physical stats:
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(75, 100);

            SetHits(1200, 1400);
            SetStam(500, 600);
            SetMana(0);  // Not using magic

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 100);

            // Improved resistances with a stone-like flavor:
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 10, 20);

            // Enhanced melee skills:
            SetSkill(SkillName.MagicResist, 70.1, 90.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 50;

            // Initialize ability cooldowns
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation = this.Location;
        }

        // Overridden movement behavior: leave behind hazard tiles and apply a mild petrifying effect
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    // 30% chance to drain stamina (simulate a petrifying effect)
                    if (Utility.RandomDouble() < 0.30)
                    {
                        int stamDrain = Utility.RandomMinMax(10, 20);
                        if (targetMobile.Stam >= stamDrain)
                        {
                            targetMobile.Stam -= stamDrain;
                            targetMobile.SendMessage(0x22, "The shifting earth saps your strength!");
                            targetMobile.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                            targetMobile.PlaySound(0x1F8);
                        }
                    }
                }
            }

            // Leave behind an EarthquakeTile hazard occasionally when moving
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // Main AI loop that selects one of the three unique abilities
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and target distance
            if (DateTime.UtcNow >= m_NextShardBarrageTime && this.InRange(Combatant.Location, 10))
            {
                StoneShardBarrageAttack();
                m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            else if (DateTime.UtcNow >= m_NextPounceTime && this.InRange(Combatant.Location, 5))
            {
                RockyPounceAttack();
                m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextRoarTime && this.InRange(Combatant.Location, 8))
            {
                EarthshatterRoar();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // Ability 1: Earthshatter Roar – an area attack that deals physical damage and may "stun" foes briefly.
        public void EarthshatterRoar()
        {
            PlaySound(0x209); // Roar sound (example sound id)
            this.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            this.Say("*The Stonefang Cougar roars, shaking the very earth!*");

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(20, 30);
                // Deal 100% physical damage
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                // 30% chance to "stun" (simulate by paralyzing the target for 1 second)
                if (Utility.RandomDouble() < 0.30)
                {
                    if (target is Mobile targetMobile)
                    {
                        targetMobile.Paralyze(TimeSpan.FromSeconds(1.0));
                        targetMobile.SendMessage(0x22, "The force of the roar leaves you momentarily stunned!");
                    }
                }
            }
        }

        // Ability 2: Rocky Pounce Attack – a leaping strike that deals heavy damage and leaves behind a hazard.
        public void RockyPounceAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*The Stonefang Cougar leaps with lethal force!*");
                PlaySound(0x31F); // Pounce sound

                DoHarmful(target);
                int damage = Utility.RandomMinMax(50, 70);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                // Leave behind an EarthquakeTile hazard at the target's location
                Point3D targetLoc = target.Location;
                if (Map.CanFit(targetLoc.X, targetLoc.Y, targetLoc.Z, 16, false, false))
                {
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(targetLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(targetLoc.X, targetLoc.Y);
                    if (Map.CanFit(targetLoc.X, targetLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(targetLoc.X, targetLoc.Y, validZ), this.Map);
                    }
                }
            }
        }

        // Ability 3: Stone Shard Barrage – sends rock shards bouncing between up to 3 targets
        public void StoneShardBarrageAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Stone shards erupt from its fangs!*");
            PlaySound(0x20A); // Attack sound

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);

            int maxTargets = 3;
            int range = 5;
            for (int i = 0; i < maxTargets - 1; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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

            // Send a moving particle effect between each target and apply damage
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x1BFE, // Example rock shard graphic
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target; // Capture for lambda
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 45);
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x376A, 10, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                        // Optionally, add a bleeding effect here
                    }
                });
            }
        }

        // OnDeath: Create a final stone explosion by scattering hazards around its corpse
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*With its final roar, the earth reclaims the beast!*");
                PlaySound(0x21B); // Death roar sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                int hazardsToDrop = Utility.RandomMinMax(4, 7);
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
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            // 2% chance for a unique artifact drop
            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public StonefangCougar(Serial serial) : base(serial)
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

            // Reinitialize ability timers on load
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
