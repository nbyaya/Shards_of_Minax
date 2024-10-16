using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class KingdomsVaultChest : WoodenChest
    {
        [Constructable]
        public KingdomsVaultChest()
        {
            Name = "Kingdom's Vault";
            Movable = false;
            Hue = Utility.RandomMinMax(1, 600);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.12);
            AddItem(CreateNamedItem<GoldEarrings>("Earrings of the Golden Queen"), 0.10);
            AddItem(CreateSimpleNote1(), 0.18);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.18);
            AddItem(CreateSimpleNote2(), 0.18);
            AddItem(CreateFullApron(), 1.0);
            AddItem(CreateShield(), 0.20);
            AddItem(CreateBow(), 0.20);
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

        private Item CreateSimpleNote1()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The crown jewels are lost, seek them.";
            note.TitleString = "King's Decree";
            return note;
        }

        private Item CreateSimpleNote2()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Long live the Golden Kingdom.";
            note.TitleString = "King's Personal Note";
            return note;
        }

        private Item CreateFullApron()
        {
            FullApron apron = new FullApron();
            apron.Name = "Tailor's Fancy Apron";
            apron.Hue = Utility.RandomMinMax(300, 1200);
            apron.ClothingAttributes.LowerStatReq = 3;
            apron.Attributes.BonusDex = 10;
            apron.Attributes.RegenMana = 2;
            apron.SkillBonuses.SetValues(0, SkillName.Tailoring, 20.0);
            return apron;
        }

        private Item CreateShield()
        {
            MetalKiteShield shield = new MetalKiteShield();
            shield.Name = "Knight's Valor Shield";
            shield.Hue = Utility.RandomMinMax(1, 1000);
            shield.BaseArmorRating = Utility.RandomMinMax(40, 85);
            shield.AbsorptionAttributes.EaterKinetic = 30;
            shield.ArmorAttributes.SelfRepair = 10;
            shield.Attributes.BonusStr = 30;
            shield.Attributes.DefendChance = 20;
            shield.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            shield.ColdBonus = 10;
            shield.EnergyBonus = 10;
            shield.FireBonus = 10;
            shield.PhysicalBonus = 10;
            shield.PoisonBonus = 10;
            return shield;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Bard's Bow of Discord";
            bow.Hue = Utility.RandomMinMax(500, 750);
            bow.MinDamage = Utility.RandomMinMax(10, 70);
            bow.MaxDamage = Utility.RandomMinMax(70, 130);
            bow.Attributes.Luck = 50;
            bow.Attributes.BonusDex = 10;
            bow.Slayer = SlayerName.Fey;
            bow.WeaponAttributes.HitHarm = 40;
            bow.SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
            bow.SkillBonuses.SetValues(1, SkillName.Musicianship, 15.0);
            return bow;
        }

        public KingdomsVaultChest(Serial serial) : base(serial)
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
