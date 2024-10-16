using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArkainesValorArms : PlateArms
{
    [Constructable]
    public ArkainesValorArms()
    {
        Name = "Arkaine's Valor Arms";
        Hue = Utility.Random(400, 850);
        BaseArmorRating = Utility.RandomMinMax(45, 85);
        Attributes.BonusStr = 20;
        Attributes.BonusHits = 30;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArkainesValorArms(Serial serial) : base(serial)
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
