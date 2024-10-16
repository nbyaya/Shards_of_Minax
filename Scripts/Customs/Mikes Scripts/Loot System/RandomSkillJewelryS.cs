using System;
using Server;
using Server.Items;

public class RandomSkillJewelryS : BaseJewel
{
    private static string[] prefixes = { "Mighty", "Powerful" };
    private static string[] suffixes = {
        "Amulet", "Talisman", "Gem", "Orb", "Ring", "Pendant", "Bracelet", "Charm", "Bead", "Chain",
        "Crystal", "Diamond", "Emerald", "Garnet", "Jade", "Opal", "Pearl", "Ruby", "Sapphire", "Topaz",
        "Agate", "Beryl", "Citrine", "Jasper", "Moonstone", "Quartz", "Turquoise", "Zircon", "Onyx", "Peridot",
        "Sunstone", "TigerEye", "Lapis", "Obsidian", "Amber", "Aquamarine", "Sardonyx", "Spinel", "Tourmaline", "Carnelian",
        "Chrysoprase", "Heliotrope", "Iolite", "Kunzite", "Labradorite", "Malachite", "Morganite", "Rhodochrosite", "Rhodonite", "Tanzanite",
        "Azurite", "Bloodstone", "Chalcedony", "Fluorite", "Hematite", "Larimar", "Moonstone", "Prehnite", "Pyrite", "Sodalite",
        "Sunstone", "Tektite", "Variscite", "Vesuvianite", "Zoisite", "Seraphinite", "Shungite", "Spirit", "Aura", "Eclipse",
        "Galaxy", "Meteor", "Nebula", "Void", "Astral", "Celestial", "Comet", "Cosmos", "Ether", "Nexus",
        "Phantom", "Shadow", "Ethereal", "Inferno", "Mystic", "Phenomenon", "Reverie", "Serenity", "Vision", "Wraith"
    };

    private static Random rand = new Random();

    private static Type[] jewelryTypes = new Type[]
    {
        typeof(GoldRing), typeof(SilverRing), typeof(GoldBracelet), typeof(SilverBracelet)
        // Add more jewelry types as needed
    };

    [Constructable]
    public RandomSkillJewelryS() : base(GetRandomItemID(), Layer.Ring)
    {
        Type selectedType = jewelryTypes[rand.Next(jewelryTypes.Length)];
        BaseJewel tempJewelry = (BaseJewel)Activator.CreateInstance(selectedType);

        string name = prefixes[rand.Next(prefixes.Length)] + " " + suffixes[rand.Next(suffixes.Length)];
        this.Name = name;

        this.Hue = rand.Next(1, 3001); // Random hue for variety

        InitializeJewelryAttributes(tempJewelry);

        tempJewelry.Delete(); // Clean up
    }

    private void InitializeJewelryAttributes(BaseJewel tempJewelry)
    {
        double tierChance = rand.NextDouble();
        if (tierChance < 0.05) // Very rare
        {
            this.Attributes.Luck = rand.Next(0, 100);
            this.Attributes.RegenHits = rand.Next(2, 5);
            this.Attributes.SpellDamage = Utility.RandomMinMax(0, 200);
            SkillBonuses.SetValues(0, SkillName.Healing, Utility.RandomMinMax(0, 100));
            SkillBonuses.SetValues(1, SkillName.Anatomy, Utility.RandomMinMax(0, 100));
            SkillBonuses.SetValues(2, SkillName.Meditation, Utility.RandomMinMax(0, 100));
            this.Attributes.BonusStam = Utility.RandomMinMax(1, 50);
        }
        else if (tierChance < 0.2) // Rare
        {
            this.Attributes.Luck = rand.Next(40, 79);
            SkillBonuses.SetValues(0, SkillName.Healing, Utility.RandomMinMax(0, 35));
            SkillBonuses.SetValues(1, SkillName.Anatomy, Utility.RandomMinMax(0, 35));
            SkillBonuses.SetValues(2, SkillName.Meditation, Utility.RandomMinMax(0, 35));
            this.Attributes.BonusStam = Utility.RandomMinMax(1, 40);
        }
        else if (tierChance < 0.5) // Uncommon
        {
            this.Attributes.Luck = rand.Next(20, 39);
            SkillBonuses.SetValues(0, SkillName.Healing, Utility.RandomMinMax(0, 24));
            SkillBonuses.SetValues(1, SkillName.Anatomy, Utility.RandomMinMax(0, 24));
            SkillBonuses.SetValues(2, SkillName.Meditation, Utility.RandomMinMax(0, 24));
            this.Attributes.BonusStam = Utility.RandomMinMax(1, 30);
        }
        else // Common
        {
            this.Attributes.Luck = rand.Next(1, 19);
            SkillBonuses.SetValues(0, SkillName.Healing, Utility.RandomMinMax(0, 10));
            SkillBonuses.SetValues(1, SkillName.Anatomy, Utility.RandomMinMax(0, 10));
            SkillBonuses.SetValues(2, SkillName.Meditation, Utility.RandomMinMax(0, 10));
        }
    }

    private static int GetRandomItemID()
    {
        Type selectedType = jewelryTypes[rand.Next(jewelryTypes.Length)];
        BaseJewel tempJewelry = (BaseJewel)Activator.CreateInstance(selectedType);
        int itemID = tempJewelry.ItemID;
        return itemID;
    }

    public RandomSkillJewelryS(Serial serial) : base(serial) { }

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
