using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrudgehideVisage : OrcishKinMask
{
    [Constructable]
    public GrudgehideVisage()
    {
        Name = "Grudgehide Visage";
        Hue = 1157; // A greenish hue, fitting for an orcish mask

        // Set attributes and bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 25;
        Attributes.BonusStam = 25;

        // Resistances
        Resistances.Physical = 20;
        Resistances.Fire = 10;
        Resistances.Cold = 5;
        Resistances.Poison = 15;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Anatomy, 10.0); // Orcish warriors are known for their brutal knowledge of anatomy
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0); // A nod to their combat prowess
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Orcs are adept hunters and trackers
        SkillBonuses.SetValues(3, SkillName.Wrestling, 10.0); // They often use their strength in hand-to-hand combat

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrudgehideVisage(Serial serial) : base(serial)
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
