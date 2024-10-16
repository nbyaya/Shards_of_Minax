using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NetherCore : BlackStaff
{
    [Constructable]
    public NetherCore()
    {
        Name = "Nether Core";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 35;
        Attributes.SpellChanneling = 1;
        Attributes.LowerManaCost = 20;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.ResistPoisonBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 30.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NetherCore(Serial serial) : base(serial)
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
