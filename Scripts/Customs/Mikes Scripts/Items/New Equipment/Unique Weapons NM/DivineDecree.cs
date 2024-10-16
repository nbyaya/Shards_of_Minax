using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DivineDecree : Cleaver
{
    [Constructable]
    public DivineDecree()
    {
        Name = "Divine Decree";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 30;
        Attributes.BonusInt = 10;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitDispel = 45;
        WeaponAttributes.HitLightning = 35;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DivineDecree(Serial serial) : base(serial)
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
