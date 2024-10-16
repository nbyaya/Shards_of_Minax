using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperOfDeath : ExecutionersAxe
{
    [Constructable]
    public WhisperOfDeath()
    {
        Name = "Whisper of Death";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.ResistPoisonBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Fencing, 15.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperOfDeath(Serial serial) : base(serial)
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
