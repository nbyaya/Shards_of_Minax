using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BanshoFanClub : MeditationFans
{
    [Constructable]
    public BanshoFanClub()
    {
        Name = "Bansho Fan";
        Hue = Utility.Random(200, 2250);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.SpellChanneling = 1;
        Attributes.RegenMana = 3;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireArea = 20;
        WeaponAttributes.HitManaDrain = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BanshoFanClub(Serial serial) : base(serial)
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
