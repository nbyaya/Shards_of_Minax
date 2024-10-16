using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskedAvengersVoice : PlateGorget
{
    [Constructable]
    public MaskedAvengersVoice()
    {
        Name = "Masked Avenger's Voice";
        Hue = Utility.Random(900, 999);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Provocation, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskedAvengersVoice(Serial serial) : base(serial)
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
