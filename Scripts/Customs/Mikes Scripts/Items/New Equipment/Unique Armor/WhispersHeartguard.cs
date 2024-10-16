using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhispersHeartguard : PlateChest
{
    [Constructable]
    public WhispersHeartguard()
    {
        Name = "Whisperer's Heartguard";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.EaterPoison = 25;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.RegenHits = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
        ColdBonus = 20;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhispersHeartguard(Serial serial) : base(serial)
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
