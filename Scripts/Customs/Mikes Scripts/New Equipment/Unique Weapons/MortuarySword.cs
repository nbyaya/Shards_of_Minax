using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MortuarySword : Scimitar
{
    [Constructable]
    public MortuarySword()
    {
        Name = "Mortuary Sword";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MortuarySword(Serial serial) : base(serial)
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
