using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardiansPlatemailGloves : PlateGloves
{
    [Constructable]
    public GuardiansPlatemailGloves()
    {
        Name = "Guardian's Platemail Gloves";
        Hue = Utility.Random(5001, 6000);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.SelfRepair = 15;
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 25;
        SkillBonuses.SetValues(0, SkillName.Parry, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 12;
        EnergyBonus = 18;
        FireBonus = 12;
        ColdBonus = 18;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardiansPlatemailGloves(Serial serial) : base(serial)
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
