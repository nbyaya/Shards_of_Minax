using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrimjawsCradle : OrcHelm
{
    [Constructable]
    public GrimjawsCradle()
    {
        Name = "Grimjaw's Cradle";
        Hue = Utility.Random(0, 1000); // The hue represents the earthy, war-worn colors of the orcs.
        BaseArmorRating = Utility.RandomMinMax(30, 80); // Toughness fitting for an orcish warrior.

        Attributes.BonusStr = 15; // Orcs are known for their raw strength.
        Attributes.BonusHits = 20; // Tough warriors have high health to survive brutal combat.
        Attributes.DefendChance = 10; // A better chance to defend, befitting a seasoned fighter.
        Attributes.ReflectPhysical = 15; // The helmet allows the wearer to reflect physical damage, invoking the toughness of an orc's hide.

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Orcs are skilled in battle tactics.
        SkillBonuses.SetValues(1, SkillName.Wrestling, 20.0); // Orcs are fierce in hand-to-hand combat.
        SkillBonuses.SetValues(2, SkillName.Macing, 15.0); // Orcs favor blunt weapons in close combat.

        PhysicalBonus = 10; // A boost to physical resistance to enhance survivability.
        PoisonBonus = 5; // Orcs have an innate resistance to poison, often used in their battlefields.

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach the item to the XmlLevelItem for handling level progression.
    }

    public GrimjawsCradle(Serial serial) : base(serial)
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
