using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a Macro Virus corpse")]
    public class MacroVirus : BaseCreature
    {
        // Timers for unique abilities
        private DateTime m_NextInfectionTime;
        private DateTime m_NextReplicationTime;
        private DateTime m_NextCorruptionTime;
        private Point3D m_LastLocation;

        // Unique hue for a sickly, virulent look (choose a hue that stands out)
        private const int UniqueHue = 0x9C8;

        [Constructable]
        public MacroVirus() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Macro Virus";
            Body = 11; // Using the same base body as Virulent
            BaseSoundID = 1170; // Same base sound as Virulent
            Hue = UniqueHue;

            // --- Advanced stats ---
            SetStr(500, 550);
            SetDex(300, 350);
            SetInt(700, 750);

            SetHits(2000, 2200);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 30);
            // Damage breakdown: a small physical component mixed with heavy poison
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Fully immune to poison
            SetResistance(ResistanceType.Energy, 70, 80);

            // --- Skills (focused on magical abilities) ---
            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 125.2, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // Initialize ability cooldowns (in seconds, randomized within ranges)
            m_NextInfectionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextReplicationTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextCorruptionTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Initial loot: some magic reagents and thematic items
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- Unique Ability: Infection Pulse ---
        // Emits a viral burst that damages and drains mana from nearby foes with a poison effect
        public void InfectionPulseAttack()
        {
            if (Map == null)
                return;

            this.Say("*The virus pulsates with infectious energy!*");
            PlaySound(0x211); // Use an explosion-like sound for emphasis

            // Particle effect on self
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
                    // Inflict significant poison damage
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // 100% poison damage

                    // Drain mana from targets
                    if (target is Mobile targetMobile)
                    {
                        int manaDrained = Utility.RandomMinMax(15, 25);
                        if (targetMobile.Mana >= manaDrained)
                        {
                            targetMobile.Mana -= manaDrained;
                            targetMobile.SendMessage(0x22, "Your magical energy is leeched by the virus!");
                            targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                            targetMobile.PlaySound(0x1F8);
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Recursive Replication ---
        // Spawns toxic spore hazards (using ToxicGasTile) at or around the combatant’s location
        public void RecursiveReplicationAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Determine target location
            Point3D targetLocation;
            if (Combatant is Mobile targetMob)
                targetLocation = targetMob.Location;
            else
                targetLocation = Combatant.Location;

            this.Say("*Replication sequence initiated!*");
            PlaySound(0x1E1); // A buzzing or uncanny sound

            // Create a small explosion effect at the target location before replication
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Delay slightly before spawning hazard tiles
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;

                // Spawn several toxic gas hazards around target
                int hazardCount = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < hazardCount; i++)
                {
                    int offsetX = Utility.RandomMinMax(-2, 2);
                    int offsetY = Utility.RandomMinMax(-2, 2);
                    Point3D spawnLoc = new Point3D(targetLocation.X + offsetX, targetLocation.Y + offsetY, targetLocation.Z);

                    // Ensure the tile can be placed
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    {
                        spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                        if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                            continue;
                    }

                    ToxicGasTile toxin = new ToxicGasTile(); // Assumes ToxicGasTile is defined elsewhere
                    toxin.Hue = UniqueHue;
                    toxin.MoveToWorld(spawnLoc, this.Map);

                    // Optional: Show a brief particle effect at each hazard location
                    Effects.SendLocationParticles(EffectItem.Create(spawnLoc, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            });
        }

        // --- Unique Ability: Corruption Chain Attack ---
        // A chain attack that bounces between targets applying poison damage and a chance to infect (apply poison)
        public void CorruptionChainAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*You cannot escape the corruption!*");
            PlaySound(0x20A); // A crackling energy sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;
            targets.Add(initialTarget);

            int maxBounces = 5;
            int bounceRange = 5;

            for (int i = 0; i < maxBounces; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m == this || m == lastTarget || targets.Contains(m))
                        continue;
                    if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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

            // Apply chain damage and potential poisoning to each target in the chain
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: show a moving particle from source to target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay the damage per bounce
                Mobile damageTarget = target; // Capture for lambda
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 50);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 100, 0); // Pure poison damage
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);

                        // 50% chance to attempt to infect (apply lethal poison)
                        if (Utility.RandomDouble() < 0.5 && damageTarget is Mobile targetMob)
                        {
                            targetMob.ApplyPoison(this, Poison.Lethal);
                        }
                    }
                });
            }
        }

        // --- Overridden OnMovement: Leave behind contaminant hazards as you move ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // When another Mobile moves within 2 tiles, occasionally leave a toxic trace
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && InRange(m.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    // Small chance to drain a bit of mana as contamination lingers
                    int drain = Utility.RandomMinMax(5, 10);
                    if (target.Mana >= drain)
                    {
                        target.Mana -= drain;
                        target.SendMessage(0x22, "A trace of viral corruption drains your energy!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                }
            }

            // Leave behind a ToxicGasTile with a 25% chance when the virus moves
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D dropLoc = m_LastLocation;
                if (Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    ToxicGasTile toxin = new ToxicGasTile();
                    toxin.Hue = UniqueHue;
                    toxin.MoveToWorld(dropLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(dropLoc.X, dropLoc.Y);
                    if (Map.CanFit(dropLoc.X, dropLoc.Y, validZ, 16, false, false))
                    {
                        ToxicGasTile toxin = new ToxicGasTile();
                        toxin.Hue = UniqueHue;
                        toxin.MoveToWorld(new Point3D(dropLoc.X, dropLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Overridden OnThink: Ability selection based on timers and range ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldowns and proximity
            if (DateTime.UtcNow >= m_NextCorruptionTime && InRange(Combatant.Location, 10))
            {
                CorruptionChainAttack();
                m_NextCorruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextReplicationTime && InRange(Combatant.Location, 12))
            {
                RecursiveReplicationAttack();
                m_NextReplicationTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextInfectionTime && InRange(Combatant.Location, 8))
            {
                InfectionPulseAttack();
                m_NextInfectionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // --- Death Effect: Viral Disintegration ---
        // On death, the Macro Virus unleashes one final outbreak by spawning several toxic hazards
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The virus… disseminates…*");
                Effects.PlaySound(this.Location, this.Map, 0x211);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                int hazards = Utility.RandomMinMax(5, 8);
                for (int i = 0; i < hazards; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                    {
                        hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);
                        if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                            continue;
                    }

                    ToxicGasTile toxin = new ToxicGasTile();
                    toxin.Hue = UniqueHue;
                    toxin.MoveToWorld(hazardLoc, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLoc, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance to drop a unique magical artifact related to the virus theme
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll()); // Placeholder for a unique drop
        }

        public MacroVirus(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_NextInfectionTime);
            writer.Write(m_NextReplicationTime);
            writer.Write(m_NextCorruptionTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextInfectionTime = reader.ReadDateTime();
            m_NextReplicationTime = reader.ReadDateTime();
            m_NextCorruptionTime  = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if needed (in case of long downtime)
            m_NextInfectionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextReplicationTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextCorruptionTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
