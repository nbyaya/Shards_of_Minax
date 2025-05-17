using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CinderskullCap : SkullCap
{
    [Constructable]
    public CinderskullCap()
    {
        Name = "Cinderskull Cap";
        Hue = 1175; // A dark, fiery hue for an eerie, smoldering look
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 10;
        Attributes.BonusMana = 20;


        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 20; // Significant fire resistance, fitting for the "Cinderskull" theme
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0); // Fits the death/fire motif
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0); // Spirit communication, related to the undead/fire theme
        SkillBonuses.SetValues(2, SkillName.Poisoning, 10.0); // Poisoning synergizes with the fiery death aspect of the item
        SkillBonuses.SetValues(3, SkillName.Magery, 5.0); // A bit of magical prowess fits the cursed theme

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CinderskullCap(Serial serial) : base(serial)
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
