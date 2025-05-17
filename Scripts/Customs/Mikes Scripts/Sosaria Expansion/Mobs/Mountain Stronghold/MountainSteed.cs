using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mountain steed corpse")]
    public class MountainSteed : BaseMount
    {
        // Timers for special abilities
        private DateTime m_NextQuakeTime;
        private DateTime m_NextChargeTime;
        private DateTime m_NextBarrageTime;
        private Point3D m_LastLocation;

        // Unique hue for the Mountain Steed (distinct from the FireSteed and SpellElemental)
        private const int UniqueHue = 1175;

        [Constructable]
        public MountainSteed() : this("a mountain steed")
        {
        }

        [Constructable]
        public MountainSteed(string name)
            : base(name, 0xBE, 0x3E9E, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(20, 35);

            // Damage type: predominantly physical with a touch of cold for that mountain chill
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills ---
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Magery, 80.0, 90.0);
            SetSkill(SkillName.Meditation, 80.0, 90.0);

            Fame = 25000;
            Karma = -25000;

            Tamable = false; // This is a formidable boss; it is not tamable.
            ControlSlots = 3;

            // --- Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(100, 200)));
            PackItem(new Ruby(Utility.RandomMinMax(10, 20)));

            // Unique drop: a rare relic from the mountain (replace with an actual item defined on your shard)
            if (Utility.RandomDouble() < 0.005) // 0.5% chance
            {
                PackItem(new MaxxiaScroll());
            }

            // Initialize ability timers
            m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation = this.Location;
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override PackInstinct PackInstinct { get { return PackInstinct.Daemon | PackInstinct.Equine; } }

        // --- Movement Effect: Drain stamina from nearby foes and leave behind an EarthquakeTile hazard
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Ensure the target is a Mobile before adjusting Mobile-specific properties.
                if (m is Mobile target)
                {
                    int staminaDrain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= staminaDrain)
                    {
                        target.Stam -= staminaDrain;
                        target.SendMessage(0x22, "The mountain's power weakens your legs!");
                        target.FixedParticles(0x373A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x2F5); // A rumble fitting for a quake effect
                    }
                }
            }

            // Movement hazard: leave behind an EarthquakeTile with a 25% chance each move tick
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    EarthquakeTile quakeTile = new EarthquakeTile(); // Assumes EarthquakeTile is defined elsewhere on your shard
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

            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Check for ability cooldowns and execute abilities based on target distance ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities: BoulderBarrage > AvalancheCharge > QuakingHoofbeat
            if (DateTime.UtcNow >= m_NextBarrageTime && this.InRange(Combatant.Location, 10))
            {
                BoulderBarrage();
                m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(Combatant.Location, 12))
            {
                AvalancheCharge();
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextQuakeTime && this.InRange(Combatant.Location, 6))
            {
                QuakingHoofbeat();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- Unique Ability: Quaking Hoofbeat ---
        // An AoE burst that deals mixed physical and cold damage while draining stamina from nearby foes.
        public void QuakingHoofbeat()
        {
            if (Map == null)
                return;

            PlaySound(0x2B3); // Deep rumble sound
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

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
                    int damage = Utility.RandomMinMax(30, 50);
                    // Deliver 70% physical and 30% cold damage
                    AOS.Damage(target, this, damage, 70, 0, 30, 0, 0);

                    // Drain a bit of stamina from the target
                    if (target is Mobile mtarget)
                    {
                        int stamDrain = Utility.RandomMinMax(10, 20);
                        if (mtarget.Stam >= stamDrain)
                        {
                            mtarget.Stam -= stamDrain;
                            mtarget.SendMessage(0x22, "The quaking hooves leave you winded!");
                            mtarget.FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                            mtarget.PlaySound(0x2F5);
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Avalanche Charge ---
        // A targeted, charge–like attack that calls down a hazardous impact (using LandmineTile) and deals heavy damage.
        public void AvalancheCharge()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
                targetLocation = target.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The earth trembles as the steed charges!*");
            PlaySound(0x1F7); // Charge sound effect

            // Visual effect at the target location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Delay to simulate the charge impact
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                // Spawn a hazardous tile; here we use LandmineTile to simulate a rocky impact.
                LandmineTile mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(spawnLoc, this.Map);

                // Damage nearby targets (within 3 tiles)
                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = Map.GetMobilesInRange(spawnLoc, 3);
                foreach (Mobile m in eable)
                {
                    if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        targets.Add(m);
                }
                eable.Free();

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(50, 70);
                    // Pure physical damage
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    m.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            });
        }

        // --- Unique Ability: Boulder Barrage ---
        // A chain attack that bounces between multiple foes, dealing heavy physical damage.
        public void BoulderBarrage()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the crushing weight of the mountains!*");
            PlaySound(0x20D); // Boulder impact sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;
            targets.Add(initialTarget);

            int maxTargets = 5; // Maximum bounces
            int range = 5;

            for (int i = 0; i < maxTargets; ++i)
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

            // Apply damage and visual effects for each hop in the barrage
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Send a moving particle effect to simulate a boulder rolling between targets
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x388C, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(35, 55);
                        // Deal pure physical damage
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x375A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Mountain Shatter ---
        // On death, the Mountain Steed explodes in a burst that spawns several hazardous tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The mountain's fury is unleashed!*");
            Effects.PlaySound(this.Location, this.Map, 0x2F6); // Explosive sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

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

                // Spawn an EarthquakeTile as the death hazard
                EarthquakeTile quakeTile = new EarthquakeTile();
                quakeTile.Hue = UniqueHue;
                quakeTile.MoveToWorld(hazardLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            // Advanced loot for a boss-level creature
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
        }

        public MountainSteed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_NextQuakeTime);
            writer.Write(m_NextChargeTime);
            writer.Write(m_NextBarrageTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextQuakeTime = reader.ReadDateTime();
            m_NextChargeTime = reader.ReadDateTime();
            m_NextBarrageTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if needed
            if (m_NextQuakeTime < DateTime.UtcNow)
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            if (m_NextChargeTime < DateTime.UtcNow)
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            if (m_NextBarrageTime < DateTime.UtcNow)
                m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
