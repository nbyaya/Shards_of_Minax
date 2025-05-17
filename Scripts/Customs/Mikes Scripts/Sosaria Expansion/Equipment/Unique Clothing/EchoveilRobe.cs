using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EchoveilRobe : Robe
{
    [Constructable]
    public EchoveilRobe()
    {
        Name = "Echoveil Robe";
        Hue = Utility.Random(1150, 1350); // A spectral shade of purples and grays

        // Set attributes and bonuses
        Attributes.BonusInt = 20;
        Attributes.BonusMana = 25;
        Attributes.RegenMana = 5;
        Attributes.SpellDamage = 15;
        Attributes.CastSpeed = 1;
        Attributes.CastRecovery = 2;
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 75;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 20;
        Resistances.Poison = 10;
        Resistances.Energy = 25;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);  // The robe enhances the use of dark magics.
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);  // Allows communication with spirits, a key aspect of shadowy magic.
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0);      // The robe blends the wearer with shadows.
        SkillBonuses.SetValues(3, SkillName.Hiding, 10.0);       // Further enhancing stealth-related activities.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EchoveilRobe(Serial serial) : base(serial)
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
