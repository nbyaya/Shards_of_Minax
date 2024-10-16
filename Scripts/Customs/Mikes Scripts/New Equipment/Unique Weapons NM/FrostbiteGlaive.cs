using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbiteGlaive : Halberd
{
    [Constructable]
    public FrostbiteGlaive()
    {
        Name = "Frostbite Glaive";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 40;
        WeaponAttributes.ResistColdBonus = 25;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbiteGlaive(Serial serial) : base(serial)
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
