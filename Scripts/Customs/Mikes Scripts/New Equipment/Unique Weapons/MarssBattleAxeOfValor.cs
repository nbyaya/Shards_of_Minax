using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MarssBattleAxeOfValor : BattleAxe
{
    [Constructable]
    public MarssBattleAxeOfValor()
    {
        Name = "Mars's BattleAxe of Valor";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(40, 95);
        MaxDamage = Utility.RandomMinMax(95, 135);
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.BattleLust = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MarssBattleAxeOfValor(Serial serial) : base(serial)
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
