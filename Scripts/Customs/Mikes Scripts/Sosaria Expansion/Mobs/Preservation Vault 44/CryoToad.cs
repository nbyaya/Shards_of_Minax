using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cryo-toad corpse")]
    public class CryoToad : BaseCreature
    {
        private DateTime m_NextFrostAuraTime;
        private DateTime m_NextShardVolleyTime;
        private DateTime m_NextGlacialRiftTime;
        private Point3D m_LastLocation;

        // An icy cyan hue
        private const int UniqueHue = 1266;

        [Constructable]
        public CryoToad()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cryo-toad";
            Body = 80;
            BaseSoundID = 0x26B;
            Hue = UniqueHue;

            // — Core Stats —
            SetStr(300, 350);
            SetDex(100, 150);
            SetInt(200, 250);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(300, 350);

            SetDamage(20, 30);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold,     80);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     10, 20);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Energy,   50, 60);
            SetResistance(ResistanceType.Poison,   30, 40);

            // — Skills —
            SetSkill(SkillName.EvalInt,     90.0, 100.0);
            SetSkill(SkillName.Magery,      90.0, 100.0);
            SetSkill(SkillName.MagicResist,100.0, 110.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);
            SetSkill(SkillName.Tactics,     80.0,  90.0);
            SetSkill(SkillName.Wrestling,   80.0,  90.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 70;
            ControlSlots = 5;

            // — Initialize ability cooldowns —
            m_NextFrostAuraTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextShardVolleyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGlacialRiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // — Loot: ice reagents + chance at a Frost Core —
            PackItem(new BlackPearl(Utility.RandomMinMax(20, 30)));
            PackItem(new Apple(Utility.RandomMinMax(5, 10)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new WyrmlanceOfDrakkonsEnd());  
        }

        // — Chill Aura on Movement —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Chill effect: small cold damage + brief stamina drain
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                    int stamLoss = Utility.RandomMinMax(5, 15);
                    target.Stam = Math.Max(0, target.Stam - stamLoss);
                    target.SendMessage(0x3F, "You feel the chill slow your movements!");
                    target.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

                    // Leave a thin frost patch behind
                    if (m_LastLocation != this.Location && Utility.RandomDouble() < 0.2)
                    {
                        var tile = new IceShardTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(m_LastLocation, this.Map);
                    }
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Frost Aura Burst (AoE slow + damage)
            if (now >= m_NextFrostAuraTime && InRange(Combatant.Location, 6))
            {
                FrostAuraBurst();
                m_NextFrostAuraTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Ice Shard Volley (projectile barrage)
            else if (now >= m_NextShardVolleyTime && InRange(Combatant.Location, 10))
            {
                IceShardVolley();
                m_NextShardVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Glacial Rift (ground hazard)
            else if (now >= m_NextGlacialRiftTime && InRange(Combatant.Location, 12))
            {
                GlacialRift();
                m_NextGlacialRiftTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // — AoE Frost Nova —
        private void FrostAuraBurst()
        {
            PlaySound(0x5C4); // icy explosion
            FixedParticles(0x376A, 20, 60, 5021, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                    target.SendMessage(0x3F, "A burst of frost chills your bones!");
                    target.FixedParticles(0x377A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.Poison = Poison.Deadly; // brief cold poison
                }
            }
        }

        // — Shard Volley: series of ice-projectiles —
        private void IceShardVolley()
        {
            Say("*Croooak—shatter!*");
            PlaySound(0x217);

            if (!(Combatant is Mobile origin)) return;

            var targets = new List<Mobile> { origin };
            int maxBounces = 4;
            int bounceRange = 5;

            for (int i = 0; i < maxBounces; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double closest = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, bounceRange))
                {
                    if (m != this && m != last && !targets.Contains(m)
                        && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m)
                        && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < closest)
                        {
                            closest = d; next = m;
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
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                Mobile captured = dst;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (captured is Mobile target && CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);
                        target.SendMessage(0x3F, "An ice shard tears into you!");
                    }
                });
            }
        }

        // — Create a lingering ice hazard under the target —
        private void GlacialRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Ribbit—freeze!*");
            PlaySound(0x1F2);
            Point3D loc = target.Location;

            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                var spawn = loc;
                var map = Map;
                if (map == null) return;

                if (!map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = map.GetAverageZ(spawn.X, spawn.Y);

                var tile = new IceShardTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawn, map);
                Effects.PlaySound(spawn, map, 0x20F);
            });
        }

        // — Frost Explosion on Death —
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            PlaySound(0x5C4);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 20, 60, UniqueHue, 0, 5021, 0);

            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                var map = Map;

                if (!map.CanFit(x, y, z, 16, false, false))
                    z = map.GetAverageZ(x, y);

                var tile = new IceShardTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(x, y, z), map);
                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(x, y, z), map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }
        }

        // — Loot & Properties —
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,            Utility.RandomMinMax(6, 10));
            AddLoot(LootPack.MedScrolls,      Utility.RandomMinMax(2, 4));
        }

        public override bool BleedImmune      => true;
        public override Poison PoisonImmune   => Poison.Deadly;
        public override int TreasureMapLevel  => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public CryoToad(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers on load
            m_NextFrostAuraTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextShardVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGlacialRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
