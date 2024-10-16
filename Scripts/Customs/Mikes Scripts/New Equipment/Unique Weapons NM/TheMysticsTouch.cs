using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheMysticsTouch : BlackStaff
{
    [Constructable]
    public TheMysticsTouch()
    {
        Name = "The Mystic's Touch";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(15, 35);
        MaxDamage = Utility.RandomMinMax(40, 80);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.ReptilianDeath;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.MageWeapon = -20;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheMysticsTouch(Serial serial) : base(serial)
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
