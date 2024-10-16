using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BerserkersEmbrace : BoneGloves
{
    [Constructable]
    public BerserkersEmbrace()
    {
        Name = "Berserker's Embrace";
        Hue = Utility.Random(300, 700);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterKinetic = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStr = 30;
        Attributes.AttackChance = 15;
        Attributes.WeaponDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BerserkersEmbrace(Serial serial) : base(serial)
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
