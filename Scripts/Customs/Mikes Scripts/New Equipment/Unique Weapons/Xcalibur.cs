using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Xcalibur : Longsword
{
    [Constructable]
    public Xcalibur()
    {
        Name = "Xcalibur";
        Hue = Utility.Random(850, 2900);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitLightning = 30;
        WeaponAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Xcalibur(Serial serial) : base(serial)
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
