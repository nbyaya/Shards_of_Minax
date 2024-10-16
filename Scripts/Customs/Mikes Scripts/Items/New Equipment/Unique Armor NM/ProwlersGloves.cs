using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ProwlersGloves : LeatherGloves
{
    [Constructable]
    public ProwlersGloves()
    {
        Name = "Prowler's Gloves";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(20, 40);
        ArmorAttributes.SelfRepair = 10;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        Attributes.BonusHits = 10;
        SkillBonuses.SetValues(0, SkillName.Stealing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 45.0);
        SkillBonuses.SetValues(2, SkillName.DetectHidden, 30.0);
        PhysicalBonus = 10;
        ColdBonus = 5;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ProwlersGloves(Serial serial) : base(serial)
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
