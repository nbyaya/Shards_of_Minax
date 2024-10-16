using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AegisOfValor : HeaterShield
{
    [Constructable]
    public AegisOfValor()
    {
        Name = "Aegis of Valor";
        Hue = Utility.Random(350, 700);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusHits = 30;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        PhysicalBonus = 25;
        PoisonBonus = 5;
        EnergyBonus = 5;
        FireBonus = 15;
        ColdBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AegisOfValor(Serial serial) : base(serial)
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
