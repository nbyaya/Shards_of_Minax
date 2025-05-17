using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bronze of the deep corpse")]
    public class BronzeOfTheDeep : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextSeismicSlam;
        private DateTime m_NextMagneticPulse;
        private DateTime m_NextMoltenJet;
        private Point3D m_LastLocation;

        // Unique bronze–deep blue hue
        private const int UniqueHue = 2053;

        [Constructable]
        public BronzeOfTheDeep()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "the Bronze of the Deep";
            Body = 108;
            BaseSoundID = 268;
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(600, 700);
            SetDex(150, 200);
            SetInt(80, 100);

            SetHits(2000, 2300);
            SetStam(200, 250);
            SetMana(0); // pure melee

            SetDamage(20, 30);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            // ——— Skills ———
            SetSkill(SkillName.Tactics, 120.1, 130.0);
            SetSkill(SkillName.Wrestling, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // ——— Initial Cooldowns ———
            m_NextSeismicSlam   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextMagneticPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMoltenJet     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_LastLocation      = this.Location;

            // ——— Loot ———
            PackItem(new BronzeOre(Utility.RandomMinMax(50, 75)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new TheMoonherder()); // your custom artifact

            AddLoot(LootPack.Gems, 5);
            AddLoot(LootPack.Rich);
        }

        // ——— Reactive Aura: Metallic Resonance ———
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if (m != this && Alive && m.Map == Map && m.InRange(Location, 2) && m.Alive && CanBeHarmful(m, false))
            {
                // Metal shards whirr off its body
                m.FixedParticles(0x3709, 10, 15, 5012, UniqueHue, 0, EffectLayer.Waist);
                m.PlaySound(0x2F1);

                // Burst bleed
                AOS.Damage(m, this, Utility.RandomMinMax(5, 10),
                    100, 0, 0, 0, 0); // 100% Physical
                m.ApplyPoison(this, Poison.Deadly);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextSeismicSlam && InRange(Combatant.Location, 1))
            {
                SeismicSlam();
                m_NextSeismicSlam = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
                return;
            }

            if (DateTime.UtcNow >= m_NextMagneticPulse && InRange(Combatant.Location, 8))
            {
                MagneticPulse();
                m_NextMagneticPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            if (DateTime.UtcNow >= m_NextMoltenJet && InRange(Combatant.Location, 12))
            {
                MoltenJet();
                m_NextMoltenJet = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                return;
            }
        }

        private void SeismicSlam()
        {
            Say("*The earth trembles!*");
            PlaySound(0x2F5);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 20, 60, UniqueHue, 0, 5039, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);

                    // big AoE damage
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                    // stun → paralyze instead of Stun()
                    m.Paralyze(TimeSpan.FromSeconds(1.0));  // :contentReference[oaicite:0]{index=0}

                    // knockback one tile
                    KnockBack(m, 1);
                }
            }
            eable.Free();

            // leave quake tile
            EarthquakeTile quake = new EarthquakeTile { Hue = UniqueHue };
            quake.MoveToWorld(Location, Map);

            m_LastLocation = Location;
        }

        private void MagneticPulse()
        {
            Say("*Feel the pull of the deep!*");
            PlaySound(0x1FA);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x370A, 15, 40, UniqueHue, 0, 5021, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);

                    // pull them in one tile
                    PullIn(m, 1);
                    m.SendMessage(0x22, "You are yanked toward the Bronze of the Deep!");
                }
            }
            eable.Free();

            m_LastLocation = Location;
        }

        private void MoltenJet()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Molten bronze!*");
            PlaySound(0x208);

            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (!Alive || Map == null) return;

                HotLavaTile lava = new HotLavaTile { Hue = UniqueHue };
                var loc = target.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                lava.MoveToWorld(loc, Map);
            });
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Say("*My forge... undone!*");
            PlaySound(0x2F6);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 60, UniqueHue, 0, 5052, 0);

            // scatter mines
            int count = Utility.RandomMinMax(4, 6);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile { Hue = UniqueHue };
                mine.MoveToWorld(loc, Map);
            }
        }

        // ——— Helpers for knockback/pull ———

        private void KnockBack(Mobile target, int tiles)
        {
            var dx = target.X - X;
            var dy = target.Y - Y;
            var nx = target.X + Math.Sign(dx) * tiles;
            var ny = target.Y + Math.Sign(dy) * tiles;
            var nz = target.Z;
            if (!Map.CanFit(nx, ny, nz, target.Body, false, false))
                nz = Map.GetAverageZ(nx, ny);
            target.MoveToWorld(new Point3D(nx, ny, nz), Map);
        }

        private void PullIn(Mobile target, int tiles)
        {
            var dx = X - target.X;
            var dy = Y - target.Y;
            var nx = target.X + Math.Sign(dx) * tiles;
            var ny = target.Y + Math.Sign(dy) * tiles;
            var nz = target.Z;
            if (!Map.CanFit(nx, ny, nz, target.Body, false, false))
                nz = Map.GetAverageZ(nx, ny);
            target.MoveToWorld(new Point3D(nx, ny, nz), Map);
        }

        // … the rest of your immunities, serialization, etc. unchanged …
		public BronzeOfTheDeep(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // timings not preserved across saves
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on load
            var now = DateTime.UtcNow;
            m_NextSeismicSlam   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextMagneticPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMoltenJet     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_LastLocation      = this.Location;
        }		
    }
}
