using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CosmicHarmony : BlackStaff
{
    [Constructable]
    public CosmicHarmony()
    {
        Name = "Cosmic Harmony";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 3;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.MageWeapon = -10;
        WeaponAttributes.HitDispel = 35;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);
        SkillBonuses.SetValues(2, SkillName.Focus, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CosmicHarmony(Serial serial) : base(serial)
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
