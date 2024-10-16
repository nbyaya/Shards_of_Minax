using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThunderlordsJudgment : WarHammer
{
    [Constructable]
    public ThunderlordsJudgment()
    {
        Name = "Thunderlord's Judgment";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 15;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 65;
        WeaponAttributes.HitFatigue = 30;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThunderlordsJudgment(Serial serial) : base(serial)
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
