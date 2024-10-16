using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WarOf1812Vault : WoodenChest
    {
        [Constructable]
        public WarOf1812Vault()
        {
            Name = "War of 1812 Vault";
            Hue = Utility.Random(1, 1500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Brock's Badge"), 0.23);
            AddItem(CreateNamedItem<Spyglass>("Laura Secord's Insight"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel4>("Treaty of Ghent Relic"), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Militia's Mark"), 0.33);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.14);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateRobe(), 0.20);
            AddItem(CreateBoneChest(), 0.20);
            AddItem(CreateDagger(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "A time of courage and unity.";
            note.TitleString = "Soldier's Journal";
            return note;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Hatchet();
            weapon.Name = "Battleaxe of Queenston Heights";
            weapon.Hue = Utility.RandomList(1, 1500);
            weapon.MaxDamage = Utility.Random(3, 4); // Adjusting min and max damage to fit the range
            return weapon;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Scribe's Robe";
            robe.Hue = Utility.RandomMinMax(200, 1200);
            robe.ClothingAttributes.MageArmor = 1;
            robe.Attributes.BonusInt = 15;
            robe.SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
            robe.SkillBonuses.SetValues(1, SkillName.Magery, 20.0);
            return robe;
        }

        private Item CreateBoneChest()
        {
            BoneChest boneChest = new BoneChest();
            boneChest.Name = "Shaftstop Armor";
            boneChest.Hue = Utility.RandomMinMax(500, 850);
            boneChest.BaseArmorRating = Utility.Random(40, 80);
            boneChest.AbsorptionAttributes.EaterKinetic = 20;
            boneChest.Attributes.BonusHits = 50;
            boneChest.Attributes.ReflectPhysical = 20;
            boneChest.PhysicalBonus = 40;
            boneChest.EnergyBonus = 5;
            boneChest.FireBonus = 10;
            boneChest.ColdBonus = 10;
            boneChest.PoisonBonus = 10;
            return boneChest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Saxon Seax";
            dagger.Hue = Utility.RandomMinMax(100, 350);
            dagger.MinDamage = Utility.Random(15, 45);
            dagger.MaxDamage = Utility.Random(45, 75);
            dagger.Attributes.BonusStr = 10;
            dagger.Attributes.DefendChance = 10;
            dagger.Slayer = SlayerName.ReptilianDeath;
            dagger.WeaponAttributes.HitPoisonArea = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return dagger;
        }

        public WarOf1812Vault(Serial serial) : base(serial)
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
