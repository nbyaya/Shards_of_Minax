using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class QuasarAxe : BattleAxe
{
    [Constructable]
    public QuasarAxe()
    {
        Name = "Quasar Axe";
        Hue = Utility.Random(650, 2900);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.WeaponSpeed = 5;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitLeechStam = 30;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 50.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public QuasarAxe(Serial serial) : base(serial)
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
