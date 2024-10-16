using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TomahawkOfTecumseh : WarAxe
{
    [Constructable]
    public TomahawkOfTecumseh()
    {
        Name = "Tomahawk of Tecumseh";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 80);
        Attributes.BonusStr = 10;
        Attributes.DefendChance = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitHarm = 25;
        WeaponAttributes.BattleLust = 15;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TomahawkOfTecumseh(Serial serial) : base(serial)
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
