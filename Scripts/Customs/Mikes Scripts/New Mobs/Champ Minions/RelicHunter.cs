using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a relic hunter")]
    public class RelicHunter : BaseCreature
    {
        private TimeSpan m_ItemUnearthDelay = TimeSpan.FromSeconds(30.0); // time between item unearthing
        public DateTime m_NextItemUnearthTime;

        [Constructable]
        public RelicHunter() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Relic Hunter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Relic Hunter";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            Item staff = new GnarledStaff();
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(boots);
            AddItem(staff);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(250, 350);

            SetHits(500, 700);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 45, 60);

            SetSkill(SkillName.Anatomy, 60.1, 80.0);
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 90.1, 110.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 90.1, 110.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 50;

            m_NextItemUnearthTime = DateTime.Now + m_ItemUnearthDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextItemUnearthTime)
            {
                UnearthItem();
                m_NextItemUnearthTime = DateTime.Now + m_ItemUnearthDelay;
            }

            base.OnThink();
        }

        private void UnearthItem()
        {
            if (Combatant != null && Combatant.Map == this.Map)
            {
                Item item = null;
                int randomEffect = Utility.Random(2);

                if (randomEffect == 0)
                {
                    item = new BuffItem();
                    this.Say(true, "The Relic Hunter has unearthed a relic that grants a temporary buff!");
                }
                else
                {
                    item = new DebuffItem();
                    this.Say(true, "The Relic Hunter has unearthed a relic that inflicts a temporary debuff!");
                }

                if (item != null)
                {
                    item.MoveToWorld(this.Location, this.Map);
                }
            }
        }

        public RelicHunter(Serial serial) : base(serial)
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

    public class BuffItem : Item
    {
        [Constructable]
        public BuffItem() : base(0x1F14)
        {
            Name = "Mystic Relic";
            Hue = 1153;
        }

        public BuffItem(Serial serial) : base(serial)
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

    public class DebuffItem : Item
    {
        [Constructable]
        public DebuffItem() : base(0x1F13)
        {
            Name = "Cursed Relic";
            Hue = 1175;
        }

        public DebuffItem(Serial serial) : base(serial)
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
