using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a skeletal horse corpse")]
    public class PharaohsSteed : BaseCreature
    {
        private DateTime m_NextChargeTime;
        private DateTime m_NextCurseTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Rich golden‐sand glow
        private const int UniqueHue = 2209;

        [Constructable]
        public PharaohsSteed() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "the Pharaoh's Steed";
            Body = 0xC8;       // horse body
            BaseSoundID = 0xA8; 
            Hue = UniqueHue;

            // —— Stats ——
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(2000, 2200);
            SetStam(300, 350);
            SetMana(100);

            SetDamage(20, 30);

            // 60% Physical, 40% Poison
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            // —— Skills ——
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Poisoning, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // —— Ability cooldowns ——            
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            m_LastLocation = this.Location;

            // —— Loot ——
            PackGold(1000, 1500);
            PackGem();
            if (Utility.RandomDouble() < 0.05)
                PackItem(new NecklaceOfTheEquineLord());
        }

        public PharaohsSteed(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && Map != null && Alive && target.Alive)
            {
                // Sandstorm Charge: frontal dash
                if (DateTime.UtcNow >= m_NextChargeTime && InRange(target, 10) && CanBeHarmful(target, false))
                {
                    SandstormCharge(target);
                    m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                }
                // Curse of the Pharaoh: heavy poison
                else if (DateTime.UtcNow >= m_NextCurseTime && InRange(target, 8) && CanBeHarmful(target, false))
                {
                    CurseOfThePharaoh(target);
                    m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
                }
                // Summon Skeletal Remnant
                else if (DateTime.UtcNow >= m_NextSummonTime)
                {
                    SummonSkeletalRemnant();
                    m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
                }
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave quicksand traps behind
            if (m_LastLocation != this.Location && Utility.RandomDouble() < 0.20)
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
        }

        private void SandstormCharge(Mobile target)
        {
            Say("*By Ra's fury!*");
            PlaySound(0x2F3);
            FixedParticles(0x375A, 8, 20, 9552, UniqueHue, 0, EffectLayer.CenterFeet);

            // Move three steps toward target
            for (int i = 0; i < 3; i++)
                this.Move(GetDirectionTo(target.Location));

            // Damage and knockback any in front
            var list = Map.GetMobilesInRange(Location, 3);
            foreach (var m in list)
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && m.InRange(Location, 2))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                }
            }
        }

        private void CurseOfThePharaoh(Mobile target)
        {
            Say("*Taste the sands of eternity!*");
            PlaySound(0x227);
            if (target is Mobile t && CanBeHarmful(t, false))
            {
                DoHarmful(t);
                t.ApplyPoison(this, Poison.Lethal);
                t.FixedParticles(0x3789, 10, 15, 5011, UniqueHue, 0, EffectLayer.Head);
            }
        }

        private void SummonSkeletalRemnant()
        {
            Say("*Rise, my steed!*");
            PlaySound(0x3E7);
            int count = Utility.RandomMinMax(1, 2);

            for (int i = 0; i < count; i++)
            {
                var bone = new Skeleton(); // assumes you have a skeleton horse class
                Point3D spawn = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                bone.MoveToWorld(spawn, Map);
                bone.Combatant = Combatant;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            // Scatter landmines around corpse
            Say("*My curse remains!*");
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3728, 8, 20, UniqueHue, 0, 5039, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus => 75.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCurseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation   = this.Location;
        }
    }
}
