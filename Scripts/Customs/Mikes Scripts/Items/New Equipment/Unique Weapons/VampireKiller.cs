using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VampireKiller : GourmandsFork
{
    [Constructable]
    public VampireKiller()
    {
        Name = "Vampire Killer";
        Hue = Utility.Random(100, 2150);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.SpellChanneling = 1;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitDispel = 25;
        WeaponAttributes.HitLeechHits = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VampireKiller(Serial serial) : base(serial)
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
