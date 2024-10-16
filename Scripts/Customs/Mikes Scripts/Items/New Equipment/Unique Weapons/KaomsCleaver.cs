using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KaomsCleaver : Cleaver
{
    [Constructable]
    public KaomsCleaver()
    {
        Name = "Kaom's Cleaver";
        Hue = Utility.Random(250, 2900);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStr = 25;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.BloodDrinker = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KaomsCleaver(Serial serial) : base(serial)
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
