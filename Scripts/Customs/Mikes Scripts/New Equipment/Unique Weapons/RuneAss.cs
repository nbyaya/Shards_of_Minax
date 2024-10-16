using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RuneAss : BattleAxe
{
    [Constructable]
    public RuneAss()
    {
        Name = "Rune Ass";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.SpellChanneling = 1;
        Attributes.BonusMana = 10;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitLightning = 35;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RuneAss(Serial serial) : base(serial)
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
