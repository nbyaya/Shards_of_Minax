using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TransmutationThighBoots : LeatherLegs
{
    [Constructable]
    public TransmutationThighBoots()
    {
        Name = "Transmutation ThighBoots";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        Attributes.NightSight = 1;
        Attributes.BonusStam = 30;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 30.0);
        PhysicalBonus = 10;
        FireBonus = 15;
        ColdBonus = 5;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TransmutationThighBoots(Serial serial) : base(serial)
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
