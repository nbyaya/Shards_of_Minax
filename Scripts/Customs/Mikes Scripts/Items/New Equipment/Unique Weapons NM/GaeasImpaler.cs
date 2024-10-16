using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaeasImpaler : Pitchfork
{
    [Constructable]
    public GaeasImpaler()
    {
        Name = "Gaea's Impaler";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 25;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitPoisonArea = 35;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaeasImpaler(Serial serial) : base(serial)
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
