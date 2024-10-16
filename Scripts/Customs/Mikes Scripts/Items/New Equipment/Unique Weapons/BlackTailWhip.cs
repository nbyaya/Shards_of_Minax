using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlackTailWhip : WarFork
{
    [Constructable]
    public BlackTailWhip()
    {
        Name = "BlackTail Whip";
        Hue = Utility.Random(600, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 85);
        Attributes.SpellDamage = 10;
        Attributes.RegenMana = 3;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.ElementalHealth;
        WeaponAttributes.HitLightning = 35;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlackTailWhip(Serial serial) : base(serial)
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
