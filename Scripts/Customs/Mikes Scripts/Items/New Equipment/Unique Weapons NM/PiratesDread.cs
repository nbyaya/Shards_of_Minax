using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PiratesDread : Cutlass
{
    [Constructable]
    public PiratesDread()
    {
        Name = "Pirate's Dread";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.ReflectPhysical = 15;
        Attributes.DefendChance = 25;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitFireball = 35;
        WeaponAttributes.HitPhysicalArea = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PiratesDread(Serial serial) : base(serial)
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
