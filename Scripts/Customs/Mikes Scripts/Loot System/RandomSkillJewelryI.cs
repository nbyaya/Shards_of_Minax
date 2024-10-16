using System;
using Server;
using Server.Items;

public class RandomSkillJewelryI : BaseJewel
{
    private static string[] prefixes = { "Radiant", "Ethereal" };
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
    public RandomSkillJewelryI() : base(GetRandomItemID(), Layer.Ring)
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
            this.Attributes.RegenStam = rand.Next(2, 5);
            this.Attributes.SpellChanneling = 1;
            SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.RandomMinMax(0, 100));
            SkillBonuses.SetValues(1, SkillName.Blacksmith, Utility.RandomMinMax(0, 100));
            SkillBonuses.SetValues(2, SkillName.Mining, Utility.RandomMinMax(0, 100));
            this.Attributes.WeaponSpeed = Utility.RandomMinMax(-20, -5);
        }
        else if (tierChance < 0.2) // Rare
        {
            this.Attributes.Luck = rand.Next(40, 79);
            SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.RandomMinMax(0, 50));
            SkillBonuses.SetValues(1, SkillName.Blacksmith, Utility.RandomMinMax(0, 50));
            SkillBonuses.SetValues(2, SkillName.Mining, Utility.RandomMinMax(0, 50));
            this.Attributes.WeaponSpeed = Utility.RandomMinMax(-15, -1);
        }
        else if (tierChance < 0.5) // Uncommon
        {
            this.Attributes.Luck = rand.Next(20, 39);
            SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.RandomMinMax(0, 25));
            SkillBonuses.SetValues(1, SkillName.Blacksmith, Utility.RandomMinMax(0, 25));
            this.Attributes.WeaponSpeed = Utility.RandomMinMax(-10, -1);
        }
        else // Common
        {
            this.Attributes.Luck = rand.Next(1, 19);
            SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.RandomMinMax(0, 10));
            SkillBonuses.SetValues(1, SkillName.Blacksmith, Utility.RandomMinMax(0, 10));
        }
    }

    private static int GetRandomItemID()
    {
        Type selectedType = jewelryTypes[rand.Next(jewelryTypes.Length)];
        BaseJewel tempJewelry = (BaseJewel)Activator.CreateInstance(selectedType);
        int itemID = tempJewelry.ItemID;
        return itemID;
    }

    public RandomSkillJewelryI(Serial serial) : base(serial) { }

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
