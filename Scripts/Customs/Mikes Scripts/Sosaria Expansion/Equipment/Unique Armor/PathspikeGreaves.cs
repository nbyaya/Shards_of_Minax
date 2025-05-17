using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PathspikeGreaves : StuddedSuneate
{
    [Constructable]
    public PathspikeGreaves()
    {
        Name = "Pathspike Greaves";
        Hue = Utility.Random(1150, 1300); // Earthy, nature-inspired hue
        BaseArmorRating = Utility.RandomMinMax(18, 55);

        // Attributes enhancing mobility and survival
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 4;

        // Reflective of the armor’s connection to the terrain and nature
        Attributes.DefendChance = 8;
        Attributes.ReflectPhysical = 10;

        // Skill bonuses that enhance movement, tracking, and survival
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0);  // Boosts tracking, fitting for Pathspike Greaves
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);    // Strengthens combat awareness
        SkillBonuses.SetValues(2, SkillName.Anatomy, 5.0);    // Assists in understanding of opponents

        // Cold and Physical resistance bonuses in the context of nature and harsh terrain
        ColdBonus = 5;
        PhysicalBonus = 10;

        // XmlLevelItem to ensure the armor is unique and part of the level-based item system
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PathspikeGreaves(Serial serial) : base(serial)
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
