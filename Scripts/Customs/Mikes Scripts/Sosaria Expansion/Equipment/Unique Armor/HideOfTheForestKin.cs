using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HideOfTheForestKin : LeatherChest
{
    [Constructable]
    public HideOfTheForestKin()
    {
        Name = "Hide of the Forest Kin";
        Hue = Utility.Random(2000, 2500); // Earthy, forest-like hues
        BaseArmorRating = Utility.RandomMinMax(15, 45); // Balanced armor rating for a lightweight, nature-themed chest piece

        // Attributes
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 10;
        Attributes.RegenStam = 3;
        Attributes.RegenHits = 2;

        // Skill Bonuses, aligned with wilderness and survival
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0);

        // Resistances, focused on natural elements
        PhysicalBonus = 10;
        FireBonus = 5;
        ColdBonus = 10;
        PoisonBonus = 15;

        // Additional beneficial attributes
        ArmorAttributes.SelfRepair = 5;
        Attributes.Luck = 10;
        Attributes.DefendChance = 5;

        // Thematically fitting with the forest kin, the armor provides an aura of protection
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HideOfTheForestKin(Serial serial) : base(serial)
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
