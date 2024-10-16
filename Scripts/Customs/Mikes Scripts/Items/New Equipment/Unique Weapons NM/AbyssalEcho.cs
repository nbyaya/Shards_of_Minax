using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AbyssalEcho : BlackStaff
{
    [Constructable]
    public AbyssalEcho()
    {
        Name = "Abyssal Echo";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 15;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitLowerDefend = 40;
        WeaponAttributes.HitMagicArrow = 50;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        SkillBonuses.SetValues(2, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AbyssalEcho(Serial serial) : base(serial)
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
