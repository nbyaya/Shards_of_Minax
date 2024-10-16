using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingWindWarMace : WarMace
{
    [Constructable]
    public WhisperingWindWarMace()
    {
        Name = "Whispering Wind WarMace";
        Hue = Utility.Random(250, 2300);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(90, 140);
        Attributes.BonusDex = 20;
        Attributes.LowerRegCost = 15;
        Slayer = SlayerName.SummerWind;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitEnergyArea = 45;
        WeaponAttributes.HitDispel = 40;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingWindWarMace(Serial serial) : base(serial)
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
