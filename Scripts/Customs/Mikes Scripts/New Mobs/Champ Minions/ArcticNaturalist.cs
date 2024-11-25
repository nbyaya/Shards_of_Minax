using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an arctic naturalist")]
    public class ArcticNaturalist : BaseCreature
    {
        private TimeSpan m_CallDelay = TimeSpan.FromSeconds(30.0); // time between calls for animal assistance
        public DateTime m_NextCallTime;

        [Constructable]
        public ArcticNaturalist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Arctic Naturalist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Arctic Naturalist";
            }

            Item robe = new Robe();
            robe.Hue = 0x480; // a cool, icy blue hue
            AddItem(robe);

            Item boots = new Boots();
            boots.Hue = 0x370;
            AddItem(boots);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Cold, 80, 100); // Higher cold resistance
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 95.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 120.5, 130.0);
            SetSkill(SkillName.Tactics, 85.1, 90.0);
            SetSkill(SkillName.Wrestling, 85.1, 90.0);

            Fame = 4000;
            Karma = 4000;

            VirtualArmor = 50;

            m_NextCallTime = DateTime.Now + m_CallDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextCallTime && Combatant != null)
            {
                CallHelp();
                m_NextCallTime = DateTime.Now + m_CallDelay;
            }
        }

        private void CallHelp()
        {
            if (Utility.RandomBool())
            {
                Say(true, "Nature, aid me!");
                BaseCreature helper = new PolarBear();
                helper.Team = this.Team;
                helper.MoveToWorld(this.Location, this.Map);
            }
            else
            {
                Say(true, "To my side, creatures of the wild!");
                BaseCreature helper = new GreyWolf();
                helper.Team = this.Team;
                helper.MoveToWorld(this.Location, this.Map);
            }
        }

        public ArcticNaturalist(Serial serial) : base(serial)
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
