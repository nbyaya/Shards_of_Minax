using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MacebearersLeggings : PlateLegs
{
    [Constructable]
    public MacebearersLeggings()
    {
        Name = "Macebearer's Leggings";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 95);
        ArmorAttributes.LowerStatReq = 45;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusStam = 30;
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 20;
        ColdBonus = 5;
        FireBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MacebearersLeggings(Serial serial) : base(serial)
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
