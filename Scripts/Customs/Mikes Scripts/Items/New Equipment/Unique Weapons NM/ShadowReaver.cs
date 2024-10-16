using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowReaver : Kryss
{
    [Constructable]
    public ShadowReaver()
    {
        Name = "Shadow Reaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(60, 120);
        Attributes.BonusDex = 20;
        Attributes.BonusStam = 15;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BloodDrinking;
        WeaponAttributes.HitFatigue = 40;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowReaver(Serial serial) : base(serial)
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
