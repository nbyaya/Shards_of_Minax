using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbiteHatchet : LargeBattleAxe
{
    [Constructable]
    public FrostbiteHatchet()
    {
        Name = "Frostbite Hatchet";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 35;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitColdArea = 55;
        WeaponAttributes.ResistColdBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbiteHatchet(Serial serial) : base(serial)
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
