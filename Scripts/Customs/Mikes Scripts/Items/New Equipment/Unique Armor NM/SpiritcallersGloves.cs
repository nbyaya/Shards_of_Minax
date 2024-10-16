using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpiritcallersGloves : LeatherGloves
{
    [Constructable]
    public SpiritcallersGloves()
    {
        Name = "Spiritcaller's Gloves";
        Hue = Utility.Random(444, 777);
        BaseArmorRating = Utility.RandomMinMax(20, 40);
        AbsorptionAttributes.ResonanceEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.CastSpeed = 2;
        Attributes.CastRecovery = 2;
        Attributes.BonusInt = 20;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 50.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 30.0);
        PhysicalBonus = 10;
        ColdBonus = 20;
        FireBonus = 10;
        EnergyBonus = 30;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpiritcallersGloves(Serial serial) : base(serial)
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
