using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TouchOfAnguish : MageWand
{
    [Constructable]
    public TouchOfAnguish()
    {
        Name = "Touch of Anguish";
        Hue = Utility.Random(250, 2900);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 85);
        Attributes.RegenMana = 5;
        Attributes.Luck = 120;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.HitColdArea = 35;
        WeaponAttributes.HitLeechMana = 20;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TouchOfAnguish(Serial serial) : base(serial)
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
