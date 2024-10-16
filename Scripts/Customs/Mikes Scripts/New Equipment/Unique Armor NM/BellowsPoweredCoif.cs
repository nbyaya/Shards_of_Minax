using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BellowsPoweredCoif : ChainCoif
{
    [Constructable]
    public BellowsPoweredCoif()
    {
        Name = "Bellows-Powered Coif";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(40, 60);
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStam = 20;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 40.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 30;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BellowsPoweredCoif(Serial serial) : base(serial)
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
