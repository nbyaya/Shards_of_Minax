using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BansheesWlil : Spear
{
    [Constructable]
    public BansheesWlil()
    {
        Name = "Banshee's Wail";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitMagicArrow = 50;
        WeaponAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BansheesWlil(Serial serial) : base(serial)
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
