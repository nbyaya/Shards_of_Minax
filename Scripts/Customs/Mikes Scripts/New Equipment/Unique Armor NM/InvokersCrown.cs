using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InvokersCrown : BoneHelm
{
    [Constructable]
    public InvokersCrown()
    {
        Name = "Invoker's Crown";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 65);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusInt = 30;
        Attributes.CastRecovery = 3;
        Attributes.SpellDamage = 20;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 40.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 30.0);
        SkillBonuses.SetValues(2, SkillName.Magery, 50.0);
        PhysicalBonus = 12;
        ColdBonus = 18;
        FireBonus = 12;
        EnergyBonus = 25;
        PoisonBonus = 13;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InvokersCrown(Serial serial) : base(serial)
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
