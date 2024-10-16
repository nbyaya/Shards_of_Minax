using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GhoulSlayersLongsword : Longsword
{
    [Constructable]
    public GhoulSlayersLongsword()
    {
        Name = "Ghoul Slayer's Longsword";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 10;
        Attributes.RegenStam = 3;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitDispel = 20;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GhoulSlayersLongsword(Serial serial) : base(serial)
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
