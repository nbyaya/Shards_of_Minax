using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WearableFencingChest : WoodenChest
    {
        [Constructable]
        public WearableFencingChest()
        {
            Name = "Fencing Champion's Chest";
            Hue = Utility.Random(1, 1157); // Set to a random hue

            // Add items to the chest
            AddItem(CreateFencingWeapon(), 0.20);
            AddItem(CreateFencingArmor(), 0.30);
            AddItem(CreateFencingBoots(), 0.25);
            AddItem(CreateFencingRing(), 0.15);
            AddItem(CreateFencingBracelet(), 0.10);
            AddItem(CreateFencingCloak(), 0.10);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateFencingWeapon()
        {
            // Create a weapon with Fencing bonus
            Longsword sword = new Longsword();
            sword.Name = "Fencer's Blade";
            sword.Hue = Utility.Random(1, 1157);
            sword.MinDamage = Utility.Random(15, 30);
            sword.MaxDamage = Utility.Random(30, 50);
            sword.Attributes.BonusDex = 5;
            sword.Attributes.SpellDamage = 10;
            sword.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0); // Bonus to Fencing skill
            return sword;
        }

        private Item CreateFencingArmor()
        {
            // Create armor with Fencing bonus
            LeatherChest armor = new LeatherChest();
            armor.Name = "Fencing Champion's Armor";
            armor.Hue = Utility.Random(1, 1157);
            armor.Attributes.BonusDex = 10;
            armor.SkillBonuses.SetValues(0, SkillName.Fencing, 15.0); // Bonus to Fencing skill
            return armor;
        }

        private Item CreateFencingBoots()
        {
            // Create boots with Fencing bonus
            Boots boots = new Boots();
            boots.Name = "Fencer's Boots";
            boots.Hue = Utility.Random(1, 1157);
            boots.Attributes.BonusDex = 5;
            boots.SkillBonuses.SetValues(0, SkillName.Fencing, 10.0); // Bonus to Fencing skill
            return boots;
        }

        private Item CreateFencingRing()
        {
            // Create a ring with Fencing bonus
            GoldRing ring = new GoldRing();
            ring.Name = "Ring of Fencing Mastery";
            ring.Hue = Utility.Random(1, 1157);
            ring.Attributes.BonusDex = 7;
            ring.SkillBonuses.SetValues(0, SkillName.Fencing, 12.0); // Bonus to Fencing skill
            return ring;
        }

        private Item CreateFencingBracelet()
        {
            // Create a bracelet with Fencing bonus
            GoldRing ring = new GoldRing();
            ring.Name = "Bracelet of the Fencer";
            ring.Hue = Utility.Random(1, 1157);
            ring.Attributes.BonusDex = 5;
            ring.SkillBonuses.SetValues(0, SkillName.Fencing, 8.0); // Bonus to Fencing skill
            return ring;
        }

        private Item CreateFencingCloak()
        {
            // Create a cloak with Fencing bonus
            Cloak cloak = new Cloak();
            cloak.Name = "Cloak of Fencing";
            cloak.Hue = Utility.Random(1, 1157);
            cloak.Attributes.BonusDex = 10;
            cloak.SkillBonuses.SetValues(0, SkillName.Fencing, 15.0); // Bonus to Fencing skill
            return cloak;
        }

        public WearableFencingChest(Serial serial) : base(serial)
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
