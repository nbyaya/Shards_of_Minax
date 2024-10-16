using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DrakescaleLongbow : Bow
{
    [Constructable]
    public DrakescaleLongbow()
    {
        Name = "Drakescale Longbow";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitFireball = 45;
        WeaponAttributes.ResistFireBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DrakescaleLongbow(Serial serial) : base(serial)
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
