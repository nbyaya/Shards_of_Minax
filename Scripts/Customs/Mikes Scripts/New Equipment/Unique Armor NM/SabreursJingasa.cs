using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SabreursJingasa : LightPlateJingasa
{
    [Constructable]
    public SabreursJingasa()
    {
        Name = "Sabreur's Jingasa";
        Hue = Utility.Random(250, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 70);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.DefendChance = 25;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Fencing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 45.0);
        PhysicalBonus = 20;
        ColdBonus = 5;
        FireBonus = 5;
        EnergyBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SabreursJingasa(Serial serial) : base(serial)
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
