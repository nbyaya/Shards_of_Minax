using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostwardensPlateGloves : PlateGloves
{
    [Constructable]
    public FrostwardensPlateGloves()
    {
        Name = "Frostwarden's PlateGloves";
        Hue = Utility.Random(600, 650);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        AbsorptionAttributes.EaterCold = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 10;
        Attributes.SpellDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);
        ColdBonus = 20;
        EnergyBonus = 5;
        FireBonus = 0;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostwardensPlateGloves(Serial serial) : base(serial)
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
