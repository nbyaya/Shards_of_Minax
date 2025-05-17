using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TorsoOfEternalSentinel : PlateDo
{
    [Constructable]
    public TorsoOfEternalSentinel()
    {
        Name = "Torso of the Eternal Sentinel";
        Hue = Utility.Random(2300, 2500);  // Slightly aged golden hue, signifying ancient power
        BaseArmorRating = Utility.RandomMinMax(50, 80);  // Strong defense to match its history

        // Key Attributes for physical defense and resilience
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 25;
        Attributes.DefendChance = 10;  // Boosts defense against attackers

        // Combat-related skill bonuses to enhance the sentinel's battle prowess
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Experience in battle tactics
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);    // Swordsmanship prowess, enhancing combat skills
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0);     // Deflects blows in combat

        // Additional defensive bonuses tied to the ancient nature of the armor
        PhysicalBonus = 15;
        FireBonus = 5;
        EnergyBonus = 5;
        ColdBonus = 5;

        // Magical properties, hinting at the armor’s mystical background
        Attributes.LowerManaCost = 10;
        Attributes.RegenStam = 5;  // Regeneration of stamina, crucial for prolonged combat

        // Attach an XmlLevelItem tag to this item to track it
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TorsoOfEternalSentinel(Serial serial) : base(serial)
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
