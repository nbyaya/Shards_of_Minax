using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BloodleafTotemMask : SavageMask
{
    [Constructable]
    public BloodleafTotemMask()
    {
        Name = "Bloodleaf Totem Mask";
        Hue = 0x497; // Dark earthy tones to match the savage theme

        // Set attributes and bonuses
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 10;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Poison = 20;
        
        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);  // Enhances interaction with animals
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);     // Helps with locating enemies or prey
        SkillBonuses.SetValues(2, SkillName.Necromancy, 10.0);   // Adds a touch of dark magic, fitting for the savage theme

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BloodleafTotemMask(Serial serial) : base(serial)
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
