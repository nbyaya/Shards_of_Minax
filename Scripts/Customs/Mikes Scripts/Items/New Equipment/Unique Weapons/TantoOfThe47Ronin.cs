using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TantoOfThe47Ronin : Dagger
{
    [Constructable]
    public TantoOfThe47Ronin()
    {
        Name = "Tanto of the 47 Ronin";
        Hue = Utility.Random(350, 2400);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 85);
        Attributes.BonusHits = 10;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitHarm = 15;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TantoOfThe47Ronin(Serial serial) : base(serial)
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
