using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpectresTouch : WarFork
{
    [Constructable]
    public SpectresTouch()
    {
        Name = "Spectre's Touch";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitManaDrain = 45;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        SkillBonuses.SetValues(1, SkillName.Wrestling, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpectresTouch(Serial serial) : base(serial)
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
