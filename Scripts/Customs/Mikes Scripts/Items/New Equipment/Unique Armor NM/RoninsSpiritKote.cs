using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoninsSpiritKote : PlateArms
{
    [Constructable]
    public RoninsSpiritKote()
    {
        Name = "Ronin's Spirit Kote";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        Attributes.BonusStr = 20;
        Attributes.BonusStam = 20;
        ArmorAttributes.SelfRepair = 15;
        SkillBonuses.SetValues(0, SkillName.Bushido, 45.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 30.0);
        PhysicalBonus = 18;
        FireBonus = 22;
        ColdBonus = 18;
        EnergyBonus = 12;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoninsSpiritKote(Serial serial) : base(serial)
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
