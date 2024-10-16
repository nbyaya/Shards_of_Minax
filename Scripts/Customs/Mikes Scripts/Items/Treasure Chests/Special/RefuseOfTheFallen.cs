using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class RefuseOfTheFallen : TrashBarrel
    {
        [Constructable]
        public RefuseOfTheFallen()
        {
            Name = "Refuse of the Fallen";
            Hue = Utility.RandomMinMax(1, 3000);

            if (Utility.RandomDouble() < 0.1)
                DropItem(new MaxxiaScroll());

            if (Utility.RandomDouble() < 0.05)
                DropItem(new Gold() { Amount = Utility.RandomMinMax(1, 5000) });

            if (Utility.RandomDouble() < 0.20)
                DropItem(new Gold() { Name = "Tarnished Coin" });

            if (Utility.RandomDouble() < 0.15)
                DropItem(new BottleOfWine() { Name = "Discarded Potion Bottle", Hue = 1123 });

            if (Utility.RandomDouble() < 0.50)
                DropItem(new GoldBracelet() { Name = "Broken Bracelet of Misfortune" });

            if (Utility.RandomDouble() < 0.15)
                DropItem(new Gold() { Amount = Utility.RandomMinMax(1, 5000) });

            if (Utility.RandomDouble() < 0.12)
                DropItem(new Sapphire() { Name = "Faded Sapphire Fragment", Hue = 1109 });

            if (Utility.RandomDouble() < 0.08)
                DropItem(new BottleOfWine() { Name = "Smashed Wine Bottle" });

            if (Utility.RandomDouble() < 0.16)
                DropItem(new Gold() { Name = "Corroded Coin" });

            if (Utility.RandomDouble() < 0.19)
                DropItem(new Sandals() { Name = "Worn Sandals of the Lost", Hue = 1152 });

            if (Utility.RandomDouble() < 0.17)
                DropItem(new GoldRing() { Name = "Rusty Ring of Disrepair" });

            if (Utility.RandomDouble() < 0.04)
                DropItem(new SimpleMap() { Name = "Map to Nowhere", Bounds = new Rectangle2D(1000, 1000, 400, 400), NewPin = new Point2D(1200, 1200), Protected = true });

            if (Utility.RandomDouble() < 0.13)
                DropItem(new Spyglass() { Name = "Scratched Spyglass" });

            if (Utility.RandomDouble() < 0.20)
                DropItem(new GreaterHealPotion() { Name = "Weak Healing Potion" });

            if (Utility.RandomDouble() < 0.20)
                DropItem(CreateWeapon());

            if (Utility.RandomDouble() < 0.30)
                DropItem(CreateArmor());

            if (Utility.RandomDouble() < 0.30)
                DropItem(CreateThighBoots());

            if (Utility.RandomDouble() < 0.30)
                DropItem(CreateGloves());

            if (Utility.RandomDouble() < 0.30)
                DropItem(CreateLongsword());
        }

        private BaseWeapon CreateWeapon()
        {
            var weapon = new Longsword
            {
                Name = "Cracked Sword",
                Hue = Utility.RandomMinMax(1, 1788),
                MaxDamage = Utility.RandomMinMax(10, 30)
            };
            return weapon;
        }

        private BaseArmor CreateArmor()
        {
            var armor = new PlateChest
            {
                Name = "Fractured Armor",
                Hue = Utility.RandomMinMax(1, 1788),
                BaseArmorRating = Utility.RandomMinMax(10, 30)
            };
            return armor;
        }

        private Item CreateThighBoots()
        {
            var boots = new ThighBoots
            {
                Name = "Old Guard's Boots",
                Hue = Utility.RandomMinMax(600, 1600)
            };
            boots.ClothingAttributes.DurabilityBonus = 1;
            boots.Attributes.DefendChance = 2;
            boots.SkillBonuses.SetValues(0, SkillName.Fencing, 5);
            return boots;
        }

        private Item CreateGloves()
        {
            var gloves = new PlateGloves
            {
                Name = "Gloves of the Fallen",
                Hue = Utility.RandomMinMax(1, 1000),
                BaseArmorRating = Utility.RandomMinMax(20, 40)
            };
            gloves.AbsorptionAttributes.EaterPoison = 5;
            gloves.ArmorAttributes.ReactiveParalyze = 1;
            gloves.Attributes.BonusDex = 5;
            gloves.Attributes.AttackChance = 2;
            gloves.SkillBonuses.SetValues(0, SkillName.Fencing, 5);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 10;
            return gloves;
        }

        private BaseWeapon CreateLongsword()
        {
            var sword = new Longsword
            {
                Name = "Broken Blade",
                Hue = Utility.RandomMinMax(50, 250),
                MinDamage = Utility.RandomMinMax(5, 15),
                MaxDamage = Utility.RandomMinMax(15, 30),
                Slayer = SlayerName.DaemonDismissal
            };
            sword.Attributes.BonusStr = 2;
            sword.Attributes.SpellDamage = 1;
            sword.WeaponAttributes.HitFireball = 10;
            sword.WeaponAttributes.SelfRepair = 1;
            sword.SkillBonuses.SetValues(0, SkillName.Fencing, 5);
            return sword;
        }

        public RefuseOfTheFallen(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
