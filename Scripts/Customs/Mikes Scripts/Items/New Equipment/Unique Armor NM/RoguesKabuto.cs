using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoguesKabuto : DecorativePlateKabuto
{
    [Constructable]
    public RoguesKabuto()
    {
        Name = "Rogue's Kabuto";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 30;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Snooping, 45.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 50.0);
        SkillBonuses.SetValues(2, SkillName.Hiding, 35.0);
        PhysicalBonus = 25;
        ColdBonus = 15;
        FireBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoguesKabuto(Serial serial) : base(serial)
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
