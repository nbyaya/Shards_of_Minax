using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StepOfTheGladewalker : LeatherLegs
{
    [Constructable]
    public StepOfTheGladewalker()
    {
        Name = "Step of the Gladewalker";
        Hue = Utility.Random(1500, 2200);
        BaseArmorRating = Utility.RandomMinMax(15, 45);

        // Attributes related to stealth, nature, and agility
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 5;
        Attributes.Luck = 15;
        Attributes.NightSight = 1;

        // Skill bonuses reflecting agility, tracking, and nature-related abilities
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 20.0);

        // Nature and elemental resistances, representing connection to the forest
        ColdBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 15;

        // Attach custom XmlLevelItem for leveling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StepOfTheGladewalker(Serial serial) : base(serial)
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
