using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EtherealPlaneChest : WoodenChest
    {
        [Constructable]
        public EtherealPlaneChest()
        {
            Name = "Ethereal Plane Chest";
            Hue = 2170;

            // Add items to the chest
            AddItem(CreateEmerald(), 0.19);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel4>("Treasure of the Dreamers"), 0.14);
            AddItem(CreateColoredItem<GoldEarrings>("Illusion's Loop Earring", 1285), 0.16);
            AddItem(new Gold(Utility.Random(1, 6000)), 0.16);
            AddItem(CreateNamedItem<Apple>("Phantom Apple"), 0.11);
            AddItem(CreateNamedItem<GreaterHealPotion>("Dreamer's Brew"), 0.13);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Mirage", 1169), 0.14);
            AddItem(CreateRandomGem(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Dreamseer's Spyglass"), 0.15);
            AddItem(CreateRandomWand(), 0.14);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateWizardsHat(), 0.20);
            AddItem(CreateChainArms(), 0.20);
            AddItem(CreateGnarledStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Spirit of the Ether";
            return emerald;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The unseen winds carry tales of dreams.";
            note.TitleString = "Ethereal Echoes";
            return note;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateRandomGem()
        {
            Ruby gem = new Ruby();
            gem.Name = "Crystal of the Dream";
            return gem;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Wand of Illusions";
            return wand;
        }

        private Item CreateWeapon()
        {
            Longsword weapon = new Longsword(); // Replace with specific weapon if needed
            weapon.Name = "Blade of the Ethereal";
            weapon.Hue = 1168;
            weapon.MaxDamage = Utility.Random(32, 72);
            return weapon;
        }

        private Item CreateWizardsHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Ranger's Hat";
            hat.Hue = Utility.RandomMinMax(100, 1100);
            hat.ClothingAttributes.ReactiveParalyze = 1;
            hat.Attributes.BonusInt = 10;
            hat.Attributes.LowerRegCost = 5;
            hat.SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
            hat.SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
            return hat;
        }

        private Item CreateChainArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Misfortune's Chains";
            arms.Hue = Utility.RandomMinMax(10, 300);
            arms.BaseArmorRating = Utility.Random(30, 70);
            arms.AbsorptionAttributes.EaterKinetic = 20;
            arms.ArmorAttributes.LowerStatReq = 5;
            arms.Attributes.IncreasedKarmaLoss = 20;
            arms.Attributes.Luck = -60;
            arms.SkillBonuses.SetValues(0, SkillName.Wrestling, -15.0);
            arms.ColdBonus = 10;
            arms.EnergyBonus = 15;
            arms.FireBonus = 10;
            arms.PhysicalBonus = 20;
            arms.PoisonBonus = 5;
            return arms;
        }

        private Item CreateGnarledStaff()
        {
            GnarledStaff staff = new GnarledStaff();
            staff.Name = "Staff of Aeons";
            staff.Hue = Utility.RandomMinMax(520, 720);
            staff.MinDamage = Utility.Random(20, 60);
            staff.MaxDamage = Utility.Random(60, 90);
            staff.Attributes.CastRecovery = 3;
            staff.Attributes.CastSpeed = 1;
            staff.Slayer = SlayerName.Ophidian;
            staff.WeaponAttributes.HitManaDrain = 20;
            staff.WeaponAttributes.MageWeapon = 1;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return staff;
        }

        public EtherealPlaneChest(Serial serial) : base(serial)
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
