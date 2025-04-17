using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ZhugeFeathersFan : MeditationFans
{
    [Constructable]
    public ZhugeFeathersFan()
    {
        Name = "Zhuge's Feather Fan";
        Hue = Utility.Random(100, 2200);
        MinDamage = Utility.RandomMinMax(10, 30);
        MaxDamage = Utility.RandomMinMax(30, 60);
        Attributes.BonusInt = 15;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ZhugeFeathersFan(Serial serial) : base(serial)
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
