using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GavelOfJustice : WarMace
{
    [Constructable]
    public GavelOfJustice()
    {
        Name = "Gavel of Justice";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(60, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.Luck = 100;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.BalronDamnation;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitFireball = 30;
        WeaponAttributes.ResistFireBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 30.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GavelOfJustice(Serial serial) : base(serial)
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
