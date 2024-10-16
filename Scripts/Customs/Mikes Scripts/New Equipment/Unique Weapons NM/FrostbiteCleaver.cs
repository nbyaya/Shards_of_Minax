using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbiteCleaver : Cleaver
{
    [Constructable]
    public FrostbiteCleaver()
    {
        Name = "Frostbite Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 60;
        WeaponAttributes.HitHarm = 30;
        SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
        SkillBonuses.SetValues(1, SkillName.Cartography, 15.0); // Assuming Survival is a valid skill; replace if needed
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbiteCleaver(Serial serial) : base(serial)
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
