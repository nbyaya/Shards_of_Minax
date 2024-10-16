using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperOfTheWoods : Bow
{
    [Constructable]
    public WhisperOfTheWoods()
    {
        Name = "Whisper of the Woods";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.Luck = 200;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);
        SkillBonuses.SetValues(2, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperOfTheWoods(Serial serial) : base(serial)
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
