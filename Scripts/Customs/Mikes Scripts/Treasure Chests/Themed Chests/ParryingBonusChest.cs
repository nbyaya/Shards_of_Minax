using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ParryBonusChest : WoodenChest
    {
        [Constructable]
        public ParryBonusChest()
        {
            Name = "Chest of Parry Mastery";
            Hue = Utility.Random(1, 1153); // Choose a suitable hue for the chest

            // Add items to the chest
            AddItem(CreateHelmet(), 0.20);
            AddItem(CreateGloves(), 0.25);
            AddItem(CreateBracers(), 0.20);
            AddItem(CreateBoots(), 0.15);
            AddItem(CreateShield(), 0.10);
            AddItem(CreateWeapon(), 0.10);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateHelmet()
        {
            Cap helmet = new Cap();
            helmet.Name = "Cap of Parry";
            helmet.Attributes.DefendChance = 15;
            helmet.SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
            return helmet;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of Parry";
            gloves.Attributes.DefendChance = 10;
            gloves.SkillBonuses.SetValues(0, SkillName.Parry, 8.0);
            return gloves;
        }

        private Item CreateBracers()
        {
            LeatherArms bracers = new LeatherArms();
            bracers.Name = "Bracers of Parry";
            bracers.Attributes.DefendChance = 12;
            bracers.SkillBonuses.SetValues(0, SkillName.Parry, 12.0);
            return bracers;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Boots of Parry";
            boots.Attributes.DefendChance = 8;
            boots.SkillBonuses.SetValues(0, SkillName.Parry, 6.0);
            return boots;
        }

        private Item CreateShield()
        {
            WoodenShield shield = new WoodenShield();
            shield.Name = "Shield of Parry";
            shield.Attributes.DefendChance = 20;
            shield.SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
            return shield;
        }

        private Item CreateWeapon()
        {
            ShortSpear spear = new ShortSpear();
            spear.Name = "Spear of Parry";
            spear.Attributes.DefendChance = 10;
            spear.SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
            return spear;
        }

        public ParryBonusChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
