using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TelvanniMagistersCap : LeatherCap
{
    [Constructable]
    public TelvanniMagistersCap()
    {
        Name = "Telvanni Magister's Cap";
        Hue = Utility.Random(600, 800);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.CastingFocus = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TelvanniMagistersCap(Serial serial) : base(serial)
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
