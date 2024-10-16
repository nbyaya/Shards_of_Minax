using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KatanaOfTheSerpentsFang : Katana
{
    [Constructable]
    public KatanaOfTheSerpentsFang()
    {
        Name = "Katana of the Serpent's Fang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 15;
        Attributes.Luck = 100;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 40;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KatanaOfTheSerpentsFang(Serial serial) : base(serial)
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
