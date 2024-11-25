using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a sword defender")]
    public class SwordDefender : BaseCreature
    {
        [Constructable]
        public SwordDefender() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Sword Defender";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Sword Defender";
            }

            Item armor = new ChainChest();
            AddItem(armor);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new PlateLegs();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new Longsword();
            AddItem(hair);
            AddItem(pants);
            AddItem(weapon);
            weapon.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(800, 1000);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(600, 800);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 100.0);
            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Parry, 100.0, 120.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;
        }

        public override bool AlwaysMurderer { get { return true; } }

        public SwordDefender(Serial serial) : base(serial)
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
