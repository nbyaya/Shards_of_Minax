using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shard master corpse")]
    public class ShardMaster : BaseCreature
    {
        private DateTime m_NextShatterTime;
        private DateTime m_NextPillarTime;
        private DateTime m_NextBarrageTime;
        private Point3D m_LastLocation;

        // Unique icy‑crystal hue
        private const int UniqueHue = 1125;

        [Constructable]
        public ShardMaster()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 2, 0.2, 0.4)
        {
            Name = "Shard Master";
            Body = 306;                 // Impaler body
            BaseSoundID = 0x2A7;        // Impaler sound
            Hue = UniqueHue;

            // — Significantly boosted stats —
            SetStr(350, 400);
            SetDex(180, 220);
            SetInt(300, 350);

            SetHits(8000, 9000);
            SetStam(300, 350);
            SetMana(1000, 1200);

            SetDamage(50, 60);

            // — Damage types —
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 90, 95);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 90, 95);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 85, 90);

            // — Skills —
            SetSkill(SkillName.Magery, 120.1, 140.0);
            SetSkill(SkillName.EvalInt, 115.1, 130.0);
            SetSkill(SkillName.MagicResist, 120.1, 140.0);
            SetSkill(SkillName.Meditation, 100.1, 115.0);
            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.Wrestling, 100.1, 115.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextShatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPillarTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation    = this.Location;

            // Basic loot
            PackItem(new MaxxiaScroll(Utility.RandomMinMax(5, 10)));
            PackGold(15000, 25000);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave small shard hazards behind
            if (this.Location != m_LastLocation && Map != null && Utility.RandomDouble() < 0.2)
            {
                Point3D drop = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                {
                    IceShardTile tile = new IceShardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(drop, Map);
                }
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

            var now = DateTime.UtcNow;

            if (now >= m_NextBarrageTime && InRange(Combatant.Location, 12))
            {
                ShardBarrage();
                m_NextBarrageTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextPillarTime && InRange(Combatant.Location, 14))
            {
                CrystalPillar();
                m_NextPillarTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            else if (now >= m_NextShatterTime && InRange(Combatant.Location, 8))
            {
                CrystalShatter();
                m_NextShatterTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // — AoE burst of razor shards + cold damage —
        public void CrystalShatter()
        {
            PlaySound(0x2A8); // custom shard break sound
            FixedParticles(0x37B3, 20, 30, 5012, UniqueHue, 0, EffectLayer.Head);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 70, 0, 30);
                m.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }

            // leave shard hazards on ground
            for (int i = 0; i < 4; i++)
            {
                var offset = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z);
                if (Map.CanFit(offset, 16, false, false))
                {
                    IceShardTile tile = new IceShardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(offset, Map);
                }
            }
        }

        // — Summon one or more crystal pillars at the combatant’s feet —
        public void CrystalPillar()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Rise... crystal throne!*");
                PlaySound(0x658);

                var loc = target.Location;
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x377A, 10, 20, UniqueHue, 0, 5039, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;

                    // Place a damaging pillar tile
                    IceShardTile pillar = new IceShardTile();
                    pillar.Hue = UniqueHue;
                    pillar.MoveToWorld(loc, Map);
                });
            }
        }

        // — Chain‑style shard projectiles hitting up to 4 targets —
        public void ShardBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Shards, obey me!*");
            PlaySound(0x658);

            var targets = new List<Mobile> { initial };
            int max = 4, range = 8;

            for (int i = 1; i < max; i++)
            {
                var last = targets[i - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (var m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !targets.Contains(m) && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }

                if (next != null)
                    targets.Add(next);
                else
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dest = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dest.Location, dest.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                int dmg = Utility.RandomMinMax(30, 45);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (dest.Alive && CanBeHarmful(dest, false))
                    {
                        DoHarmful(dest);
                        AOS.Damage(dest, this, dmg, 0, 0, 50, 0, 50);
                        dest.FixedParticles(0x376A, 8, 20, 5032, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("*Shards... remain.*");
            PlaySound(0x2A8);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x37B3, 20, 30, UniqueHue, 0, 5032, 0);

            // Scatter hazard tiles around corpse
            for (int i = 0; i < 6; i++)
            {
                var p = new Point3D(
                    X + Utility.RandomMinMax(-4, 4),
                    Y + Utility.RandomMinMax(-4, 4),
                    Z);
                if (Map.CanFit(p, 16, false, false))
                {
                    IceShardTile tile = new IceShardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(p, Map);
                }
            }
        }

        // — Standard properties & loot overrides —
        public override bool BleedImmune  => true;
        public override Poison HitPoison  => Poison.Greater;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 160.0;
        public override double DispelFocus     => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 20));

            if (Utility.RandomDouble() < 0.03)
                PackItem(new MaxxiaScroll());  // 3% chance for unique helm
        }

        public ShardMaster(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑initialize timers
            m_NextShatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPillarTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation    = this.Location;
        }
    }
}
