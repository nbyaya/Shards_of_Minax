using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WraithsWhisper : Longsword
{
    [Constructable]
    public WraithsWhisper()
    {
        Name = "Wraith's Whisper";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 25;
        Attributes.LowerManaCost = 15;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.BloodDrinking;
        WeaponAttributes.HitFatigue = 50;
        WeaponAttributes.HitLeechMana = 20;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WraithsWhisper(Serial serial) : base(serial)
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
