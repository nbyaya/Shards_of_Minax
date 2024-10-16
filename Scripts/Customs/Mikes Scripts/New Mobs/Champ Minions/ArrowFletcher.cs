using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an arrow fletcher")]
    public class ArrowFletcher : BaseCreature
    {
        private TimeSpan m_CraftDelay = TimeSpan.FromSeconds(5.0); // time between arrow crafting
        public DateTime m_NextCraftTime;

        [Constructable]
        public ArrowFletcher() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Arrow Fletcher";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Arrow Fletcher";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(boots);

            Item bow = new Bow();
            AddItem(bow);
            bow.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(300, 400);
            SetDex(250, 350);
            SetInt(100, 200);

            SetHits(200, 300);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Archery, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.Anatomy, 75.1, 90.0);
            SetSkill(SkillName.MagicResist, 50.0, 70.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextCraftTime = DateTime.Now + m_CraftDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextCraftTime)
            {
                CraftArrows();
                m_NextCraftTime = DateTime.Now + m_CraftDelay;
            }
        }

        private void CraftArrows()
        {
            if (Backpack != null)
            {
                Arrow arrows = new Arrow(Utility.RandomMinMax(5, 10));
                Backpack.DropItem(arrows);
                this.Say(true, "I have crafted some arrows.");
            }
        }

        public ArrowFletcher(Serial serial) : base(serial)
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
