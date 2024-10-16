using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InquisitorsGavel : Mace
{
    [Constructable]
    public InquisitorsGavel()
    {
        Name = "The Inquisitor's Gavel";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 30;
        Attributes.BonusInt = 10;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.HitLowerDefend = 40;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InquisitorsGavel(Serial serial) : base(serial)
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
