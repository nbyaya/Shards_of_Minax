using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EldritchAegis : BlackStaff
{
    [Constructable]
    public EldritchAegis()
    {
        Name = "Eldritch Aegis";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 25;
        Attributes.DefendChance = 20;
        Attributes.ReflectPhysical = 5;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.Fey;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EldritchAegis(Serial serial) : base(serial)
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
