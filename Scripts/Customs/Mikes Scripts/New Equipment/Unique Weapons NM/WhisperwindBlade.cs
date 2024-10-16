using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperwindBlade : Cutlass
{
    [Constructable]
    public WhisperwindBlade()
    {
        Name = "Whisperwind Blade";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 10;
        Attributes.CastSpeed = 1;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitMagicArrow = 40;
        WeaponAttributes.ResistPoisonBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperwindBlade(Serial serial) : base(serial)
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
