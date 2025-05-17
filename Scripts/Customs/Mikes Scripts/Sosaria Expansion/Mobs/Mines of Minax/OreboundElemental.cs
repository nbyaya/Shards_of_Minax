using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an orebound elemental corpse")]
    public class OreboundElemental : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextMagnetTime;
        private DateTime m_NextSeismicTime;
        private DateTime m_NextShardBarrageTime;

        // For tracking movement for passive effect
        private Point3D m_LastLocation;

        // Unique metallic hue
        private const int UniqueHue = 2600; // a dark iron-gray with glints

        [Constructable]
        public OreboundElemental() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name            = "an orebound elemental";
            Body            = 107;
            BaseSoundID     = 268;
            Hue             = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(100, 140);

            SetHits(2000, 2300);
            SetStam(200, 250);
            SetMana(0); // no spells

            // Physical brawler
            SetDamage(30, 40);
            SetDamageType(ResistanceType.Physical, 100);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   20, 30);
            SetResistance(ResistanceType.Energy,   30, 40);

            // Skills
            SetSkill(SkillName.Tactics,      100.1, 120.0);
            SetSkill(SkillName.Wrestling,    100.1, 120.0);
            SetSkill(SkillName.MagicResist,   80.0, 100.0);

            Fame            = 18000;
            Karma           = -18000;
            VirtualArmor    = 70;
            ControlSlots    = 4;

            // Start cooldowns staggered
            m_NextMagnetTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSeismicTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 18));
            m_NextShardBarrageTime= DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation        = this.Location;

            // Initial ore loot
            PackItem(new IronOre(Utility.RandomMinMax(20, 40)));
            PackGold(Utility.RandomMinMax(1500, 2500));
        }

        // Passive: on movement leaves a tiny ore fragment tile
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var dropLoc = m_LastLocation;
                if (Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    LandmineTile rockSpike = new LandmineTile();
                    rockSpike.Hue = UniqueHue;
                    rockSpike.MoveToWorld(dropLoc, this.Map);
                }
            }
            m_LastLocation = this.Location;
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Magnetic Pull (every 10–15s, range 8)
            if (DateTime.UtcNow >= m_NextMagnetTime && InRange(Combatant.Location, 8))
            {
                DoMagneticPulse();
                m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Seismic Slam (every 15–20s, AoE 6)
            else if (DateTime.UtcNow >= m_NextSeismicTime && InRange(Combatant.Location, 10))
            {
                DoSeismicSlam();
                m_NextSeismicTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Ore Shard Barrage (every 20–30s, ranged multi-target)
            else if (DateTime.UtcNow >= m_NextShardBarrageTime && InRange(Combatant.Location, 12))
            {
                DoShardBarrage();
                m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // === Ability 1: Magnetic Pulse ===
        private void DoMagneticPulse()
        {
            this.Say("*The earth trembles!*");
            PlaySound(0x2F3);

            foreach (var m in Map.GetMobilesInRange(Location, 8))
            {
                if (m == this || !CanBeHarmful(m, false) || !(m is Mobile target)) continue;

                // Pull target toward elemental
                int dx = X - target.X;
                int dy = Y - target.Y;
                Point3D pullTo = new Point3D(target.X + Math.Sign(dx)*2, target.Y + Math.Sign(dy)*2, target.Z);

                if (Map.CanFit(pullTo.X, pullTo.Y, pullTo.Z, 16, false, false))
                    target.Location = pullTo;

                DoHarmful(target);
                int dmg = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);
                target.FixedParticles(0x3779, 10, 15, 9502, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // === Ability 2: Seismic Slam ===
        private void DoSeismicSlam()
        {
            this.Say("*Feel the weight of stone!*");
            PlaySound(0x1F7);

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x375A, 20, 30, UniqueHue, 0, 5033, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (var o in Map.GetMobilesInRange(Location, 6))
            {
                if (o != this && CanBeHarmful(o, false) && o is Mobile target)
                    targets.Add(target);
            }

            foreach (var tgt in targets)
            {
                DoHarmful(tgt);
                int dmg = Utility.RandomMinMax(35, 50);
                AOS.Damage(tgt, this, dmg, 100, 0, 0, 0, 0);

                // Create earthquake tile beneath them
                var tile = new EarthquakeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(tgt.Location, Map);
            }
        }

        // === Ability 3: Ore Shard Barrage ===
        private void DoShardBarrage()
        {
            this.Say("*Shards of the deep!*");
            PlaySound(0x208);

            var initial = Combatant as Mobile;
            if (initial == null || !CanBeHarmful(initial, false)) return;

            List<Mobile> chain = new List<Mobile> { initial };
            int max = 4;

            for (int i = 0; i < max; i++)
            {
                Mobile last = chain[chain.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (var m in Map.GetMobilesInRange(last.Location, 8))
                {
                    if (m == this || chain.Contains(m) || !(m is Mobile cand) || !CanBeHarmful(cand, false))
                        continue;

                    double d = last.GetDistanceToSqrt(cand);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        next = cand;
                    }
                }

                if (next == null) break;
                chain.Add(next);
            }

            for (int i = 0; i < chain.Count; i++)
            {
                var src = (i == 0 ? this : chain[i - 1]);
                var tgt = chain[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(tgt, false))
                    {
                        DoHarmful(tgt);
                        int dmg = Utility.RandomMinMax(25, 35);
                        AOS.Damage(tgt, this, dmg, 0, 0, 0, 0, 100);
                        tgt.FixedParticles(0x376A, 8, 10, 9502, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // === Death Effect: Scattering Ore Landmines ===
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("*The core shatters!*");
            PlaySound(0x3D);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 40, UniqueHue, 0, 5052, 0);

            int mines = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < mines; i++)
            {
                int ox = Utility.RandomMinMax(-3, 3);
                int oy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + ox, Y + oy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                LandmineTile spike = new LandmineTile();
                spike.Hue = UniqueHue;
                spike.MoveToWorld(loc, Map);
            }
        }

        public override bool BleedImmune   => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus     => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(5, 10));
            AddLoot(LootPack.MedScrolls,  Utility.RandomMinMax(1, 3));

            // Rare ore core
            if (Utility.RandomDouble() < 0.05)
                PackItem(new HonorboundTreads());
        }

        public OreboundElemental(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on restart
            m_NextMagnetTime       = DateTime.UtcNow + TimeSpan.FromSeconds(8);
            m_NextSeismicTime      = DateTime.UtcNow + TimeSpan.FromSeconds(14);
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_LastLocation         = this.Location;
        }
    }
}
