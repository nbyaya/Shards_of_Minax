using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReapersToll : LargeBattleAxe
{
    [Constructable]
    public ReapersToll()
    {
        Name = "Reaper's Toll";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(60, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitLeechHits = 30;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Wrestling, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReapersToll(Serial serial) : base(serial)
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
