using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperOfTheHollowKings : Bascinet
{
    [Constructable]
    public WhisperOfTheHollowKings()
    {
        Name = "Whisper of the Hollow Kings";
        Hue = Utility.Random(1100, 1600);  // A regal, dark hue—representing ancient nobility.
        BaseArmorRating = Utility.RandomMinMax(40, 70); // A solid protective value, hinting at its regal nature.

        // Attributes: Focus on wisdom, defense, and magic
        Attributes.BonusStr = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 10;
        Attributes.DefendChance = 12;  // Helping the wearer protect themselves from attacks.
        Attributes.CastSpeed = 1;  // A slight bonus to speed for casting spells.
        Attributes.LowerManaCost = 10;  // Reduced mana cost for magical actions, linking to the forgotten power.

        // Skill Bonuses: Themes of ancient leadership, mysticism, and combat.
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);  // Ties into the haunting whispers and the connection to the dead.
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);  // Strategic mindset in battle, a feature of ancient kings.
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0);  // Tapping into the ancient magical energies of the Hollow Kings.

        // Elemental Bonuses: Ancient powers that have lasted through the ages.
        ColdBonus = 10;
        FireBonus = 5;
        PoisonBonus = 5;

        // Special effects tied to the whispers and ancient legacies.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperOfTheHollowKings(Serial serial) : base(serial)
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
