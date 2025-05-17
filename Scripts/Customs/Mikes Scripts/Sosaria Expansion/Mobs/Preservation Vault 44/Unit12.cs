using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh;   // for Chain Lightning-like logic
using Server.Engines.Harvest;  // although not mining, it shows the pattern
using Server.ContextMenus;
using Server.Regions;

namespace Server.Mobiles
{
    [CorpseName("the shattered husk of Unit-12")]
    public class Unit12 : BaseCreature
    {
        private DateTime m_NextOverload;      // AoE stun + energy burst
        private DateTime m_NextDistortion;    // Temporal slow field
        private DateTime m_NextNaniteSwarm;   // Poison cloud
        private DateTime m_NextBeamBarrage;   // Chained energy beams
        private Point3D  m_LastLocation;

        // Glowing emerald‑cyber tone
        private const int CyberHue = 1271;

        [Constructable]
        public Unit12() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "Unit-12";
            Body = 714;             // same as Iron Beetle
            BaseSoundID = 397;      // same sound
            Hue = CyberHue;

            // Stat block — extremely formidable
            SetStr(900, 1000);
            SetDex(200, 250);
            SetInt(700, 800);

            SetHits(2500, 3000);
            SetStam(400, 500);
            SetMana(1200, 1500);

            SetDamage(20, 30);

            // Damage types: mostly energy, minor physical
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // High resistances, especially vs. energy/poison
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   90, 100);

            // Skills tuned for magic/AI control
            SetSkill(SkillName.EvalInt,    120.0, 140.0);
            SetSkill(SkillName.Magery,     120.0, 140.0);
            SetSkill(SkillName.MagicResist,130.0, 150.0);
            SetSkill(SkillName.Meditation,100.0, 120.0);
            SetSkill(SkillName.Tactics,    100.0, 110.0);
            SetSkill(SkillName.Wrestling,  100.0, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;
            ControlSlots = 6;

            // Initialize ability timers
            m_NextOverload    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextDistortion  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextNaniteSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
            m_NextBeamBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Base loot: scrap components & reagent “cores”
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 25)));
        }
		
		public Unit12(Serial serial) : base(serial) { }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave behind unstable energy nodes (VortexTiles)
            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.20)
            {
                Point3D dropLoc = m_LastLocation;
                m_LastLocation = this.Location;

                int z = dropLoc.Z;
                if (!Map.CanFit(dropLoc.X, dropLoc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var node = new VortexTile();
                node.Hue = CyberHue;
                node.MoveToWorld(new Point3D(dropLoc.X, dropLoc.Y, z), Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Overload Surge: AoE stun + energy burst
            if (DateTime.UtcNow >= m_NextOverload && InRange(Combatant.Location, 8))
            {
                OverloadSurge();
                m_NextOverload = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Temporal Distortion: slow‐field hazard
            else if (DateTime.UtcNow >= m_NextDistortion)
            {
                TemporalDistortion();
                m_NextDistortion = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Chained Beam Barrage
            else if (DateTime.UtcNow >= m_NextBeamBarrage && InRange(Combatant.Location, 12))
            {
                BeamBarrage();
                m_NextBeamBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Nanite Swarm: poison cloud
            else if (DateTime.UtcNow >= m_NextNaniteSwarm)
            {
                NaniteSwarm();
                m_NextNaniteSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
            }
        }

        // --- Overload Surge: AoE stun + burst ---
        private void OverloadSurge()
        {
            Say("*SYSTEM OVERLOAD!*");
            PlaySound(0x1F7);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                          0x3709, 20, 30, CyberHue, 0, 5052, 0);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target)
                    list.Add(target);
            }

            foreach (var target in list)
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(50, 80), 0, 0, 0, 0, 100);
                // 30% stun
                if (Utility.RandomDouble() < 0.3)
                    target.Freeze(TimeSpan.FromSeconds(3.0));
            }
        }

        // --- Temporal Distortion: creates a slowing field (LandmineTile variant) ---
        private void TemporalDistortion()
        {
            Say("*INITIATING TEMPORAL FIELD*");
            PlaySound(0x22F);

            Point3D centre = Combatant is Mobile tgt ? tgt.Location : Combatant.Location;
            int radius = 4;
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                Point3D p = new Point3D(centre.X + dx, centre.Y + dy, centre.Z);
                int z = p.Z;
                if (!Map.CanFit(p.X, p.Y, z, 16, false, false))
                    z = Map.GetAverageZ(p.X, p.Y);

                var tile = new LandmineTile();
                tile.Hue = CyberHue;
                tile.MoveToWorld(new Point3D(p.X, p.Y, z), Map);
            }
        }

        // --- Beam Barrage: chain energy bolts among up to 6 targets ---
        private void BeamBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false)) 
                return;

            Say("*BEAM PROTOCOL ACTIVE!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            int max = 6, range = 8;

            for (int i = 1; i < max; i++)
            {
                Mobile last = targets[i - 1], next = null;
                double best = double.MaxValue;
                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !targets.Contains(m) && CanBeHarmful(m, false))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < best)
                        {
                            best = d;
                            next = m;
                        }
                    }
                }
                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x3818, 7, 0, false, false, CyberHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                    }
                });
            }
        }

        // --- Nanite Swarm: spawns a cloud of PoisonTile hazards ---
        private void NaniteSwarm()
        {
            Say("*DEPLOYING NANITE SWARM*");
            PlaySound(0x2F1);

            Point3D centre = Location;
            for (int i = 0; i < 8; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var p = new Point3D(centre.X + dx, centre.Y + dy, centre.Z);
                int z = p.Z;
                if (!Map.CanFit(p.X, p.Y, z, 16, false, false))
                    z = Map.GetAverageZ(p.X, p.Y);

                var cloud = new PoisonTile();
                cloud.Hue = CyberHue;
                cloud.MoveToWorld(new Point3D(p.X, p.Y, z), Map);
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*SHUTTING DOWN...*");
                PlaySound(0x211);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 60, CyberHue, 0, 5052, 0);

                // drop random hazard tiles around corpse
                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-4, 4);
                    int dy = Utility.RandomMinMax(-4, 4);
                    var p = new Point3D(X + dx, Y + dy, Z);
                    int z = p.Z;
                    if (!Map.CanFit(p.X, p.Y, z, 16, false, false))
                        z = Map.GetAverageZ(p.X, p.Y);

                    var tile = new LightningStormTile();
                    tile.Hue = CyberHue;
                    tile.MoveToWorld(new Point3D(p.X, p.Y, z), Map);
                }
            }

            base.OnDeath(c);

            // 3% chance for “Corrupted Core” drop
            if (Utility.RandomDouble() < 0.03)
                c.DropItem(new ThroatOfTheWildBond());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new HonorboundTreads());  // rare artifact
        }

        public override bool BleedImmune      { get { return true; } }
        public override int  TreasureMapLevel { get { return 6;    } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // re‑init timers
            m_NextOverload    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextDistortion  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextNaniteSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
            m_NextBeamBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
