using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesHeartAmulet : StuddedGorget
{
    [Constructable]
    public WitchesHeartAmulet()
    {
        Name = "Witch's Heart Amulet";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 10;
        Attributes.BonusHits = 20;
        SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesHeartAmulet(Serial serial) : base(serial)
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
