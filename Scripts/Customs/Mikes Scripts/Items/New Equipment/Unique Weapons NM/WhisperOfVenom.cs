using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperOfVenom : SkinningKnife
{
    [Constructable]
    public WhisperOfVenom()
    {
        Name = "Whisper of Venom";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperOfVenom(Serial serial) : base(serial)
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
