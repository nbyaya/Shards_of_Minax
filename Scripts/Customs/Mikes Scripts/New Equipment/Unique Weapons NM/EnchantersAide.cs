using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EnchantersAide : GnarledStaff
{
    [Constructable]
    public EnchantersAide()
    {
        Name = "Enchanter's Aide";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 2;
        Attributes.LowerRegCost = 15;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.Fey;
        WeaponAttributes.SelfRepair = 5;
        WeaponAttributes.HitMagicArrow = 30;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EnchantersAide(Serial serial) : base(serial)
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
