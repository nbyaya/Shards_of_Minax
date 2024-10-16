using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesSane : LargeBattleAxe
{
    [Constructable]
    public GargoylesSane()
    {
        Name = "Gargoyle's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 30;
        Attributes.BonusStr = 20;
        Slayer = SlayerName.GargoylesFoe;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitMagicArrow = 40;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesSane(Serial serial) : base(serial)
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
