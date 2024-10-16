using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsTaul : WarHammer
{
    [Constructable]
    public SerpentsTaul()
    {
        Name = "Serpent's Maul";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 50;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsTaul(Serial serial) : base(serial)
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
