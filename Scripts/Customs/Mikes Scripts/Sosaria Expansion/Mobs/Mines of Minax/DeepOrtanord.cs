using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a deep ortanord corpse")]
    public class DeepOrtanord : BaseCreature
    {
        private DateTime m_NextFissureTime;
        private DateTime m_NextMiasmaTime;
        private DateTime m_NextVolleyTime;
        private Point3D m_LastLocation;

        // A deep, blood‑red hue
        private const int UniqueHue = 2210;

        [Constructable]
        public DeepOrtanord() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Deep Ortanord";
            Body = 58;
            BaseSoundID = 466;
            Hue = UniqueHue;

            // — Stats —
            SetStr(150, 180);
            SetDex(120, 150);
            SetInt(250, 300);

            SetHits(2000, 2300);
            SetStam(200, 250);
            SetMana(1500, 1800);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 40);
            SetDamageType(ResistanceType.Energy, 50);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 80, 90);

            // — Skills —
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 115.0, 130.0);
            SetSkill(SkillName.MagicResist, 115.0, 125.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(12);

            m_LastLocation = this.Location;

            // Rich loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            if (Utility.RandomDouble() < 0.25)
                PackItem(new DaemonBone(20));
        }

        // Leave quicksand tiles behind as it moves
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || !this.Alive)
                return;

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var spot = m_LastLocation;
                if (!Map.CanFit(spot.X, spot.Y, spot.Z, 16, false, false))
                    spot.Z = Map.GetAverageZ(spot.X, spot.Y);

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(spot, this.Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !this.Alive || this.Map == null || this.Map == Map.Internal)
                return;

            // Chain abilities in priority order
            if (DateTime.UtcNow >= m_NextVolleyTime && this.InRange(Combatant.Location, 12))
            {
                ShadowShardVolley();
                m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            }
            else if (DateTime.UtcNow >= m_NextFissureTime && this.InRange(Combatant.Location, 10))
            {
                EruptingFissure();
                m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextMiasmaTime && this.InRange(Combatant.Location, 8))
            {
                PoisonousMiasma();
                m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
        }

        // — Ability #1: Shadow Shard Volley — 
        private void ShadowShardVolley()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*From the depths, shards emerge!*");
            PlaySound(0x207);

            var nearby = new List<Mobile> { target };
            // Attempt up to 4 extra bounces
            var eable = Map.GetMobilesInRange(this.Location, 8);
            foreach (Mobile m in eable)
            {
                if (nearby.Count >= 5) break;
                if (m != this && m != target && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    nearby.Add(m);
            }
            eable.Free();

            for (int i = 0; i < nearby.Count; i++)
            {
                var src = (i == 0 ? this : nearby[i - 1]);
                var dst = nearby[i];
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x374A, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile capture = dst;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(capture, false))
                    {
                        DoHarmful(capture);
                        AOS.Damage(capture, this, Utility.RandomMinMax(25, 35), 0, 0, 0, 40, 60);
                        capture.FixedParticles(0x3735, 10, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // — Ability #2: Erupting Fissure — 
        private void EruptingFissure()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*The earth rends beneath you!*");
            PlaySound(0x2F3);

            var loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x2285, 8, 20, UniqueHue, 0, 5027, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.6), () =>
            {
                if (this.Map == null) return;

                var fissure = new LandmineTile();
                fissure.Hue = UniqueHue;
                fissure.MoveToWorld(loc, this.Map);
                Effects.PlaySound(loc, this.Map, 0x292);
            });
        }

        // — Ability #3: Poisonous Miasma — 
        private void PoisonousMiasma()
        {
            Say("*Breathe deep the toxic depths!*");
            PlaySound(0x21A);

            var list = new List<Mobile>();
            var eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }
            eable.Free();

            foreach (var m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 100, 0);

                if (m is Mobile mob)
                    mob.ApplyPoison(this, Poison.Deadly);

                var pt = m.Location;
                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(pt, this.Map);
            }
        }

        // — Death: spill more hazards —
        public override void OnDeath(Container c)
        {
            if (this.Map != null)
            {
                Say("*Silence falls upon the depths...*");
                PlaySound(0x1FE);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 12, 40, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < Utility.RandomMinMax(5, 8); i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    var hazard = (Utility.RandomBool() 
                        ? (Item)new PoisonTile() 
                        : new QuicksandTile());
                    hazard.Hue = UniqueHue;
                    hazard.MoveToWorld(new Point3D(x, y, z), this.Map);
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 160.0; } }
        public override double DispelFocus    { get { return 75.0;  } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 3));
            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new OrtanordsCrownOfFrost());
        }

        public DeepOrtanord(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init cooldowns
            m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(12);

            m_LastLocation = this.Location;
        }
    }
}
