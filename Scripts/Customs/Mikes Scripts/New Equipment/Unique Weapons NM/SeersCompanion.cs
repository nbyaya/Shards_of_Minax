using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeersCompanion : GnarledStaff
{
    [Constructable]
    public SeersCompanion()
    {
        Name = "Seer's Companion";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 40;
        Attributes.CastSpeed = 1;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.ResistPoisonBonus = 15;
        WeaponAttributes.HitManaDrain = 35;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeersCompanion(Serial serial) : base(serial)
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
