using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RockNRallVault : WoodenChest
    {
        [Constructable]
        public RockNRallVault()
        {
            Name = "Rock 'n' Roll Vault";
            Hue = Utility.RandomList(1, 2120);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Emerald>("Elvis's Gem"), 0.28);
            AddItem(CreateNamedItem<GreaterHealPotion>("Woodstock Brew"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel3>("Vinyl Record"), 0.23);
            AddItem(CreateNamedItem<GoldEarrings>("Guitar Pick Earrings"), 0.37);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4600)), 0.16);
            AddItem(CreateRandomInstrument(), 0.18);
            AddItem(CreatePotion(), 0.18);
            AddItem(CreateSkirt(), 0.20);
            AddItem(CreateLegplates(), 0.20);
            AddItem(CreateBattleAxe(), 0.20);
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

        private Item CreateRandomInstrument()
        {
            Lute instrument = new Lute();
            instrument.Name = "60's Electric Guitar";
            return instrument;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Rockstar Elixir";
            return potion;
        }

        private Item CreateSkirt()
        {
            Kilt skirt = new Kilt();
            skirt.Name = "Poodle Skirt of Charm";
            skirt.Hue = Utility.RandomMinMax(200, 1200);
            skirt.Attributes.Luck = 20;
            skirt.SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
            return skirt;
        }

        private Item CreateLegplates()
        {
            PlateLegs legplates = new PlateLegs();
            legplates.Name = "Hammerlord's Legplates";
            legplates.Hue = Utility.RandomMinMax(350, 650);
            legplates.BaseArmorRating = Utility.Random(45, 80);
            legplates.AbsorptionAttributes.EaterCold = 10;
            legplates.ArmorAttributes.DurabilityBonus = 20;
            legplates.Attributes.BonusStam = 20;
            legplates.PhysicalBonus = 20;
            legplates.ColdBonus = 15;
            legplates.EnergyBonus = 5;
            legplates.FireBonus = 10;
            legplates.PoisonBonus = 5;
            return legplates;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe battleAxe = new BattleAxe();
            battleAxe.Name = "Alamo Defender's Axe";
            battleAxe.Hue = Utility.RandomMinMax(200, 400);
            battleAxe.MinDamage = Utility.Random(30, 70);
            battleAxe.MaxDamage = Utility.Random(70, 110);
            battleAxe.Attributes.BonusHits = 10;
            battleAxe.Attributes.RegenStam = 3;
            battleAxe.WeaponAttributes.HitDispel = 20;
            battleAxe.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
            return battleAxe;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The Beatles rule!";
            note.TitleString = "Rocker's Memo";
            return note;
        }

        public RockNRallVault(Serial serial) : base(serial)
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
