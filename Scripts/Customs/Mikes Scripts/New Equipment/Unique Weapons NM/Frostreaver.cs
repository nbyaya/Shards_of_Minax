using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Frostreaver : TwoHandedAxe
{
    [Constructable]
    public Frostreaver()
    {
        Name = "Frostreaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitLowerAttack = 25;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Frostreaver(Serial serial) : base(serial)
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
