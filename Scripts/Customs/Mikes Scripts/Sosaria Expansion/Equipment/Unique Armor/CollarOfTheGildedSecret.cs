using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CollarOfTheGildedSecret : ElegantCollar
{
    [Constructable]
    public CollarOfTheGildedSecret()
    {
        Name = "Collar of the Gilded Secret";
        Hue = 1153;  // A mystical golden hue for a secretive aura
        Weight = 1.0;

        // Bonus Attributes
        Attributes.BonusInt = 15;  // Increase Intelligence to enhance magical abilities
        Attributes.BonusDex = 10;  // Boost Dexterity for those who need agility
        Attributes.Luck = 25;  // For those who delve into secrets and hidden knowledge, a bit of luck is always useful
        
        // Skill Bonuses (Fitting the secretive and scholarly theme)
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);  // Inscribe, for magical writing and deciphering ancient texts
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);  // Detect hidden, to uncover secrets and hidden paths
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // Meditation, for better mana regeneration and focus on hidden knowledge

        // Elemental Bonuses
        ColdBonus = 10;
        EnergyBonus = 5;
        
        // Adding the XmlLevelItem to make this item a level item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CollarOfTheGildedSecret(Serial serial) : base(serial)
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
