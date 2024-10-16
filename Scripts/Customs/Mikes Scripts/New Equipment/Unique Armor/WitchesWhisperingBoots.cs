using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesWhisperingBoots : LeatherLegs
{
    [Constructable]
    public WitchesWhisperingBoots()
    {
        Name = "Witch's Whispering Boots";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterCold = 10;
        Attributes.NightSight = 1;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesWhisperingBoots(Serial serial) : base(serial)
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
