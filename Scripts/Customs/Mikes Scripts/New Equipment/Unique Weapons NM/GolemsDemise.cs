using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GolemsDemise : WarHammer
{
    [Constructable]
    public GolemsDemise()
    {
        Name = "Golem's Demise";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 40;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitFireArea = 45;
        WeaponAttributes.HitManaDrain = 35;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 10.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GolemsDemise(Serial serial) : base(serial)
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
