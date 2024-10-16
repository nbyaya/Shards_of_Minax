using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JuggernautHammer : WarHammer
{
    [Constructable]
    public JuggernautHammer()
    {
        Name = "Juggernaut Hammer";
        Hue = Utility.Random(300, 2400);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusHits = 20;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitHarm = 35;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JuggernautHammer(Serial serial) : base(serial)
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
