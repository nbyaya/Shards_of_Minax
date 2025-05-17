using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfTheFinalHammer : PlateGloves
{
    [Constructable]
    public GauntletsOfTheFinalHammer()
    {
        Name = "Gauntlets of the Final Hammer";
        Hue = Utility.Random(1000, 2000); // Custom color for uniqueness
        BaseArmorRating = Utility.RandomMinMax(35, 80); // Base armor rating for strong protection
        
        Attributes.BonusStr = 15; // Increased Strength for better combat effectiveness
        Attributes.BonusDex = 5; // A slight boost to Dexterity to aid in combat speed and mobility
        Attributes.BonusHits = 25; // Added hits to increase survivability in battle
        
        Attributes.DefendChance = 10; // A boost to defensive capabilities
        Attributes.ReflectPhysical = 15; // Reflect damage back to attackers (the final hammer striking back)
        Attributes.WeaponSpeed = 5; // Slight increase to weapon speed for quicker strikes

        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0); // Enhances Tactics, making the wearer a master of battlefield strategy
        SkillBonuses.SetValues(1, SkillName.Swords, 30.0); // Boosts Swords for greater damage with sword-based weapons
        SkillBonuses.SetValues(2, SkillName.Macing, 15.0); // Adds a small bonus for Macing, as the "hammer" theme could apply to both maces and swords
        
        ColdBonus = 5; // Small bonus to cold resistance, indicative of the gauntlets' protective properties
        EnergyBonus = 10; // Added energy resistance, helping the wearer survive elemental magic
        FireBonus = 10; // A defensive edge against fire damage as part of their protection theme
        PhysicalBonus = 15; // Enhanced physical resistance for overall defense
        PoisonBonus = 5; // A bit of poison resistance for added protection in combat situations

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Add the XML Level Item for the unique item functionality
    }

    public GauntletsOfTheFinalHammer(Serial serial) : base(serial)
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
