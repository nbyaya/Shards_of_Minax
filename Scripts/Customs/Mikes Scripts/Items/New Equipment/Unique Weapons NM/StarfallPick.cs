using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarfallPick : HammerPick
{
    [Constructable]
    public StarfallPick()
    {
        Name = "Starfall Pick";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 10;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitLightning = 25;
        WeaponAttributes.ResistPhysicalBonus = 10;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarfallPick(Serial serial) : base(serial)
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
