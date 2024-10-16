using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KatanaOfTheInferno : Katana
{
    [Constructable]
    public KatanaOfTheInferno()
    {
        Name = "Katana of the Inferno";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusStam = 10;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.ResistFireBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KatanaOfTheInferno(Serial serial) : base(serial)
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
