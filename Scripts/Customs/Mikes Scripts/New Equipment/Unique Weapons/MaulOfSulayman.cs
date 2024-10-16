using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaulOfSulayman : Maul
{
    [Constructable]
    public MaulOfSulayman()
    {
        Name = "Maul of Sulayman";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(75, 115);
        Attributes.BonusInt = 12;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.ElementalHealth;
        WeaponAttributes.HitDispel = 35;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaulOfSulayman(Serial serial) : base(serial)
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
