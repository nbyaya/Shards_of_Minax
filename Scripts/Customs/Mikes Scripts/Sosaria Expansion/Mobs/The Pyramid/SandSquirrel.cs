using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; 

namespace Server.Mobiles
{
    [CorpseName("a sand squirrel corpse")]
    public class SandSquirrel : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSandstormTime;
        private DateTime m_NextPitTime;
        private DateTime m_NextBarrageTime;
        private Point3D m_LastLocation;

        // Unique sand‑gold hue
        private const int UniqueHue = 1369;

        [Constructable]
        public SandSquirrel()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sand squirrel";
            Body = 0x116;                // Use the squirrel body
            BaseSoundID = 0x7F3;         // Squirrel‑like chittering
            Hue = UniqueHue;

            // — Stats —
            SetStr(150, 180);
            SetDex(120, 150);
            SetInt(100, 120);

            SetHits(700, 900);
            SetStam(150, 200);
            SetMana(300, 400);

            SetDamage(10, 15);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 10);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            // — Skills —
            SetSkill(SkillName.EvalInt, 80.0, 95.0);
            SetSkill(SkillName.Magery, 85.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 105.0);
            SetSkill(SkillName.Meditation, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize ability cooldowns
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPitTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextBarrageTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Loot
            PackGold(500, 750);
            PackItem(new BonePile(Utility.RandomMinMax(2, 5)));  // Bones
            PackItem(new Granite(Utility.RandomMinMax(5, 10))); // Desert stone
        }

        public SandSquirrel(Serial serial)
            : base(serial)
        {
        }

        // — Leave quicksand pits as it moves —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (Map == null || Map == Map.Internal)
                return;

            // 20% chance to drop a quicksand tile where it just was
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D drop = m_LastLocation;
                int z = drop.Z;

                if (!Map.CanFit(drop.X, drop.Y, z, 16, false, false))
                    z = Map.GetAverageZ(drop.X, drop.Y);

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(new Point3D(drop.X, drop.Y, z), Map);
            }

            m_LastLocation = this.Location;
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // Sandstorm: AoE whirlwind of blades + blind
            if (now >= m_NextSandstormTime && InRange(Combatant.Location, 8))
            {
                SandstormAttack();
                m_NextSandstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                return;
            }

            // Bark in pit: targeted quicksand trap under the foe
            if (now >= m_NextPitTime && InRange(Combatant.Location, 12))
            {
                QuicksandPitAttack();
                m_NextPitTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // Bone Shard Barrage: chain projectile
            if (now >= m_NextBarrageTime && InRange(Combatant.Location, 10))
            {
                BoneShardBarrage();
                m_NextBarrageTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
        }

        // — Sandstorm: AoE damage + blinding grit —
        private void SandstormAttack()
        {
            PlaySound(0x2F3);
            FixedParticles(0x3709, 10, 30, 5022, UniqueHue, 0, EffectLayer.Waist);

            var targets = new List<Mobile>();
            foreach (var obj in Map.GetMobilesInRange(Location, 8))
            {
                if (obj != this && CanBeHarmful(obj, false) && SpellHelper.ValidIndirectTarget(this, obj))
                    targets.Add(obj);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(35, 55);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);  // 100% physical
                m.SendLocalizedMessage(1061687);           // You are blinded by sand! (example)
                m.FixedParticles(0x3779, 5, 15, 5032, 0, 0, EffectLayer.Head);

                // 50% chance to blind (simulate with paralysis)
                if (Utility.RandomDouble() < 0.50)
                    m.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
            }
        }

        // — Quicksand Pit: spawn trap under the target —
        private void QuicksandPitAttack()
        {
            if (!(Combatant is Mobile target))
                return;

            PlaySound(0x228);
            Point3D loc = target.Location;
            int z = loc.Z;

            if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                z = Map.GetAverageZ(loc.X, loc.Y);

            var pit = new QuicksandTile();
            pit.Hue = UniqueHue;
            pit.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);

            DoHarmful(target);
            target.SendMessage("The sands rise to swallow you!");
        }

        // — Bone Shard Barrage: chain physical projectiles —
        private void BoneShardBarrage()
        {
            PlaySound(0x23D);
            var hits = new List<Mobile>();

            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false) || !SpellHelper.ValidIndirectTarget(this, initial))
                return;

            hits.Add(initial);
            int maxBounces = 3, range = 6;

            for (int i = 1; i < maxBounces; i++)
            {
                var last = hits[i - 1];
                Mobile nextTarget = null;
                double bestDist = double.MaxValue;

                foreach (var m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && m != last && !hits.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist) { bestDist = d; nextTarget = m; }
                    }
                }

                if (nextTarget != null)
                    hits.Add(nextTarget);
                else
                    break;
            }

            for (int i = 0; i < hits.Count; i++)
            {
                var src = (i == 0 ? this : hits[i - 1]);
                var dst = hits[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36F4, 5, 0, false, false, 0, 0, 0, 0, 0, EffectLayer.Head, 0);

                int dmg = Utility.RandomMinMax(20, 35);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, dmg, 100, 0, 0, 0, 0);
                        dst.ApplyPoison(this, Poison.Lesser);
                    }
                });
            }
        }

        // — Death Effect: collapsing sands and hazards —
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            PlaySound(0x2A6);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x370A, 10, 30, UniqueHue, 0, 5022, 0);

            // Scatter quicksand and poison gas around corpse
            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                int z = loc.Z;

                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                var hazard = (Utility.RandomBool() ? (Item)new QuicksandTile() : new PoisonTile());
                hazard.Hue = UniqueHue;
                hazard.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 8));

            // 3% chance for a unique desert‑themed artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new VerdantEmbrace()); 
        }

        public override bool BleedImmune   { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus      { get { return 60.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init ability timers
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPitTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextBarrageTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
