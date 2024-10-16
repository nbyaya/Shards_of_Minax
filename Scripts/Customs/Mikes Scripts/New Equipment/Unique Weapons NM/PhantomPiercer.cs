using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PhantomPiercer : ShortSpear
{
    [Constructable]
    public PhantomPiercer()
    {
        Name = "Phantom Piercer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 40;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLeechStam = 45;
        WeaponAttributes.HitLowerAttack = 30;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PhantomPiercer(Serial serial) : base(serial)
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
