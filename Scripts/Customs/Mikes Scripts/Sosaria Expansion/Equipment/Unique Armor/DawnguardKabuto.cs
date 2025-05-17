using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DawnguardKabuto : StandardPlateKabuto
{
    [Constructable]
    public DawnguardKabuto()
    {
        Name = "Dawnguard Kabuto";
        Hue = 1153;  // A noble, deep hue fit for a protector
        BaseArmorRating = Utility.RandomMinMax(30, 80);  // Suitable protection for a warrior of the dawn

        Attributes.BonusStr = 15;  // Increases physical strength
        Attributes.BonusDex = 10;  // Dexterity to allow for quick reaction in combat
        Attributes.BonusHits = 25;  // Increases health to withstand more damage
        Attributes.DefendChance = 10;  // Increases defense against attacks

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Boosts Tactics for strategic advantage
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);  // Enhances swordsmanship for melee combat
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0);  // Increases parry to defend against enemy strikes

        ColdBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;  // Strong against physical attacks, emphasizing defense and protection

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Assigns a level item to the armor
    }

    public DawnguardKabuto(Serial serial) : base(serial)
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
