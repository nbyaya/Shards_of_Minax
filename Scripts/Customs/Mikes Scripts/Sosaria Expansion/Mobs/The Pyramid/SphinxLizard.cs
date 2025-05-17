using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sphinx lizard corpse")]
    public class SphinxLizard : BaseCreature
    {
        private DateTime m_NextSandstorm;
        private DateTime m_NextGaze;
        private DateTime m_NextRoar;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1367;

        [Constructable]
        public SphinxLizard()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a sphinx lizard";
            Body = 716;
            Hue = UniqueHue;
            BaseSoundID = 1511;

            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(400, 450);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 100.0, 115.0);
            SetSkill(SkillName.Magery, 100.0, 115.0);
            SetSkill(SkillName.MagicResist, 110.0, 125.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 70;
            ControlSlots = 5;

            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGaze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            PackItem(new MaxxiaScroll(Utility.RandomMinMax(2, 4)));
            PackItem(new GoldOre(Utility.RandomMinMax(4, 8)));

            m_LastLocation = this.Location;
        }

        public override int GetIdleSound() { return 1511; }
        public override int GetAngerSound() { return 1508; }
        public override int GetHurtSound() { return 1510; }
        public override int GetDeathSound() { return 1509; }

        public override void OnMovement(Mobile m, Point3D old)
        {
            base.OnMovement(m, old);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(5, 15);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x3B, "The shifting sands sap your strength!");
                        target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x1EE);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.20)
            {
                var sand = new QuicksandTile { Hue = UniqueHue };
                sand.MoveToWorld(m_LastLocation, this.Map);
            }

            m_LastLocation = this.Location;

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextRoar && this.InRange(Combatant.Location, 12))
            {
                RiddleRoar();
                m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextSandstorm && this.InRange(Combatant.Location, 16))
            {
                SummonSandstorm();
                m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextGaze && this.InRange(Combatant.Location, 10))
            {
                CurseGaze();
                m_NextGaze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        public void RiddleRoar()
        {
            this.Say("Can you answer my riddle?");
            PlaySound(0x212);

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                m.SendMessage(0x2A, "Your mind reels at the sphinx's question!");
                m.Paralyze(TimeSpan.FromSeconds(3.0));
                m.FixedParticles(0x374A, 5, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        public void SummonSandstorm()
        {
            this.Say("*The desert rises!*");
            PlaySound(0x3E3);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1.0)),
                0x3728, 10, 50, UniqueHue, 0, 5039, 0);

            foreach (Mobile m in Map.GetMobilesInRange(Location, 10))
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                    m.SendMessage(0x3C, "You choke on the stinging sands!");
                    m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                    m.PlaySound(0x1EE);

                    int stamDrain = Utility.RandomMinMax(10, 20);
                    if (m.Stam >= stamDrain)
                        m.Stam -= stamDrain;
                }
            }
        }

        public void CurseGaze()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("Feel the weight of ancient curses...");
                PlaySound(0x209);
                DoHarmful(target);

                target.ApplyPoison(this, Poison.Lesser);

                for (int i = 0; i < 5; i++)
                {
                    ResistanceMod mod = new ResistanceMod((ResistanceType)i, -10);
                    target.AddResistanceMod(mod);

                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                    {
                        target.RemoveResistanceMod(mod);
                    });
                }

                target.SendMessage(0x22, "Your defenses are sapped by the curse!");
                target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (Map == null)
				return;

            this.Say("*My final enigma...*");
            PlaySound(0x207);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var qs = new QuicksandTile { Hue = UniqueHue };
                qs.MoveToWorld(new Point3D(x, y, z), Map);

                var tox = new PoisonTile();
                tox.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 125.0;
        public override double DispelFocus => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new HuntingQueensTrail());
        }

        public SphinxLizard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextGaze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
