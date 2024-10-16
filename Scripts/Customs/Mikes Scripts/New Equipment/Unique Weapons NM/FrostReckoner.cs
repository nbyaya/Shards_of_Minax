using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostReckoner : BattleAxe
{
    [Constructable]
    public FrostReckoner()
    {
        Name = "Frost Reckoner";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 50;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Chivalry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostReckoner(Serial serial) : base(serial)
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
