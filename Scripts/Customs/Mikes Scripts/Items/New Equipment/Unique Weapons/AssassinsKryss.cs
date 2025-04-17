using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AssassinsKryss : FocusKryss
{
    [Constructable]
    public AssassinsKryss()
    {
        Name = "Assassin's Kryss";
        Hue = Utility.Random(1, 1000);
        MinDamage = Utility.RandomMinMax(15, 65);
        MaxDamage = Utility.RandomMinMax(65, 120);
        Attributes.AttackChance = 15;
        Attributes.LowerRegCost = 10;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AssassinsKryss(Serial serial) : base(serial)
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
