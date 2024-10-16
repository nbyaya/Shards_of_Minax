using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaleForce : Spear
{
    [Constructable]
    public GaleForce()
    {
        Name = "Gale Force";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.AttackChance = 25;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitEnergyArea = 30;
        WeaponAttributes.HitLowerAttack = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaleForce(Serial serial) : base(serial)
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
