using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BondsOfTheSpiraledVine : LeatherBustierArms
{
    [Constructable]
    public BondsOfTheSpiraledVine()
    {
        Name = "Bonds of the Spiraled Vine";
        Hue = Utility.Random(1, 1000);  // Random color, you can customize this based on desired appearance
        BaseArmorRating = Utility.RandomMinMax(15, 35);

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 10;
        Attributes.RegenHits = 3;
        Attributes.RegenStam = 5;
        Attributes.DefendChance = 10;

        // Skill Bonuses tied thematically to nature, agility, and healing
        SkillBonuses.SetValues(0, SkillName.Anatomy, 15.0);  // Boosts healing skills
        SkillBonuses.SetValues(1, SkillName.Healing, 20.0);   // Increases effectiveness of healing
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // For those dealing with animals and healing them

        // Elemental bonuses that fit the nature theme
        PhysicalBonus = 10;
        PoisonBonus = 5;
        FireBonus = 5;

        // Attach XML Level Item for integration into the world
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BondsOfTheSpiraledVine(Serial serial) : base(serial)
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
