using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadesEdge : ButcherKnife
{
    [Constructable]
    public ShadesEdge()
    {
        Name = "Shade's Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 10;
        Attributes.DefendChance = 20;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitMagicArrow = 40;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadesEdge(Serial serial) : base(serial)
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
