using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EldritchWhisper : ArtificerWand
{
    [Constructable]
    public EldritchWhisper()
    {
        Name = "Eldritch Whisper";
        Hue = Utility.Random(40, 50);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(85, 135);
        Attributes.BonusInt = 30;
        Attributes.RegenMana = 10;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitMagicArrow = 50;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EldritchWhisper(Serial serial) : base(serial)
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
