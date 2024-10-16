using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BardsBowOfDiscord : Bow
{
    [Constructable]
    public BardsBowOfDiscord()
    {
        Name = "Bard's Bow of Discord";
        Hue = Utility.Random(500, 750);
        MinDamage = Utility.RandomMinMax(10, 70);
        MaxDamage = Utility.RandomMinMax(70, 130);
        Attributes.Luck = 50;
        Attributes.BonusDex = 10;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitHarm = 40;
        SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
        SkillBonuses.SetValues(1, SkillName.Musicianship, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BardsBowOfDiscord(Serial serial) : base(serial)
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
