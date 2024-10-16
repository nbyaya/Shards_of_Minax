using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TyraelsVigil : PlateChest
{
    [Constructable]
    public TyraelsVigil()
    {
        Name = "Tyrael's Vigil";
        Hue = Utility.Random(400, 700);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterDamage = 20;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.DefendChance = 20;
        Attributes.RegenHits = 10;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TyraelsVigil(Serial serial) : base(serial)
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
