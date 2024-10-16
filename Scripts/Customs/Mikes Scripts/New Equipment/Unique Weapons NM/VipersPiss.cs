using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VipersPiss : WarFork
{
    [Constructable]
    public VipersPiss()
    {
        Name = "Viper's Kiss";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.Luck = 200;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.ScorpionsBane;
        WeaponAttributes.HitPoisonArea = 45;
        WeaponAttributes.HitLeechStam = 30;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VipersPiss(Serial serial) : base(serial)
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
