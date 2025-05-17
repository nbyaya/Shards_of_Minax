using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PathboundSilence : MonkRobe
{
    [Constructable]
    public PathboundSilence()
    {
        Name = "Pathbound Silence";
        Hue = 1100;  // Soft, earthy tone like a robe worn by monks in meditation.

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 20;
        Attributes.BonusMana = 10;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 10;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);   // Enhances meditation and spiritual growth.
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);      // Increases stealth abilities to move silently, evoking the monk's quietude.
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);       // Enhances the ability to blend into shadows.
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 10.0);  // Channeling inner peace through SpiritSpeak.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PathboundSilence(Serial serial) : base(serial)
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
