using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AxeOfTheLeviathan : LargeBattleAxe
{
    [Constructable]
    public AxeOfTheLeviathan()
    {
        Name = "Axe of the Leviathan";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitDispel = 35;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AxeOfTheLeviathan(Serial serial) : base(serial)
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
