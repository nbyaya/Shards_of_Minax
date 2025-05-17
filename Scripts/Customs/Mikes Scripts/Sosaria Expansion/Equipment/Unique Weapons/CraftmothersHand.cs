using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CraftmothersHand : CarpentersHammer
{
    [Constructable]
    public CraftmothersHand()
    {
        Name = "Craftmother's Hand";
        Hue = Utility.Random(1200, 1300);  // Warm earthy tones to reflect the craftsmanship
        MinDamage = Utility.RandomMinMax(20, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);

        // Attributes relevant to the crafting and woodcutting theme
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusHits = 10;

        // Skill bonuses related to Carpentry, Lumberjacking, and Healing (for crafting aid and protection)
        SkillBonuses.SetValues(0, SkillName.Carpentry, 25.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 10.0);

        // Slayer effect for crafting-related enemies or wood-based creatures
        Slayer = SlayerName.OgreTrashing; // Used to represent the ability to smash through the tough wood of large creatures

        // Weapon attributes to make it useful in both combat and utility
        WeaponAttributes.HitLeechHits = 10;
        WeaponAttributes.HitLeechMana = 5;
        WeaponAttributes.BattleLust = 10;

        // Additional thematic bonus for crafting and working with wood
        Attributes.Luck = 20;

        // Attach the XML level item for additional customization or effects
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CraftmothersHand(Serial serial) : base(serial)
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
