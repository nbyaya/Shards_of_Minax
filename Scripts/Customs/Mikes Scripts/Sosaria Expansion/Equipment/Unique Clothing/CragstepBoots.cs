using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CragstepBoots : Boots
{
    [Constructable]
    public CragstepBoots()
    {
        Name = "Cragstep Boots";
        Hue = Utility.Random(0x8A5, 1000); // earthy tones that match the rugged environment of Sosaria

        // Set attributes and bonuses
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 25;
        Attributes.RegenStam = 3;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Mining, 10.0);  // Ties to the rugged, miner/stonework theme
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0);   // For stealthy movements through craggy terrains
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0); // Helps track creatures through wilderness and craggy paths

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CragstepBoots(Serial serial) : base(serial)
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
