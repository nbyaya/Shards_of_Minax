using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TitansReach : Spear
{
    [Constructable]
    public TitansReach()
    {
        Name = "Titan's Reach";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TitansReach(Serial serial) : base(serial)
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
