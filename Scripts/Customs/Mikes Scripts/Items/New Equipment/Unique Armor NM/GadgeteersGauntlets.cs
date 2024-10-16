using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GadgeteersGauntlets : PlateGloves
{
    [Constructable]
    public GadgeteersGauntlets()
    {
        Name = "Gadgeteer's Gauntlets";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.SelfRepair = 15;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 20;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 30.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 30.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 50.0);
        PhysicalBonus = 10;
        ColdBonus = 20;
        FireBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GadgeteersGauntlets(Serial serial) : base(serial)
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
