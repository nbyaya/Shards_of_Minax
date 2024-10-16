using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EchoesOfSilence : DoubleAxe
{
    [Constructable]
    public EchoesOfSilence()
    {
        Name = "Echoes of Silence";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 15;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitManaDrain = 50;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EchoesOfSilence(Serial serial) : base(serial)
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
