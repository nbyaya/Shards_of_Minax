using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CthulhusGaze : BoltRod
{
    [Constructable]
    public CthulhusGaze()
    {
        Name = "Cthulhu's Gaze";
        Hue = Utility.Random(10, 20);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(95, 150);
        Attributes.NightSight = 1;
        Attributes.LowerManaCost = 20;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitManaDrain = 50;
        WeaponAttributes.HitLeechHits = 30;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CthulhusGaze(Serial serial) : base(serial)
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
