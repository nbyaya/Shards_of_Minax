using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArmsOfTheHearthguard : LeatherArms
{
    [Constructable]
    public ArmsOfTheHearthguard()
    {
        Name = "Arms of the Hearthguard";
        Hue = Utility.Random(2000, 2500); // A warm, earthy hue representing firelight or hearth
        BaseArmorRating = Utility.RandomMinMax(30, 60); // Balanced armor rating suitable for light or medium protection

        // Hearth and defense themed attributes
        Attributes.BonusStr = 5;  // Adds strength to defend the hearth
        Attributes.BonusHits = 10;  // Increases survivability, guarding the hearth
        Attributes.DefendChance = 15;  // Protection against attackers, fitting for the Hearthguard theme
        Attributes.LowerManaCost = 10;  // Makes spells easier to cast when protecting loved ones or fighting for safety

        // Thematic skill bonuses to align with protection and community
        SkillBonuses.SetValues(0, SkillName.Healing, 15.0); // Hearthguards care for the injured and sick
        SkillBonuses.SetValues(1, SkillName.Camping, 10.0); // Hearthguards often maintain the hearth while in the wilderness
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Knowledge of the body helps in protection and healing

        // Elemental bonuses, reflecting the Hearthguard's connection to warmth and fire
        FireBonus = 15;  // They are protective like a fire's warmth, giving an edge in fiery environments
        PhysicalBonus = 5;  // They offer physical protection from attacks

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaching a custom XmlLevelItem for special functionality
    }

    public ArmsOfTheHearthguard(Serial serial) : base(serial)
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
