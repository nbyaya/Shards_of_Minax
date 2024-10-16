using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ExodusBarrier : MetalKiteShield
{
    [Constructable]
    public ExodusBarrier()
    {
        Name = "Exodus Barrier";
        Hue = Utility.Random(333, 444);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        AbsorptionAttributes.ResonanceEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 20;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
        ColdBonus = 5;
        EnergyBonus = 25;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ExodusBarrier(Serial serial) : base(serial)
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
