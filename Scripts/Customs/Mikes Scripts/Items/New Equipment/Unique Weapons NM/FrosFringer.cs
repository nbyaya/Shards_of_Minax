using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrosFringer : WarHammer
{
    [Constructable]
    public FrosFringer()
    {
        Name = "Frost Bringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 55;
        WeaponAttributes.ResistColdBonus = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrosFringer(Serial serial) : base(serial)
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
