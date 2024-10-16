using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ShinobiSecretsChest : WoodenChest
    {
        [Constructable]
        public ShinobiSecretsChest()
        {
            Name = "Shinobi Secrets Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Nightshade Gem", 1124), 0.20);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel2>("Ninja's Hidden Cache"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Masked Moon Earring", 1175), 0.15);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.20);
            AddItem(CreateNamedItem<Apple>("Poisoned Apple"), 0.08);
            AddItem(CreateNamedItem<GreaterHealPotion>("Shadow Brew"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Silent Step", 1170), 0.19);
            AddItem(CreateNamedItem<Kama>("Shadow's Blade"), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Ninja's Vision"), 0.12);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Move with the shadow, strike with silence.";
            note.TitleString = "Shinobi Scroll";
            return note;
        }

        private Item CreateRandomLoot<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherLegs(), new LeatherArms(), new LeatherGloves());
            armor.Name = "Armor of the Night";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 60);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Lumberjack's Rugged Tunic";
            tunic.Hue = Utility.RandomMinMax(700, 1600);
            tunic.ClothingAttributes.DurabilityBonus = 4;
            tunic.Attributes.BonusStr = 10;
            tunic.Attributes.BonusStam = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 25.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Mystic's Guard";
            plateChest.Hue = Utility.RandomMinMax(200, 900);
            plateChest.BaseArmorRating = Utility.Random(40, 80);
            plateChest.AbsorptionAttributes.EaterEnergy = 15;
            plateChest.ArmorAttributes.MageArmor = 1;
            plateChest.Attributes.BonusMana = 40;
            plateChest.Attributes.CastSpeed = 1;
            plateChest.Attributes.SpellChanneling = 1;
            plateChest.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            plateChest.ColdBonus = 15;
            plateChest.EnergyBonus = 15;
            plateChest.FireBonus = 5;
            plateChest.PhysicalBonus = 10;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff blackStaff = new BlackStaff();
            blackStaff.Name = "The Oculus";
            blackStaff.Hue = Utility.RandomMinMax(250, 450);
            blackStaff.MinDamage = Utility.Random(20, 60);
            blackStaff.MaxDamage = Utility.Random(60, 90);
            blackStaff.Attributes.CastRecovery = 2;
            blackStaff.Attributes.SpellChanneling = 1;
            blackStaff.Attributes.BonusInt = 15;
            blackStaff.Slayer = SlayerName.ElementalHealth;
            blackStaff.WeaponAttributes.HitEnergyArea = 25;
            blackStaff.WeaponAttributes.MageWeapon = 1;
            blackStaff.SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            blackStaff.SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
            return blackStaff;
        }

        public ShinobiSecretsChest(Serial serial) : base(serial)
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
