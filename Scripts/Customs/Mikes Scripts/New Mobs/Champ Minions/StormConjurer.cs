using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a storm conjurer")]
    public class StormConjurer : BaseCreature
    {
        private TimeSpan m_StormDelay = TimeSpan.FromSeconds(10.0); // time between storm summons
        public DateTime m_NextStormTime;

        [Constructable]
        public StormConjurer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Storm Conjurer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Storm Conjurer";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 80.5, 100.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;

            m_NextStormTime = DateTime.Now + m_StormDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextStormTime)
            {
                SummonStorm();
                m_NextStormTime = DateTime.Now + m_StormDelay;
            }

            base.OnThink();
        }

        public void SummonStorm()
        {
            Map map = this.Map;

            if (map == null)
                return;

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0);
                    m.FixedParticles(0x374A, 10, 30, 5013, EffectLayer.Waist);
                    m.PlaySound(0x208);
                }
            }

            this.Say(true, "Feel the wrath of the storm!");
        }

        public override void GenerateLoot()
        {
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            if (Utility.Random(2) == 0)
                PackItem(new StormStaff());
        }

        public StormConjurer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class StormStaff : BaseStaff
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.WhirlwindAttack; } }

        public override int AosStrengthReq { get { return 30; } }
        public override int AosMinDamage { get { return 13; } }
        public override int AosMaxDamage { get { return 15; } }
        public override int AosSpeed { get { return 33; } }

        [Constructable]
        public StormStaff() : base(0x13F8)
        {
            Weight = 6.0;
            Hue = 1153; // Electric blue hue
            Name = "Storm Staff";
        }

        public StormStaff(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
