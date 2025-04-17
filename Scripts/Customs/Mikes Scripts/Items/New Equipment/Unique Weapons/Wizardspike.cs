using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Wizardspike : SewingNeedle
{
    [Constructable]
    public Wizardspike()
    {
        Name = "Wizardspike";
        Hue = Utility.Random(600, 2900);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 85);
        Attributes.BonusMana = 50;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitColdArea = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Wizardspike(Serial serial) : base(serial)
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
