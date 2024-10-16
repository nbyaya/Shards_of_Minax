using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EldritchBowOfShadows : Bow
{
    [Constructable]
    public EldritchBowOfShadows()
    {
        Name = "Eldritch Bow of Shadows";
        Hue = Utility.Random(900, 2950);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 150);
        Attributes.SpellChanneling = 1;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitManaDrain = 35;
        SkillBonuses.SetValues(0, SkillName.Archery, 30.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EldritchBowOfShadows(Serial serial) : base(serial)
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
