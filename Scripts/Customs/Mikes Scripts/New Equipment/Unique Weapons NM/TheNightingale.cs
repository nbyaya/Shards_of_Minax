using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheNightingale : Crossbow
{
    [Constructable]
    public TheNightingale()
    {
        Name = "The Nightingale";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Vacuum;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitEnergyArea = 40;
        WeaponAttributes.MageWeapon = -10;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Inscribe, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheNightingale(Serial serial) : base(serial)
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
