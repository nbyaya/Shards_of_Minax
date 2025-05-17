using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EmberhideWrap : FurSarong
{
    [Constructable]
    public EmberhideWrap()
    {
        Name = "Emberhide Wrap";
        Hue = Utility.Random(2300, 2500); // Warm tones to reflect the fiery theme
        
        // Set attributes and bonuses
        Attributes.BonusStr = 8;
        Attributes.BonusDex = 8;
        Attributes.BonusHits = 30;
        Attributes.BonusStam = 20;
        Attributes.BonusMana = 10;


        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 25; // Embodies the fire aspect of Emberhide
        Resistances.Cold = 5;
        Resistances.Poison = 5;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0); // Animal lore is related to understanding the creatures of the fire realms and the natural world
        SkillBonuses.SetValues(1, SkillName.Herding, 10.0); // The user may also work with fire-based creatures, such as fire elementals, etc.
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0); // Mysticism aligns with the magical fire nature of the Emberhide
        SkillBonuses.SetValues(3, SkillName.Veterinary, 5.0); // Helping heal and calm fire-resistant creatures

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EmberhideWrap(Serial serial) : base(serial)
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
