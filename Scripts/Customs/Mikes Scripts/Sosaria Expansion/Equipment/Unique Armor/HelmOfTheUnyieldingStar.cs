using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HelmOfTheUnyieldingStar : PlateHelm
{
    [Constructable]
    public HelmOfTheUnyieldingStar()
    {
        Name = "Helm of the Unyielding Star";
        Hue = Utility.Random(1000, 1100); // Celestial or star-like hue (for thematic flair)
        BaseArmorRating = Utility.RandomMinMax(40, 90); // A robust defensive value for this legendary helm

        // Attribute bonuses based on the theme of unyielding strength and celestial power
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 25;  // Strength and endurance for battle
        Attributes.DefendChance = 10; // Deflect more attacks
        Attributes.RegenHits = 2;     // Some health regeneration, enhancing survivability in battle

        // Skill bonuses to fit the theme of defense and celestial resilience
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);    // Tactics for battle strategy and defense
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);   // Anatomy for increased damage resistance and healing potential

        // Elemental bonuses tied to resilience and celestial influences
        FireBonus = 15;
        PhysicalBonus = 10;

        // Attach the unique item-level XML behavior
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HelmOfTheUnyieldingStar(Serial serial) : base(serial)
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
