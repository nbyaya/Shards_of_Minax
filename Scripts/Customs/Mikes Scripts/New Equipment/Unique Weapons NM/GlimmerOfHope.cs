using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlimmerOfHope : Kryss
{
    [Constructable]
    public GlimmerOfHope()
    {
        Name = "Glimmer of Hope";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 25;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitLightning = 35;
        WeaponAttributes.ResistColdBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlimmerOfHope(Serial serial) : base(serial)
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
