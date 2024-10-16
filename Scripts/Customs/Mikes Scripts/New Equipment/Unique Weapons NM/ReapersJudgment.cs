using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReapersJudgment : ExecutionersAxe
{
    [Constructable]
    public ReapersJudgment()
    {
        Name = "Reaper's Judgment";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitHarm = 45;
        WeaponAttributes.HitLowerAttack = 35;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReapersJudgment(Serial serial) : base(serial)
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
