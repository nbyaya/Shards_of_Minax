using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MillenniumTimeCapsule : WoodenChest
    {
        [Constructable]
        public MillenniumTimeCapsule()
        {
            Name = "Millennium Time Capsule";
            Hue = Utility.Random(1, 1550);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<SilverNecklace>("Y2K Amulet"), 0.27);
            AddItem(CreateNamedItem<Apple>("iPod of Tunes"), 0.32);
            AddItem(CreateNamedItem<TreasureLevel1>("Decade's Memorabilia"), 0.21);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4700)), 0.17);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateBodySash(), 0.20);
            AddItem(CreateBoneHelm(), 0.20);
            AddItem(CreateWarFork(), 0.20);
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
            note.NoteString = "Remembering the turn of the century.";
            note.TitleString = "Millennium Scrapbook";
            return note;
        }

        private Item CreateRandomClothing()
        {
            // Assuming RandomClothing is similar to BaseClothing
            Robe clothing = new Robe(); // Example
            clothing.Name = "Retro Outfit";
            clothing.Hue = Utility.RandomList(1, 1550);
            return clothing;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = "Dial-up Dagger";
            weapon.Hue = Utility.RandomList(1, 1550);
            return weapon;
        }

        private Item CreateBodySash()
        {
            BodySash sash = new BodySash();
            sash.Name = "Mummy's Wrappings";
            sash.Hue = Utility.RandomMinMax(650, 750);
            sash.Attributes.DefendChance = 15;
            sash.Attributes.LowerRegCost = 10;
            sash.SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            sash.SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);
            return sash;
        }

        private Item CreateBoneHelm()
        {
            BoneHelm helm = new BoneHelm();
            helm.Name = "Mondain's Skull";
            helm.Hue = Utility.RandomMinMax(666, 777);
            helm.BaseArmorRating = Utility.RandomMinMax(40, 70);
            helm.Attributes.ReflectPhysical = 10;
            helm.Attributes.BonusMana = 20;
            helm.AbsorptionAttributes.EaterEnergy = 20;
            helm.ColdBonus = 10;
            helm.EnergyBonus = 20;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 15;
            return helm;
        }

        private Item CreateWarFork()
        {
            WarFork fork = new WarFork();
            fork.Name = "Vampire Killer";
            fork.Hue = Utility.RandomMinMax(100, 150);
            fork.MinDamage = Utility.RandomMinMax(20, 50);
            fork.MaxDamage = Utility.RandomMinMax(50, 80);
            fork.Attributes.SpellChanneling = 1;
            fork.Attributes.NightSight = 1;
            fork.Slayer = SlayerName.DaemonDismissal;
            fork.Slayer2 = SlayerName.BalronDamnation;
            fork.WeaponAttributes.HitDispel = 25;
            fork.WeaponAttributes.HitLeechHits = 20;
            fork.SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
            return fork;
        }

        public MillenniumTimeCapsule(Serial serial) : base(serial)
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
