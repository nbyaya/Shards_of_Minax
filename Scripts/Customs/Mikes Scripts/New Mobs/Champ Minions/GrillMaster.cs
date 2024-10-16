using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a grill master")]
    public class GrillMaster : BaseCreature
    {
        private TimeSpan m_FireDamageDelay = TimeSpan.FromSeconds(15.0); // time between fire damage
        public DateTime m_NextFireDamageTime;

        [Constructable]
        public GrillMaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Grill Master";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Grill Master";
            }

            Item apron = new Server.Items.FullApron();
            AddItem(apron);

            Item hat = new Server.Items.Bonnet();
            AddItem(hat);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new ShortPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(pants);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextFireDamageTime = DateTime.Now + m_FireDamageDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextFireDamageTime)
            {
                foreach (Mobile m in this.GetMobilesInRange(3))
                {
                    if (m != this && m.Player && m.Alive && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 100, 0, 0, 0, 0);
                        m.SendMessage("You are scorched by the grill master's flames!");
                    }
                }
                m_NextFireDamageTime = DateTime.Now + m_FireDamageDelay;
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);
            PackItem(new RawRibs(Utility.RandomMinMax(1, 5)));
        }

        public GrillMaster(Serial serial) : base(serial)
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
