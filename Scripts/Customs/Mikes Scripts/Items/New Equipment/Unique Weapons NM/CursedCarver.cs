using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CursedCarver : Cleaver
{
    [Constructable]
    public CursedCarver()
    {
        Name = "Cursed Carver";
        Hue = 666;
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.AttackChance = 20;
        Attributes.CastSpeed = -1;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitCurse = 40;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        SkillBonuses.SetValues(2, SkillName.Cooking, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CursedCarver(Serial serial) : base(serial)
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
