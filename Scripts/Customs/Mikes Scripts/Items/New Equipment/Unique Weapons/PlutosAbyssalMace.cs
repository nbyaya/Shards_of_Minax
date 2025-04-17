using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PlutosAbyssalMace : SewingNeedle
{
    [Constructable]
    public PlutosAbyssalMace()
    {
        Name = "Pluto's Needle";
        Hue = Utility.Random(700, 2900);
        MinDamage = Utility.RandomMinMax(30, 85);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.BonusHits = 25;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitFireArea = 25;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PlutosAbyssalMace(Serial serial) : base(serial)
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
