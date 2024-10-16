using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheJudge : Club
{
    [Constructable]
    public TheJudge()
    {
        Name = "The Judge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 20;
        Attributes.ReflectPhysical = 10;
        Attributes.WeaponSpeed = 20;
        Slayer = SlayerName.BalronDamnation;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitManaDrain = 40;
        WeaponAttributes.ResistPhysicalBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheJudge(Serial serial) : base(serial)
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
