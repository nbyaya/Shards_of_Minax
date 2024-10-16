using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DreadlordsBoneChest : BoneChest
{
    [Constructable]
    public DreadlordsBoneChest()
    {
        Name = "Dreadlord's BoneChest";
        Hue = Utility.Random(555, 999);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.LowerStatReq = 40;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        Attributes.Luck = 150;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 50.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 40.0);
        SkillBonuses.SetValues(2, SkillName.Poisoning, 35.0);
        PhysicalBonus = 20;
        ColdBonus = 20;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DreadlordsBoneChest(Serial serial) : base(serial)
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
