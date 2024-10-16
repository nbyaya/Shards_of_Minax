using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NajsArcaneVestment : PlateChest
{
    [Constructable]
    public NajsArcaneVestment()
    {
        Name = "Naj's Arcane Vestment";
        Hue = Utility.Random(250, 750);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NajsArcaneVestment(Serial serial) : base(serial)
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
