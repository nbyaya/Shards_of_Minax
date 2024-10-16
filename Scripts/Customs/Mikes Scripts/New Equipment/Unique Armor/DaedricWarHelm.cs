using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DaedricWarHelm : NorseHelm
{
    [Constructable]
    public DaedricWarHelm()
    {
        Name = "Daedric War Helm";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(45, 80);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.SpellDamage = 15;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DaedricWarHelm(Serial serial) : base(serial)
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
