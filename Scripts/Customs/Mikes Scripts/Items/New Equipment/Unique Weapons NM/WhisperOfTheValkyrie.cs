using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperOfTheValkyrie : VikingSword
{
    [Constructable]
    public WhisperOfTheValkyrie()
    {
        Name = "Whisper of the Valkyrie";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.ScorpionsBane;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperOfTheValkyrie(Serial serial) : base(serial)
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
